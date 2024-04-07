using JobBoard.Application.Interfaces;
using JobBoard.Infrastructure.Auth;

namespace JobBoard.WebApi.Application.Auth
{
    public static class JWTAuthenticationConfiguration
    {
        public static IServiceCollection AddJWTAuthenticationDataProvider(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CookieSettings>(configuration.GetSection("CookieSettings"));
            return services.AddScoped<IAuthenticationDataProvider, JWTAuthenticationDataProvider>();
        }

        public static IServiceCollection AddCurrentAccountProvider(this IServiceCollection services)
        {
            return services.AddScoped<ICurrentAccountProvider, CurrentAccountProvider>();   
        }
    }
}
