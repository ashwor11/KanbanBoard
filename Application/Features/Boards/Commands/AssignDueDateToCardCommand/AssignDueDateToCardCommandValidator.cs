using FluentValidation;

namespace Application.Features.Boards.Commands.AssignDueDateToCardCommand;

public class AssignDueDateToCardCommandValidator : AbstractValidator<AssignDueDateToCardCommand>
{
    public AssignDueDateToCardCommandValidator()
    {
        RuleFor(x=>x.AssignDueDateToCardDto.BoardId).NotEmpty().NotNull();
        RuleFor(x=>x.AssignDueDateToCardDto.CardId).NotEmpty().NotNull();
        RuleFor(x=>x.AssignDueDateToCardDto.DueDate).NotEmpty().NotNull().GreaterThanOrEqualTo(DateTime.Now).WithMessage("Date can't older than today.");
    }
}