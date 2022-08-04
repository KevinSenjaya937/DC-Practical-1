using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Banking Server");
            ServiceHost host;
            NetTcpBinding tcp = new NetTcpBinding();

            host = new ServiceHost(typeof(CustomerServerImplementation));

            host.AddServiceEndpoint(typeof(CustomerServerInterface), tcp, "net.tcp://0.0.0.0:8100/CustomerService");

            host.Open();
            Console.WriteLine("System Online");
            Console.ReadLine();

            host.Close();
        }
    }
}
