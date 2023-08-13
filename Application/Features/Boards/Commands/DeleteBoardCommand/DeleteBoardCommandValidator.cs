using FluentValidation;

namespace Application.Features.Boards.Commands.DeleteBoardCommand;

public class DeleteBoardCommandValidator : AbstractValidator<DeleteBoardCommand>
{
    public DeleteBoardCommandValidator()
    {

    }
}