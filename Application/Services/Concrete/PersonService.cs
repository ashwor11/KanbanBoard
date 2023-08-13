using System.Security.Authentication;
using Application.Repositories;
using Application.Services.Abstract;
using Core.Persistence.Paging;
using Core.Security.Entities;
using Core.Security.Hashing;
using Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Concrete;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;
    private readonly IUserOperationClaimRepository _userOperationClaimRepository;
    private readonly IPersonBoardRepository _personBoardRepository;

    public PersonService(IPersonRepository personRepository, IUserOperationClaimRepository userOperationClaimRepository, IPersonBoardRepository personBoardRepository)
    {
        _personRepository = personRepository;
        _userOperationClaimRepository = userOperationClaimRepository;
        _personBoardRepository = personBoardRepository;
    }

    public async Task<Person?> GetPersonWithEmail(string email)
    {
        Person person = await _personRepository.GetAsync(predicate:x => x.Email == email)!;
        return person;
    }

    public async Task<IList<OperationClaim>> GetOperationClaimsForPerson(Person person)
    {
        IPaginate<UserOperationClaim> userOperationClaims = await _userOperationClaimRepository
            .GetListAsync(x => x.UserId == person.Id,
                include: x => x.Include(x => x.OperationClaim));

        return userOperationClaims.Items.Select(x => x.OperationClaim).ToList();
    }

    public void DoesGivenPasswordMatchesWithPersonsPassword(string password, Person person)
    {
        bool doesPasswordMatches = HashingHelper.VerifyPasswordHash(password, person.PasswordHash, person.PasswordSalt);
        if (!doesPasswordMatches) throw new AuthenticationException("Passwords does not match");
    }

    public async Task AddBoardToPerson(int personId, int boardId)
    {
        await _personBoardRepository.CreateAsync(new PersonBoard() { BoardId = boardId, PersonId = personId });
        
    }

    public async Task<Person> GetPersonWithId(int personId)
    {
        Person person = await _personRepository.GetAsync(predicate: x => x.Id == personId)!;
        return person;
    }
}