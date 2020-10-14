using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    [Serializable]
    public class Host
    {
        public long hostKey = Configuration._HostKey++;
        public String HostKey { get { return hostKey.ToString(Configuration.HostKeyFormat); } }
        public String PrivateName { get; set; }
        public String FamilyName { get; set; }

        public long phoneNumber { set; get; }
        public String PhoneNumber { get { return phoneNumber.ToString("0000000000"); } }
        public String MailAddress { get; set; }

        public BankBranch BankBranchDetails { get; set; }//changed

        public long bankAccountNumber { set; get; }
        public string BankAccountNumber { get { return bankAccountNumber.ToString("00000000"); } }

        public bool CollectionClearance { get; set; }

        public Host()
        {

        }

        //public Host(string PrivateName, String FamilyName, long phoneNumber, String MailAddress, BankBranch BankBranchDetails, string BankAccountNumber, String CollectionClearance)
        //{ 
        //    hostKey = Configuration._HostKey++;//updating the id of the hoster
        //    this.PrivateName = PrivateName;
        //    this.FamilyName = FamilyName;
        //    this.phoneNumber = phoneNumber;
        //    this.MailAddress = MailAddress;

        //    this.BankBranchDetails = BankBranchDetails;
        //    bankAccountNumber = Configuration._BankAccountNumber++;
        //    if (CollectionClearance == "yes" || CollectionClearance =="no")//maybe other option- try and catch
        //    {
        //        this.CollectionClearance = CollectionClearance;
        //    }
        //}

        public override string ToString()
        {
            return ("Details of Host: \n " + "key of host: " + HostKey + "\n private name: " + PrivateName + "\n family name: " + FamilyName + "\n"
                + "phone number: " + PhoneNumber + "\n mail address: " + MailAddress + "\n" + "Bank Branch Details: " + BankBranchDetails +
                    "\n collection clearance: " + CollectionClearance + "\n bank account number: " + BankAccountNumber + "\n");
        }
    }
}

