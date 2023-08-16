using FluentValidation;

namespace Application.Features.Boards.Commands.ChangeCardNameCommand;

public class ChangeCardNameCommandValidator : AbstractValidator<ChangeCardNameCommand>
{
    public ChangeCardNameCommandValidator()
    {
        RuleFor(x => x.ChangeCardNameDto.BoardId).NotEmpty().NotNull();
        RuleFor(x => x.ChangeCardNameDto.CardId).NotEmpty().NotNull();
        RuleFor(x => x.ChangeCardNameDto.Name).NotNull();


    }
}