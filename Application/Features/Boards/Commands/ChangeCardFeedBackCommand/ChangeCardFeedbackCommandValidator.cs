using FluentValidation;

namespace Application.Features.Boards.Commands.ChangeCardFeedBackCommand;

public class ChangeCardFeedbackCommandValidator : AbstractValidator<ChangeCardFeedbackCommand>
{
    public ChangeCardFeedbackCommandValidator()
    {
        RuleFor(x => x.ChangeCardFeedbackDto.BoardId).NotEmpty().NotNull();
        RuleFor(x => x.ChangeCardFeedbackDto.CardId).NotNull().NotEmpty();
        RuleFor(x => x.ChangeCardFeedbackDto.CardFeedbackId).NotNull().NotEmpty();

    }

}