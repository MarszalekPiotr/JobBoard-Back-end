using JobBoard.Application.Logic.Abstractions;
using JobBoard.Infrastructure.Persistance;
using Serilog;
using JobBoard.WebApi.DI;
using JobBoard.Infrastructure.Auth;
using JobBoard.WebApi.Application.Auth;
using JobBoard.Application.DI;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;

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
            if (builder.Environment.IsDevelopment())
            {
                builder.Configuration.AddJsonFile("appsettings.Development.local.json");
            }



            // Add SerLog configuration to DI container
            builder.Host.UseSerilog((context, services, configuration) => configuration
            .Enrich.WithProperty("Application", _APP_NAME)
            .Enrich.WithProperty("MachineName", Environment.MachineName)
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            );

            builder.Services.AddServicesInfrastructure();

            builder.Services.AddHttpContextAccessor();
            // Add services to the container.
            builder.Services.AddCache();
            builder.Services.AddSqlDatabase(builder.Configuration.GetConnectionString("MainDbConnectionString")!);
            builder.Services.AddJWTAuthenticationDataProvider(builder.Configuration);
            builder.Services.AddCurrentAccountProvider();
            builder.Services.AddPasswordManager();
            builder.Services.AddControllersWithViews(options =>
            {
                if (!builder.Environment.IsDevelopment())
                {
                    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                }
            });

            builder.Services.AddControllers()
            .AddJsonOptions(options => {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                options.JsonSerializerOptions.Converters.Add(new FormDefinitionConverter());
            }          
             );
            
            
            

            builder.Services.AddMediatR(c =>
            {
                c.RegisterServicesFromAssemblyContaining(typeof(BaseCommandHandler));
            });
            builder.Services.AddSwaggerGen(o =>
            {
                o.CustomSchemaIds(x =>
                {
                    var name = x.FullName;
                    if (name != null)
                    {
                        name = name.Replace("+", "_"); // swagger bug fix
                    }

                    return name;
                });
            });

            builder.Services.AddAntiforgery(o => {
                o.HeaderName = "X-XSRF-TOKEN";
            });

            builder.Services.Configure<JWTAuthenticationOptions>(builder.Configuration.GetSection("JwtAuthentication"));
            builder.Services.AddJwtAuth();

            builder.Services.AddValidators();

            builder.Services.AddCors();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Configure the HTTP request pipeline.
            // Cors must be first middleware 
            app.UseCors(builder => builder
        .   WithOrigins(app.Configuration.GetValue<string>("WebAppBaseUrl") ?? "")
        .   WithOrigins(app.Configuration.GetSection("AdditionalCorsOrigins").Get<string[]>() ?? new string[0])
           .WithOrigins((Environment.GetEnvironmentVariable("AdditionalCorsOrigins") ?? "").Split(',').Where(h => !string.IsNullOrEmpty(h)).Select(h => h.Trim()).ToArray())
           .AllowAnyHeader()
           .AllowCredentials()
           .AllowAnyMethod());



            app.UseExceptionResultMiddleware();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}