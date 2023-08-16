using Application.Features.Boards.Requirements;
using MediatR.Behaviors.Authorization;

namespace Application.Features.Boards.Commands.RemoveAssignedPersonFromCardCommand;

public class RemoveAssignedPersonFromCardAuthorizer : AbstractRequestAuthorizer<RemoveAssignedPersonFromCardCommand>
{
    public override void BuildPolicy(RemoveAssignedPersonFromCardCommand request)
    {
        UseRequirement(new UserMustBeCreatedBoardRequirement()
        {
            BoardId = request.RemoveAssignedPersonFromCardDto.BoardId,
            PersonId = request.PersonId
        });
    }
}