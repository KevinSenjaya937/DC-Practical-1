using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIClassLibrary
{
    public class DataIntermed
    {
        public int bal              { get; set; }
        public uint acctNo          { get; set; }
        public uint pin             { get; set; }
        public string fName         { get; set; }
        public string lName         { get; set; }
        public string profPicPath   { get; set; }
    }

    public class SearchData
    {
        public string searchStr { get; set; }
    }
}
