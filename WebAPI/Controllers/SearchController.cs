using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using APIClassLibrary;
using InterfaceToDLL;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [RoutePrefix("api/search")]
    public class SearchController : Controller
    {
        [Route("post/{searchData}")]
        [Route("post")]
        [HttpPost]
        public DataIntermed Post(SearchData searchData)
        {
            DatabaseServer databaseServer = new DatabaseServer();
            BankingInterface foob = databaseServer.GetDataServer();

            var regexItem = new System.Text.RegularExpressions.Regex("^[a-zA-Z]*$");

            if (regexItem.IsMatch(searchData.searchStr))
            {
                int index = -1;
                Boolean found = false;
                int entries = foob.GetNumEntries();
                for (int i = 0; i < entries; i++)
                {
                    foob.GetValuesForEntry(i, out _, out _, out _, out _, out string lastName, out _);
                    if (lastName.ToUpper().Equals(searchData.searchStr.ToUpper()))
                    {
                        index = i;
                        found = true;
                        break;
                    }
                }
                if (found)
                {
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
                else
                {
                    throw new FaultException("No customer with matching last name found.");
                }
            }
            else
            {
                throw new FaultException("Bad Input Detected. Input must be a valid last name with no special characters.");
            }
        }
    }
}