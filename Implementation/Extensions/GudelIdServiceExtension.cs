using GudelIdService.Implementation.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GudelIdService.Implementation.Extensions
{
    public static class GudelIdServiceExtension
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, Action<DbContextOptionsBuilder> configure)
        {
            services.AddDbContext<AppDbContext>(configure);
            return services;
        }
    }
}
