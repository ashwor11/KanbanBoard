using FluentValidation;

namespace Application.Features.Boards.Commands.DeleteJobCommand;

public class DeleteJobCommandValidator : AbstractValidator<DeleteJobCommand>
{
    public DeleteJobCommandValidator()
    {
        RuleFor(x => x.DeleteJobDto.BoardId).NotEmpty().NotNull();
        RuleFor(x => x.DeleteJobDto.CardId).NotEmpty().NotNull();
        RuleFor(x => x.DeleteJobDto.JobId).NotEmpty().NotNull();

    }
}