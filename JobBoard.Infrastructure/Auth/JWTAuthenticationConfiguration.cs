using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Infrastructure.Auth
{
    public static class JWTAuthenticationConfiguration
    {
        public static IServiceCollection AddJwtAuth(this IServiceCollection services)
        {
            services.AddSingleton<JWTManager>();
            return services;
        }
    }
}
