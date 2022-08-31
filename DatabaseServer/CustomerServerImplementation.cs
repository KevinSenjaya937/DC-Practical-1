using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DC_Practical_1;
using InterfaceToDLL;
using System.Drawing;

namespace DatabaseServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class CustomerServerImplementation : BankingInterface
    {
        CustomerDatabase data;
        public CustomerServerImplementation()
        {
            this.data = new CustomerDatabase();
        }

        
        int BankingInterface.GetNumEntries() => data.GetNumRecords();
        public void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out string profPicPath)
        {
            try
            {
                acctNo = data.GetAcctNoByIndex(index);
                pin = data.GetPINByIndex(index);
                bal = data.GetBalanceByIndex(index);
                fName = data.GetFirstNameByIndex(index);
                lName = data.GetLastNameByIndex(index);
                profPicPath = data.GetProfPicPath(index);
            }
            catch(ArgumentOutOfRangeException)
            {
                CustomException fault = new CustomException();
                throw new FaultException<CustomException>(fault, "Bad Index - GetValuesForEntry");
            }
        }
    }
}

