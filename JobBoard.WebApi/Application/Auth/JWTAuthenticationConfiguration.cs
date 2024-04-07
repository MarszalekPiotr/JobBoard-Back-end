using JobBoard.Application.Interfaces;

namespace JobBoard.WebApi.Application.Auth
{
    public static class JWTAuthenticationConfiguration
    {
        public static IServiceCollection AddJWTAuthenticationDataProvider(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CookieSettings>(configuration.GetSection("CookieSettings"));
            return services.AddScoped<IAuthenticationDataProvider, JWTAuthenticationDataProvider>();
        }
    }
}
