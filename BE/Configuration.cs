using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Configuration// initializing all global static variable
    {
        public static long _GuestRequestKey = 10000000; // initializing hosti ng unit number
        public static long _HostKey = 10000000;
        public static long _HostingUnitKey = 10000000;
        public static long _OrderKey = 10000000;
        public static long _Fee = 10;
        //public static string TypeDAL = ConfigurationSettings.AppSettings.Get("TypeDS");
        public static string HostKeyFormat = "00000000";

        //------------mail details:
        public static string MailAddress = "hosttzimerproject@gmail.com";
        public static string MailPassWord = "5780_TzimerProject";

        //---------clearing orders satus once a day
        public static string DateToday = (DateTime.Now).ToShortDateString();
        public static bool IsClearOrdrs = false;//did we "cleared" an order status today


    }
}
