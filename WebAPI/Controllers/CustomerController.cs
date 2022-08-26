using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;


namespace WebAPI.Controllers
{
    [RoutePrefix("api/calculator")]
    public class CustomerController : ApiController
    {
        public int GetNumberOfEntries()
        {
            return CustomerList.GetNumOfEntries();
        }
    }
}
