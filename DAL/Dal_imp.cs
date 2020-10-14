using DS;
using System;
using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using System.Text.RegularExpressions;

//namespace System.ComponentModel.DataAnnotations;

//using System.Attribute;


namespace DAL
{
   public class Dal_imp : Idal//IDAl implemention
   {     
        #region Singleton
        private static Dal_imp instance = null;      
        public static Dal_imp getMyDal()
        {
            if (instance == null)
                instance = new Dal_imp();
            return instance;
        }

        private Dal_imp()
        {

        }
        #endregion

        #region Guest request

        public void add_GuestRequest(BE.GuestRequest addReq)
        {            
            if (DataSource.guestRequests.Exists(gr=> gr.guestRequestKey== addReq.guestRequestKey))
                  throw new DuplicatedKeyException("this guest request was already made");
          
            addReq.Status = BE.Enum.GuestRequestStatus.ACTIVE;
            if (addReq.Adults <= 0)
                throw new NegativeNumberException("there must be at least one adult!");
            if (addReq.Children < 0)
                throw new NegativeNumberException("number of children can not be negative!");
            //var isValid = new System.ComponentModel.DataAnnotations.EmailAddressAttribute().isValid(addReq.MailAddress);
            //if (!isValid)
            //    throw new NotValidException("inValid MailAddress");

            DS.DataSource.guestRequests.Add(addReq.Clone());
            Console.WriteLine("guest added successfully");  
        }
    
        public void update_GuestRequest(BE.GuestRequest updateReq)
        {
            
            int count = 0;
            for (int i = 0; i < DS.DataSource.guestRequests.Count(); i++)
            {
                if (DS.DataSource.guestRequests[i].guestRequestKey == updateReq.guestRequestKey && updateReq.Status != BE.Enum.GuestRequestStatus.DEAL_CLOSED_EXPIRED)
                {
                    if (updateReq.Adults <= 0)
                        throw new NegativeNumberException("there must be at least one adult!");
                    if (updateReq.Children < 0)
                        throw new NegativeNumberException("number of children can not be negative!");                     

                    DS.DataSource.guestRequests[i] = updateReq.Clone();
                    count++;
                }
            }
            
            if (count == 0)
            {
                add_GuestRequest(updateReq);        // if this request does not exist we were asked to add it
                throw new KeyDoesNotExistException("this request does not exist so it can not be updated");
            }
        }
        #endregion


        #region Hosting Unit
        public void add_HostingUnit(BE.HostingUnit addUnit)
        {           
            if (DataSource.hostingUnits.Exists(hu => hu.hostingUnitKey == addUnit.hostingUnitKey))
                throw new DuplicatedKeyException("this unit already exists!");
            DS.DataSource.hostingUnits.Add(addUnit.Clone());
            Console.WriteLine("hosting unit added successfully");
            //addUnit.hostingUnitKey = Configuration._HostingUnitKey++;
        }

        public void update_HostingUnit(BE.HostingUnit updateUnit)
        {   
            int count = 0;
            for (int i = 0; i < DS.DataSource.hostingUnits.Count(); i++)
            {
                if (DS.DataSource.hostingUnits[i].hostingUnitKey == updateUnit.hostingUnitKey)
                {
                    DS.DataSource.hostingUnits[i] = updateUnit.Clone();
                    count++;
                }
            }
            if (count == 0)
            {
                add_HostingUnit(updateUnit);
                throw new KeyDoesNotExistException("This Unit does not exist therefore you can not update it!");
            }
        }

        public void delete_HostingUnit(BE.HostingUnit deleteUnit)// no need of clone because we wanted to remove the reall one
        {
            int count = 0;
            for (int i = 0; i <= DS.DataSource.hostingUnits.Count(); i++)
            {
                if (DS.DataSource.hostingUnits[i].HostingUnitKey == deleteUnit.HostingUnitKey)
                {
                    DS.DataSource.hostingUnits.RemoveAt(i);//removes the element of the specified index
                    count++;
                }
            }
            if (count == 0)
            {
                throw new KeyDoesNotExistException("Unit does not exist therefore you can not delete it!");
            }
        }
        #endregion


        #region Order
        /// to add a new reservation
        public void add_Order(BE.Order addOrder)
        {          
            if (DataSource.orders.Exists(or=>or.orderKey== addOrder.orderKey))
                throw new DuplicatedKeyException("this order was already made");

            addOrder.CreateDate = DateTime.Now;
            DS.DataSource.orders.Add(addOrder.Clone());     
            Console.WriteLine("Order added successfully");
        }

        /// to update the status
        public void update_Order(BE.Order updateOrder)
        {
            int count = 0;
            for (int i = 0; i < DS.DataSource.orders.Count(); i++)
            {
                if (DS.DataSource.orders[i].orderKey == updateOrder.orderKey)
                {
                    DS.DataSource.orders[i] = updateOrder.Clone();
                    count++;
                }
            }
            if (count == 0) // if we could'nt find the one we wanted to update
            {
                add_Order(updateOrder);
                throw new KeyDoesNotExistException("this order does not exist, therefore it can not be updated");
            }
        }
        #endregion


        #region Lists
        /// A list of all the accommodation units
        public List<BE.HostingUnit> get_All_HostingUnits(Func<HostingUnit, bool> predicat = null)
        {
            return (from hosting_unit in DataSource.hostingUnits
                   select hosting_unit.Clone()).ToList();
        }

        /// A list of all the customers
      
        public List<BE.GuestRequest> get_All_Guests(Func<GuestRequest, bool> predicat = null)
        {
            return (from guest_request in DataSource.guestRequests
                    select guest_request.Clone()).ToList();

            //GuestRequest[] gRequests = new GuestRequest[DS.DataSource.guestRequests.Count];
            //DataSource.guestRequests.
        }
        /// A list of all the orders
      
        public List<BE.Order> get_All_Orders(Func<Order, bool> predicat = null)
        {
            return (from order in DataSource.orders
                   select order.Clone()).ToList();
        }

        /// A list of all the banks in israel
        //in this part of the project this func will return a list of 5 branches of "Bank Leumi".
        //afterwards, from the offical website of "Bank Israel"
        public List <BE.BankBranch> get_All_Bank_Branches(Func<BankBranch, bool> predicat = null) 
        {
            //string str = "Bank Leumi ";
            //BE.BankBranch[] tmp = new BE.BankBranch[5];
            //for (int i = 0; i < 5; i++)
            //{
            //    str += i;//numerize
            //    tmp[i].BankName = str;
            //}
            //return tmp.ToList();
            return (from unit in DataSource.hostingUnits
                    select unit.Owner.BankBranchDetails.Clone()).ToList();

        }
        #endregion

        public void UpdateConfigFile() // needed in dal_xml and the bl layer needs to call this func....
        {

        }
   }
}

