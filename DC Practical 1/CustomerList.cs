using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC_Practical_1
{
    public class CustomerList
    {
        public static List<Customer> Customers()
        {
            List<Customer> cList = new List<DC_Practical_1.Customer>();

            DatabaseGenerator generator = new DatabaseGenerator();

            for (int i = 0; i < 20; i++)
            { 
                generator.GetNextAccount(out uint acctNo, out uint pin, out int balance, out string firstname, out string lastname);

                Customer customer = new Customer(acctNo, pin, balance, firstname, lastname);

                cList.Add(customer);
            }

            return cList;
        }
    }

    
}
