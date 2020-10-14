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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using BL;
using System.ComponentModel;


namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
  
    public partial class MainWindow : Window
    {
        
        private IBL myBL;

        public MainWindow()
        {
            InitializeComponent();
            myBL = FactoryBL.getBL();                   
        }

         

        private void GuestRequest_button_Click(object sender, RoutedEventArgs e)
        {
            new GuestRequest().ShowDialog();//blocks the previous window       
           // new GuestRequest().ShowDialog();//blocks the previous window   
        }

        private void HostingUnits_button_Click(object sender, RoutedEventArgs e)
        {
            new HostingUnitOptions().ShowDialog();
        }

        private void Order_button_Click(object sender, RoutedEventArgs e)
        {
            new OrderOptions().ShowDialog();
        }

        private void WebManeger_button_Click(object sender, RoutedEventArgs e)
        {
            Window WebManagerEntry = new WebManagerEntry();
            WebManagerEntry.ShowDialog();
            if (WebManagerEntry.DialogResult == true)
            {
                new WebManager().ShowDialog();//open webManager details
            }

        }
    }
}
