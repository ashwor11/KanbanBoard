using System.Security.Authentication;
using System.Security.Claims;
using Core.CrossCuttingConcerns.Exceptions;
using Core.Security.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IMediator? Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
        private IMediator? _mediator;

        protected int GetPersonId()
        {
            IfUserIsNotAuthenticatedThrowException();

            string? idInString = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;
            return Convert.ToInt32(idInString);
        }

        private void IfUserIsNotAuthenticatedThrowException()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
                throw new AuthorizationException("You should be logged in for this request.");
        }

        protected string GetIpAddress()
        {
            return HttpContext.Connection.RemoteIpAddress!.ToString();
           
        }
    }
}
