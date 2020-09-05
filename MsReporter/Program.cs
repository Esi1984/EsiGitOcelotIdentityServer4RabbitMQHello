using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MsReporter.EnvetBus;

namespace MsReporter
{
    public class Program
    {
        public static void Main(string[] args)
        {
          //  RabbitManager rm = new RabbitManager();
          //  rm.Listener();
            CreateHostBuilder(args).Build().Run();

          //  RabbitManager dd = new RabbitManager();
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        
    }
}
