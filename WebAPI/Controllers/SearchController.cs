using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Http;
using APIClassLibrary;
using InterfaceToDLL;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [RoutePrefix("api/search")]
    public class SearchController : ApiController
    {
        private readonly DatabaseServer dataTier = DatabaseServer.Instance();

        [Route("post/{searchData}")]
        [HttpPost]
        public DataIntermed Post([FromBody] SearchData searchData) => dataTier.SearchCustomer(searchData);
    }
}