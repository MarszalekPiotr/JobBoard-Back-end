﻿using JobBoard.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
            Action<IServiceProvider, DbContextOptionsBuilder> sqlOptions = (serviceProvider, options) => options.UseSqlServer(connectionString,
                o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery));

            services.AddDbContext<IApplicationDbContext, MainDbContext>(sqlOptions);

            return services;
        }
    }
}
