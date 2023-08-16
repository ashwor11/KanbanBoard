using FluentValidation;

namespace Application.Features.Boards.Commands.DeleteCardFeedbackCommand;

public class DeleteCardFeedbackCommandValidator : AbstractValidator<DeleteCardFeedbackCommand>
{
    public DeleteCardFeedbackCommandValidator()
    {
        RuleFor(x => x.DeleteCardFeedbackDto.BoardId).NotEmpty().NotNull();
        RuleFor(x => x.DeleteCardFeedbackDto.CardId).NotEmpty().NotNull();
        RuleFor(x => x.DeleteCardFeedbackDto.CardFeedbackId).NotEmpty().NotNull();

    }
}