using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InterfaceToDLL;
using System.ServiceModel;
using APIClassLibrary;

namespace WebAPI.Models
{
    public sealed class DatabaseServer
    {
        private readonly BankingInterface foob;
        private static readonly DatabaseServer instance = new DatabaseServer();

        
        private DatabaseServer()
        {
            ChannelFactory<BankingInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            string URL = "net.tcp://localhost:8100/CustomerService";
            foobFactory = new ChannelFactory<BankingInterface>(tcp, URL);
            this.foob = foobFactory.CreateChannel();

        }
        public static DatabaseServer Instance() { return instance; }

        public int GetNumEntries()
        {
            return this.foob.GetNumEntries();
        }

        public DataIntermed GetCustomer(int index)
        {
            
            this.foob.GetValuesForEntry(index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out string profPicPath);

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

        public DataIntermed SearchCustomer(SearchData searchData)
        {
            int entries = this.GetNumEntries();
            DataIntermed customer;
            var regexItem = new System.Text.RegularExpressions.Regex("^[a-zA-Z]*$");

            if (regexItem.IsMatch(searchData.searchStr))
            {
                for (int i = 0; i < entries; i++)
                {
                    customer = this.GetCustomer(i);

                    if (customer.lName.ToUpper() == searchData.searchStr.ToUpper())
                    {
                        return customer;
                    }
                }
            }
            return null;
        }
    }
}