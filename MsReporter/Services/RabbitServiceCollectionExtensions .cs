using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MsReporter.Services
{
    public static class RabbitServiceCollectionExtensions
    {

        public static IServiceCollection AddRabbit(this IServiceCollection services, IConfiguration configuration)
        {
         //   var rabbitConfig = configuration.GetSection("rabbit");
          //  services.Configure<RabbitOptions>(rabbitConfig);

        //    services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
         //   services.AddSingleton<IPooledObjectPolicy<IModel>, RabbitModelPooledObjectPolicy>();

          //  services.AddSingleton<IRabbitManager, RabbitManager>();

            return services;
        }
    }
}
