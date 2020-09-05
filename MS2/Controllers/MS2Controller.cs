using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MS2.Model;

namespace MS2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MS2Controller : ControllerBase
    {
        public MS2Controller(Ms2DbContext mydb, EsiRabbitMQ.IRabbitConnect rabbitConnect)
        {
            db = mydb;
            _rabbitConnect = rabbitConnect;
        }
        private Ms2DbContext db;
        private EsiRabbitMQ.IRabbitConnect _rabbitConnect;


        // GET: api/MS2
        [HttpGet]
        public IEnumerable<string> Get()
        {

            var obj = new ModelMS2();
            obj.message2 = "MS2 Say Hello:  " + DateTime.Now.ToString();

            db.Entry(obj).State = EntityState.Added;

            db.SaveChanges();

            _rabbitConnect.EsiPublish(obj.message2);

            return new string[] { "MS2", obj.message2 };


        }

        // GET: api/MS2/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/MS2
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/MS2/5
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
