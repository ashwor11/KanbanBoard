using Application.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using Persistence.Repositories;

namespace Persistence;

public static class PersistenceServiceRegistration 
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<KanbanDbContext>(cfg => cfg.UseSqlServer(configuration.GetConnectionString("KanbanDb")));



        services.AddScoped<IBoardRepository, BoardRepository>();
        services.AddScoped<IPersonBoardRepository, PersonBoardRepository>();
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IUserOperationClaimRepository,UserOperationClaimRepository>();


        return services;
    }
}