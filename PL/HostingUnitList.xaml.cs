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
    /// Interaction logic for HostingUnitList.xaml
    /// </summary>
    public partial class HostingUnitList : Window
    {
        IBL myBL;
        public HostingUnitList()
        {
            InitializeComponent();
            myBL = FactoryBL.getBL();
        }

        private void HostingUnitList_combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (HostingUnitList_combobox.SelectedIndex == 0)
                {
                    HostingUnitList_DataGrid.Items.Clear();
                    HostingUnitList_DataGrid.CanUserResizeColumns = false;
                    HostingUnitList_DataGrid.CanUserResizeRows = false;
                    HostingUnitList_DataGrid.CanUserReorderColumns = false;
                    HostingUnitList_DataGrid.CanUserSortColumns = false;

                    var a = myBL.group_of_HostingUnits_by_Area();
                    foreach (var group in a)
                    {
                        foreach (var gs in group)
                        {
                            HostingUnitList_DataGrid.Items.Add(gs);
                        }
                        HostingUnitList_DataGrid.Items.Add(0);
                    }

                }
                if (HostingUnitList_combobox.SelectedIndex == 1)
                {
                    HostingUnitList_DataGrid.Items.Clear();
                    HostingUnitList_DataGrid.CanUserResizeColumns = false;
                    HostingUnitList_DataGrid.CanUserResizeRows = false;
                    HostingUnitList_DataGrid.CanUserReorderColumns = false;
                    HostingUnitList_DataGrid.CanUserSortColumns = false;

                    var a = myBL.group_of_HostingUnits_by_their_Hosts();
                    foreach (var group in a)
                    {
                        foreach (var gs in group)
                        {
                            HostingUnitList_DataGrid.Items.Add(gs);
                        }
                        HostingUnitList_DataGrid.Items.Add(0);
                    }
                }

                if (HostingUnitList_combobox.SelectedIndex == 2)
                {
                    HostingUnitList_DataGrid.Items.Clear();
                    HostingUnitList_DataGrid.CanUserResizeColumns = false;
                    HostingUnitList_DataGrid.CanUserResizeRows = false;
                    HostingUnitList_DataGrid.CanUserReorderColumns = false;
                    HostingUnitList_DataGrid.CanUserSortColumns = false;

                    var a = myBL.group_of_Hosts_by_their_num_of_HostingUnits();
                    foreach (var group in a)
                    {
                        foreach (var gs in group)
                        {
                            HostingUnitList_DataGrid.Items.Add(gs);
                        }
                        HostingUnitList_DataGrid.Items.Add(0);
                    }

                }

                if (HostingUnitList_combobox.SelectedIndex == 3)
                {
                    HostingUnitList_DataGrid.Items.Clear();
                    HostingUnitList_DataGrid.CanUserResizeColumns = false;
                    HostingUnitList_DataGrid.CanUserResizeRows = false;
                    HostingUnitList_DataGrid.CanUserReorderColumns = false;
                    HostingUnitList_DataGrid.CanUserSortColumns = false;

                    var a = myBL.get_All_HostingUnits();
                    foreach (var item in a)
                    {
                        HostingUnitList_DataGrid.Items.Add(item);
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
