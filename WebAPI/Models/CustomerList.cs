using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InterfaceToDLL;
using System.ServiceModel;

namespace WebAPI.Models
{
    public class CustomerList
    {
        public static int GetNumOfEntries()
        {
            BankingInterface foob;
            ChannelFactory<BankingInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();

            string URL = "net.tcp://localhost:8100/CustomerService";
            foobFactory = new ChannelFactory<BankingInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();

            return foob.GetNumEntries();
        }
    }
}