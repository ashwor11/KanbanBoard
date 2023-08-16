using FluentValidation;

namespace Application.Features.Boards.Commands.ChangeJobFeedbackCommand;

public class ChangeJobFeedbackCommandValidator : AbstractValidator<ChangeJobFeedbackCommand>
{
    public ChangeJobFeedbackCommandValidator()
    {
        RuleFor(x => x.ChangeJobFeedbackDto.BoardId).NotEmpty().NotNull();
        RuleFor(x => x.ChangeJobFeedbackDto.CardId).NotNull().NotEmpty();
        RuleFor(x => x.ChangeJobFeedbackDto.JobId).NotNull().NotEmpty();
        RuleFor(x => x.ChangeJobFeedbackDto.JobFeedbackId).NotNull().NotEmpty();

    }
}