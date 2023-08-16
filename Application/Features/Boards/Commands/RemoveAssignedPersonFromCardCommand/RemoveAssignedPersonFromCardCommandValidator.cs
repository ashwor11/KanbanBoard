using FluentValidation;

namespace Application.Features.Boards.Commands.RemoveAssignedPersonFromCardCommand;

public class RemoveAssignedPersonFromCardCommandValidator : AbstractValidator<RemoveAssignedPersonFromCardCommand>
{
    public RemoveAssignedPersonFromCardCommandValidator()
    {
        RuleFor(x => x.RemoveAssignedPersonFromCardDto.BoardId).NotEmpty().NotNull();
        RuleFor(x => x.RemoveAssignedPersonFromCardDto.CardId).NotEmpty().NotNull();
    }
}