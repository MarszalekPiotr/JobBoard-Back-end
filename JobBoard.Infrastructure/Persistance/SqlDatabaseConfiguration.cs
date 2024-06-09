using EFCoreSecondLevelCacheInterceptor;
using JobBoard.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Infrastructure.Persistance
{
    public static class SqlDatabaseConfiguration
    {
        public static IServiceCollection AddSqlDatabase(this IServiceCollection services, string connectionString)
        {
            Action<IServiceProvider, DbContextOptionsBuilder> sqlOptions = (serviceProvider, options) => options
           .UseSqlServer(connectionString,
               o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery))
           .AddInterceptors(serviceProvider.GetRequiredService<SecondLevelCacheInterceptor>())
           .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information)
           .EnableSensitiveDataLogging(); // Log parameter values as well

            services.AddDbContext<IApplicationDbContext, MainDbContext>(sqlOptions);

            return services;
        }
    }
}
