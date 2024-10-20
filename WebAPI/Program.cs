using Application;
using Core.CrossCuttingConcerns;
using Core.CrossCuttingConcerns.Exceptions;
using Core.Security;
using Core.Security.Encryption.Helpers;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence;
using System.Reflection;
using System.Text.Json;
using Infrastructure;
using Persistence.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.AllowEmptyInputInBodyModelBinding = true;
});




builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddCoreCCSServiceRegistration(builder.Configuration);
builder.Services.AddCoreSecurityService(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>options.AddPolicy("CorsPolicy",
    builder => builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()));



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters()
    {
        ClockSkew = TimeSpan.Zero,
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetSection("TokenOptions")["Issuer"],
        ValidAudience = builder.Configuration.GetSection("TokenOptions")["Audience"],
        IssuerSigningKey =
            SecurityKeyHelper.CreateSecurityKey(builder.Configuration.GetSection("TokenOptions")["SecurityKey"])
    };
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "Kanban Board API", Version = "v1",
        Description = "A WebAPI for Kanban Board",
        

        
    });



    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Scheme = "bearer",
        Description = "Enter jwt token"

    });


    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });

    //creating xml documentation file

    string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string xmlPath = $"{Path.Combine(AppContext.BaseDirectory, xmlFile)}";

    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddSignalR();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();




var app = builder.Build();

//For docker container, create the database if it isn't created before
var serviceScope = app.Services.CreateScope();
var context = serviceScope.ServiceProvider.GetRequiredService<KanbanDbContext>();
context.Database.EnsureCreated();


app.UseSwagger();
    app.UseSwaggerUI();

app.ConfigureCustomExceptionMiddleware();





app.UseRouting();
app.UseCors("CorsPolicy");


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseWhen(_ => _.Request.Path.StartsWithSegments("/api/board") & !_.Request.Path.StartsWithSegments("/api/board/subscribe"), appBuilder =>
{
    appBuilder.UseMiddleware<BoardUpdaterMiddleware>();
});



app.Run();
