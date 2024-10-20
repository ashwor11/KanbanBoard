using FluentValidation;

namespace Application.Features.Boards.Commands.AddCartToBoardCommand;

public class AddCardToBoardCommandValidator : AbstractValidator<AddCardToBoardCommand>
{
    public AddCardToBoardCommandValidator()
    {
        RuleFor(x=>x.CardToAddDto.BoardId).NotEmpty().NotNull();
    }
}