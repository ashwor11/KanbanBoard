using FluentValidation;

namespace Application.Features.Boards.Commands.ChangeStatusOfCardCommand;

public class ChangeStatusOfCardCommandValidator : AbstractValidator<ChangeStatusOfCardCommand>
{
    public ChangeStatusOfCardCommandValidator()
    {
        RuleFor(x=>x.ChangeCardStatusDto.BoardId).NotEmpty().NotNull();
        RuleFor(x => x.ChangeCardStatusDto.CardId).NotEmpty().NotNull();
        RuleFor(x => x.ChangeCardStatusDto.Status).NotEmpty().NotNull();

    }
}