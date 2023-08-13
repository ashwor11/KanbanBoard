using FluentValidation;

namespace Application.Features.Boards.Commands.CreateBoardCommand;

public class CreateBoardCommandValidator : AbstractValidator<CreateBoardCommand>
{
    public CreateBoardCommandValidator()
    {
        RuleFor(x => x.BoardToCreateDto.Name).NotEmpty().NotNull();
    }
}