using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        [Route("post/")]
        [HttpPost]
        public DataIntermed Post([FromBody] SearchData searchData)
        {
            DataIntermed customer = dataTier.SearchCustomer(searchData);
                
            if (customer.profPicPath == null)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No customer with matching last name found."));
            }
            return customer;
        }
    }
}