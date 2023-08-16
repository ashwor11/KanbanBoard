using FluentValidation;

namespace Application.Features.Boards.Commands.AssignCardToPersonCommand;

public class AssignCardToPersonCommandValidator : AbstractValidator<AssignCardToPersonCommand>
{
    public AssignCardToPersonCommandValidator()
    {
        RuleFor(x => x.AssignCardToPersonDto.BoardId).NotEmpty().NotNull();
        RuleFor(x => x.AssignCardToPersonDto.CardId).NotEmpty().NotNull();

        RuleFor(x => x.AssignCardToPersonDto.PersonId).NotEmpty().NotNull();

    }
}