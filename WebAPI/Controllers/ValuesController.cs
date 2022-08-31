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
    [RoutePrefix("api/values")]
    public class ValuesController : ApiController
    {
        // GET api/values
        private readonly DatabaseServer dataTier = DatabaseServer.Instance();

        [Route("numEntries")]
        [HttpGet]
        public int Get()
        {
            return dataTier.GetNumEntries();
        }

        public int Get(int id)
        {
            return 1;
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
