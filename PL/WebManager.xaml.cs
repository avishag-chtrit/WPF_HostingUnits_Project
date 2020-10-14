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
using BE;

namespace PL
{
    /// <summary>
    /// Interaction logic for WebManager.xaml
    /// </summary>
    public partial class WebManager : Window
    {
        IBL myBL;
        public WebManager()
        {

            InitializeComponent();
            myBL = FactoryBL.getBL();
        }

        private void GuestRequest_button_Click(object sender, RoutedEventArgs e)
        {
            new GuestRequestList().ShowDialog();//blocks the previous window  
        }

        private void HostingUnit_button_Click(object sender, RoutedEventArgs e)
        {
            new HostingUnitList().ShowDialog();
        }

        private void Order_button_Click(object sender, RoutedEventArgs e)
        {
            new OrderList().ShowDialog();
        }

        private void BankBranch_button_Click(object sender, RoutedEventArgs e)
        {
            new BankBranckList().ShowDialog();
        }

        private void MoreFunctions_button_Click(object sender, RoutedEventArgs e)
        {
            new MoreFunctions().ShowDialog();
        }
    }
}
