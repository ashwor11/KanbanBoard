using FluentValidation;

namespace Application.Features.Boards.Commands.AddJobToCardCommand;

public class AddJobToCardCommandValidator : AbstractValidator<AddJobToCardCommand>
{
    public AddJobToCardCommandValidator()
    {
        RuleFor(x => x.AddJobToCardDto.BoardId).NotEmpty().NotNull();
        RuleFor(x => x.AddJobToCardDto.CardId).NotEmpty().NotNull();

    }
}