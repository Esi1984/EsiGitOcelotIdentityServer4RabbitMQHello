using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MsReporter3.Models;

namespace MsReporter3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {

        public ReportController(ReportDBContext mydb)
        {
            db = mydb;
           // _rabbitConnect = rabbitConnect;
        }
        private ReportDBContext db;
       // private EsiRabbitMQ.IRabbitConnect _rabbitConnect;


        // GET: api/Report
        [HttpGet]
        public IEnumerable<string> Get()
        {

            var myResult1 = db.ReportHello.Where(x => x.message.Contains("MS1"));
            var myResult2 = db.ReportHello.Where(x => x.message.Contains("MS2"));

            string str = "MS1: " + myResult1.First().message + " Count:" + myResult1.Count().ToString();
            string str2 = "MS2: " + myResult2.First().message + " Count:" + myResult2.Count().ToString();


            return new string[] { str, str2 };
        }

        // GET: api/Report/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Report
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Report/5
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
