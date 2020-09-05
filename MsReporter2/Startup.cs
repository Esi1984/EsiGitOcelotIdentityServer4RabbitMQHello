using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using EsiRabbitMQ;
using MsReporter3.Models;
using Microsoft.EntityFrameworkCore;
using MsReporter3.EsiEventBus;

namespace MsReporter3
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
          //  DbContextOptions dbo = new DbContextOptions();
        //    dbo.UseSqlServer(Configuration.GetConnectionString("EsiConnectionString"));

        //    ReportDBContext rbd = new ReportDBContext(dbo);

            services.AddDbContext<ReportDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("EsiConnectionString")));

            //services.addte<EsiEventManager>();
            services.AddSingleton<RabbitConnect>(sp =>
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

                var myEE =  new RabbitConnect(factory,Configuration["EsiExname"],Configuration["EsiQname"]);
                //          var myem = new EsiEventManager(DbContextOptions< ReportDBContext>(options =>
                //options.UseSqlServer(Configuration.GetConnectionString("EsiConnectionString")));
                DbContextOptionsBuilder<ReportDBContext> yy = new DbContextOptionsBuilder<ReportDBContext>();
                var tt = yy.UseSqlServer(Configuration.GetConnectionString("EsiConnectionString"));
                ReportDBContext db = new ReportDBContext(tt.Options);
                var dd = new EsiEventManager( db);
                myEE.EsiEventReceived += dd.MyEventBut_EsiEventReceived;
                return myEE;
            });
            services.AddOptions();


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

            app.UseEsiRabbit();
        }
    }


    public static class ApplicationBuilderExtentions
    {
        public static RabbitConnect Listener { get; set; }

        public static IApplicationBuilder UseEsiRabbit(this IApplicationBuilder app)
        {
            Listener = app.ApplicationServices.GetService<RabbitConnect>();
            var life = app.ApplicationServices.GetService<Microsoft.Extensions.Hosting.IHostApplicationLifetime>();
            life.ApplicationStarted.Register(OnStarted);

            //press Ctrl+C to reproduce if your app runs in Kestrel as a console app
            life.ApplicationStopping.Register(OnStopping);
            return app;
        }

        private static void OnStarted()
        {
            Listener.CreateChannel();
        }

        private static void OnStopping()
        {
            Listener.Disconnect();
        }
    }
}
