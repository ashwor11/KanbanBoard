using Application.Features.Auth.Commands;
using Application.Features.Auth.Commands.LoginCommand;
using Application.Features.Auth.Commands.RefreshCommand;
using Application.Features.Auth.Commands.RegisterCommand;
using Application.Features.Auth.Dtos;
using Core.Security.JWT;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] PersonToRegisterDto personToRegisterDto)
        {
            string ipAddress = GetIpAddress();
            RegisterPersonCommand command = new RegisterPersonCommand() { PersonToRegisterDto = personToRegisterDto, IpAddress = ipAddress};
            RegisteredPersonDto registeredPersonDto = await Mediator.Send(command);
            return Ok(registeredPersonDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] PersonToLoginDto personToLoginDto)
        {
            string ipAddress = GetIpAddress();
            LoginCommand loginCommmand = new LoginCommand() { PersonToLoginDto = personToLoginDto, IpAddress =ipAddress };
            LoggedInPersonDto loggedInPersonDto = await Mediator.Send(loginCommmand);
            return Ok(loggedInPersonDto);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto refreshTokenDto)
        {
            string ipAddress = GetIpAddress();
            RefreshCommand refreshTokenCommand = new RefreshCommand() { RefreshTokenDto = refreshTokenDto,IpAddress = ipAddress};
            RefreshedTokenDto refreshedTokenDto = await Mediator.Send(refreshTokenCommand);
            return Ok(refreshedTokenDto);
        }
    }
}
