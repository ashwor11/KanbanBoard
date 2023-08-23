using FluentValidation;

namespace Application.Features.Boards.Commands.MarkJobAsUnDoneCommand;

public class MarkJobAsUnDoneCommandValidator : AbstractValidator<MarkJobAsUnDoneCommand>
{
    public MarkJobAsUnDoneCommandValidator()
    {
        RuleFor(x => x.MarkJobDto.BoardId).NotEmpty().NotNull();
        RuleFor(x=>x.MarkJobDto.JobId).NotEmpty().NotNull();
    }
}