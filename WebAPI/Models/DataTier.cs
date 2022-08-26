using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InterfaceToDLL;
using System.ServiceModel;
using BusinessTier;

namespace WebAPI.Models
{
    public class DatabaseServer
    {
        public BankingInterface GetDataServer()
        {
            BankingInterface foob;
            ChannelFactory<BankingInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            string URL = "net.tcp://localhost:8100/CustomerService";
            foobFactory = new ChannelFactory<BankingInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();
            return foob;
        }
    }
}