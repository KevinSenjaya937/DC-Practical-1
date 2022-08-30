using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;
using InterfaceToDLL;

namespace WebAPI.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        static DatabaseServer databaseServer = new DatabaseServer();
        BankingInterface foob = databaseServer.GetDataServer();
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public int Get(int id)
        {
            DatabaseServer databaseServer = new DatabaseServer();
            BankingInterface foob = databaseServer.GetDataServer();
            return foob.GetNumEntries();
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
