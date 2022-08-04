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
        int CustomerServerInterface.GetNumEntries() => CustomerList.Customers().Count;
        public void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName)
        {
            List<Customer> cList = CustomerList.Customers();
            acctNo = cList[index-1].acctNo;
            pin = cList[index-1].pin;
            bal = cList[index-1].balance;
            fName = cList[index-1].firstname;
            lName = cList[index-1].lastname;
        }
    }
    
}

