using FluentValidation;

namespace Application.Features.Boards.Commands.DeleteBoardCommand;

public class DeleteBoardCommandValidator : AbstractValidator<DeleteBoardCommand>
{
    public DeleteBoardCommandValidator()
    {
        RuleFor(x => x.BoardId).NotEmpty().NotNull();
    }
}