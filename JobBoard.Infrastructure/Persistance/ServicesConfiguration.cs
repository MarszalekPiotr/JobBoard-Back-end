
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Infrastructure.Persistance
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddServicesInfrastructure(this IServiceCollection services)
        {
            return services;
        }
    }
}
