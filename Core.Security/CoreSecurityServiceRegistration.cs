using Core.Security.JWT;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security
{
    public static class CoreSecurityServiceRegistration
    {
        public static IServiceCollection AddCoreSecurityService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TokenOptions>(options => { configuration.GetSection("TokenOptions"); });
            services.Configure<TokenValidationParameters>(options => { configuration.GetSection("TokenValidationParameters"); });
            return services;
        }
    }
}
