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
    /// Interaction logic for updateHostingUnit.xaml
    /// </summary>
    public partial class updateHostingUnit : Window
    {
        BE.HostingUnit unit;
        IBL bl;

        public updateHostingUnit()
        {
            InitializeComponent();

            unit = new BE.HostingUnit();
            DataContext = unit;
            bl = BL.FactoryBL.getBL();

            this.UnitToUpdade_comboBox.ItemsSource = bl.get_All_HostingUnits();
            this.UnitToUpdade_comboBox.DisplayMemberPath = "HostingUnitName";//"hostingUnitKey"
            this.UnitToUpdade_comboBox.SelectedValuePath = "hostingUnitKey";

            this.UnitToUpdade_comboBox.SelectedIndex = 0;
        }

        private long GetSelectedUnitKey()
        {
            object key = this.UnitToUpdade_comboBox.SelectedValue;
            if (key == null)
                throw new NoUnitSelectedException("you must select the unit first");
            return (long)key;                      
        }

        //private void UnitToUpdade_comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (sender is ComboBox && ((ComboBox)sender).SelectedIndex > -1)
        //        this.InitializeUnit(GetSelectedUnitKey());
        //}

        //private void InitializeUnit(long unitkey)
        //{
        //    var currentUnit = bl.get_All_HostingUnits().Where(u => u.hostingUnitKey == unitkey);
        //    currentUnit.ToList();
        //    unit = currentUnit.First();

        //    SelectedUnitToUpdate.
        //}



    }
}
