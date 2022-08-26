using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;
using InterfaceToDLL;
using APIClassLibrary;



namespace WebAPI.Controllers
{
    [RoutePrefix("api/customer")]
    public class CustomerController : ApiController
    {
        [Route("get/{index}")]
        [Route("get")]
        [HttpGet]
        public DataIntermed Get(int index)
        {
            DatabaseServer databaseServer = new DatabaseServer();
            BankingInterface foob = databaseServer.GetDataServer();
            foob.GetValuesForEntry(index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out string profPicPath);

            DataIntermed customer = new DataIntermed
            {
                acctNo = acctNo,
                pin = pin,
                bal = bal,
                fName = fName,
                lName = lName,
                profPicPath = profPicPath
            };

            return customer;
        }
    }
}
