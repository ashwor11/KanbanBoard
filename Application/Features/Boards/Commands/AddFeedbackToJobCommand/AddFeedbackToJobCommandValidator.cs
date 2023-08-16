using FluentValidation;

namespace Application.Features.Boards.Commands.AddFeedbackToJobCommand;

public class AddFeedbackToJobCommandValidator : AbstractValidator<AddFeedbackToJobCommand>
{
    public AddFeedbackToJobCommandValidator()
    {
        RuleFor(x => x.JobFeedbackToAddDto.BoardId).NotEmpty().NotNull();
        RuleFor(x => x.JobFeedbackToAddDto.CardId).NotNull().NotEmpty();
        RuleFor(x => x.JobFeedbackToAddDto.JobId).NotNull().NotEmpty();
    }   
}