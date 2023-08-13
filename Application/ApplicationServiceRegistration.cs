using System.Reflection;
using Application.Features.Auth.Rules;
using Application.Features.Boards.Rules;
using Application.Services.Abstract;
using Application.Services.Concrete;
using Core.Application.Pipelines.Validation;
using Core.Security.JWT;
using Core.Security.Mailing;
using Core.Security.Mailing.Mailkit;
using FluentValidation;
using MediatR;
using MediatR.Behaviors.Authorization.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {

        services.AddMediatR(c => c.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(Assembly.GetExecutingAssembly());



        #region Decorater

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        // Policy Authorization via MediatR.Behaviors.Authorization
        services.AddMediatorAuthorization(Assembly.GetExecutingAssembly());
        services.AddAuthorizersFromAssembly(Assembly.GetExecutingAssembly());


        #endregion




        #region Helpers

        services.AddScoped<ITokenHelper, JwtHelper>();

        #endregion


        #region Rules

        services.AddScoped<AuthBusinessRules>();
        services.AddScoped<BoardBusinessRules>();

        #endregion
        

        #region Services


        services.AddScoped<IMailService, MailkitService>();

        services.AddScoped<IPersonService, PersonService>();
        services.AddScoped<IBoardService, BoardService>();
        #endregion

        return services;
    }
}