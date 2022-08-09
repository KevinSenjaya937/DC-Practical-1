using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace DC_Practical_1
{
    public class DatabaseGenerator
    {
        Random rand = new Random();
        private string[] firstnames = {"Jack", "Chris", "Matthew", "Ryan", "Michael", "Steven"};
        private string[] lastnames = { "Love", "Brown", "Green", "McCarter", "O'Brian", "Griffith" };
        private uint[] pins = {1111, 2222, 3333, 4444, 5555, 6666 };
        private uint[] acctNos = { 000001, 000002, 000003, 000004, 000005, 000006 };
        private int[] balances = { -100, 0, 2000, 3000, 50000, 100000};
        private static string parentPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

        private string[] profPicPaths = { 
            parentPath + @"\DC Practical 1\Images\profpic1.jpg",
            parentPath + @"\DC Practical 1\Images\profpic2.jpg",
            parentPath + @"\DC Practical 1\Images\profpic3.jpg",
            parentPath + @"\DC Practical 1\Images\profpic4.jpg",
            parentPath + @"\DC Practical 1\Images\profpic5.jpg",
            parentPath + @"\DC Practical 1\Images\profpic6.jpg",
        };

        public DatabaseGenerator()
        {
        }

        private string GetFirstname() => firstnames[GenerateRandNum()];
        private string GetLastname() => lastnames[GenerateRandNum()];
        private uint GetPIN() => pins[GenerateRandNum()];
        private uint GetAcctNo() => acctNos[GenerateRandNum()];
        private int GetBalance() => balances[GenerateRandNum()];
        private string GetProfPicPath() => profPicPaths[GenerateRandNum()];




        public void GetNextAccount( out uint acctNo, out uint pin, out int balance, out string firstname, out string lastname, out string profPicPath)
        {
            pin = GetPIN();
            acctNo = GetAcctNo();
            firstname = GetFirstname(); 
            lastname = GetLastname();   
            balance = GetBalance();
            profPicPath = GetProfPicPath();
        }

        private int GenerateRandNum()
        {
            return rand.Next(0, 5);
        }
    }
}
