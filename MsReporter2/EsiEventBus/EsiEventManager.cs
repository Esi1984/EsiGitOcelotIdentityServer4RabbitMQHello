using MsReporter3.Controllers;
using MsReporter3.Models;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MsReporter3.EsiEventBus
{
    public  class EsiEventManager
    {
        private  ReportDBContext _reportDBContext;
        public EsiEventManager(ReportDBContext reportDBContext)
        {

            _reportDBContext = reportDBContext;
        }
        public  void MyEventBut_EsiEventReceived(object sender, BasicDeliverEventArgs e)
        {
            // throw new NotImplementedException(object sender, BasicDeliverEventArgs e);
          //  var obj = new ReportModels();
            var messagein = Encoding.UTF8.GetString(e.Body);
            _reportDBContext.ReportHello.Add(new ReportModels() { message= messagein });
            _reportDBContext.SaveChanges();
         //    IServiceProvider.
            //   var dbc = new ReportDBContext();
            //_reportDBContext.ReportHello.Add(new _reportDBContext.ReportHello { });

        }

    }
}
