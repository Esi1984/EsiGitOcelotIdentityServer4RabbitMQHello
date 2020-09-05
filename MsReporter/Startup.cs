using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Examples.AdvancedConfiguration.MessageHandlers;
using Examples.AdvancedConfiguration.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
//using Microsoft.IdentityModel.Protocols.WSTrust;
using MsReporter.EnvetBus;
using MsReporter.Model;
using MsReporter.Services;
using RabbitMQ.Client.Core.DependencyInjection;
using Microsoft.Extensions.Hosting.Internal;
using EventBusRabbitMQ;
using EventBus.Abstractions;
using EventBus;
using RabbitMQ.Client;
using Autofac;
//using Microsoft.IdentityModel.Protocols.WSTrust;

namespace MsReporter
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
            //   services.AddSingleton<IRabbitMQService, RabbitMQService>(); 
            // Need a single instance so we can keep the referenced connect with RabbitMQ open



            //  services.AddSignalR();
            //       var rabbitMqConsumerSection = Configuration.GetSection("RabbitMqConsumer");
            //       var rabbitMqProducerSection = Configuration.GetSection("RabbitMqProducer");

            //       var producingExchangeSection = Configuration.GetSection("ProducingExchange");
            //       var consumingExchangeSection = Configuration.GetSection("ConsumingExchange");
            //       services.AddRabbitMqConsumingClientSingleton(rabbitMqConsumerSection)
            //.AddRabbitMqProducingClientSingleton(rabbitMqProducerSection)
            //.addra
            //.AddProductionExchange("logs", producingExchangeSection)
            //.AddConsumptionExchange("consumption.exchange", consumingExchangeSection)
            //.AddMessageHandlerTransient<CustomMessageHandler>("routing.key")
            //.AddAsyncMessageHandlerTransient<CustomAsyncMessageHandler>(new[] { "routing.key", "another.routing.key" })
            //.AddNonCyclicMessageHandlerSingleton<CustomNonCyclicMessageHandler>("*.*", "consumption.exchange")
            //.AddAsyncNonCyclicMessageHandlerSingleton<CustomAsyncNonCyclicMessageHandler>("#", "consumption.exchange");

            //     services.AddHostedService<ConsumingHostedService>();


            //  services.addrabit
            // services.AddRabbit(Configuration);

            // services.AddSingleton<RabbitManager>();
            //    <IRabbitManager, RabbitManager>();

            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                var factory = new ConnectionFactory()
                {
                    HostName = Configuration["EventBusConnection"],
                    DispatchConsumersAsync = true
                };

                if (!string.IsNullOrEmpty(Configuration["EventBusUserName"]))
                {
                    factory.UserName = Configuration["EventBusUserName"];
                }

                if (!string.IsNullOrEmpty(Configuration["EventBusPassword"]))
                {
                    factory.Password = Configuration["EventBusPassword"];
                }

                var retryCount = 5;
                if (!string.IsNullOrEmpty(Configuration["EventBusRetryCount"]))
                {
                    retryCount = int.Parse(Configuration["EventBusRetryCount"]);
                }

                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            });

            RegisteEsiRabbit(services);


            services.AddDbContextPool<MsrDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("EsiConnectionString")));            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostApplicationLifetime lifetime)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            app.UseHttpsRedirection();
            
            app.UseRouting();

            app.UseAuthorization();

            // Prior to .NET Core 3, IHostApplicationLifetime was actually IApplicationLifetime

            // Add the ChatHub to the configuration. This has changed for .NET Core 3. Prior versions used app.AddSignalR at this stage
            //app.UseEndpoints(configure =>
            //{
            //    configure.MapHub<ChatHub>("/chatHub");
            //});

            // Run 'RegisterSignalRWithRabbitMQ' when the application has started.
           // lifetime.ApplicationStarted.Register(() => RegisterSignalRWithRabbitMQ(app.ApplicationServices));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //app.UseEndpoints(configure =>
            //{
            //    configure.MapHub<ChatHub>("/chatHub");
            //});
        }


        public void RegisterSignalRWithRabbitMQ(IServiceProvider serviceProvider)
        {
            // Connect to RabbitMQ
            var rabbitMQService = (IRabbitMQService)serviceProvider.GetService(typeof(IRabbitMQService));
            rabbitMQService.Connect();
        }

        public void RegisteEsiRabbit(IServiceCollection services)
        {
            var subscriptionClientName = Configuration["SubscriptionClientName"];
            services.AddSingleton<IEventBus, EsiEventBusRabbitMQ>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<EsiEventBusRabbitMQ>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                var retryCount = 5;
                if (!string.IsNullOrEmpty(Configuration["EventBusRetryCount"]))
                {
                    retryCount = int.Parse(Configuration["EventBusRetryCount"]);
                }

                return new EsiEventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
            });

        }
    }
}
