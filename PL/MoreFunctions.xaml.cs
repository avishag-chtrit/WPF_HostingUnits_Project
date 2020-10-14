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
    /// Interaction logic for MoreFunctions.xaml
    /// </summary>
    public partial class MoreFunctions : Window
    {
        IBL myBL;
        BE.HostingUnit hs;
        BE.Order or;
        DateTime start;
        DateTime f1, f2;
        List<HostingUnit> empty_units;
        List<Order> orders;
        int numOfdays1 ,numOfdays2;
        int num;
        public MoreFunctions()
        {
            InitializeComponent();
            myBL = FactoryBL.getBL();
            hs = new BE.HostingUnit();
            or = new BE.Order();

            this.emptyHostingUnitPerDates_StatDate_datepicker.SelectedDate = DateTime.Now;//first func

            this.DatesInterval_1_DatePicker.SelectedDate = DateTime.Now;//second func
            this.DatesInterval_2_DatePicker.SelectedDate = DateTime.Now/*.AddDays(1)*/;


        }

        #region empty hosting unit list

        private void emptyHostingUnitPerDates_CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (myBL.get_All_HostingUnits().Count() == 0 && emptyHostingUnitPerDates_CheckBox.IsChecked == true)
                {
                    emptyHostingUnitPerDates_CheckBox.IsChecked = false;
                    throw new DoesNotExistException("there are no units, therefore you can not see any empty ones.");
                }

                if (myBL.get_All_HostingUnits().Count() != 0 && emptyHostingUnitPerDates_CheckBox.IsChecked == true)
                {
                    emptyHostingUnitPerDates_StatDate_datepicker.Visibility = System.Windows.Visibility.Visible;
                    emptyHostingUnitPerDates_enterDAYS_textbox.Visibility = System.Windows.Visibility.Visible;
                    ok_button.Visibility = System.Windows.Visibility.Visible;
                }
            }
            catch (DoesNotExistException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void emptyHostingUnitPerDates_CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            emptyHostingUnitPerDates_comboBox.Visibility = System.Windows.Visibility.Hidden;
            emptyHostingUnitPerDates_StatDate_datepicker.Visibility = System.Windows.Visibility.Hidden;
            emptyHostingUnitPerDates_enterDAYS_textbox.Visibility = System.Windows.Visibility.Hidden;
            ok_button.Visibility = System.Windows.Visibility.Hidden;

            emptyHostingUnitPerDates_CheckBox.IsChecked = false;
        }

        private void emptyHostingUnitPerDates_comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (emptyHostingUnitPerDates_comboBox.SelectedIndex == -1)
                return;
        }

        private void ok_CheckBox_Click(object sender, RoutedEventArgs e)
        {
            emptyHostingUnitPerDates_comboBox.Visibility = System.Windows.Visibility.Visible;
            start = DateTime.Parse(emptyHostingUnitPerDates_StatDate_datepicker.Text);
            num = int.Parse(emptyHostingUnitPerDates_enterDAYS_textbox.Text);
            empty_units = myBL.unbooked_units(start, num);
            this.emptyHostingUnitPerDates_comboBox.ItemsSource = empty_units;
        }
        #endregion

        #region Days intreval Func
        private void DatesInterval_CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (DatesInterval_CheckBox.IsChecked == true)
            {
                DatesInterval_1_DatePicker.Visibility = System.Windows.Visibility.Visible;
                DatesInterval_2_DatePicker.Visibility = System.Windows.Visibility.Visible;
                DatesInterval_button.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void DatesInterval_CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            DatesInterval_1_DatePicker.Visibility = System.Windows.Visibility.Hidden;
            DatesInterval_2_DatePicker.Visibility = System.Windows.Visibility.Hidden;
            DatesInterval_button.Visibility = System.Windows.Visibility.Hidden;
            DatesInterval_Textblock.Visibility = System.Windows.Visibility.Hidden;

            DatesInterval_CheckBox.IsChecked = false;
        }

        private void DatesInterval_button_Click(object sender, RoutedEventArgs e)
        {
            DatesInterval_Textblock.Visibility = System.Windows.Visibility.Visible;
            f1 = DateTime.Parse(DatesInterval_1_DatePicker.Text);
            f2 = DateTime.Parse(DatesInterval_2_DatePicker.Text);
            numOfdays1 = myBL.dates_subtraction(f1, f2);

            this.DatesInterval_Textblock.Text = numOfdays1.ToString();
        }


        #endregion

        #region Orders func
        private void DurationOfOrders_checkbox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((myBL.get_All_Orders().Count() == 0 && DurationOfOrders_checkbox.IsChecked == true))
                {
                    DurationOfOrders_checkbox.IsChecked = false;
                    throw new DoesNotExistException("there are no orders, therefore you can not see any empty ones.");
                }
                if (myBL.get_All_Orders().Count() != 0 && DurationOfOrders_checkbox.IsChecked == true)
                {
                    DurationOfOrders_textbox.Visibility = System.Windows.Visibility.Visible;
                    DurationOfOrders_button.Visibility = System.Windows.Visibility.Visible;
                }
            }
            catch (DoesNotExistException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void DurationOfOrders_checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            DurationOfOrders_textbox.Visibility = System.Windows.Visibility.Hidden;
            DurationOfOrders_button.Visibility = System.Windows.Visibility.Hidden;
            DurationOfOrders_comboBox.Visibility = System.Windows.Visibility.Hidden;

            DurationOfOrders_checkbox.IsChecked = false;
        }

        private void DurationOfOrders_button_Click(object sender, RoutedEventArgs e)
        {
            DurationOfOrders_comboBox.Visibility = System.Windows.Visibility.Visible;
            numOfdays2 = int.Parse(DurationOfOrders_textbox.Text);
            orders = myBL.duration_of_orders(numOfdays2);
            this.DurationOfOrders_comboBox.ItemsSource = orders;
        }
        #endregion





    }

}
