using Application.Features.Boards.Requirements;
using MediatR.Behaviors.Authorization;

namespace Application.Features.Boards.Commands.ChangeCardNameCommand;

public class ChangeCardNameCommandAuthorizer : AbstractRequestAuthorizer<ChangeCardNameCommand>
{
    public override void BuildPolicy(ChangeCardNameCommand request)
    {
        UseRequirement(new UserMustBeCreatedBoardRequirement()
        {
            BoardId = request.ChangeCardNameDto.BoardId,
            PersonId = request.PersonId
        });
    }
}