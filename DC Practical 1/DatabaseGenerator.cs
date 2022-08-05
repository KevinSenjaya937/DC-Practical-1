using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC_Practical_1
{
    public class DatabaseGenerator
    {
        Random rand = new Random();
        private string[] firstnames = {"Jack", "Chris", "Matthew", "Heather", "Michelle", "Jennifer"};
        private string[] lastnames = { "Love", "Brown", "Green", "McCarter", "O'Brian", "Griffith" };
        private uint[] pins = {1111, 2222, 3333, 4444, 5555, 6666 };
        private uint[] acctNos = { 000001, 000002, 000003, 000004, 000005, 000006 };
        private int[] balances = { -100, 0, 2000, 3000, 50000, 100000};

        public DatabaseGenerator()
        {
        }

        private string GetFirstname() => firstnames[GenerateRandNum()];
        private string GetLastname() => lastnames[GenerateRandNum()];
        private uint GetPIN() => pins[GenerateRandNum()];
        private uint GetAcctNo() => acctNos[GenerateRandNum()];
        private int GetBalance() => balances[GenerateRandNum()];


        public void GetNextAccount( out uint acctNo, out uint pin, out int balance, out string firstname, out string lastname)
        {
            pin = GetPIN();
            acctNo = GetAcctNo();
            firstname = GetFirstname(); 
            lastname = GetLastname();   
            balance = GetBalance();
        }

        private int GenerateRandNum()
        {
            return rand.Next(0, 5);
        }


    }
}
