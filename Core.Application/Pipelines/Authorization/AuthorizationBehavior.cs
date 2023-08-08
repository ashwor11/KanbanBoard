using Core.CrossCuttingConcerns.Exceptions;
using Core.Security.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Core.Application.Pipelines.Authorization
{
    public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>, IAuthorizableRequest
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public AuthorizationBehavior(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            List<string>? userRoles = _contextAccessor.HttpContext.User.GetRoles();

            if (!request.RequiredRoles.Any(requiredRole => userRoles.Contains(requiredRole)))
                throw new AuthorizationException("You are not authorized for this operation.");
            TResponse response = await next();
            return response;
        }
    }
}
