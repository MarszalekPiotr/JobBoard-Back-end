using JobBoard.WebApi.Middlewares;

namespace JobBoard.WebApi.DI
{
    public static  class ExceptionResultMiddlewareExtension
    {
        public static IApplicationBuilder UseExceptionResultMiddleware(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionResultMiddleware>();
    }
    }
}
