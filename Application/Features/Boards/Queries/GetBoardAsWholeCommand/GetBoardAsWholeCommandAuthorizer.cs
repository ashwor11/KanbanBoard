using Application.Features.Boards.Requirements;
using MediatR.Behaviors.Authorization;

namespace Application.Features.Boards.Queries.GetBoardAsWholeCommand;

public class GetBoardAsWholeCommandAuthorizer : AbstractRequestAuthorizer<GetBoardAsWholeQuery>
{
    public override void BuildPolicy(GetBoardAsWholeQuery request)
    {
        UseRequirement(new PersonMustBeAMemberOfBoardRequirement()
        {
            BoardId = request.BoardId,
            PersonId = request.PersonId
        });
    }
}