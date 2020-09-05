using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using MsReporter.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace MsReporter.EnvetBus
{
    public  class RabbitManager     :  Hub
    {
        public RabbitManager()
        {
            Listener();

        }
         public  void Listener()
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

                    using (MsrDbContext db = new MsrDbContext(null))
                    {
                        var obj = new MsrModel();
                        obj.CounterMs1 = 1;
                        obj.CounterMs2 = 1;
                        db.ReportsHello.Add(obj);
                        db.SaveChanges();
                    }

                };
                channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

               // Console.WriteLine(" Press [enter] to exit.");
              //  Console.ReadLine();
            }
        }
    }
}
