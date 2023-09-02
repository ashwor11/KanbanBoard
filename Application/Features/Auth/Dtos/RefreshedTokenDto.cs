using Core.Security.JWT;

namespace Application.Features.Auth.Dtos;

public class RefreshedTokenDto
{
    public AccessToken AccessToken { get; set; }
    public string RefreshToken { get; set; }
}