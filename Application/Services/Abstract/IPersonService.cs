using Core.Security.Entities;
using Domain.Entities.Concrete;

namespace Application.Services.Abstract;

public interface IPersonService
{
    public Task<Person?> GetPersonWithEmail(string email);
    public Task<IList<OperationClaim>> GetOperationClaimsForPerson(Person person);
    public void DoesGivenPasswordMatchesWithPersonsPassword(string password, Person person);
    public Task AddBoardToPerson(int personId, int boardId);
    public Task<Person> GetPersonWithId(int personId);
}