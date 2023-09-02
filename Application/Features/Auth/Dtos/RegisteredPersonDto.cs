using Core.Security.JWT;

namespace Application.Features.Auth.Dtos;

public class RegisteredPersonDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public AccessToken AccessToken { get; set; }
    public string RefreshToken { get; set;}
}