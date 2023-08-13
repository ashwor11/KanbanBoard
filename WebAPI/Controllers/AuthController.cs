using Application.Features.Auth.Commands;
using Application.Features.Auth.Commands.LoginCommand;
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
            RegisterPersonCommand command = new RegisterPersonCommand() { PersonToRegisterDto = personToRegisterDto };
            AccessToken accessToken = await Mediator.Send(command);
            return Ok(accessToken);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] PersonToLoginDto personToLoginDto)
        {
            LoginCommand loginCommmand = new LoginCommand() { PersonToLoginDto = personToLoginDto };
            AccessToken accessToken = await Mediator.Send(loginCommmand);
            return Ok(accessToken);
        }
    }
}
