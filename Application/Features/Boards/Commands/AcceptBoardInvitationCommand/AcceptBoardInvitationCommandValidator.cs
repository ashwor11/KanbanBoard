using FluentValidation;

namespace Application.Features.Boards.Commands.AcceptBoardInvitationCommand;

public class AcceptBoardInvitationCommandValidator : AbstractValidator<AcceptBoardInvitationCommand>
{
    public AcceptBoardInvitationCommandValidator()
    {
        RuleFor(x => x.InvitationToken).NotEmpty().NotNull();
    }
}