using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using System.Net.Mail;
using System.Threading;

namespace BL //לשנות את הזריקה- לא להשתמש ב-exception 
{
    public class _BL : IBL 
    {
        #region Singleton

        private static _BL instance = null;
        Idal myDal;

        public static _BL getMyBL()
        {
            if (instance == null)
                instance = new _BL();
            return instance;
        }

        private _BL()
        {
            try
            {
                myDal = FactoryDal.getDal();
            }
            catch(CanNotLoadFileException ex)
            {
                throw new CanNotLoadFileInBlException(ex.Message);
            }

            Thread thread = new Thread(() => clearOrdersFunc());

            if(Configuration.DateToday==(DateTime.Now).ToShortDateString() && Configuration.IsClearOrdrs==false)
            {
                Configuration.IsClearOrdrs = true;
                myDal.UpdateConfigFile();//updating at the file also

                thread.Start();
            }


        }

        #endregion

        #region Help Functions
        public bool is_valid_dates(GuestRequest gs)
        {
            int result2 = -1;

            int result1 = DateTime.Compare(gs.EntryDate, gs.ReleaseDate);
            if (gs.EntryDate.Year == DateTime.Now.Year && gs.EntryDate.Month == DateTime.Now.Month && gs.EntryDate.Day >= DateTime.Now.Day)
                result2 = 0;    
            if (gs.EntryDate.Year == DateTime.Now.Year && gs.EntryDate.Month > DateTime.Now.Month)
                result2 = 0;
            int result3 = DateTime.Compare(gs.ReleaseDate, DateTime.Now);
            if ((result1==-1) && (result2 == 0) && (result3==1))
                return true;
            else
                return false; ;
        }

        public void checkIfGuestAndUnitExists(Order order)
        {
            var v = from item in get_All_Guests()
                    where (item.guestRequestKey == order.guestRequestKey)
                    select new { guestRequestKey = item.guestRequestKey };
            v.ToList(); // list of elements of type long

            var u = from item in get_All_HostingUnits()
                    where (item.hostingUnitKey == order.hostingUnitKey)
                    select new { hostingUnitKey = item.hostingUnitKey };
            u.ToList();// list of elements of type long

            if (v == null || u == null)
            {
                throw new DoesNotExistException("hosting unit or guest does not exist");
            }
        }

        public bool is_collection_clearance(Order order)
        {
            List<HostingUnit> HostingUnits = get_All_HostingUnits();
            var v = HostingUnits.Where(hosting_unit => hosting_unit.hostingUnitKey == order.hostingUnitKey);
            v.ToList();

            if (v == null)
            {
                throw new DoesNotExistException("this unit does not exist");
            }
            if (v.First().Owner.CollectionClearance)
                return true;
            return false;
        }

        public bool is_unit_unbooked(Order order)
        {
            int count = 0;
            List<HostingUnit> HostingUnits = get_All_HostingUnits();
            var v = HostingUnits.Where(hosting_unit => hosting_unit.hostingUnitKey == order.hostingUnitKey);
            v.ToList();

            if (v == null)
            {
                throw new DoesNotExistException("this unit does not exist");
            }

            List<GuestRequest> GuestRequests = get_All_Guests();
            var u = GuestRequests.Where(gs => gs.guestRequestKey == order.guestRequestKey);
            u.ToList();

            if (u == null)
            {
                throw new DoesNotExistException("this guest request does not exist");
            }

            DateTime t1 = u.First().EntryDate;
            DateTime t2 = u.First().ReleaseDate;
            TimeSpan T3 = t2 - t1;

            for (int i = 0; i < T3.Days; i++)
            {
                if ((v.First().Diary)[t1.Month, t1.Day])
                {
                    count++;
                }
                t1.AddDays(i);
            }
            if (count == 0)
                return true;
            return false;
        }

        public int calcDays(long guestreqKey)
        {
            List<GuestRequest> GuestRequests = get_All_Guests();
            var u = GuestRequests.Where(gs => gs.guestRequestKey == guestreqKey);
            u.ToList();

            if (u == null)
            {
                throw new DoesNotExistException("this guest request does not exist");
            }

            DateTime t1 = u.First().EntryDate;
            DateTime t2 = u.First().ReleaseDate;
            TimeSpan T3 = t2 - t1;

            return T3.Days;
        }

        public int calcFee(Order order)
        {
            List<GuestRequest> GuestRequests = get_All_Guests();
            var u = GuestRequests.Where(gs => gs.guestRequestKey == order.guestRequestKey);
            u.ToList();

            if (u == null)
            {
                throw new DoesNotExistException("this guest request does not exist");
            }

            DateTime t1 = u.First().EntryDate;
            DateTime t2 = u.First().ReleaseDate;
            TimeSpan T3 = t2 - t1;

            return (int)(T3.Days * Configuration._Fee);
        }

        public void markAsBooked(Order order)
        {
            List<HostingUnit> HostingUnits = get_All_HostingUnits();
            var v = HostingUnits.Where(hosting_unit => hosting_unit.hostingUnitKey == order.hostingUnitKey);
            v.ToList();

            if (v == null)
            {
                throw new DoesNotExistException("this unit does not exist");
            }

            List<GuestRequest> GuestRequests = get_All_Guests();
            var u = GuestRequests.Where(gs => gs.guestRequestKey == order.guestRequestKey);
            u.ToList();

            if (u == null)
            {
                throw new DoesNotExistException("this guest request does not exist");
            }

            DateTime t1 = u.First().EntryDate;
            DateTime t2 = u.First().ReleaseDate;
            TimeSpan T3 = t2 - t1;

            for (int i = 0; i < T3.Days; i++)
            {
                ((v.First().Diary)[t1.Month, t1.Day]) = true;
                t1.AddDays(i);
            }
            update_HostingUnit(v.First());
        }

        public void markGuestRequestStatus(Order order)
        {
            List<GuestRequest> guest_req = get_All_Guests();
            var v = guest_req.Where(gs => gs.guestRequestKey == order.guestRequestKey);
            v.ToList();

            if (v == null)
            {
                throw new DoesNotExistException("this guest request does not exist");
            }
            v.First().Status = BE.Enum.GuestRequestStatus.DEAL_CLOSED_VIA_WEB; // changed guest request's status

            update_GuestRequest(v.First());
            List<Order> _orders = get_All_Orders();
            var ORDER = _orders.Where(ordr => ordr.guestRequestKey == order.guestRequestKey);
            ORDER.ToList(); // all orders of the same guest

            if (ORDER == null)
                throw new DoesNotExistException("there must be at least 1 order of this guest");

            foreach (var item in ORDER)
            {
                item.Status = BE.Enum.OrderStatus.CLOSED_REPLIED;
            }

        }

        public bool ApproveRequest(bool[,] Diary, DateTime start, DateTime release, int numOfDays)//helper func to see if the unit unbooked
        {
            DateTime tmp1 = start;
            DateTime tmp2 = start;
            for (int i = 0; i < (numOfDays - 1); i++)//-1 for num of  nights, checks
            {
                if (Diary[tmp1.Month, tmp1.Day] == true)//means it all booked
                {
                    return false;
                }
                tmp1.AddDays(i);//going all over the days
            }
            for (int i = 0; i < (numOfDays - 1); i++)//marking the dates at the Diary
            {

                Diary[tmp2.Month, tmp2.Day] = true;
                tmp2.AddDays(i);
            }

            return true;
        }

        public int dates_subtraction(params DateTime[] dates)//Duration between 2 dates 
        {
            DateTime start = dates[0];
            TimeSpan duration;
            if (!(dates[1] == null))//if there is another date
            {
                DateTime end = dates[1];
                duration = end - start;
                return duration.Days;
            }

            duration = DateTime.Now - start;
            return duration.Days;

        }
        #endregion

        #region Guest Request
        public void add_GuestRequest(BE.GuestRequest addReq)
        {

            
            if (is_valid_dates(addReq))
            {
                try
                {
                    addReq.RegistrationDate = DateTime.Now;
                    myDal.add_GuestRequest(addReq);
                } 
                catch(CanNotLoadFileException ex)
                {
                    throw new CanNotLoadFileInBlException(ex.Message);
                }
                catch(NegativeNumberException ex)
                {
                    throw new NegativeNumberInBLException(ex.Message);
                }
                catch (DuplicatedKeyException ex)
                {
                    throw new  DoubleKeyException(ex.Message);
                    //throw new DuplicatedKeyInBLException(ex.Message);
                }               
            }
            else
                throw new NotValidException("entry date must be at least 1 day before release date ");
        }

        public void update_GuestRequest(BE.GuestRequest upReq)
        {
            if (is_valid_dates(upReq))//time span
            {
                try
                {
                    myDal.update_GuestRequest(upReq);
                }
                catch (CanNotLoadFileException ex)
                {
                    throw new CanNotLoadFileInBlException(ex.Message);
                }
                catch (KeyDoesNotExistException ex)
                {
                    throw new DoesNotExistException(ex.Message);
                }
                catch(NegativeNumberException ex)
                {
                    throw new NegativeNumberInBLException(ex.Message);
                }
            }
            else
                throw new NotValidException("entry date must be at least 1 day before release date ");
        }
        #endregion

        #region Hosting Unit
        public void add_HostingUnit(HostingUnit addUnit)
        {
            try
            {
                myDal.add_HostingUnit(addUnit);
            }
            catch(DuplicatedKeyException ex)
            {
                throw new DoubleKeyException(ex.Message);
                //throw new DuplicatedKeyInBLException("this hosting unit already exists");
            }
            catch (CanNotLoadFileException ex)
            {
                throw new CanNotLoadFileInBlException(ex.Message);
            }
        }

        public void update_HostingUnit(HostingUnit updateUnit)
        {
            List<HostingUnit> units = get_All_HostingUnits();
            var unitExists = units.Where(u => u.hostingUnitKey == updateUnit.hostingUnitKey);
            unitExists.ToList();
            if (unitExists == null)
            {
                throw new DoesNotExistException("this unit does not exist so it can not be updated");
            }
            HostingUnit OriginalUnit = unitExists.First();

            if (OriginalUnit.Owner.CollectionClearance != updateUnit.Owner.CollectionClearance && updateUnit.Owner.CollectionClearance == false)
            {
                var hostingUnitsOfSameOwner = units.Where(u => u.Owner.hostKey == updateUnit.Owner.hostKey); // all units that has the same oowner
                if (hostingUnitsOfSameOwner == null) // if this owner has only one unit
                {
                    List<Order> orders = get_All_Orders();
                    var ordersOfOriginalUnit = orders.Where(ordr => ordr.hostingUnitKey == updateUnit.hostingUnitKey); // we'll go through all of this unit's orders
                    ordersOfOriginalUnit.ToList();

                    if (ordersOfOriginalUnit == null)// if there's none
                    {
                        try
                        {
                            myDal.update_HostingUnit(updateUnit);// we don't need to check if the orders are closed
                            return;
                        }
                        catch (CanNotLoadFileException ex)
                        {
                            throw new CanNotLoadFileInBlException(ex.Message);
                        }
                        catch (KeyDoesNotExistException ex)
                        {
                            throw new DoesNotExistException(ex.Message);//Console.WriteLine("this unit does not exist");
                        }
                    }
                    else // if there are orders of this single unit 
                    {
                        foreach (var item in ordersOfOriginalUnit) // we'll go through all of them and check if they're closed
                        {
                            if (item.Status == BE.Enum.OrderStatus.EMAIL_SENT || item.Status == BE.Enum.OrderStatus.NOT_YET)
                                throw new CanNotRemoveCollectionClearanceException("can not remove collection clearance while there are upen orders");
                        }
                        try
                        {
                            myDal.update_HostingUnit(updateUnit); // only if they're closed we'll update the unit
                            return;
                        }
                        catch (KeyDoesNotExistException ex)
                        {
                            throw new DoesNotExistException(ex.Message); //Console.WriteLine("this unit does not exist");
                        }
                        catch (CanNotLoadFileException ex)
                        {
                            throw new CanNotLoadFileInBlException(ex.Message);
                        }
                    }
                }
                else // if there are more units of same owner
                {
                    foreach (var item in hostingUnitsOfSameOwner) // for each of the units that has the same owner
                    {
                        List<Order> orders = get_All_Orders();
                        var ordersOfCurrentlUnit = orders.Where(ordr => ordr.hostingUnitKey == item.hostingUnitKey); // we'll collect all of the orders of this current unit
                        ordersOfCurrentlUnit.ToList();

                        if (ordersOfCurrentlUnit != null)  // if the current unit has orders
                        {
                            foreach (var item1 in ordersOfCurrentlUnit) // we'll go through all of them to check if they're closed
                            {
                                if (item1.Status == BE.Enum.OrderStatus.EMAIL_SENT || item1.Status == BE.Enum.OrderStatus.NOT_YET)
                                    throw new CanNotRemoveCollectionClearanceException("can not remove collection clearance while there are upen orders");
                            }
                        }
                    }
                    try // if we went through all units and saw that all of the orders of ech and every unit- are closed
                    {
                        myDal.update_HostingUnit(updateUnit); // we can update the unit
                        return;
                    }
                    catch (KeyDoesNotExistException ex)
                    {
                        throw new DoesNotExistException(ex.Message);   //Console.WriteLine("this unit does not exist");
                    }
                    catch (CanNotLoadFileException ex)
                    {
                        throw new CanNotLoadFileInBlException(ex.Message);
                    }
                }
            }
            else
            {
                try
                {
                    myDal.update_HostingUnit(updateUnit); // only if they're closed we'll update the unit
                    return;
                }
                catch (KeyDoesNotExistException ex)
                {
                    throw new DoesNotExistException(ex.Message); //Console.WriteLine("this unit does not exist");
                }
                catch (CanNotLoadFileException ex)
                {
                    throw new CanNotLoadFileInBlException(ex.Message);
                }
            }

        }

        public void delete_HostingUnit(HostingUnit deleteUnit)
        {
            List<Order> orders = get_All_Orders();

            if(orders.Count==0)
            {
                try
                {
                    myDal.delete_HostingUnit(deleteUnit);
                    return;
                }
                catch (KeyDoesNotExistException ex)
                {
                    throw new DoesNotExistException(ex.Message);
                }
                catch (CanNotLoadFileException ex)
                {
                    throw new CanNotLoadFileInBlException(ex.Message);
                }
            }

            var v = orders.Where(order => order.hostingUnitKey == deleteUnit.hostingUnitKey);
            // var v = get_All_HostingUnits().Where(h => h.hostingUnitKey == deleteUnit.hostingUnitKey);
            v.ToList();
            if (v != null)//there is a unit and it can't be deleted
                throw new CanNotDeleteException("this unit can not be deleted as long as some of the related orders are not settled");

            try
            {
                myDal.delete_HostingUnit(deleteUnit);
            }
            catch(KeyDoesNotExistException ex)
            {
                throw new DoesNotExistException(ex.Message);
            }
            catch (CanNotLoadFileException ex)
            {
                throw new CanNotLoadFileInBlException(ex.Message);
            }
        }
        #endregion

        #region Order
        public void add_Order(Order addOrder)
        {
            try
            {
                checkIfGuestAndUnitExists(addOrder);
            }
            catch(DoesNotExistException ex)
            {
                throw ex;
            }
           
            var gs = get_All_Guests().Where(g => g.guestRequestKey == addOrder.guestRequestKey);
            gs.ToList();

            if (gs.FirstOrDefault().Status == BE.Enum.GuestRequestStatus.DEAL_CLOSED_EXPIRED || gs.FirstOrDefault().Status == BE.Enum.GuestRequestStatus.DEAL_CLOSED_VIA_WEB)
                throw new NotValidException("this guest request was closed, therefore you can not make an order with it!");

            if (addOrder.Status == BE.Enum.OrderStatus.NOT_YET)//still open
            {
                try
                {
                    if (!is_collection_clearance(addOrder))
                        throw new ChangeCollectionClearanceException("order could not be added-host did not confirm collection clearance- try to order another unit");
                }
                catch(DoesNotExistException ex)
                {
                    throw ex;
                }
            }

            try
            {
                if (is_unit_unbooked(addOrder) && is_collection_clearance(addOrder))//if the owner has "collection clearance and all the days are clear- he can add the booking
                {
                    addOrder.Status = BE.Enum.OrderStatus.EMAIL_SENT;
                    addOrder.OrderDate = DateTime.Now;
                     
                    try
                    {
                        myDal.add_Order(addOrder);//advancing at the days 
                        markAsBooked(addOrder);
                    }
                    catch (DuplicatedKeyException ex)
                    {
                        throw new NotValidException(ex.Message);
                    }
                    catch (CanNotLoadFileException ex)
                    {
                        throw new CanNotLoadFileInBlException(ex.Message);
                    }
                    catch (DoesNotExistException ex)
                    {
                        throw ex;
                    }
                }
                else
                {
                    if (is_unit_unbooked(addOrder))/* && !is_collection_clearance(addOrder) && addOrder.Status == BE.Enum.OrderStatus.CLOSED_REPLIED && addOrder.Status == BE.Enum.OrderStatus.CLOSED_NOT_REPLIED)//all the other orders must be shown*/
                    {
                        try
                        {
                            myDal.add_Order(addOrder);//advancing at the days
                            markAsBooked(addOrder);
                        }
                        catch (DuplicatedKeyException ex)
                        {
                            throw new NotValidException(ex.Message);
                        }
                        catch (CanNotLoadFileException ex)
                        {
                            throw new CanNotLoadFileInBlException(ex.Message);
                        }
                        catch (DoesNotExistException ex)
                        {
                            throw ex;
                        }
                    }
                    else
                    {
                        if (!is_unit_unbooked(addOrder))
                            throw new UnitBookedException("The unit is occupied in these days ");
                    }
                }
            }
            catch(DoesNotExistException ex)
            {
                throw ex;
            }
        }

        public void update_Order(Order updateOrder)
        {
            var gs = get_All_Guests().Where(g => g.guestRequestKey == updateOrder.guestRequestKey);
            gs.ToList();

            if (gs.FirstOrDefault().Status == BE.Enum.GuestRequestStatus.DEAL_CLOSED_EXPIRED || gs.FirstOrDefault().Status == BE.Enum.GuestRequestStatus.DEAL_CLOSED_VIA_WEB)
                throw new NotValidException("this guest request was closed, therefore you can not make an order with it!");

            if (updateOrder.Status == BE.Enum.OrderStatus.CLOSED_NOT_REPLIED || updateOrder.Status == BE.Enum.OrderStatus.CLOSED_REPLIED)
                throw new NotValidException("you can not update a closed order!");// if the order is closed we can't update it!          

            if (updateOrder.Status == BE.Enum.OrderStatus.EMAIL_SENT || updateOrder.Status == BE.Enum.OrderStatus.NOT_YET)//still open
            {
                try
                {
                    if (!is_collection_clearance(updateOrder))
                        throw new ChangeCollectionClearanceException("order could not be added-host did not confirm collection clearance- try to order another unit");
                }
                catch (DoesNotExistException ex)
                {
                    throw ex;
                }
            }

            if (updateOrder.Status == BE.Enum.OrderStatus.EMAIL_SENT)//if the owner has "collection clearance and all the days are clear- he can add the booking
            {
                updateOrder.Status = BE.Enum.OrderStatus.CLOSED_REPLIED;

                //advancing at the days                
                try
                {
                    int fee = calcFee(updateOrder);
                    markAsBooked(updateOrder);
                    markGuestRequestStatus(updateOrder);
                    myDal.update_Order(updateOrder);
                }
                catch (KeyDoesNotExistException ex)
                {
                    throw new DoesNotExistException(ex.Message);
                }
                catch (CanNotLoadFileException ex)
                {
                    throw new CanNotLoadFileInBlException(ex.Message);
                }
                catch (DoesNotExistException ex)
                {
                    throw ex;
                }
            }
            else
            {
                updateOrder.Status = BE.Enum.OrderStatus.CLOSED_NOT_REPLIED;
                try
                {
                    myDal.update_Order(updateOrder);
                }
                catch (KeyDoesNotExistException ex)
                {
                    throw new DoesNotExistException(ex.Message);
                }
                catch (CanNotLoadFileException ex)
                {
                    throw new CanNotLoadFileInBlException(ex.Message);
                }
            }
        }
        #endregion

        #region Lists And Sheiltot
        public List<HostingUnit> get_All_HostingUnits()
        {
            return myDal.get_All_HostingUnits();
        }

        public List<Order> get_All_Orders()
        {
            return myDal.get_All_Orders();
        }

        public List<GuestRequest> get_All_Guests()
        {
            return myDal.get_All_Guests();
        }

        public List<BankBranch> get_All_Bank_Branches()
        {
            return myDal.get_All_Bank_Branches();
        }

        public List<Host> get_All_Host()
        {
            List<HostingUnit> hostingUnitsList = myDal.get_All_HostingUnits();
            List<Host> hostList = new List<Host>();

            foreach (var item in hostingUnitsList)
            {
                Host owner = new Host();
                owner = item.Owner.Clone(); // to avoid shalow copy
                hostList.Add(owner);
            }

            List < Host > UnDuplicatedHostsList = new List<Host>();
       
            foreach(Host h1 in hostList)
            {
                if (UnDuplicatedHostsList.Count != 0)
                {
                    int count = 0;

                    foreach (Host h2 in UnDuplicatedHostsList)
                    {
                        if (h2.hostKey == h1.hostKey)
                            count++;
                    }
                    if (count == 0)
                        UnDuplicatedHostsList.Add(h1);

                }
                else
                    UnDuplicatedHostsList.Add(h1);
            }
               
            return UnDuplicatedHostsList;
        }

        public List<HostingUnit> unbooked_units(DateTime start, int numOfDays)//returning a list with all the unbooked units accrodding to start days and duration of time
        {
            DateTime release = new DateTime(start.Year, start.Month, start.Day);
            release.AddDays(numOfDays);//creating the release date
            List<HostingUnit> hostingUnits_list = get_All_HostingUnits().FindAll(host => ApproveRequest(host.Diary, start, release, numOfDays) == true);
            return hostingUnits_list;
        }
       
        public List<Order> duration_of_orders(int numOfDays)//returns all orders that the duration that passed (sience they were created / a mail has been sent to the customer) >= to numOfDays 
        {
            List<Order> orders = get_All_Orders();
            var list_orders = from item in orders
                              where (((DateTime.Now - item.CreateDate).Days >= numOfDays) && (item.Status == BE.Enum.OrderStatus.NOT_YET) ||      //all orders that were created
                                      (((DateTime.Now - item.OrderDate).Days >= numOfDays) && (item.Status == BE.Enum.OrderStatus.EMAIL_SENT)))  //all orders that a mail has been sent
                              select item;

            return (list_orders.ToList());
        }

        public List<GuestRequest> filter_Guests_by_terms(Func<GuestRequest, bool> terms)//according to some terms will create filtered list of guest request
        {
            List<GuestRequest> gr = get_All_Guests();
            var filterGR = from item in gr
                           where (terms(item))
                           orderby item.FamilyName, item.PrivateName
                           select item;

            return (filterGR.ToList());
        }

        public int num_booking_toGuest(GuestRequest gr)//returns num of bookings accroding to some guest request
        {
            List<Order> or = get_All_Orders();
            var numBookings = or.FindAll(booking => (booking.guestRequestKey == gr.guestRequestKey));

            return numBookings.Count();
        }

        //int num_of_booking_per_unit(HostingUnit h)//from the website
        //{
        //    List<Order> or = get_All_Orders();
        //    var numBookings = or.FindAll(booking => (booking.hostingUnitKey == h.hostingUnitKey));
        //    var numBookings2 = or.FindAll(booking => (booking.hostingUnitKey == h.hostingUnitKey));
        //    int a = numBookings.Count() + numBookings2.Count();

        //    return a;
        //}

        public int num_of_booking_per_unit(HostingUnit h)//returning num of booking that were sent / were closed throgh the web
        {
            if (get_All_HostingUnits().Count == 0)
                throw new DoesNotExistException("there are no hosting units currently, therefore we can not group them!"); 

            List<Order> or = get_All_Orders();
            var numOfBooking = from item in or
                               let key = item.hostingUnitKey
                               where ((key == h.hostingUnitKey) && ((item.Status == BE.Enum.OrderStatus.CLOSED_REPLIED) || item.Status == BE.Enum.OrderStatus.EMAIL_SENT))
                               select item;

            return numOfBooking.Count();
        }

        //List<IGrouping<BE.Enum.AreaOfV, GuestRequest>> group_of_GuestRequest_by_Area()
        //{
        //    List<GuestRequest> req = get_All_Guests();
        //    IEnumerable<IGrouping<BE.Enum.AreaOfV, GuestRequest>> result = from item in req
        //                                                                   group item by item.Area;

        //    return result.ToList();
        //

        // grouping:

        public List<IGrouping<BE.Enum.subArea, GuestRequest>> group_of_GuestRequest_by_Area()
        {
            if (get_All_Guests().Count == 0)
                throw new DoesNotExistException("there are no guest requests currently, therefore we can not group them!");

            return (from guest in myDal.get_All_Guests()
                    orderby guest.guestRequestKey
                    group guest by guest.SubArea
                   into g
                    orderby g.Key
                    select g).ToList();
        }

        public List<IGrouping<int, GuestRequest>> group_of_GuestRequest_by_num_of_guests()
        {
            if (get_All_Guests().Count == 0)
                throw new DoesNotExistException("there are no guest requests currently, therefore we can not group them!");

            return (from item in myDal.get_All_Guests()
                    orderby item.guestRequestKey
                    group item by (item.Adults + item.Children)/*num_booking_toGuest(item)*/
                   into g
                    orderby g.Key
                    select g).ToList();
        }

        public List<IGrouping<int, BE.HostingUnit>> group_of_Hosts_by_their_num_of_HostingUnits()
        {
            return (from hostingunit in myDal.get_All_HostingUnits()
                    orderby hostingunit.Owner.hostKey
                    group hostingunit by (int)hostingunit.Owner.hostKey
                   into g
                    orderby g.Key
                    select g).ToList();
        }

        public List<IGrouping<BE.Enum.subArea, BE.HostingUnit>> group_of_HostingUnits_by_Area()
        {
            if (get_All_HostingUnits ().Count == 0)
                throw new DoesNotExistException("there are no hosting units currently, therefore we can not group them!"); 

            return (from hostingU in myDal.get_All_HostingUnits()
                    orderby hostingU.hostingUnitKey
                    group hostingU by hostingU.SubArea
                    into g
                    orderby g.Key
                    select g).ToList();
        }

        public List<IGrouping<BE.Enum.UnitType, GuestRequest>> group_of_GuestRequests_by_unitType()
        {
            if (get_All_Guests().Count == 0)
                throw new DoesNotExistException("there are no guest requests currently, therefore we can not group them!");

            return (from guestRequest in myDal.get_All_Guests()
                    orderby guestRequest.guestRequestKey
                    group guestRequest by guestRequest.Type
                   into g
                    orderby g.Key
                    select g).ToList();
        }

        public List<IGrouping<string, BankBranch>> group_of_BankBranches_by_BankNames()
        {
            return (from bank in myDal.get_All_Bank_Branches()
                    orderby bank.branchNumber
                    group bank by bank.BankName
                   into g
                    orderby g.Key
                    select g).ToList();
        }

        public List<IGrouping<long, HostingUnit>> group_of_HostingUnits_by_their_Hosts()
        {
            if (get_All_HostingUnits().Count == 0)
                throw new DoesNotExistException("there are no hosting units currently, therefore we can not group them!");

            return (from item in myDal.get_All_HostingUnits()
                    orderby item.hostingUnitKey
                    group item by item.Owner.hostKey
                   into g
                    orderby g.Key
                    select g).ToList();
        }
        #endregion

        #region Mail 

        public void sendMailToGuest(string from, string to, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(to);  // we are sending the mail to the guest-about the order of the unit which were scheduled
            mail.From = new MailAddress(from); // the owner sends it to the guest
            mail.Subject = subject;
            mail.Body = body;

            //mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new System.Net.NetworkCredential(Configuration.MailAddress, Configuration.MailPassWord);
            //smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
           
            //try
            //{
                smtp.Send(mail);
            //}
            //catch (SmtpException)
            //{
            //    throw new NotValidException("error occurred while connecting to server");
            //}
            //catch(InvalidOperationException)
            //{
            //    throw new NotValidException("Worng Mail Address");
            //}
        }

        #endregion

        #region clearOrdersThread

        private void clearOrdersFunc()
        {
            List<Order> Orders = get_All_Orders();

            if (Orders == null)
                return;
            
            foreach(Order order in Orders)
            {
                if(order.OrderDate.AddMonths(1) <= DateTime.Now) // determins wether or not at least a month has passed since order date
                {
                    order.Status = BE.Enum.OrderStatus.CLOSED_NOT_REPLIED;

                    var guestRequestsOfSameOrder = get_All_Guests().Where(gs => gs.guestRequestKey == order.guestRequestKey);
                    guestRequestsOfSameOrder.ToList();
                     
                    if(guestRequestsOfSameOrder != null)
                    {
                        foreach(GuestRequest g in guestRequestsOfSameOrder)
                        {
                            g.Status = BE.Enum.GuestRequestStatus.DEAL_CLOSED_EXPIRED;
                        }
                    }
                }
            }  
        }

        #endregion

        #region Bank Branch

        public List<long> GetBrachNumbers(string bankName)
        {
            try
            {
                var v = from branch in get_All_Bank_Branches()
                        where branch.BankName == bankName
                        select branch.branchNumber;

                List<long> branchesNumbers = new List<long>();
                v.ToList(); 

                foreach (var number in v)
                {
                    if (!branchesNumbers.Contains(number))
                        branchesNumbers.Add(number);
                }

                branchesNumbers.Sort();
                return branchesNumbers;
            }
            catch (CanNotLoadFileException ex)
            {
                throw new DoesNotExistException(ex.Message);
            }
        }

        public List<string> GetBankNames()
        {
            try
            {
                var v = from branch in get_All_Bank_Branches()
                        select branch.BankName;

                v.ToList();
                List<string> newList = new List<string>();

                foreach(var branch in v)
                {                   
                    if (!newList.Contains(branch))
                        newList.Add(branch);
                  
                    if(branch.Contains("\n"))
                        newList.RemoveAt(newList.Count - 1);
                }
                 
                return newList;
            }
            catch(CanNotLoadFileException ex)
            {
                throw new DoesNotExistException(ex.Message);
            }
        }

        public BE.BankBranch GetBranchByNumberAndName(long number, string name)
        {
            try
            {
                foreach (BE.BankBranch branch in get_All_Bank_Branches())
                    if (branch.branchNumber == number && branch.BankName == name)
                        return branch;
                return new BE.BankBranch();
            }
            catch (CanNotLoadFileException ex)
            {
                throw new DoesNotExistException(ex.Message);
            }

        }
        #endregion

        //public List<IGrouping<Host, Order>> group_of_Orders_by_Hosts()
        // {
        //     return (from item in group_of_Hosts_by_their_num_of_HostingUnits()
        //             group item by item.
        // }
           
    }

}
