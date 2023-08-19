using Core.CrossCuttingConcerns.Exceptions.Handlers;
using Microsoft.AspNetCore.Builder;

namespace Core.CrossCuttingConcerns.Exceptions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<HttpExceptionHandler>();
    }
}