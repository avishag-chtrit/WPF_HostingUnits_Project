using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DS
{
    public class DataSource
    {      
        public static List<BE.HostingUnit> hostingUnits = new List<BE.HostingUnit>()
        {
        //    new BE.HostingUnit()
        //    {
        //        hostingUnitKey=10000009,
        //        HostingUnitName="נוף פרת",
        //        Owner=new BE.Host()
        //        {
        //            hostKey=12345678,
        //            PrivateName="Dani",
        //            FamilyName="Alfasi",
        //            phoneNumber= 0547201224,
        //            MailAddress="Dani@gmail.com",
        //             BankBranchDetails=new BE.BankBranch()
        //             {
        //                 bankNumber=11,
        //                 BankName="דיסקונט",
        //                 branchNumber = 111,
        //                 BranchAddress="יפו 220",
        //                 BranchCity="ירושלים",
        //             },
        //             bankAccountNumber=166685,
        //             CollectionClearance=true
        //        }

        //    },

        //    new BE.HostingUnit()
        //    {
        //        hostingUnitKey=10000010,
        //        HostingUnitName="על הנחל",
        //        Owner=new BE.Host()
        //        {
        //            hostKey=12345678,
        //            PrivateName="Yoram",
        //            FamilyName="Ohanuna",
        //            phoneNumber= 0544239575,
        //            MailAddress="Yoram@gmail.com",
        //             BankBranchDetails=new BE.BankBranch()
        //             {
        //                 bankNumber=12,
        //                 BankName="בנק הפועלים",
        //                 branchNumber = 112,
        //                 BranchAddress="יפו 118",
        //                 BranchCity="ירושלים",                      
        //             },
        //              bankAccountNumber=6699,
        //             CollectionClearance=true
        //        }
        //    },

        //    new BE.HostingUnit()
        //    {
        //        hostingUnitKey=10000011,
        //        HostingUnitName="נוף כנען",
        //        Owner=new BE.Host()
        //        {
        //            hostKey=12345678,
        //            PrivateName="Igal",
        //            FamilyName="Canaan",
        //            phoneNumber= 0544239575,
        //            MailAddress="Igal@gmail.com",
        //             BankBranchDetails=new BE.BankBranch()
        //             {
        //                 bankNumber=13,
        //                 BankName="בנק יהב",
        //                 branchNumber = 130,
        //                 BranchAddress="בוגרשוב 25 ",
        //                 BranchCity="תל אביב",                       
        //             },
        //              bankAccountNumber=1234,
        //             CollectionClearance=false
        //        }
        //    }
        };

        public static List<BE.Order> orders = new List<BE.Order>()
        {
            //new BE.Order()
            //{
            //    hostingUnitKey=10000009,
            //    guestRequestKey=10000002,
            //    orderKey=20000183,
            //    Status = BE.Enum.OrderStatus.EMAIL_SENT,

            //    CreateDate=new DateTime(2020,05,06),
            //    OrderDate= new DateTime(2020,05,08)
            //},

            //new BE.Order()
            //{
            //    hostingUnitKey=10000010,
            //    guestRequestKey=10000003,
            //    orderKey=20000184,
            //    Status = BE.Enum.OrderStatus.CLOSED_NOT_REPLIED,

            //    CreateDate=new DateTime(2020,05,09),
            //    OrderDate= new DateTime(2020,05,11),

            //},

            //new BE.Order()
            //{
            //    hostingUnitKey=10000011,
            //    guestRequestKey=10000004,
            //    orderKey=20000185,
            //    Status = BE.Enum.OrderStatus.NOT_YET,

            //    CreateDate=new DateTime(2020,05,12),
            //    OrderDate= new DateTime(2020,05,14),

            //}
        };

        public static List<BE.GuestRequest> guestRequests = new List<BE.GuestRequest>()
        {
            //new BE.GuestRequest()
            //{
            //    guestRequestKey = 10000002,
            //    PrivateName = "ירון",
            //    FamilyName = "קאופמן",
            //    MailAddress = "Yaron@gmail.com",
            //    Status = BE.Enum.GuestRequestStatus.ACTIVE,
            //    RegistrationDate = new DateTime(2020,03,01),

            //    EntryDate =new DateTime(2020,06,06),
            //    ReleaseDate= new DateTime(2020,06,10),

            //    Area = BE.Enum.AreaOfV.SOUTH,
            //    SubArea = BE.Enum.subArea.ARAVA,
            //    Type = BE.Enum.UnitType.ZIMMER,
            //    Adults = 2,
            //    Children = 4,
            //    pool = BE.Enum.Pool.NECESSARY,
            //    jacuzzi = BE.Enum.Jacuzzi.NOT_INTERESTED,
            //    garden = BE.Enum.Garden.NECESSARY,
            //    childAt = BE.Enum.ChildrensAttractions.POSSIBLE
            //},

            // new BE.GuestRequest()
            //{
            //    guestRequestKey = 10000003,
            //    PrivateName = "יואב",
            //    FamilyName = "כנען",
            //    MailAddress = "Yoav@gmail.com",
            //    Status = BE.Enum.GuestRequestStatus.DEAL_CLOSED_EXPIRED,
            //    RegistrationDate = new DateTime(2020,03,03),

            //    EntryDate =new DateTime(2020,08,06),
            //    ReleaseDate= new DateTime(2020,08,10),

            //    Area = BE.Enum.AreaOfV.NORTH,
            //    SubArea = BE.Enum.subArea.EMEK_HACHULA,
            //    Type = BE.Enum.UnitType.SUBLET,
            //    Adults = 2,
            //    Children = 0,
            //    pool = BE.Enum.Pool.POSSIBLE,
            //    jacuzzi = BE.Enum.Jacuzzi.NECESSARY,
            //    garden = BE.Enum.Garden.POSSIBLE,
            //    childAt = BE.Enum.ChildrensAttractions.NOT_INTERESTED
            // },

            //new BE.GuestRequest()
            //{
            //    guestRequestKey = 10000004,
            //    PrivateName = "רינת",
            //    FamilyName = "עובדיה",
            //    MailAddress = "rinat@gmail.com",
            //    Status = BE.Enum.GuestRequestStatus.DEAL_CLOSED_VIA_WEB,
            //    RegistrationDate = new DateTime(2020,03,05),

            //    EntryDate =new DateTime(2020,04,07),
            //    ReleaseDate= new DateTime(2020,04,11),

            //    Area = BE.Enum.AreaOfV.CENTER,
            //    SubArea = BE.Enum.subArea.MISHOR_HACHOD,
            //    Type = BE.Enum.UnitType.HROOM,
            //    Adults = 2,
            //    Children = 2,
            //    pool = BE.Enum.Pool.NECESSARY,
            //    jacuzzi = BE.Enum.Jacuzzi.POSSIBLE,
            //    garden = BE.Enum.Garden.NOT_INTERESTED,
            //    childAt = BE.Enum.ChildrensAttractions.POSSIBLE
            // }
        };

    }
}
