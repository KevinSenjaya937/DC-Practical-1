using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DC_Practical_1;

namespace DatabaseServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class CustomerServerImplementation : CustomerServerInterface
    {
        CustomerDatabase data = new CustomerDatabase();

        int CustomerServerInterface.GetNumEntries() => data.GetNumRecords();
        public void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName)
        {
            acctNo = data.GetAcctNoByIndex(index);
            pin = data.GetPINByIndex(index);
            bal = data.GetBalanceByIndex(index);
            fName = data.GetFirstNameByIndex(index);
            lName = data.GetLastNameByIndex(index);
        }
    }
}

