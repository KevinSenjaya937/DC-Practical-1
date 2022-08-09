using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace DC_Practical_1
{
    public class Customer
    {
        public uint acctNo;
        public uint pin;
        public int balance;
        public string firstname;
        public string lastname;
        public string profPicPath;

        public Customer()
        {
            acctNo = 0;
            pin = 0; 
            balance = 0;
            firstname = string.Empty;
            lastname = string.Empty;
            profPicPath = string.Empty;
        }

        public Customer(uint acctNo, uint pin, int balance, string firstname, string lastname, string profPicPath)
        {
            this.acctNo = acctNo;
            this.pin = pin;
            this.balance = balance;
            this.firstname = firstname;
            this.lastname = lastname;
            this.profPicPath = profPicPath;
        }
    }
}
