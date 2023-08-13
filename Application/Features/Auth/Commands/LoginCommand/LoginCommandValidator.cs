using FluentValidation;

namespace Application.Features.Auth.Commands.LoginCommand;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.PersonToLoginDto.Email).NotEmpty().NotNull().EmailAddress();
        RuleFor(x => x.PersonToLoginDto.Password).MinimumLength(6);
    }
}