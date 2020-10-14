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
using System.Text.RegularExpressions;
using BE;
using BL;

namespace PL
{
    /// <summary>
    /// Interaction logic for addGuestRequest.xaml
    /// </summary>
    public partial class GuestRequest : Window
    {

        BE.GuestRequest gr;
        IBL myBL;

        //---------------------------------Converter
        List<string> keystrings = new List<string>();
        List<long> grkey = new List<long>();
        //--------------------------------------------

        //--------------------------------Dependency Property
        public object updateLabel
        {
            get { return (object)GetValue(updateLabelProperty); }
            set { SetValue(updateLabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for updateLabel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty updateLabelProperty =
            DependencyProperty.Register("updateLabel", typeof(object), typeof(GuestRequest), new PropertyMetadata("Choose The Request to Update"));
        //---------------------------------------------

        public GuestRequest()//initialize window
        {
            InitializeComponent();
            gr = new BE.GuestRequest();
            gr.EntryDate = DateTime.Now;
            gr.ReleaseDate = DateTime.Now.AddDays(1);
            myBL = FactoryBL.getBL();
            this.GuestRequestKey_block.DataContext = gr.GuestRequestKey;
            this.GuestRequestGrid.DataContext = gr;

            this.UpdateRequestComboBox.ItemsSource = myBL.get_All_Guests();
            //UpdateRequestComboBox.DisplayMemberPath = GuestRequestKey;
            //UpdateRequestComboBox.SelectedValuePath = GuestRequestKey;

            //binding here instead of XAML
                                
            this.areaComboBox.ItemsSource = System.Enum.GetValues(typeof(BE.Enum.AreaOfV));
            this.subAreaComboBox.ItemsSource = System.Enum.GetValues(typeof(BE.Enum.subArea));
            this.typeComboBox.ItemsSource = System.Enum.GetValues(typeof(BE.Enum.UnitType));
            this.childAtComboBox.ItemsSource = System.Enum.GetValues(typeof(BE.Enum.ChildrensAttractions));
            this.gardenComboBox.ItemsSource = System.Enum.GetValues(typeof(BE.Enum.Garden));
            this.jacuzziComboBox.ItemsSource = System.Enum.GetValues(typeof(BE.Enum.Jacuzzi));
            this.poolComboBox.ItemsSource = System.Enum.GetValues(typeof(BE.Enum.Pool));
 
        }

        //private void PrivateName_tb_LostFocus(object sender, RoutedEventArgs e)
        //{
        //   // try
        //    //{
        //        gr.PrivateName = PrivateName_tb.Text;
        //   // }
        //    //catch (Exception exp)
        //    //{
        //    //    PrivateName_tb.BorderBrush = Brushes.Red;
        //    //    PrivateName_label.Content = exp.Message;
        //    //}
        //}

        //private void PrivateName_tb_GotFocus(object sender, RoutedEventArgs e)
        //{
        //    PrivateName_label.Content = "";
        //    PrivateName_tb.BorderBrush = Brushes.Black;
        //}

        //private void LastName_tb_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    // try
        //    //{
        //    gr.FamilyName = FamilyName_tb.Text;
        //    // }
        //    //catch (Exception exp)
        //    //{
        //    //    PrivateName_tb.BorderBrush = Brushes.Red;
        //    //    PrivateName_label.Content = exp.Message;
        //    //}
        //}

        //private void LasteName_tb_GotFocus(object sender, RoutedEventArgs e)
        //{
        //    FamilyName_label.Content = "";
        //    FamilyName_tb.BorderBrush = Brushes.Black;
        //}


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource guestRequestViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("guestRequestViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // guestRequestViewSource.Source = [generic data source]
        }

        #region converter
        public static long stringKeyToLong(string skey)
        {
            return long.Parse(skey);
        }

      
        #endregion

        private void AddGuestRequest_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {           
                if (privateNameTextBox.Text == "" || familyNameTextBox.Text == "" || mailAddressTextBox.Text == "" ||
                    areaComboBox.SelectedIndex == -1 || subAreaComboBox.SelectedIndex == -1 || typeComboBox.SelectedIndex == -1 || Adults_tb.Text == "" ||
                    childAtComboBox.SelectedIndex == -1 || childrenTextBox.Text == "" || gardenComboBox.SelectedIndex == -1 || jacuzziComboBox.SelectedIndex == -1 || poolComboBox.SelectedIndex == -1)
                {
                    throw new EmptyInputException("one of the feilds is empty!");
                }

                //checks email
                string strR = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

                Regex r = new Regex(strR, RegexOptions.IgnoreCase);

                if (!r.IsMatch(mailAddressTextBox.Text.ToString()))
                {
                    throw new NotValidException("invalid Email Address of Guest ");
                }

                //adults number check
                Regex regex = new Regex(@"[0-9]");
                Match match = regex.Match(Adults_tb.Text.ToString());
                if (!match.Success)
                {
                    throw new NotValidException("Number of adults should be number and not string ");
                }


                if (int.Parse(Adults_tb.Text.ToString()) < 0 )
                {
                    throw new NotValidException("Number of adults must be positive ");
                }


                //children
                match = regex.Match(childrenTextBox.Text.ToString());
                if (!match.Success)
                {
                    throw new NotValidException("Number of adults should be number and not string ");
                }


                if (int.Parse(childrenTextBox.Text.ToString()) < 0)
                   {
                   throw new NotValidException("Number of children must be positive ");
                   }
           
                keystrings.Add(GuestRequestKey_block.Text);
                grkey = keystrings.ConvertAll(new Converter<string, long>(stringKeyToLong));
                gr.guestRequestKey = grkey.FirstOrDefault();
                keystrings.RemoveAt(0);
                grkey.RemoveAt(0);
                //------------------------------------------ instead of this one simple line!!!
                //gr.guestRequestKey = long.Parse(GuestRequestKey_block.Text);
                //-----------------------------------------

                //---------------------------- לא עובד הביינדינג בהכנסה שנייה ולכן... צריך לבדוק את           
                this.GuestRequestKey_block.DataContext = gr.GuestRequestKey;              
                gr.PrivateName = privateNameTextBox.Text;
                gr.FamilyName = familyNameTextBox.Text;
                gr.MailAddress = mailAddressTextBox.Text;
                gr.Adults = int.Parse(Adults_tb.Text);
                gr.EntryDate = (DateTime)entryDateDatePicker.SelectedDate;
                gr.ReleaseDate = (DateTime)releaseDateDatePicker.SelectedDate;
                gr.Adults = int.Parse(Adults_tb.Text);
                gr.Children = int.Parse(childrenTextBox.Text);
                //---------------------------

                gr.Area = (BE.Enum.AreaOfV)areaComboBox.SelectedIndex;
                gr.SubArea = (BE.Enum.subArea)subAreaComboBox.SelectedIndex;
                gr.Type = (BE.Enum.UnitType)typeComboBox.SelectedIndex;
                gr.childAt = (BE.Enum.ChildrensAttractions)childAtComboBox.SelectedIndex;
                gr.garden = (BE.Enum.Garden)gardenComboBox.SelectedIndex;
                gr.pool = (BE.Enum.Pool)poolComboBox.SelectedIndex;
                gr.jacuzzi = (BE.Enum.Jacuzzi)jacuzziComboBox.SelectedIndex;

                // isUpdate = (bool)Update_CheckBox.DataContext;

                myBL.add_GuestRequest(gr);
                gr = new BE.GuestRequest();
                gr.EntryDate = DateTime.Now;
                gr.ReleaseDate = DateTime.Now.AddDays(1);
                GuestRequestGrid.DataContext = gr;

                GuestRequestKey_block.DataContext = gr.GuestRequestKey;
                GuestRequestKey_block.Text = gr.GuestRequestKey;
                RefreshGuestRequestKeysCombo();// so that the combobox will have the new gr that we added

                this.privateNameTextBox.ClearValue(TextBox.TextProperty);
                this.familyNameTextBox.ClearValue(TextBox.TextProperty);
                this.mailAddressTextBox.ClearValue(TextBox.TextProperty);
                this.Adults_tb.ClearValue(TextBox.TextProperty);
                this.childrenTextBox.ClearValue(TextBox.TextProperty);
                areaComboBox.ClearValue(ComboBox.TextProperty);
                subAreaComboBox.ClearValue(ComboBox.TextProperty);
                typeComboBox.ClearValue(ComboBox.TextProperty);
                childAtComboBox.ClearValue(ComboBox.TextProperty);
                gardenComboBox.ClearValue(ComboBox.TextProperty);
                jacuzziComboBox.ClearValue(ComboBox.TextProperty);
                poolComboBox.ClearValue(ComboBox.TextProperty);                                                           

            }
            catch (CanNotLoadFileInBlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (NotValidException ex)
            {
                MessageBox.Show(ex.Message);               
            }
            catch (EmptyInputException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (NegativeNumberInBLException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch(DoubleKeyException ex)
            {
                MessageBox.Show(ex.Message);

                gr = new BE.GuestRequest();
                gr.EntryDate = DateTime.Now;
                gr.ReleaseDate = DateTime.Now.AddDays(1);
                GuestRequestGrid.DataContext = gr;

                GuestRequestKey_block.DataContext = gr.GuestRequestKey;
                GuestRequestKey_block.Text = gr.GuestRequestKey;
                RefreshGuestRequestKeysCombo();// so that the combobox will have the new gr that we added

                this.privateNameTextBox.ClearValue(TextBox.TextProperty);
                this.familyNameTextBox.ClearValue(TextBox.TextProperty);
                this.mailAddressTextBox.ClearValue(TextBox.TextProperty);
                this.Adults_tb.ClearValue(TextBox.TextProperty);
                this.childrenTextBox.ClearValue(TextBox.TextProperty);
                areaComboBox.ClearValue(ComboBox.TextProperty);
                subAreaComboBox.ClearValue(ComboBox.TextProperty);
                typeComboBox.ClearValue(ComboBox.TextProperty);
                childAtComboBox.ClearValue(ComboBox.TextProperty);
                gardenComboBox.ClearValue(ComboBox.TextProperty);
                jacuzziComboBox.ClearValue(ComboBox.TextProperty);
                poolComboBox.ClearValue(ComboBox.TextProperty);

                Update_CheckBox.IsChecked = false;
            }
            //catch (DuplicatedKeyInBLException ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void updateGuestRequest_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {                             
                if (Update_CheckBox.IsChecked == false)
                    throw new EmptyInputException("you must confirm your will to update a request first!");

                //אמורים להופיע כל השדות של הבקשה הקיימת שאנחנו רוצים לעדכן
                
                if (UpdateRequestComboBox.SelectedIndex == -1)
                    throw new EmptyInputException("you must select first the request you would like to update");

                if (privateNameTextBox.Text == "" || familyNameTextBox.Text == "" || mailAddressTextBox.Text == "" ||
                   areaComboBox.SelectedIndex == -1 || subAreaComboBox.SelectedIndex == -1 || typeComboBox.SelectedIndex == -1 || Adults_tb.Text == "" ||
                   childAtComboBox.SelectedIndex == -1 || childrenTextBox.Text == "" || gardenComboBox.SelectedIndex == -1 || jacuzziComboBox.SelectedIndex == -1 || poolComboBox.SelectedIndex == -1)
                {
                    throw new EmptyInputException("one of the feilds is empty!");
                }

                //checks email
                string strR = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

                Regex r = new Regex(strR, RegexOptions.IgnoreCase);

                if (!r.IsMatch(mailAddressTextBox.Text.ToString()))
                {
                    throw new NotValidException("invalid Email Address of Guest ");
                }

                //adults number check
                Regex regex = new Regex(@"[0-9]");
                Match match = regex.Match(Adults_tb.Text.ToString());
                if (!match.Success)
                {
                    throw new NotValidException("Number of adults should be number and not Letters ");
                }


                if (int.Parse(Adults_tb.Text.ToString()) < 0)
                {
                    throw new NotValidException("Number of adults must be positive ");
                }


                //children
                match = regex.Match(childrenTextBox.Text.ToString());
                if (!match.Success)
                {
                    throw new NotValidException("Number of adults should be number and not Letters ");
                }


                if (int.Parse(childrenTextBox.Text.ToString()) < 0)
                {
                    throw new NotValidException("Number of children must be positive ");
                }

                gr.RegistrationDate = GetSelectedGuestRequest().RegistrationDate;
                gr.guestRequestKey = long.Parse(GuestRequestKey_block.Text);
                gr.PrivateName = privateNameTextBox.Text;
                gr.FamilyName = familyNameTextBox.Text;
                gr.MailAddress = mailAddressTextBox.Text;
                gr.EntryDate = (DateTime)entryDateDatePicker.SelectedDate;
                gr.ReleaseDate = (DateTime)releaseDateDatePicker.SelectedDate;
                gr.Adults = int.Parse(Adults_tb.Text);
                gr.Children = int.Parse(childrenTextBox.Text);

                gr.Area = (BE.Enum.AreaOfV)areaComboBox.SelectedIndex;
                gr.SubArea = (BE.Enum.subArea)subAreaComboBox.SelectedIndex;
                gr.Type = (BE.Enum.UnitType)typeComboBox.SelectedIndex;
                gr.childAt = (BE.Enum.ChildrensAttractions)childAtComboBox.SelectedIndex;
                gr.garden = (BE.Enum.Garden)gardenComboBox.SelectedIndex;
                gr.pool = (BE.Enum.Pool)poolComboBox.SelectedIndex;
                gr.jacuzzi = (BE.Enum.Jacuzzi)jacuzziComboBox.SelectedIndex;

                myBL.update_GuestRequest(gr);
                gr = new BE.GuestRequest();
                gr.EntryDate = DateTime.Now;
                gr.ReleaseDate = DateTime.Now.AddDays(1);
                GuestRequestGrid.DataContext = gr;
                GuestRequestKey_block.DataContext = gr.GuestRequestKey;
                GuestRequestKey_block.Text = gr.GuestRequestKey;
                // צריך לגרום לצק בוקס להיות אן צקט
                Update_CheckBox.IsChecked = false;                            
                UpdateRequestComboBox.SelectedIndex = -1;
                //-----------------------

                this.privateNameTextBox.ClearValue(TextBox.TextProperty);
                this.familyNameTextBox.ClearValue(TextBox.TextProperty);
                this.mailAddressTextBox.ClearValue(TextBox.TextProperty);
                this.Adults_tb.ClearValue(TextBox.TextProperty);
                this.childrenTextBox.ClearValue(TextBox.TextProperty);
                areaComboBox.ClearValue(ComboBox.TextProperty);
                subAreaComboBox.ClearValue(ComboBox.TextProperty);
                typeComboBox.ClearValue(ComboBox.TextProperty);
                childAtComboBox.ClearValue(ComboBox.TextProperty);
                gardenComboBox.ClearValue(ComboBox.TextProperty);
                jacuzziComboBox.ClearValue(ComboBox.TextProperty);
                poolComboBox.ClearValue(ComboBox.TextProperty);

                RefreshGuestRequestKeysCombo();
            }
            catch (CanNotLoadFileInBlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (NotValidException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (DoesNotExistException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (EmptyInputException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch(NegativeNumberInBLException ex)
            {
                MessageBox.Show(ex.Message);
            }           
        }

        private BE.GuestRequest GetSelectedGuestRequest()
        {      
            BE.GuestRequest g = this.UpdateRequestComboBox.SelectedValue as BE.GuestRequest;

            if (g == null)
                throw new EmptyInputException("you must select a guest request key first");

            return g;
            
        }
 
       private void RefreshGuestRequestKeysCombo()
        {
            this.UpdateRequestComboBox.ItemsSource = myBL.get_All_Guests();
        }


        private void Update_CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (myBL.get_All_Guests().Count == 0 && Update_CheckBox.IsChecked == true)
                {
                    Update_CheckBox.IsChecked = false;
                    throw new DoesNotExistException("there are no guest requests, therefore you can not update one!");
                }
                if(myBL.get_All_Guests().Count != 0 && Update_CheckBox.IsChecked == true)
                {
                    UpdateRequestComboBox.Visibility = System.Windows.Visibility.Visible;
                    update2_label.Visibility = System.Windows.Visibility.Visible;
                    

                    //long guestKey = long.Parse(GetSelectedGuestRequestKey());
                    //var v = myBL.get_All_Guests().Where(g => g.guestRequestKey == guestKey);
                    //v.ToList();
                    //gr = v.First();

                }
            }
            catch (DoesNotExistException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch(EmptyInputException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Update_CheckBox_unchecked(object sender, RoutedEventArgs e)
        {
            UpdateRequestComboBox.Visibility = System.Windows.Visibility.Hidden;
            update2_label.Visibility = System.Windows.Visibility.Hidden;

            gr = new BE.GuestRequest();
            gr.EntryDate = DateTime.Now;
            gr.ReleaseDate = DateTime.Now.AddDays(1);
            GuestRequestGrid.DataContext = gr;
            GuestRequestKey_block.DataContext = gr.GuestRequestKey;
            GuestRequestKey_block.Text = gr.GuestRequestKey;
            UpdateRequestComboBox.SelectedIndex = -1;
            //-----------------------
            this.privateNameTextBox.ClearValue(TextBox.TextProperty);
            this.familyNameTextBox.ClearValue(TextBox.TextProperty);
            this.mailAddressTextBox.ClearValue(TextBox.TextProperty);
            this.Adults_tb.ClearValue(TextBox.TextProperty);
            this.childrenTextBox.ClearValue(TextBox.TextProperty);
            areaComboBox.ClearValue(ComboBox.TextProperty);
            subAreaComboBox.ClearValue(ComboBox.TextProperty);
            typeComboBox.ClearValue(ComboBox.TextProperty);
            childAtComboBox.ClearValue(ComboBox.TextProperty);
            gardenComboBox.ClearValue(ComboBox.TextProperty);
            jacuzziComboBox.ClearValue(ComboBox.TextProperty);
            poolComboBox.ClearValue(ComboBox.TextProperty);

        }

        private void UpdateRequestComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UpdateRequestComboBox.SelectedIndex == -1) // happens only when we initialize to zero in order to make it posibol to update the same request- in the same intex-twice
                return;

            BE.GuestRequest gs = GetSelectedGuestRequest();
            GuestRequestGrid.DataContext = gs;
            

            GuestRequestKey_block.Text = gs.GuestRequestKey;//.GuestRequestKey;
            privateNameTextBox.Text = gs.PrivateName;
            familyNameTextBox.Text = gs.FamilyName;
            mailAddressTextBox.Text = gs.MailAddress;
            Adults_tb.Text = (gs.Adults).ToString();
            entryDateDatePicker.Text = gs.EntryDate.ToLongDateString();
            releaseDateDatePicker.Text = gs.ReleaseDate.ToLongDateString();
            areaComboBox.Text = (gs.Area).ToString();
            subAreaComboBox.Text = (gs.SubArea).ToString();
            typeComboBox.Text = (gs.Type).ToString();
            Adults_tb.Text = (gs.Adults).ToString();
            childAtComboBox.Text = (gs.childAt).ToString();
            childrenTextBox.Text = (gs.Children).ToString();
            gardenComboBox.Text = (gs.garden).ToString();
            jacuzziComboBox.Text = (gs.jacuzzi).ToString();
            poolComboBox.Text = (gs.pool).ToString();

           
        }     
    }
}
