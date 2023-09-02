using FluentValidation;

namespace Application.Features.Auth.Commands.RefreshCommand;

public class RefreshCommandValidator : AbstractValidator<RefreshCommand>
{
    public RefreshCommandValidator()
    {
        RuleFor(x=>x.RefreshTokenDto.Token).NotNull().NotEmpty().WithMessage("Token is required");
        RuleFor(x=>x.RefreshTokenDto.RefreshToken).NotNull().NotEmpty().WithMessage("Refresh token is required");
        
    }
}