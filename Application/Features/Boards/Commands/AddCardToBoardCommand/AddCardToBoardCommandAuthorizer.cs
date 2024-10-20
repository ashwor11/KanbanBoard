using Application.Features.Boards.Requirements;
using MediatR.Behaviors.Authorization;

namespace Application.Features.Boards.Commands.AddCartToBoardCommand;

public class AddCardToBoardCommandAuthorizer : AbstractRequestAuthorizer<AddCardToBoardCommand>
{
    public override void BuildPolicy(AddCardToBoardCommand request)
    {
        UseRequirement(new PersonMustBeAMemberOfBoardRequirement()
        {
            BoardId = request.CardToAddDto.BoardId, PersonId = request.PersonId
        });
    }
}