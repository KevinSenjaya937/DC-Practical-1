using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace DC_Practical_1
{
    public class CustomerDatabase
    {
        List<Customer> cList;

        public CustomerDatabase()
        {
            cList = new List<DC_Practical_1.Customer>();
            cList = Customers();
        }
        public List<Customer> Customers()
        {

            DatabaseGenerator generator = new DatabaseGenerator();

            for (int i = 0; i < 20; i++)
            { 
                generator.GetNextAccount(out uint acctNo, out uint pin, out int balance, out string firstname, out string lastname, out string profPicPath);

                Customer customer = new Customer(acctNo, pin, balance, firstname, lastname, profPicPath);

                cList.Add(customer);
            }
            return cList;
        }

        public uint GetAcctNoByIndex(int index) => cList[index].acctNo;
        public uint GetPINByIndex(int index) => cList[index].pin;
        public string GetFirstNameByIndex(int index) => cList[(int)index].firstname;
        public string GetLastNameByIndex(int index) => cList[(int)index].lastname;
        public int GetBalanceByIndex(int index) => cList[(int)index].balance;
        public string GetProfPicPath(int index) => cList[index].profPicPath;
        public int GetNumRecords() => cList.Count;
    }

    
}
