using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EsiRabbitMQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MS2.EsiEventBus;
using MS2.Model;
using RabbitMQ.Client;

namespace MS2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<Ms2DbContext>(options =>
      options.UseSqlServer(Configuration.GetConnectionString("EsiConnectionString")));


            services.AddSingleton<IRabbitConnect, RabbitConnect>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<RabbitConnect>>();

                var factory = new ConnectionFactory()
                {
                    HostName = Configuration["EsiIp"]
                };

                if (!string.IsNullOrEmpty(Configuration["EsiEventUserName"]))
                {
                    factory.UserName = Configuration["EsiEventUserName"];
                }

                if (!string.IsNullOrEmpty(Configuration["EsiEventPassword"]))
                {
                    factory.Password = Configuration["EsiEventPassword"];
                }
                // sp.EsiEventReceived += EsiEventManager.MyEventBut_EsiEventReceived;

                var myEE = new RabbitConnect(factory, Configuration["EsiExname"], Configuration["EsiQname"]);
                myEE.EsiEventReceived += EsiEventManager.MyEventBut_EsiEventReceived;
                return myEE;
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
