using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    public static class Cloning
    {
        public static BankBranch Clone( this BankBranch original)
        {
            BankBranch target = new BankBranch();

            target.BankName = original.BankName;
            target.bankNumber = original.bankNumber;
            target.BranchAddress = original.BranchAddress;
            target.BranchCity = original.BranchCity;
            target.branchNumber = original.branchNumber;
            return target;
        }

        public static GuestRequest Clone(this GuestRequest original)
        {
            GuestRequest target = new GuestRequest();
            //target.guestRequestKey++;

            target.guestRequestKey = original.guestRequestKey;//clerify 
            target.PrivateName = original.PrivateName;
            target.FamilyName = original.FamilyName;
            target.MailAddress = original.MailAddress;
            target.Status = original.Status;
            target.RegistrationDate = original.RegistrationDate;
            target.EntryDate = original.EntryDate;
            target.ReleaseDate = original.ReleaseDate;
            target.Area = original.Area;
            target.SubArea = original.SubArea;
            target.Type = original.Type;
            target.Adults = original.Adults;
            target.Children = original.Children;
            target.pool = original.pool;
            target.jacuzzi = original.jacuzzi;
            target.garden = original.garden;
            target.childAt = original.childAt;

            return target;
        }
      
        public static Host Clone (this Host original)
        {
            Host target = new Host();

            target.hostKey = original.hostKey;//updating the id of the hoster
            target.PrivateName = original.PrivateName;
            target.FamilyName = original.FamilyName;
            target.phoneNumber = original.phoneNumber;
            target.MailAddress = original.MailAddress;

            target.BankBranchDetails = original.BankBranchDetails;
            target.bankAccountNumber = original.bankAccountNumber;

            target.CollectionClearance = original.CollectionClearance;

            return target;
        }

        public static HostingUnit Clone (this HostingUnit original)
        {
            HostingUnit target = new HostingUnit();
            target.hostingUnitKey = original.hostingUnitKey;
            target.HostingUnitName = original.HostingUnitName;
            target.Owner = original.Owner;
            target.Area = original.Area;
            target.SubArea = original.SubArea;
            target.Type = original.Type;

            for (int i =0; i < 12; i++)
            {
                for (int j = 1; j < 31; j++)
                {
                    target.Diary[i, j] = original.Diary[i, j];
                }
            }
               
       

            return target;
        }

        public static Order Clone (this Order original)
        {
            Order target = new Order();

            target.orderKey = original.orderKey;// the order key will bw created here
            target.guestRequestKey = original.guestRequestKey;//comes from other classes
            target.hostingUnitKey = original.hostingUnitKey;//comes from other classes
            target.Status = original.Status;
            target.CreateDate = original.CreateDate;
            target.OrderDate = original.OrderDate;

            return target;
        }
    }
}
