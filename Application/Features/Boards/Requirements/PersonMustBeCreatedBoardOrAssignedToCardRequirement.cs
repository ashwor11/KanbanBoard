using Application.Features.Boards.Rules;
using Application.Repositories;
using Domain.Entities.Concrete;
using MediatR.Behaviors.Authorization;

namespace Application.Features.Boards.Requirements;

public class PersonMustBeCreatedBoardOrAssignedToCardRequirement : IAuthorizationRequirement
{
    public int PersonId { get; set; }
    public int BoardId { get; set; }
    public int CardId { get; set; }

    public class UserMustBeCreatorOfTheBoardRequirementHandler : IAuthorizationHandler<PersonMustBeCreatedBoardOrAssignedToCardRequirement>
    {
        private readonly IBoardRepository _boardRepository;
        private readonly BoardBusinessRules _boardBoardBusinessRules;

        public UserMustBeCreatorOfTheBoardRequirementHandler(IBoardRepository boardRepository, BoardBusinessRules boardBoardBusinessRules)
        {
            _boardRepository = boardRepository;
            _boardBoardBusinessRules = boardBoardBusinessRules;

        }

        public async Task<AuthorizationResult> Handle(PersonMustBeCreatedBoardOrAssignedToCardRequirement requirement,
            CancellationToken cancellationToken)
        {
            Board board = await _boardRepository.GetBoardWithCardsAsync(requirement.BoardId)!;
            _boardBoardBusinessRules.DoesBoardAndTheCardExist(board,requirement.CardId);
            if (board.CreatorUserId != requirement.PersonId && board.Cards.FirstOrDefault(x=>x.Id == requirement.CardId).AssignedPersonId != requirement.PersonId)
                return AuthorizationResult.Fail("You are not authorized to this operation on this board.");

            return AuthorizationResult.Succeed();
        }
    }
}