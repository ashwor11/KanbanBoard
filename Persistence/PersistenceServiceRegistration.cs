﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;

namespace Persistence;

public static class PersistenceServiceRegistration 
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<KanbanDbContext>(cfg => cfg.UseSqlServer(configuration.GetConnectionString("KanbanDb")));



        //Repositories Dependency Injections


        return services;
    }
}