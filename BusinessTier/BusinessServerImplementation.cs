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
        public uint LogNumber = 0;
        public List<string> Logs;
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
            Log("Returning total number of entries in Database.");
            return foob.GetNumEntries();
        }

        void BusinessServerInterface.GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out string profPicPath)
        {
            try
            {
                Log("Using an index of a list to return a customer's details.");
                foob.GetValuesForEntry(index, out acctNo, out pin, out bal, out fName, out lName, out profPicPath);
            }
            catch (FaultException<ArgumentOutOfRangeException>)
            {
                Log("Bad Index. Index provided out of range of customer list indexing.");
                acctNo = 0;
                pin = 0;
                bal = 0;
                fName = String.Empty;
                lName = String.Empty;
                profPicPath = String.Empty;
            }

        }

        void BusinessServerInterface.SearchCustomer(string searchValue, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out string profPicPath)
        {
            int index = -1;
            Boolean found = false;
            for (int i = 0; i < foob.GetNumEntries(); i++)
            {
                foob.GetValuesForEntry(i, out _, out _, out _, out _, out string lastName, out _);
                if (lastName.ToUpper().Equals(searchValue.ToUpper()))
                {
                    index = i;
                    found = true;
                    break;
                }
            }
            if (found)
            {
                Log("Matching customer found. Returning customer's details to user.");
                foob.GetValuesForEntry(index, out acctNo, out pin, out bal, out fName, out lName, out profPicPath);
            }
            else
            {
                Log("No natching customer found. Returning dummy data.");
                acctNo = 0;
                pin = 0;
                bal = 0;
                fName = String.Empty;
                lName = String.Empty;
                profPicPath = String.Empty;
            }
        }

        void Log(string logString)
        {
            LogNumber++;
            Logs.Add(logString + " Number of tasks performed: " + LogNumber);
        }
    }
}
