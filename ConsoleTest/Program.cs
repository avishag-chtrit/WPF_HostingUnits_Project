using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
using BE;


namespace ConsoleTest
{
    class Program
    {
        //  static IBL blAccess = FactoryBL.getBL();

        static void Main(string[] args)
        {
            IBL blAccess = FactoryBL.getBL();
            try
            {
                #region Add GuestRequest

                BE.GuestRequest num1 = new BE.GuestRequest()
                {
                    PrivateName = "ירון",
                    FamilyName = "קאופמן",
                    MailAddress = "Yaron@gmail.com",
               //     Status = BE.Enum.GuestRequestStatus.ACTIVE,
                    //RegistrationDate = new DateTime(2020, 03, 01),

                    EntryDate = new DateTime(2020, 06, 06),
                    ReleaseDate = new DateTime(2020, 06, 10),

                    Area = BE.Enum.AreaOfV.SOUTH,
                    SubArea = BE.Enum.subArea.ARAVA,
                    Type = BE.Enum.UnitType.ZIMMER,
                    Adults = 2,
                    Children = 4,
                    pool = BE.Enum.Pool.NECESSARY,
                    jacuzzi = BE.Enum.Jacuzzi.NOT_INTERESTED,
                    garden = BE.Enum.Garden.NECESSARY,
                    childAt = BE.Enum.ChildrensAttractions.POSSIBLE
                };

                GuestRequest num2 = new GuestRequest
                {
                    PrivateName = "יואב",
                    FamilyName = "כנען",
                    MailAddress = "Yoav@gmail.com",
                  //  Status = BE.Enum.GuestRequestStatus.DEAL_CLOSED_EXPIRED,
                   // RegistrationDate = new DateTime(2020, 03, 03),

                    EntryDate = new DateTime(2020, 08, 06),
                    ReleaseDate = new DateTime(2020, 08, 10),

                    Area = BE.Enum.AreaOfV.NORTH,
                    SubArea = BE.Enum.subArea.EMEK_HACHULA,
                    Type = BE.Enum.UnitType.SUBLET,
                    Adults = 2,
                    Children = 0,
                    pool = BE.Enum.Pool.POSSIBLE,
                    jacuzzi = BE.Enum.Jacuzzi.NECESSARY,
                    garden = BE.Enum.Garden.POSSIBLE,
                    childAt = BE.Enum.ChildrensAttractions.NOT_INTERESTED
                };

                GuestRequest num3 = new GuestRequest
                {
                    PrivateName = "רינת",
                    FamilyName = "עובדיה",
                    MailAddress = "rinat@gmail.com",
                   // Status = BE.Enum.GuestRequestStatus.DEAL_CLOSED_VIA_WEB,
                   // RegistrationDate = new DateTime(2020, 03, 05),

                    EntryDate = new DateTime(2020, 04, 07),
                    ReleaseDate = new DateTime(2020, 04, 11),

                    Area = BE.Enum.AreaOfV.CENTER,
                    SubArea = BE.Enum.subArea.MISHOR_HACHOD,
                    Type = BE.Enum.UnitType.HROOM,
                    Adults = 2,
                    Children = 2,
                    pool = BE.Enum.Pool.NECESSARY,
                    jacuzzi = BE.Enum.Jacuzzi.POSSIBLE,
                    garden = BE.Enum.Garden.NOT_INTERESTED,
                    childAt = BE.Enum.ChildrensAttractions.POSSIBLE
                };

                blAccess.add_GuestRequest(num1);
                blAccess.add_GuestRequest(num2);
                blAccess.add_GuestRequest(num3);



                foreach (var guestRequest in blAccess.get_All_Guests())
                {
                    Console.WriteLine("{0}", guestRequest.ToString());
                }
                #endregion

                #region Add HostingUnit
                BE.HostingUnit unit1 = new HostingUnit()
                {
                    HostingUnitName = "על הנחל",
                    Owner = new BE.Host()
                    {
                        PrivateName = "Yoram",
                        FamilyName = "Ohanuna",
                        phoneNumber = 0544239575,
                        MailAddress = "Yoram@gmail.com",
                        BankBranchDetails = new BE.BankBranch()
                        {
                            bankNumber = 12,
                            BankName = "בנק הפועלים",
                            branchNumber = 112,
                            BranchAddress = "יפו 118",
                            BranchCity = "ירושלים",
                        },
                        bankAccountNumber = 6699,
                        CollectionClearance = true
                    },

                };

                BE.HostingUnit unit2 = new HostingUnit()
                {
                    HostingUnitName = "נוף כנען",
                    Owner = new BE.Host()
                    {
                        PrivateName = "Igal",
                        FamilyName = "Canaan",
                        phoneNumber = 0544239575,
                        MailAddress = "Igal@gmail.com",
                        BankBranchDetails = new BE.BankBranch()
                        {
                            bankNumber = 13,
                            BankName = "בנק יהב",
                            branchNumber = 130,
                            BranchAddress = "בוגרשוב 25 ",
                            BranchCity = "תל אביב",
                        },
                        bankAccountNumber = 1234,
                        CollectionClearance = true

                    },
                };

                BE.HostingUnit unit3 = new HostingUnit()
                {
                    HostingUnitName = "נוף פרת",
                    Owner = new BE.Host()
                    {
                        PrivateName = "Dani",
                        FamilyName = "Alfasi",
                        phoneNumber = 0547201224,
                        MailAddress = "Dani@gmail.com",
                        BankBranchDetails = new BE.BankBranch()
                        {
                            bankNumber = 11,
                            BankName = "דיסקונט",
                            branchNumber = 111,
                            BranchAddress = "יפו 220",
                            BranchCity = "ירושלים",
                        },
                        bankAccountNumber = 166685,
                        CollectionClearance = false
                    },
                };

                BE.HostingUnit unit4 = new HostingUnit()
                {
                    HostingUnitName = "אחלה",
                    Owner = new BE.Host()
                    {
                        PrivateName = "אב",
                        FamilyName = "רג",
                        phoneNumber = 0547201224,
                        MailAddress = "Dani@gmail.com",
                        BankBranchDetails = new BE.BankBranch()
                        {
                            bankNumber = 11,
                            BankName = "דיסקונט",
                            branchNumber = 111,
                            BranchAddress = "יפו 220",
                            BranchCity = "ירושלים",
                        },
                        bankAccountNumber = 166685,
                        CollectionClearance = false
                    },
                };



                blAccess.add_HostingUnit(unit1);
                blAccess.add_HostingUnit(unit2);
                blAccess.add_HostingUnit(unit3);
                blAccess.add_HostingUnit(unit4);

                foreach (var hostingUnit in blAccess.get_All_HostingUnits())
                {
                    Console.WriteLine("{0}", hostingUnit.ToString());
                }

                #endregion




                
                #region Add Order

                BE.Order order1 = new BE.Order()
                {
                    Status = BE.Enum.OrderStatus.EMAIL_SENT,
                    guestRequestKey = 10000000,
                    hostingUnitKey = 10000000,
                    CreateDate = new DateTime(2020, 05, 06),
                    OrderDate = new DateTime(2020, 05, 08)
                };

                BE.Order order2 = new BE.Order()
                {
                    Status = BE.Enum.OrderStatus.EMAIL_SENT,
                    guestRequestKey = 10000001,
                    hostingUnitKey = 10000001,
                    CreateDate = new DateTime(2020, 05, 09),
                    OrderDate = new DateTime(2020, 05, 11),
                };

                BE.Order order3 = new BE.Order()
                {
                    Status = BE.Enum.OrderStatus.NOT_YET,
                    guestRequestKey = 10000002,
                    hostingUnitKey = 10000002,
                    CreateDate = new DateTime(2020, 05, 12),
                    OrderDate = new DateTime(2020, 05, 14),
                };

                blAccess.add_Order(order1);
                blAccess.add_Order(order2);
                blAccess.add_Order(order3);

                //BE.Order order2Updated= blAccess.myDal
                //order2.Status = BE.Enum.OrderStatus.CLOSED_REPLIED;
                //order2.guestRequestKey = 10000001;
                //order2.hostingUnitKey = 10000001;
                //order2.CreateDate = new DateTime(2020, 05, 09);
                //order2.OrderDate = new DateTime(2020, 05, 11);                 
                //blAccess.update_Order(order2);

                foreach (var order in blAccess.get_All_Orders())
                {
                    Console.WriteLine("{0}", order.ToString());
                }






                #endregion

                #region delete GuestRequest
              //  blAccess.delete_HostingUnit(unit4);//עוד לא טיפלתי
                //     num1.Status = BE.Enum.GuestRequestStatus.DEAL_CLOSED_VIA_WEB;
                //         blAccess.update_GuestRequest(num1);

                //     unit2.HostingUnitName = "sababa";
                //   blAccess.update_HostingUnit(unit2);

                order2.Status = BE.Enum.OrderStatus.CLOSED_REPLIED;
               blAccess.update_Order(order2);
                #endregion


            }

            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }

            Console.ReadKey();


        }
    }
}
