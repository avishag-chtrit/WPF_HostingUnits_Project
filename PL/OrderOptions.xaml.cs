using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
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
    /// Interaction logic for OrderOptions.xaml
    /// </summary>
    public partial class OrderOptions : Window
    {
        BE.Order order;
        IBL bl;

        public OrderOptions()
        {
            InitializeComponent();
            order = new BE.Order();
            DataContext = order;
            bl = BL.FactoryBL.getBL();
            //GuestRequestKey_block.Text = order.OrderKey;

            Status_block.DataContext = order.Status;
            OrderKey_block.DataContext = order.OrderKey;
            HostingUnit_block.DataContext = order.HostingUnitKey;
            GuestRequestKey_block.DataContext = order.GuestRequestKey;
            createDateDatePicker1.DataContext = order.CreateDate;
            orderDateDatePicker1.DataContext = order.OrderDate;


            createDateDatePicker1.Text = order.CreateDate.ToShortDateString(); //DateTime.Now.ToShortDateString();
            orderDateDatePicker1.Text = order.OrderDate.ToShortDateString();//DateTime.Now.ToShortDateString();
            Status_block.ClearValue(TextBlock.TextProperty);


            UpdatecomboBox.ItemsSource = bl.get_All_Orders();
            GuestRequest_Combobox.ItemsSource = bl.get_All_Guests();
            HostingUnit_Combobox.ItemsSource = bl.get_All_HostingUnits();

            //this.GuestRequest_Combobox.DisplayMemberPath = "guestRequestKey";
            //this.GuestRequest_Combobox.SelectedValuePath = "guestRequestKey";

            //this.HostingUnit_Combobox.DisplayMemberPath = "hostingUnitKey";
            //this.HostingUnit_Combobox.SelectedValuePath = "hostingUnitKey";

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource orderViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("orderViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // orderViewSource.Source = [generic data source]
        }
        
        private void AddOrder_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (bl.get_All_Guests().Count()==0 || bl.get_All_HostingUnits().Count() == 0)
                    throw new EmptyInputException("there are no guest requests or units, therefore you can not make an order!");

                if (GuestRequest_Combobox.SelectedIndex == -1)
                    throw new EmptyInputException("you must choose your request first!");

                if (HostingUnit_Combobox.SelectedIndex == -1)
                    throw new EmptyInputException("you must choose your unit first!");
                
                bl.add_Order(order);

                //---------------------------------------------------
                var currUnit = bl.get_All_HostingUnits().Where(hu => hu.hostingUnitKey == order.hostingUnitKey);
                currUnit.ToList();
                BE.HostingUnit hostingUnit = currUnit.FirstOrDefault();

                BE.Host host = hostingUnit.Owner;  // we need to send the owner and the guest that made the order....

                var currGuest = bl.get_All_Guests().Where(gs => gs.guestRequestKey == order.guestRequestKey);
                currGuest.ToList();
                BE.GuestRequest guest = currGuest.FirstOrDefault();

                string from = host.MailAddress;
                string to = guest.MailAddress;
                string subject = "Your Scheduled Order";
                string body = string.Format
               ("The Hosting Unit: " + hostingUnit.HostingUnitKey + " has been scheduled for: " + guest.GuestRequestKey + " " + guest.PrivateName + " \n" +
                "for: " + guest.EntryDate.ToShortDateString() + " to " + guest.ReleaseDate.ToShortDateString() + "\n" +
                "Detailes of the unit: \n" +
                "Name: " + hostingUnit.HostingUnitName + "\n" +
                "Area of unit: " + hostingUnit.Area + "\n" +
                "Type of unit: " + hostingUnit.Type + "\n" +
                "Have a nice day! ^-^ ");

                Thread thread = new Thread(() => bl.sendMailToGuest(from, to, subject, body)); // lambda func
                thread.Start();

                MessageBoxResult result = MessageBox.Show("an Email has been sent successfully");

                //catch (SmtpException)
                //{
                //    MessageBox.Show("error occurred while connecting to server");
                //}
                //catch (InvalidOperationException)
                //{
                //    MessageBox.Show("Worng Mail Address");
                //}

                //--------------------------------------------------------

                order = new BE.Order();
                
                this.DataContext = order;
                OrderKey_block.Text = order.OrderKey;

                createDateDatePicker1.Text = order.CreateDate.ToShortDateString();
                orderDateDatePicker1.Text = order.OrderDate.ToShortDateString();
                GuestRequestKey_block.ClearValue(TextBlock.TextProperty);
                HostingUnit_block.ClearValue(TextBlock.TextProperty);
                Status_block.ClearValue(TextBlock.TextProperty);

                GuestRequest_Combobox.SelectedIndex = -1;
                HostingUnit_Combobox.SelectedIndex = -1;

                refreshOrderCombo();
                RefreshHostingUnitCombo();
                RefreshGuestRequestKeysCombo();

            }
            catch (ChangeCollectionClearanceException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch(EmailErrorException ex)
            {
                MessageBox.Show(ex.Message);

                order = new BE.Order();
                order.CreateDate = new DateTime();
                order.OrderDate = new DateTime();
                this.DataContext = order;
                OrderKey_block.Text = order.OrderKey;

                createDateDatePicker1.Text = order.CreateDate.ToShortDateString();
                orderDateDatePicker1.Text = order.OrderDate.ToShortDateString();
                GuestRequestKey_block.ClearValue(TextBlock.TextProperty);
                HostingUnit_block.ClearValue(TextBlock.TextProperty);
                Status_block.ClearValue(TextBlock.TextProperty);

                GuestRequest_Combobox.SelectedIndex = -1;
                HostingUnit_Combobox.SelectedIndex = -1;
                UpdateOrdercheckBox.IsChecked = false;              
            }
            catch (DoesNotExistException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (UnitBookedException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (EmptyInputException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch(CanNotLoadFileInBlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (NotValidException ex)
            {
                MessageBox.Show(ex.Message);

                order = new BE.Order();
                order.CreateDate =new DateTime();
                order.OrderDate = new DateTime();
                this.DataContext = order;
                OrderKey_block.Text = order.OrderKey;

                createDateDatePicker1.Text = order.CreateDate.ToShortDateString();
                orderDateDatePicker1.Text = order.OrderDate.ToShortDateString();
                GuestRequestKey_block.ClearValue(TextBlock.TextProperty);
                HostingUnit_block.ClearValue(TextBlock.TextProperty);
                Status_block.ClearValue(TextBlock.TextProperty);

                GuestRequest_Combobox.SelectedIndex = -1;
                HostingUnit_Combobox.SelectedIndex = -1;
                UpdateOrdercheckBox.IsChecked = false;
            }
        }

        private BE.GuestRequest GetGuestRequestKey()
        {
            BE.GuestRequest result = GuestRequest_Combobox.SelectedValue as BE.GuestRequest;
            if (result == null)
                throw new EmptyInputException("you must choose your guest request");

            return result;
        }

        private void RefreshGuestRequestKeysCombo()
        {
            GuestRequest_Combobox.ItemsSource = bl.get_All_Guests();
        }

        private BE.HostingUnit GetHostingUnittKey()
        {
            BE.HostingUnit result = HostingUnit_Combobox.SelectedValue as BE.HostingUnit;
            if (result == null)
                throw new EmptyInputException("you must choose your hosting unit first");

            return result;

        }

        private void RefreshHostingUnitCombo()
        {
            HostingUnit_Combobox.ItemsSource = bl.get_All_HostingUnits();
        }

        private void GuestRequest_Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GuestRequest_Combobox.SelectedIndex == -1)
                return;
            order.guestRequestKey = GetGuestRequestKey().guestRequestKey;
            GuestRequestKey_block.Text = order.GuestRequestKey;
            //GuestRequest_Combobox.Text = "";
        }

        private void HostingUnit_Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HostingUnit_Combobox.SelectedIndex == -1)
                return;
            order.hostingUnitKey = GetHostingUnittKey().hostingUnitKey;
            HostingUnit_block.Text = order.HostingUnitKey;

        }

        private void UpdateComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UpdatecomboBox.SelectedIndex == -1)
                return;

            order = GetSelectedOrderToUpdate();
            DataContext = order;

            GuestRequestKey_block.Text = order.GuestRequestKey;
            HostingUnit_block.Text = order.HostingUnitKey;
            createDateDatePicker1.Text = order.CreateDate.ToShortDateString();
            orderDateDatePicker1.Text = order.OrderDate.ToShortDateString();
            OrderKey_block.Text = order.OrderKey;
            Status_block.Text = order.Status.ToString();
            //order.Status = O.Status;
        }

        private BE.Order GetSelectedOrderToUpdate()
        {
            BE.Order or = UpdatecomboBox.SelectedValue as BE.Order;

            if (or == null)
                throw new EmptyInputException("you must select an order to update first!");
            return or;
        }

        private void refreshOrderCombo()
        {
            UpdatecomboBox.ItemsSource = bl.get_All_Orders();
        }

        private void UpdateOrdercheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdatecomboBox.Visibility = System.Windows.Visibility.Hidden;
            UpdateOrder2_label.Visibility = System.Windows.Visibility.Hidden;
        }

        private void UpdateOrdercheckBox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (bl.get_All_Orders().Count == 0 && UpdateOrdercheckBox.IsChecked == true)
                {
                    UpdateOrdercheckBox.IsChecked = false;
                    throw new DoesNotExistException("there are no Orders, therefore you can not update one!");
                }
                    

                if (bl.get_All_Orders().Count != 0 && UpdateOrdercheckBox.IsChecked == true)
                {
                    UpdatecomboBox.Visibility = System.Windows.Visibility.Visible;
                    UpdateOrder2_label.Visibility = System.Windows.Visibility.Visible;
                }
            }
            catch (DoesNotExistException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateOrder_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (UpdateOrdercheckBox.IsChecked == false)
                    throw new EmptyInputException("you must confirm your will to update an order first");

                if (UpdatecomboBox.SelectedIndex == -1)
                    throw new EmptyInputException("you must choose the order you want to update first!");

                if (GuestRequest_Combobox.SelectedIndex == -1)
                    throw new EmptyInputException("you must choose your request first!");

                if (HostingUnit_Combobox.SelectedIndex == -1)
                    throw new EmptyInputException("you must choose your unit first!");          

                bl.update_Order(order);
              
               order = new BE.Order();
                DataContext = order;
                OrderKey_block.Text = order.OrderKey;

                UpdateOrdercheckBox.IsChecked = false;
                UpdatecomboBox.SelectedIndex = -1;
                GuestRequest_Combobox.SelectedIndex = -1;
                HostingUnit_Combobox.SelectedIndex = -1;

                GuestRequestKey_block.ClearValue(TextBlock.TextProperty);
                HostingUnit_block.ClearValue(TextBlock.TextProperty);
                Status_block.ClearValue(TextBlock.TextProperty);
                createDateDatePicker1.Text = order.CreateDate.ToShortDateString(); //DateTime.Now.ToShortDateString();
                orderDateDatePicker1.Text = order.OrderDate.ToShortDateString();

                refreshOrderCombo();
                RefreshHostingUnitCombo();
                RefreshGuestRequestKeysCombo();

            }
            catch (EmptyInputException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (DoesNotExistException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (ChangeCollectionClearanceException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch(NotValidException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (CanNotLoadFileInBlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
// 