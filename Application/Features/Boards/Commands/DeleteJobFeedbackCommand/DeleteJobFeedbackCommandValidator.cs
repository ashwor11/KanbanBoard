using FluentValidation;

namespace Application.Features.Boards.Commands.DeleteJobFeedbackCommand;

public class DeleteJobFeedbackCommandValidator : AbstractValidator<DeleteJobFeedbackCommand>
{
    public DeleteJobFeedbackCommandValidator()
    {
        RuleFor(x => x.DeleteJobFeedbackDto.BoardId).NotEmpty().NotNull();
        RuleFor(x => x.DeleteJobFeedbackDto.CardId).NotEmpty().NotNull();
        RuleFor(x => x.DeleteJobFeedbackDto.JobId).NotEmpty().NotNull();
        RuleFor(x => x.DeleteJobFeedbackDto.JobFeedbackId).NotEmpty().NotNull();

    }
}