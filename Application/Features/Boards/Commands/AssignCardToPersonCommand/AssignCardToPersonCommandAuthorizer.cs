using Application.Features.Boards.Requirements;
using MediatR.Behaviors.Authorization;

namespace Application.Features.Boards.Commands.AssignCardToPersonCommand;

public class AssignCardToPersonCommandAuthorizer : AbstractRequestAuthorizer<AssignCardToPersonCommand>
{
    public override void BuildPolicy(AssignCardToPersonCommand request)
    {
        UseRequirement(new UserMustBeCreatedBoardRequirement()
        {
            BoardId = request.AssignCardToPersonDto.BoardId,
            PersonId = request.PersonId
        });
    }
}