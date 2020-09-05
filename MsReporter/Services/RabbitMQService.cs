using Microsoft.AspNetCore.SignalR;
using MsReporter.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsReporter.Services
{
    // RabbitMQService.cs
    public class RabbitMQService : IRabbitMQService
    {
        protected readonly ConnectionFactory _factory;
        protected readonly IConnection _connection;
        protected readonly IModel _channel;

        protected readonly IServiceProvider _serviceProvider;

        public RabbitMQService(IServiceProvider serviceProvider)
        {
            // Opens the connections to RabbitMQ
            _factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            _serviceProvider = serviceProvider;
        }

        public virtual void Connect2()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

                var queueName = channel.QueueDeclare().QueueName;
                channel.QueueBind(queue: queueName, exchange: "logs", routingKey: "");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] {0}", message);

                    //using (MsrDbContext db = new MsrDbContext(null))
                    //{
                    //    var obj = new MsrModel();
                    //    obj.CounterMs1 = 1;
                    //    obj.CounterMs2 = 1;
                    //    db.ReportsHello.Add(obj);
                    //    db.SaveChanges();
                    //}

                };
                channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            }
        }
        public virtual void Connect()
        {

            _channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: queueName, exchange: "logs", routingKey: "");

            var consumer = new EventingBasicConsumer(_channel);


            // Declare a RabbitMQ Queue
           // _channel.QueueDeclare(queue: "Logs", durable: true, exclusive: false, autoDelete: false);

           // var consumer = new EventingBasicConsumer(_channel);

            // When we receive a message from SignalR
            consumer.Received += delegate (object model, BasicDeliverEventArgs ea) {
                // Get the ChatHub from SignalR (using DI)
                var chatHub = (IHubContext<ChatHub>)_serviceProvider.GetService(typeof(IHubContext<ChatHub>));

                // Send message to all users in SignalR
                chatHub.Clients.All.SendAsync("messageReceived", "You have received a message");

            };

            // Consume a RabbitMQ Queue
            _channel.BasicConsume(queue: "Logs", autoAck: true, consumer: consumer);
        }

    }
}
