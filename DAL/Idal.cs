using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    public interface Idal
    {
        void add_GuestRequest(BE.GuestRequest addReq);
        void update_GuestRequest(BE.GuestRequest addReq);

        void add_HostingUnit(HostingUnit addUnit);//adding hosting unit
        void update_HostingUnit(HostingUnit updateUnit);//updating hosting unit
        void delete_HostingUnit(HostingUnit deleteUnit);//deleting hosting unit

        void add_Order(Order addOrder);//add order
        void update_Order(Order updateOrder);//update order

        List<HostingUnit> get_All_HostingUnits(Func<HostingUnit, bool> predicat = null);//getting all hosting unit lists
        List<Order> get_All_Orders(Func<Order, bool> predicat = null);
        List<GuestRequest> get_All_Guests(Func<GuestRequest, bool> predicat = null);// we can get all costomers by all of the guest requests
        List<BankBranch> get_All_Bank_Branches(Func<BankBranch, bool> predicat = null); //we can get all the banks by all  of the bank accounts :)

        void UpdateConfigFile();

        //???
        // IEnumerable<Course> GetAllCourses(Func<Course, bool> predicat = null);
    }
}
