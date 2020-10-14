using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
   public class Order
   {
        public long hostingUnitKey { set; get; }
        public string HostingUnitKey { get { return hostingUnitKey.ToString("00000000"); } }

        public long guestRequestKey { set; get; }
        public string GuestRequestKey { get { return guestRequestKey.ToString("00000000"); } }

        public long orderKey=Configuration._OrderKey++;
        public string OrderKey { get { return orderKey.ToString("00000000"); } }
        public Enum.OrderStatus Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime OrderDate { get; set; }

        public override string ToString()//details
        {
            return ("Details of Order: \n " + "key of unit: " + hostingUnitKey + "\n" + "key of guest request: " + guestRequestKey + "\n" + "order key: " + orderKey + "\n"
                    + "current satus of order: " + Status + "\n" + "order was created on: " + CreateDate + "\n" + "mail was sent on: " + OrderDate + "\n");
        }

        //public Order(Enum.OrderStatus Status, DateTime CreateDate, long hostingUnitKey, long guestRequestKey)
        //{
        //    orderKey = Configuration._OrderKey++;// the order key will bw created here
        //    this.guestRequestKey = guestRequestKey;//comes from other classes
        //    this.hostingUnitKey = hostingUnitKey;//comes from other classes
        //    this.Status = Status;
        //    this.CreateDate = CreateDate;
        //    //----createdate!!!!
        //}
        public Order()
        {

        }
    }
}
