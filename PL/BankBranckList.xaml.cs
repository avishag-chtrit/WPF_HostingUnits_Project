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
    /// Interaction logic for BankBranckList.xaml
    /// </summary>
    public partial class BankBranckList : Window
    {
        IBL myBL;
        public BankBranckList()
        {
            InitializeComponent();
            myBL = FactoryBL.getBL();
        }

        private void BanlBranchList_comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (BanlBranchList_comboBox.SelectedIndex == 0)
                {
                    BanlBranchList_DataGrid.Items.Clear();
                    BanlBranchList_DataGrid.CanUserResizeColumns = false;
                    BanlBranchList_DataGrid.CanUserResizeRows = false;
                    BanlBranchList_DataGrid.CanUserReorderColumns = false;
                    BanlBranchList_DataGrid.CanUserSortColumns = false;

                    var a = myBL.group_of_BankBranches_by_BankNames();
                    foreach (var group in a)
                    {
                        foreach (var gs in group)
                        {
                            BanlBranchList_DataGrid.Items.Add(gs);
                        }
                        BanlBranchList_DataGrid.Items.Add(0);
                    }

                }
          
                if (BanlBranchList_comboBox.SelectedIndex == 1)
                {
                    BanlBranchList_DataGrid.Items.Clear();
                    BanlBranchList_DataGrid.CanUserResizeColumns = false;
                    BanlBranchList_DataGrid.CanUserResizeRows = false;
                    BanlBranchList_DataGrid.CanUserReorderColumns = false;
                    BanlBranchList_DataGrid.CanUserSortColumns = false;

                    var a = myBL.get_All_Bank_Branches();
                    foreach (var item in a)
                    {
                        BanlBranchList_DataGrid.Items.Add(item);
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
