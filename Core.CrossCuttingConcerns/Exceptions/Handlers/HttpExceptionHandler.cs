using Core.CrossCuttingConcerns.Exceptions.HttpExceptions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR.Behaviors.Authorization.Exceptions;

namespace Core.CrossCuttingConcerns.Exceptions.Handlers
{
    public class HttpExceptionHandler : ExceptionHandler
    {
        
        private readonly RequestDelegate _next;

        public HttpExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                await HandleExceptionAsync(context,ex);
            }
        }

        protected override Task HandleException(HttpContext context, BusinessException businessException)
        {
            string details = new BusinessProblemDetails(businessException.Message).AsJson();
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return context.Response.WriteAsync(details);


        }

        protected override Task HandleException(HttpContext context, ValidationException validationException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            string details = new HttpExceptions.ValidationProblemDetails(validationException.Message, validationException.Errors).AsJson();
            return context.Response.WriteAsync(details);
        }

        protected override Task HandleException(HttpContext context, AuthorizationException authorizationException)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            string details = new AuthorizationProblemDetails(authorizationException.Message).AsJson();
            return context.Response.WriteAsync(details);
        }

        protected override Task HandleException(HttpContext context, NotFoundException notFoundException)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            string details = new NotFoundProblemDetails(notFoundException.Message).AsJson();
            return context.Response.WriteAsync(details);
        }

        protected override Task HandleException(HttpContext context, UnauthorizedException unauthorizedException)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            string details = new AuthorizationProblemDetails(unauthorizedException.Message).AsJson();
            return context.Response.WriteAsync(details);
        }

        protected override Task HandleException(HttpContext context, Exception exception)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            string details = new InternalServerErrorProblemDetails(exception.Message).AsJson();
            return context.Response.WriteAsync(details);
        }
    }
}
