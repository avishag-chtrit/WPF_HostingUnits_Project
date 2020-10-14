using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
namespace BL
{
    public interface IBL
    {
        void add_GuestRequest(GuestRequest addReq);
        void update_GuestRequest(BE.GuestRequest addReq);

        void add_HostingUnit(HostingUnit addUnit);//adding hosting unit
        void update_HostingUnit(HostingUnit updateUnit);//updating hosting unit
        void delete_HostingUnit(HostingUnit deleteUnit);//deleting hosting unit

        void add_Order(Order addOrder);//add order
        void update_Order(Order updateOrder);//update order

        List<HostingUnit> get_All_HostingUnits();//getting all hosting unit lists
        List<Order> get_All_Orders();
        List<GuestRequest> get_All_Guests();// we can get all costomers by all of the guest requests
        List<BankBranch> get_All_Bank_Branches(); //we can get all the banks by all  of the bank accounts :)
        List<Host> get_All_Host();
        //--------------------
        bool is_collection_clearance(Order order);
        bool is_unit_unbooked(Order order);
        bool is_valid_dates(GuestRequest gs);
        int calcFee(Order order);
        void markAsBooked(Order order);
        void markGuestRequestStatus(Order order);
       // void change_collection_clearance(Order order);
        void checkIfGuestAndUnitExists(Order order);
        int calcDays(long guestreqKey);
        //....................

        bool ApproveRequest(bool[,] Diary, DateTime start, DateTime release, int numOfDays);//helper to "unbookedUnits"
        List<HostingUnit> unbooked_units(DateTime start, int numOfDays);

        int dates_subtraction(params DateTime[] dates);//one date or two
        List<Order> duration_of_orders(int numOfDays);


        List<GuestRequest> filter_Guests_by_terms(Func<GuestRequest, bool> terms);
        int num_booking_toGuest(GuestRequest gr);//costumer requirements
        int num_of_booking_per_unit(HostingUnit h);//from the website

        // grouping functions:

        List<IGrouping<BE.Enum.subArea, GuestRequest>> group_of_GuestRequest_by_Area();

        List<IGrouping<int, GuestRequest>> group_of_GuestRequest_by_num_of_guests();

        List<IGrouping<int, BE.HostingUnit>> group_of_Hosts_by_their_num_of_HostingUnits();

        List<IGrouping<BE.Enum.subArea, BE.HostingUnit>> group_of_HostingUnits_by_Area();

        List<IGrouping<BE.Enum.UnitType, GuestRequest>> group_of_GuestRequests_by_unitType();

        List<IGrouping<string, BankBranch>> group_of_BankBranches_by_BankNames();

        List<IGrouping<long, HostingUnit>> group_of_HostingUnits_by_their_Hosts();
        //List<IGrouping<Host, Order>> group_of_Orders_by_Hosts();

        void sendMailToGuest(string from, string to, string subject, string body);

        List<long> GetBrachNumbers(string bankName);

        List<string> GetBankNames();

        BE.BankBranch GetBranchByNumberAndName(long number, string name);
    }
}
