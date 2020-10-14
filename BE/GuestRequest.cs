using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class GuestRequest
    {
        public long guestRequestKey = Configuration._GuestRequestKey++;
       public string GuestRequestKey { get { return guestRequestKey.ToString("00000000"); } }
        public string PrivateName { get; set; }
        public string FamilyName { get; set; }
        public string MailAddress { get; set; }
        public Enum.GuestRequestStatus Status { get; set; }//enum
        public DateTime RegistrationDate { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        public Enum.AreaOfV Area { get; set; }//enum
        public Enum.subArea SubArea { get; set; }
        public Enum.UnitType Type { get; set; }//enum
        public int Adults { get; set; }
        //exeption
        //private int adults;
        //public int Adults
        //{
        //    get{ return adults; }
        //    set
        //    {
        //        if (value < 0)
        //            throw new Exception("adults number can't be negative..");
        //        this.adults = value;
        //    }
        //}
        public int Children { get; set; }
        public Enum.Pool pool { get; set; }
        public Enum.Jacuzzi jacuzzi { get; set; }
        public Enum.Garden garden{ get; set; }
        public Enum.ChildrensAttractions childAt { get; set; }

       

     
        public GuestRequest()
        {

        }

        public override string ToString()//all the details
        {
            return ("Details of Guest Requests: \n " + "key of guest request: " + guestRequestKey + "     private name: " + PrivateName + "     family name: " + FamilyName + "\n" + "mail address: " + MailAddress + "     status: " + Status +
            " registration date: " + RegistrationDate + " entry date: " + EntryDate + " release date: " + ReleaseDate + " area: " + Area + " sub area: " + SubArea + " type: " + Type +
            "     adults: " + Adults + "\n" + "children: " + Children + "     pool: " + pool + "     jacuzzi: " + jacuzzi + "\n" + "garden: " + garden + "     Childrens Attractions: " + childAt + "\n");
        }



    }
}
