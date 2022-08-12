using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseServer;
using System.ServiceModel;
using InterfaceToDLL;

namespace BusinessTier
{

    internal class BusinessServerImplementation : BusinessServerInterface
    {
        private BankingInterface foob;
        public BusinessServerImplementation()
        {
            ChannelFactory<BankingInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();

            string URL = "net.tcp://localhost:8100/CustomerService";
            foobFactory = new ChannelFactory<BankingInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();
        }



        int BusinessServerInterface.GetNumEntries()
        {
            return foob.GetNumEntries();
        }

        void BusinessServerInterface.GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out string profPicPath)
        {
            try
            {
                foob.GetValuesForEntry(index, out acctNo, out pin, out bal, out fName, out lName, out profPicPath);
            }
            catch (FaultException<ArgumentOutOfRangeException>)
            {
                acctNo = 0;
                pin = 0;
                bal = 0;
                fName = String.Empty;
                lName = String.Empty;
                profPicPath = String.Empty;
            }
          
        }

        int BusinessServerInterface.SearchCustomer(string lastName)
        {
            int index = 0;
            try
            {
                index = foob.SearchCustomer(lastName);
            }
            catch (FaultException<ArgumentOutOfRangeException>) 
            {

            }
            return index;
        }
    }
}
