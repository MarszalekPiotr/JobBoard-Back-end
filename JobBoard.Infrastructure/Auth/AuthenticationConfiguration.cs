using JobBoard.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Infrastructure.Auth
{
    public static class AuthenticationConfiguration
    {
        public static IServiceCollection AddJwtAuth(this IServiceCollection services)
        {
          
            services.AddSingleton<JWTManager>();
            return services;
        }

        public static IServiceCollection AddPasswordManager(this IServiceCollection services)
        {

            services.AddScoped<IPasswordManager, PasswordManager>();
            services.AddScoped(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));
            return services;
        }
      
    }
}
