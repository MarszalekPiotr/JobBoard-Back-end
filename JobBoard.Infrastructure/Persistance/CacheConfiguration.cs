using EFCoreSecondLevelCacheInterceptor;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Infrastructure.Persistance
{
    public static class CacheConfiguration
    {
        public static IServiceCollection AddCache(this IServiceCollection services)
        {
            services.AddEFSecondLevelCache(options =>
            options.UseMemoryCacheProvider(CacheExpirationMode.Absolute, TimeSpan.FromMinutes(5)).UseCacheKeyPrefix("EF_")
            );

            return services;
        }
    }
}
