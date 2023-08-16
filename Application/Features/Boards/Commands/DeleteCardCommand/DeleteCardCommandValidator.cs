using FluentValidation;

namespace Application.Features.Boards.Commands.DeleteCardCommand;

public class DeleteCardCommandValidator : AbstractValidator<DeleteCardCommand>
{
    public DeleteCardCommandValidator()
    {
        RuleFor(x => x.DeleteCardDto.BoardId).NotEmpty().NotNull();
        RuleFor(x => x.DeleteCardDto.CardId).NotEmpty().NotNull();

    }
}