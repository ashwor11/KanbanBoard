using Application.Features.Boards.Requirements;
using MediatR.Behaviors.Authorization;

namespace Application.Features.Boards.Commands.DeleteBoardCommand;

public class DeleteBoardCommandAuthorizer : AbstractRequestAuthorizer<DeleteBoardCommand>
{
    public override void BuildPolicy(DeleteBoardCommand request)
    {
        UseRequirement(new UserMustBeCreatedBoardRequirement(){BoardId = request.BoardId, PersonId = request.PersonId});
    }
}