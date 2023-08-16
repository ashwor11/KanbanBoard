using Application.Features.Boards.Requirements;
using MediatR.Behaviors.Authorization;

namespace Application.Features.Boards.Commands.RemoveAssignedDueDateFromCardCommand;

public class RemoveAssignedDueDateFromCardCommandAuthorizer : AbstractRequestAuthorizer<RemoveAssignedDueDateFromCardCommand>
{
    public override void BuildPolicy(RemoveAssignedDueDateFromCardCommand request)
    {
        UseRequirement(new UserMustBeCreatedBoardRequirement()
        {
            BoardId = request.RemoveAssignedDueDateFromCardDto.BoardId,
            PersonId = request.PersonId
        });
    }
}