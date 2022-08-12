using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using InterfaceToDLL;
using DatabaseServer;

namespace BusinessTier
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Banking Server");
            ServiceHost host;
            NetTcpBinding tcp = new NetTcpBinding();

            host = new ServiceHost(typeof(BusinessServerImplementation));

            host.AddServiceEndpoint(typeof(BusinessServerInterface), tcp, "net.tcp://0.0.0.0:8200/BusinessService");

            host.Open();
            Console.WriteLine("System Online");
            Console.ReadLine();

            host.Close();
        }
    }
}
