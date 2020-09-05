using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace EsiRabbitMQ
{
    public  class RabbitConnect : IRabbitConnect
    {

        private readonly IConnectionFactory _connectionFactory;
        IConnection _connection;
        bool _disposed;
        private IModel _Channel;
        private readonly string _ExName;
        private readonly string _QName;

        public RabbitConnect(IConnectionFactory connectionFactory,string ExName, string Qname)
        {

            _QName = Qname;
            _ExName = ExName;
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            if (!IsConnected)
            {
                TryConnect();
            }
        }
        public bool IsConnected
        {
            get
            {
                return _connection != null && _connection.IsOpen && !_disposed;
            }
        }

        public IModel CreateChannel()
        {

            if (!IsConnected)
            {
                TryConnect();
            }

            //  var channel = _persistentConnection.CreateModel();
            //  using (var connection = _connectionFactory.CreateConnection())
            var channel = _connectionFactory.CreateConnection().CreateModel();
            
                channel.QueueDeclare(queue: _QName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel.ExchangeDeclare(exchange: _ExName, type: ExchangeType.Fanout);
                channel.QueueBind(queue: _QName, exchange: _ExName, routingKey: "");
                var consumer = new EventingBasicConsumer(channel);
            

            //Create event when something receive
            consumer.Received += ReceivedEvent;



            channel.BasicConsume(queue: _QName, autoAck: true, consumer: consumer);
            channel.CallbackException += (sender, ea) =>
            {
                _Channel.Dispose();
                _Channel = CreateChannel();
            };
            return channel;
        }


        // declare delegate 
        public delegate void EsiDelegateReceived(object sender, BasicDeliverEventArgs e);

        //declare event of type delegate
        public event EsiDelegateReceived EsiEventReceived;

        private void ReceivedEvent(object sender, BasicDeliverEventArgs e)
        {
            if (e.RoutingKey == "")
            {
                
                EsiEventReceived( sender,  e);
                using (var channel = _connection.CreateModel())
                {

                    var consumer = new EventingBasicConsumer(channel);
                    channel.BasicConsume(queue: _QName, autoAck: true, consumer: consumer);
                }
                // var message = Encoding.UTF8.GetString(body);

                //   string Iget = JsonConvert.DeserializeObject<string>(message);
                // UserSaveFeedback saveFeedback = _userService.InsertUsers(userList);

                //   PublishUserSaveFeedback("userInsertMsgQ_feedback", saveFeedback, e.BasicProperties.Headers);
            }

            if (e.RoutingKey == "emailSendMsgQ")
            {
                //Implementation here
            }
        }

        public void Disconnect()
        {
            if (_disposed)
            {
                return;
            }
            Dispose();
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;

            try
            {
                _connection.Dispose();
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void EsiPublish(string message)
        {
            using (var channel = _connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: _ExName, type: ExchangeType.Fanout);

               // var message = GetMessage(args);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "Ex03", routingKey: "", basicProperties: null, body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }
        }

        public void EsiRecive()
        {
            throw new NotImplementedException();
        }

        public bool TryConnect()
        {

            try
            {
                Console.WriteLine("RabbitMQ Client is trying to connect");
                _connection = _connectionFactory.CreateConnection();
            }
            catch (BrokerUnreachableException e)
            {
                Thread.Sleep(5000);
                Console.WriteLine("RabbitMQ Client is trying to reconnect");
                _connection = _connectionFactory.CreateConnection();
            }

            if (IsConnected)
            {
                _connection.ConnectionShutdown += OnConnectionShutdown;
                _connection.CallbackException += OnCallbackException;
                _connection.ConnectionBlocked += OnConnectionBlocked;

                Console.WriteLine($"RabbitMQ persistent connection acquired a connection {_connection.Endpoint.HostName} and is subscribed to failure events");

                return true;
            }
            else
            {
                //  implement send warning email here
                //-----------------------
                Console.WriteLine("FATAL ERROR: RabbitMQ connections could not be created and opened");
                return false;
            }

        }

        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed) return;
            Console.WriteLine("A RabbitMQ connection is shutdown. Trying to re-connect...");
            TryConnect();
        }

        void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (_disposed) return;
            Console.WriteLine("A RabbitMQ connection throw exception. Trying to re-connect...");
            TryConnect();
        }

        void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
        {
            if (_disposed) return;
            Console.WriteLine("A RabbitMQ connection is on shutdown. Trying to re-connect...");
            TryConnect();
        }
    }
}
