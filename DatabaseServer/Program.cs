using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using InterfaceToDLL;


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

            host.AddServiceEndpoint(typeof(BankingInterface), tcp, "net.tcp://0.0.0.0:8100/CustomerService");

            host.Open();
            Console.WriteLine("System Online");
            string workingDirectory = Environment.CurrentDirectory;
            string path = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            Console.WriteLine(path);

            Console.ReadLine();

            host.Close();
        }
    }
}
