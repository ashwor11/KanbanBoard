using FluentValidation;

namespace Application.Features.Boards.Commands.ChangeJobDescriptionCommand;

public class ChangeJobDescriptionCommandValidator : AbstractValidator<ChangeJobDescriptionCommand>
{
    public ChangeJobDescriptionCommandValidator()
    {
        RuleFor(x => x.ChangeJobDescriptionDto.BoardId).NotNull().NotEmpty();
    }
}