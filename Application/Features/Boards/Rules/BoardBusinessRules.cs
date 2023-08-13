using Application.Repositories;
using Core.CrossCuttingConcerns.Exceptions;
using Domain.Entities.Concrete;

namespace Application.Features.Boards.Rules;

public class BoardBusinessRules
{
    private readonly IPersonBoardRepository _personBoardRepository;
    private readonly IPersonRepository _personRepository;

    public BoardBusinessRules(IPersonBoardRepository personBoardRepository, IPersonRepository personRepository)
    {
        _personBoardRepository = personBoardRepository;
        _personRepository = personRepository;
    }

    public void DoesBoardExist(Board board)
    {
        if (board == null) throw new BusinessException("The board wanted to be deleted does not exist.");
    }

    public async Task IsPersonAlreadyInBoard(string personEmail, int boardId)
    {
        Person person = await _personRepository.GetAsync(x => x.Email == personEmail);
        if(person == null)
            return;
        PersonBoard personBoard =
            await _personBoardRepository.GetAsync(x => x.BoardId == boardId && x.PersonId == person.Id);
        if (personBoard != null)
            throw new BusinessException("Specified person already in board.");
    }

    public void DoPersonIdAndEmailDirectSamePerson(Person person, int personId, string email)
    {
        if (!(person.Id == personId && person.Email == email))
            throw new BusinessException("Email and person id do not direct same person");
    }
}