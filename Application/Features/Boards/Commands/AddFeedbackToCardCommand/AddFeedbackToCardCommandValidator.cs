using FluentValidation;

namespace Application.Features.Boards.Commands.AddFeedbackToCardCommand;

public class AddFeedbackToCardCommandValidator : AbstractValidator<AddFeedbackToCardCommand>
{
    public AddFeedbackToCardCommandValidator()
    {
        RuleFor(x => x.CardFeedbackToAddDto.BoardId).NotEmpty().NotNull();
        RuleFor(x => x.CardFeedbackToAddDto.CardId).NotNull().NotEmpty();
    }
}