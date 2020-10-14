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
    /// Interaction logic for GuestRequestList.xaml
    /// </summary>
    public partial class GuestRequestList : Window
    {
        // BE.GuestRequest gr;
        IBL myBL;
        public GuestRequestList()
        {
            InitializeComponent();
            myBL = FactoryBL.getBL();
        }
        
        //private void GuestRequestList_dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
            
        //}

        private void GuestRequestList_combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (GuestRequestList_combobox.SelectedIndex == 0)
                {
                    GuestRequestList_dataGrid.Items.Clear();
                    GuestRequestList_dataGrid.CanUserResizeColumns = false;
                    GuestRequestList_dataGrid.CanUserResizeRows = false;
                    GuestRequestList_dataGrid.CanUserReorderColumns = false;
                    GuestRequestList_dataGrid.CanUserSortColumns = false;

                    var a = myBL.group_of_GuestRequest_by_num_of_guests();
                    foreach (var group in a)
                    {
                        foreach (var gs in group)
                        {
                            GuestRequestList_dataGrid.Items.Add(gs);
                        }
                        GuestRequestList_dataGrid.Items.Add(0);
                    }

                }
                if (GuestRequestList_combobox.SelectedIndex == 1)
                {
                    GuestRequestList_dataGrid.Items.Clear();
                    GuestRequestList_dataGrid.CanUserResizeColumns = false;
                    GuestRequestList_dataGrid.CanUserResizeRows = false;
                    GuestRequestList_dataGrid.CanUserReorderColumns = false;
                    GuestRequestList_dataGrid.CanUserSortColumns = false;

                    var a = myBL.group_of_GuestRequest_by_Area();
                    foreach (var group in a)
                    {
                        foreach (var gs in group)
                        {
                            GuestRequestList_dataGrid.Items.Add(gs);
                        }
                        GuestRequestList_dataGrid.Items.Add(0);
                    }
                }

                if (GuestRequestList_combobox.SelectedIndex == 2)
                {
                    GuestRequestList_dataGrid.Items.Clear();
                    GuestRequestList_dataGrid.CanUserResizeColumns = false;
                    GuestRequestList_dataGrid.CanUserResizeRows = false;
                    GuestRequestList_dataGrid.CanUserReorderColumns = false;
                    GuestRequestList_dataGrid.CanUserSortColumns = false;

                    var a = myBL.group_of_GuestRequests_by_unitType();
                    foreach (var group in a)
                    {
                        foreach (var gs in group)
                        {
                            GuestRequestList_dataGrid.Items.Add(gs);
                        }
                        GuestRequestList_dataGrid.Items.Add(0);
                    }
                }

                if (GuestRequestList_combobox.SelectedIndex == 3)
                {
                    GuestRequestList_dataGrid.Items.Clear();
                    GuestRequestList_dataGrid.CanUserResizeColumns = false;
                    GuestRequestList_dataGrid.CanUserResizeRows = false;
                    GuestRequestList_dataGrid.CanUserReorderColumns = false;
                    GuestRequestList_dataGrid.CanUserSortColumns = false;

                    var a = myBL.get_All_Guests();
                    foreach (var item in a)
                    {
                        GuestRequestList_dataGrid.Items.Add(item);
                    }
                }
            }
            catch (DoesNotExistException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

}