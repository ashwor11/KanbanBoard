using FluentValidation;

namespace Application.Features.Auth.Commands.RegisterCommand;

public class RegisterPersonCommandValidator : AbstractValidator<RegisterPersonCommand>
{
    public RegisterPersonCommandValidator()
    {
        RuleFor(x => x.PersonToRegisterDto.Email).NotEmpty().NotNull().EmailAddress();
        RuleFor(x => x.PersonToRegisterDto.Password).NotEmpty().NotNull();
        RuleFor(x => x.PersonToRegisterDto.ConfirmPassword).NotEmpty().NotNull().MinimumLength(6);
        RuleFor(x => x.PersonToRegisterDto).NotEmpty().NotNull();
        RuleFor(x => x.PersonToRegisterDto.ConfirmPassword).Equal(x => x.PersonToRegisterDto.Password).WithMessage("Passwords should be same.");
        RuleFor(x => x.PersonToRegisterDto.FirstName).NotEmpty().NotNull();
        RuleFor(x => x.PersonToRegisterDto.LastName).NotEmpty().NotNull();




    }
}