using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BL;

namespace PL
{
    /// <summary>
    /// Interaction logic for UpdateOrder.xaml
    /// </summary>
    /// 
     
    public partial class UpdateOrder : Window
    {
        BE.Order order;
        IBL bl;
        public UpdateOrder()
        {
            InitializeComponent();
            order = new BE.Order();
            DataContext = order;
            bl = BL.FactoryBL.getBL();

            Status_block.DataContext = order.Status;
            OrderKey_block.DataContext = order.OrderKey;
            HostingUnit_block.DataContext = order.HostingUnitKey;
            GuestRequestKey_block.DataContext = order.GuestRequestKey;


        }
    }
}
