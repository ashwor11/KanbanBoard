using FluentValidation;

namespace Application.Features.Boards.Commands.RemoveAssignedDueDateFromCardCommand;

public class RemoveAssignedDueDateFromCardCommandValidator : AbstractValidator<RemoveAssignedDueDateFromCardCommand>
{
    public RemoveAssignedDueDateFromCardCommandValidator()
    {
        RuleFor(x => x.RemoveAssignedDueDateFromCardDto.BoardId).NotEmpty().NotNull();
        RuleFor(x => x.RemoveAssignedDueDateFromCardDto.CardId).NotEmpty().NotNull();

    }
}