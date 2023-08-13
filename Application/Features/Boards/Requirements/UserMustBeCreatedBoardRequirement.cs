using Application.Repositories;
using Domain.Entities.Concrete;
using MediatR.Behaviors.Authorization;

namespace Application.Features.Boards.Requirements;

public class UserMustBeCreatedBoardRequirement : IAuthorizationRequirement
{
    public int PersonId { get; set; }
    public int BoardId { get; set; }

    public class UserMustBeCreatorOfTheBoardRequirementHandler : IAuthorizationHandler<UserMustBeCreatedBoardRequirement>
    {
        private readonly IBoardRepository _boardRepository;

        public UserMustBeCreatorOfTheBoardRequirementHandler(IBoardRepository boardRepository)
        {
            _boardRepository = boardRepository;
        }

        public async Task<AuthorizationResult> Handle(UserMustBeCreatedBoardRequirement requirement,
            CancellationToken cancellationToken)
        {
            Board board = await _boardRepository.GetAsync(x => x.Id == requirement.BoardId)!;
            if (board.CreatorUserId!= requirement.PersonId)
                return AuthorizationResult.Fail("You are not authorized to this operation on this board.");

            return AuthorizationResult.Succeed();
        }
    }
}