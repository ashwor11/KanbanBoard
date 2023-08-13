using Application.Features.Boards.Requirements;
using MediatR.Behaviors.Authorization;

namespace Application.Features.Boards.Commands.AddCartToBoardCommand;

public class AddCartToBoardCommandAuthorizer : AbstractRequestAuthorizer<AddCartToBoardCommand>
{
    public override void BuildPolicy(AddCartToBoardCommand request)
    {
        UseRequirement(new UserMustBeCreatedBoardRequirement()
        {
            BoardId = request.CardToAddDto.BoardId, PersonId = request.PersonId
        });
    }
}