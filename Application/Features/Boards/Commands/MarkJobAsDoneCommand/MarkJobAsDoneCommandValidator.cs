using FluentValidation;

namespace Application.Features.Boards.Commands.MarkJobAsDone;

public class MarkJobAsDoneCommandValidator : AbstractValidator<MarkJobAsDoneCommand>
{
    public MarkJobAsDoneCommandValidator()
    {
        RuleFor(x => x.MarkJobDto.JobId).NotNull().NotEmpty();
        RuleFor(x => x.MarkJobDto.BoardId).NotNull().NotEmpty();

    }
}