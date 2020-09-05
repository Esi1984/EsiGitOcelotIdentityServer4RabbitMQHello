using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace EsiRabbitMQ
{
    public interface IRabbitConnect: IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

   //     IModel CreateModel();


        IModel CreateChannel();

        void EsiPublish(string Message);

        void EsiRecive();

        void Disconnect();

    }
}
