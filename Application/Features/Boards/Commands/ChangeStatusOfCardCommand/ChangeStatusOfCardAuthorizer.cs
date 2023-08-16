using Application.Features.Boards.Requirements;
using MediatR.Behaviors.Authorization;

namespace Application.Features.Boards.Commands.ChangeStatusOfCardCommand;

public class ChangeStatusOfCardAuthorizer : AbstractRequestAuthorizer<ChangeStatusOfCardCommand>
{
    public override void BuildPolicy(ChangeStatusOfCardCommand request)
    {
        UseRequirement(new PersonMustBeCreatedBoardOrAssignedToCardRequirement()
        {
            BoardId = request.ChangeCardStatusDto.BoardId,
            PersonId = request.PersonId,
            CardId = request.ChangeCardStatusDto.CardId
        });
    }
}