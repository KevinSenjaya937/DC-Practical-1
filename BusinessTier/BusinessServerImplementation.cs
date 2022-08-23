using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseServer;
using System.ServiceModel;
using InterfaceToDLL;
using System.Runtime.CompilerServices;

namespace BusinessTier
{
    internal class BusinessServerImplementation : BusinessServerInterface
    {
        private BankingInterface foob;
        public uint LogNumber = 0;
        public List<string> Logs;

        public BusinessServerImplementation()
        {
            this.Logs = new List<string>();
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
                foob.GetValuesForEntry(index-1, out acctNo, out pin, out bal, out fName, out lName, out profPicPath);
            }
            catch (FaultException<ArgumentOutOfRangeException>)
            {
                Log("Bad index was provided. Throwing a FaultException");
                throw new FaultException("Bad Index. Index provided is out of range of database.");
            }
        }

        void BusinessServerInterface.SearchCustomer(string searchValue, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out string profPicPath)
        {
            var regexItem = new System.Text.RegularExpressions.Regex("^[a-zA-Z]*$");

            if (regexItem.IsMatch(searchValue))
            {
                int index = -1;
                Boolean found = false;
                int entries = foob.GetNumEntries();
                for (int i = 0; i < entries; i++)
                {
                    Console.WriteLine(i);
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
                    Log("No matching customer found. Throwing FaultException.");
                    throw new FaultException("No customer with matching last name found.");
                }
            }
            else
            {
                throw new FaultException("Bad Input Detected. Input must be a valid last name with no special characters.");
            }

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        void Log(string logString)
        {
            LogNumber++;
            Logs.Add(logString + " Number of tasks performed: " + LogNumber);
        }
    }
}
