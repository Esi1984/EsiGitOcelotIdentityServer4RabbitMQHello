using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MS1.Model;
using RabbitMQ.Client;
//using Ms1DbContext;

namespace MS1.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class MS1Controller : ControllerBase
    {

        public MS1Controller(Ms1DbContext mydb,EsiRabbitMQ.IRabbitConnect rabbitConnect)
        {
            db = mydb;
            _rabbitConnect = rabbitConnect;
        }
        private Ms1DbContext db;
        private EsiRabbitMQ.IRabbitConnect _rabbitConnect;
        

        // GET: api/MS1
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var obj = new ModelMS1();
            obj.message = "MS1 got a Message at:  " + DateTime.Now.ToString() ;

            db.Entry(obj).State = EntityState.Added;

            db.SaveChanges();

            _rabbitConnect.EsiPublish(obj.message);

            //////////////////////////////////////////

            //var factory = new ConnectionFactory() { HostName = "localhost" };
            //using (var connection = factory.CreateConnection())
            //using (var channel = connection.CreateModel())
            //{
            //    channel.ExchangeDeclare(exchange: "Esiexchange", type: ExchangeType.Fanout);

            //    var message = obj.message;
            //    var body = Encoding.UTF8.GetBytes(message);
            //    channel.BasicPublish(exchange: "Esiexchange", routingKey: "", basicProperties: null, body: body);
            //    Console.WriteLine(" [x] Sent {0}", message);
            //}
            //////////////////////////////////////

            return new string[] { "MS1", obj.message };
        }

        // GET: api/MS1/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/MS1
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/MS1/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
