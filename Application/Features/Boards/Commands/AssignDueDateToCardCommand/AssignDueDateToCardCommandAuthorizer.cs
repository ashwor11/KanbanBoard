using Application.Features.Boards.Requirements;
using MediatR.Behaviors.Authorization;

namespace Application.Features.Boards.Commands.AssignDueDateToCardCommand;

public class AssignDueDateToCardCommandAuthorizer : AbstractRequestAuthorizer<AssignDueDateToCardCommand>
{
    public override void BuildPolicy(AssignDueDateToCardCommand request)
    {
        UseRequirement(new UserMustBeCreatedBoardRequirement()
        {
            BoardId = request.AssignDueDateToCardDto.BoardId,
            PersonId = request.PersonId
        });
    }
}