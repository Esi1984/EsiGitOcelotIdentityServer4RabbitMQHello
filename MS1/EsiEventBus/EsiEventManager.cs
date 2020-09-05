using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MS1.EsiEventBus
{
    public static class EsiEventManager
    {
        public static void MyEventBut_EsiEventReceived(object sender, BasicDeliverEventArgs e)
        {
           // throw new NotImplementedException(object sender, BasicDeliverEventArgs e);
        }

    }
}
