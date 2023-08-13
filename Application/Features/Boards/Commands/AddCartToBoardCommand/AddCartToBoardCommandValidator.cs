using FluentValidation;

namespace Application.Features.Boards.Commands.AddCartToBoardCommand;

public class AddCartToBoardCommandValidator : AbstractValidator<AddCartToBoardCommand>
{
    public AddCartToBoardCommandValidator()
    {
        RuleFor(x=>x.CardToAddDto.BoardId).NotEmpty().NotNull();
    }
}