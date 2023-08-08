using Core.CrossCuttingConcerns.Logging.SeriLog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns
{
    public static class CoreCCSServiceRegistration
    {
        public static IServiceCollection AddCoreCCSServiceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FileLogConfiguration>(options => { configuration.GetSection("FileLogConfiguration"); });
            return services;
        }
    }
}
