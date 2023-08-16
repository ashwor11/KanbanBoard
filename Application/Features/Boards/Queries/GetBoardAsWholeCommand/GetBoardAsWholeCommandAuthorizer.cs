using Application.Features.Boards.Requirements;
using MediatR.Behaviors.Authorization;

namespace Application.Features.Boards.Queries.GetBoardAsWholeCommand;

public class GetBoardAsWholeCommandAuthorizer : AbstractRequestAuthorizer<GetBoardAsWholeCommand>
{
    public override void BuildPolicy(GetBoardAsWholeCommand request)
    {
        UseRequirement(new PersonMustBeAMemberOfBoardRequirement()
        {
            BoardId = request.BoardId,
            PersonId = request.PersonId
        });
    }
}