﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;
using InterfaceToDLL;
using APIClassLibrary;
using System.ServiceModel;

namespace WebAPI.Controllers
{
    [RoutePrefix("api/customer")]
    public class CustomerController : ApiController
    {
        private readonly DatabaseServer dataTier = DatabaseServer.Instance();

        [Route("get/{index}")]
        [HttpGet]
        public DataIntermed Get(int index)
        {
            return dataTier.GetCustomer(index);
        }
    }
}
