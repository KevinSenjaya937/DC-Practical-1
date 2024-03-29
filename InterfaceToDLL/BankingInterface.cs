﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Drawing;

namespace InterfaceToDLL
{
    [ServiceContract]
    public interface BankingInterface
    {
        [OperationContract]
        [FaultContract(typeof(CustomException))]
        int GetNumEntries();

        [OperationContract]
        [FaultContract(typeof(ArgumentOutOfRangeException))]
        void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out string profPicPath);
    }

    [DataContractFormat]
    public class CustomException
    {
        private string strReason;
        public string Reason
        {
            get { return strReason; }
            set { Reason = value; }
        }
    }
}

