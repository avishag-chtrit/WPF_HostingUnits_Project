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
using BE;
using BL;

namespace PL
{
    /// <summary>
    /// Interaction logic for OrderList.xaml
    /// </summary>
    public partial class OrderList : Window
    {
        IBL myBL;
        public OrderList()
        {
            InitializeComponent();
            myBL = FactoryBL.getBL();
        }

        private void OrderList_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           // try
           // {
                if (OrderList_ComboBox.SelectedIndex == 0)
                {
                    OrderList_DataGrid.Items.Clear();
                    OrderList_DataGrid.CanUserResizeColumns = false;
                    OrderList_DataGrid.CanUserResizeRows = false;
                    OrderList_DataGrid.CanUserReorderColumns = false;
                    OrderList_DataGrid.CanUserSortColumns = false;

                    var a = myBL.get_All_Orders();
                    foreach (var item in a)
                    {
                        OrderList_DataGrid.Items.Add(item);
                    }
                }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}

        }
    }
}
