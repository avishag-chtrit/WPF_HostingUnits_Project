using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    [Serializable]
    public class BankBranch
    {
        public long bankNumber { set; get; }
        public string BankNumber { get { return bankNumber.ToString("00000000"); } }

     //acrodding the changes  // public long bankAccountNumber;
      //  public string BankAccountNumber { get { return bankAccountNumber.ToString("00000000"); } }
        public string BankName { get; set; }
        public long branchNumber { get; set; }
        public string BranchNumber { get { return branchNumber.ToString("00000000"); } }
        public string BranchAddress { get; set; }
        public string BranchCity { get; set; }

        //public BankBranch(string BankName, long branchNumber, string BranchAddress, string BranchCity/* string BankAccountNumber*/)
        //{
        //  //  bankAccountNumber = Configuration._BankAccountNumber++;
        //    this.BankName = BankName;
        //    this.branchNumber = branchNumber;
        //    this.BranchAddress = BranchAddress;
        //    this.BranchCity = BranchCity;
        //}

        public BankBranch()
        {

        }

        public override string ToString()
        {
            return( " bank number: " + BankNumber + " bank name: " + BankName + " number of branch: " + BranchNumber + " address of branch: " + BranchAddress +
                    "city of branch: " + BranchCity );
        }

    }
}
