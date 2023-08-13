using Core.Security.JWT;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Security.Encryption.Helpers;
using Core.Security.Mailing;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Core.Security
{
    public static class CoreSecurityServiceRegistration
    {
        public static IServiceCollection AddCoreSecurityService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TokenOptions>(options => { configuration.GetSection("TokenOptions").Bind(options); });
            services.Configure<MailSettings>(options => { configuration.GetSection("MailSettings").Bind(options); });

            services.Configure<JwtBearerOptions>(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration.GetSection("TokenOptions")["Issuer"],
                    ValidAudience = configuration.GetSection("TokenOptions")["Audience"],
                    IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(configuration.GetSection("TokenOptions")["SecurityKey"])
                };
            });
            return services;
        }
    }
}
