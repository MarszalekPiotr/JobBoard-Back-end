using Serilog;

namespace JobBoard.WebApi
{
    public class Program
    {
        public static string _APP_NAME = "JobBoard.WebApi";
        public static void Main(string[] args)
        {

            //Add loggs to console

            Log.Logger = new LoggerConfiguration()
                  .Enrich.WithProperty("Application", _APP_NAME)
                  .Enrich.WithProperty("MacineName", Environment.MachineName)
                  .Enrich.FromLogContext()
                  .WriteTo.Console()
                  .CreateBootstrapLogger();


            var builder = WebApplication.CreateBuilder(args);


            // Add SerLog configuration to DI container
            builder.Host.UseSerilog((context, services, configuration) => configuration
            .Enrich.WithProperty("Application", _APP_NAME)
            .Enrich.WithProperty("MacineName", Environment.MachineName)
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            );


            // Add services to the container.

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}