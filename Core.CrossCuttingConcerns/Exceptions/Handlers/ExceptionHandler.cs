using FluentValidation;
using MediatR.Behaviors.Authorization.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Core.CrossCuttingConcerns.Exceptions.Handlers
{
    public abstract class ExceptionHandler
    {
        public Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (exception is BusinessException businessException)
                return HandleException(context, businessException);

            else if (exception is ValidationException validationException)
                return HandleException(context, validationException);

            else if (exception is AuthorizationException authorizationException)
                return HandleException(context, authorizationException);

            else if (exception is NotFoundException notFoundException)
                return HandleException(context, notFoundException);
            else if (exception is UnauthorizedException unauthorizedException)
                return HandleException(context, unauthorizedException);

            else
                return HandleException(context, exception);
        }

        protected abstract Task HandleException(HttpContext context, BusinessException businessException);
        protected abstract Task HandleException(HttpContext context, ValidationException validationException);
        protected abstract Task HandleException(HttpContext context, AuthorizationException authorizationException);
        protected abstract Task HandleException(HttpContext context, NotFoundException notFoundException);
        protected abstract Task HandleException(HttpContext context, UnauthorizedException unauthorizedException);
        protected abstract Task HandleException(HttpContext context, Exception exception);
    }
}
