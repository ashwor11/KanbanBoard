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

    public void DoesBoardAndTheCardExist(Board board, int cardId)
    {
        DoesBoardExist(board);
        if (board.Cards.FirstOrDefault(x => x.Id == cardId) == null)
            throw new BusinessException("Specified card does not exist.");
    }

    public void DoesBoardCardAndTheJobExist(Board board, int cardId, int jobId)
    {
        DoesBoardAndTheCardExist(board, cardId);
        if (board.Cards.FirstOrDefault(x => x.Id == cardId).Jobs.FirstOrDefault(x=>x.Id == jobId) == null)
            throw new BusinessException("Specified job does not exist.");
    }

    public void DoesBoardCardAndCardFeedbackExist(Board board, int cardId, int cardFeedbackId)
    {
        DoesBoardAndTheCardExist(board,cardId);
        if (board.Cards.FirstOrDefault(x => x.Id == cardId).Feedbacks.FirstOrDefault(x => x.Id == cardFeedbackId) ==
            null)
            throw new BusinessException("Specified card feedback does not exist.");
    }

    public void DoesBoardCardJobAndTheJobFeedbackExist(Board board, int cardId, int jobId, int jobFeedbackId)
    {
        DoesBoardCardAndTheJobExist(board,cardId,jobId);
        if (board.Cards.FirstOrDefault(x => x.Id == cardId).Jobs.FirstOrDefault(x => x.Id == jobId).Feedbacks.FirstOrDefault(x=>x.Id == jobFeedbackId) ==
            null)
            throw new BusinessException("Specified job feedback does not exist.");
    }

    public void IsNull(object obj)
    {
        if (obj == null)
            throw new BusinessException($" The specified {obj.GetType().Name} is not found");
    }
}