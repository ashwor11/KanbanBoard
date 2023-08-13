using System.Security.Authentication;
using Core.CrossCuttingConcerns.Exceptions;
using Core.Security.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Core.Application.Pipelines.Authorization
{
    public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>, ISecuredRequest
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public AuthorizationBehavior(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            
            List<string>? userRoles = GetUserRoles();
            if (!CheckIfUserLoggedInWithRoles(userRoles))
                throw new AuthenticationException("You must be logged in for this operation");

            if (!DoesUserHaveAnyRequiredRoles(request.RequiredRoles,userRoles))
                throw new AuthorizationException("You are not authorized for this operation.");

            TResponse response = await next();
            return response;
        }

        private List<string>? GetUserRoles()
        {
            return _contextAccessor.HttpContext.User.GetRoles();
        }

        private bool CheckIfUserLoggedInWithRoles(List<string> roles)
        {
            if(roles.Count == 0)
                return false;

            return true;
        }

        private bool DoesUserHaveAnyRequiredRoles(string[] requiredRoles, List<string> userRoles)
        {
            if (requiredRoles.Any(requiredRole => userRoles.Contains(requiredRole)))
                return true;

            return false;


        }
    }
}
