using Application.Features.Boards.Rules;
using Application.Repositories;
using Domain.Entities.Concrete;
using MediatR.Behaviors.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Boards.Requirements;

public class PersonMustBeAMemberOfBoardRequirement : IAuthorizationRequirement
{
    public int BoardId { get; set; }
    public int PersonId { get; set; }

    public class
        PersonMustBeAMemberOfBoardRequirementHandler : IAuthorizationHandler<PersonMustBeAMemberOfBoardRequirement>
    {
        private readonly IBoardRepository _boardRepository;
        private readonly BoardBusinessRules _boardBusinessRules;

        public PersonMustBeAMemberOfBoardRequirementHandler(IBoardRepository boardRepository,
            BoardBusinessRules boardBusinessRules)
        {
            _boardRepository = boardRepository;
            _boardBusinessRules = boardBusinessRules;
        }

        public async Task<AuthorizationResult> Handle(PersonMustBeAMemberOfBoardRequirement requirement,
            CancellationToken cancellationToken = new CancellationToken())
        {
            Board board = await _boardRepository.GetAsync(x => x.Id == requirement.BoardId,
                include: x => x.Include(x => x.PersonBoards).ThenInclude(x => x.Person));
            _boardBusinessRules.DoesBoardExist(board);
            List<int> personIds = board.PersonBoards.Select(x => x.Person).Select(x => x.Id).ToList();

            if (personIds.Any(x => x == requirement.PersonId))
                return AuthorizationResult.Succeed();
            return AuthorizationResult.Fail("You should be the member of the board to do this operation");
        }
    }
}