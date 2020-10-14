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
using BL;

namespace PL
{
    /// <summary>
    /// Interaction logic for addHostingUnit.xaml
    /// </summary>
    public partial class HostingUnitOptions : Window
    {
        BE.HostingUnit unit;
        IBL bl;

        public HostingUnitOptions()
        {
            InitializeComponent();
          
            unit = new BE.HostingUnit();
            unit.Owner = new BE.Host();
            unit.Owner.BankBranchDetails = new BE.BankBranch();         
            bl = BL.FactoryBL.getBL();
            gridOfUnitDetailes.DataContext = unit;
            HostingUnitKey_block.DataContext = unit.hostingUnitKey;
            HostingUnitKey_block.Text = unit.HostingUnitKey;
            //---------------------------
            HostKey_Block.DataContext = unit.Owner.HostKey;
            HostKey_Block.Text = unit.Owner.HostKey;

            PrivateNameHostGrid.DataContext = unit.Owner.PrivateName;
            FamilyNameHostGrid.DataContext = unit.Owner.FamilyName;
            PhoneNumHostGrid.DataContext = unit.Owner.PhoneNumber;
            MailHostGrid.DataContext = unit.Owner.MailAddress;
            CollectionHostGrid.DataContext = unit.Owner.CollectionClearance;
            BankAccHostGrid.DataContext = unit.Owner.BankAccountNumber;
            BankDetailHostGrid.DataContext = unit.Owner.BankBranchDetails;
            //-----------------------------------

            this.areaComboBox.ItemsSource = System.Enum.GetValues(typeof(BE.Enum.AreaOfV));
            this.subAreaComboBox.ItemsSource = System.Enum.GetValues(typeof(BE.Enum.subArea));
            this.typeComboBox.ItemsSource = System.Enum.GetValues(typeof(BE.Enum.UnitType));            

            this.UnitToUpdade_comboBox.ItemsSource = bl.get_All_HostingUnits();
            this.ExistingHost_comboBox.ItemsSource = bl.get_All_Host();
            try
            {
                this.BankNamecomboBox.ItemsSource = bl.GetBankNames();
            }          
            catch(DoesNotExistException ex)
            {
                MessageBox.Show(ex.Message);
            }         
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource hostingUnitViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("hostingUnitViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // hostingUnitViewSource.Source = [generic data source]
            System.Windows.Data.CollectionViewSource hostViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("hostViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // hostViewSource.Source = [generic data source]
        }

        //private long GetSelectedUnitKey()
        //{
        //    object key = this.UnitToUpdade_comboBox.SelectedValue;
        //    if (key == null)
        //        throw new NoUnitSelectedException("you must select the unit first");
        //    return (long)key;
        //}


        private void AddUnit_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {             
                if (hostingUnitNameTextBox.Text == "" || areaComboBox.SelectedIndex == -1 || subAreaComboBox.SelectedIndex == -1 || typeComboBox.SelectedIndex == -1
                   || privateNameTextBox.Text == "" || familyNameTextBox.Text == "" || phoneNumberTextBox.Text == "" || mailAddressTextBox.Text == "" 
                   || bankAccountNumberTextBox.Text == "" || BankNamecomboBox.SelectedIndex ==-1 || bankNumberTextBox.Text == "" ||
                   branchAddressTextBox.Text == "" || branchCityTextBox.Text == "" || BranchNumbercomboBox.SelectedIndex == -1)
                    throw new EmptyInputException("one of the fields is empty!");

                #region regex check
                Regex regex = new Regex(@"[0-9]");
                Match match = regex.Match(phoneNumberTextBox.Text.ToString());
                if (!match.Success)
                {
                    throw new NotValidException("invalid phone number of Host ");
                }

                //checks email
                string strR = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

                Regex r = new Regex(strR, RegexOptions.IgnoreCase);

                if (!r.IsMatch(mailAddressTextBox.Text.ToString()))
                {
                    throw new EmptyInputException("invalid Email Address of Host ");
                }

                //checks number bankAccountNumberTextBox
                regex = new Regex(@"[0-9]");
                match = regex.Match(bankAccountNumberTextBox.Text.ToString());
                if (!match.Success)
                {
                    throw new NotValidException("Bank Account number should be number and not Letters ");
                }


                if (int.Parse(bankAccountNumberTextBox.Text.ToString()) < 0)
                {
                    throw new NotValidException("Bank Account number must be positive ");
                }

                //checks bankNumberTextBox
                match =  regex.Match(bankNumberTextBox.Text.ToString());
                if (!match.Success)
                {
                    throw new NotValidException("Bank number should be number and not Letters ");
                }


                if (int.Parse(bankAccountNumberTextBox.Text.ToString()) < 0)
                {
                    throw new NotValidException("Bank number must be positive ");
                }

                //checks branchNumberTextBox
                match = regex.Match(BranchNumbercomboBox.Text.ToString());
                if (!match.Success)
                {
                    throw new NotValidException("Branch number should be number and not Letters ");
                }

                if (int.Parse(BranchNumbercomboBox.Text.ToString()) < 0)
                {
                    throw new NotValidException("Branch number must be positive ");
                }
                #endregion

                unit.hostingUnitKey = long.Parse(HostingUnitKey_block.Text);
                unit.HostingUnitName = hostingUnitNameTextBox.Text;
                unit.Owner.hostKey = long.Parse(HostKey_Block.Text);
                unit.Owner.PrivateName = privateNameTextBox.Text;
                unit.Owner.FamilyName = familyNameTextBox.Text;
                unit.Owner.phoneNumber = long.Parse(phoneNumberTextBox.Text);
                unit.Owner.MailAddress = mailAddressTextBox.Text;
                unit.Owner.CollectionClearance = (bool)collectionClearanceCheckBox.IsChecked;
                unit.Owner.bankAccountNumber = long.Parse(bankAccountNumberTextBox.Text);
                unit.Owner.BankBranchDetails.BankName = BankNamecomboBox.SelectedValue.ToString();
                unit.Owner.BankBranchDetails.bankNumber = long.Parse(bankNumberTextBox.Text);
                unit.Owner.BankBranchDetails.BranchAddress = branchAddressTextBox.Text;
                unit.Owner.BankBranchDetails.BranchCity = branchCityTextBox.Text;
                unit.Owner.BankBranchDetails.branchNumber = long.Parse(BranchNumbercomboBox.Text);

                unit.Area = (BE.Enum.AreaOfV)areaComboBox.SelectedIndex;
                unit.SubArea = (BE.Enum.subArea)subAreaComboBox.SelectedIndex;
                unit.Type = (BE.Enum.UnitType)typeComboBox.SelectedIndex;


                bl.add_HostingUnit(unit);
                unit = new BE.HostingUnit();
                unit.Owner = new BE.Host();
                unit.Owner.BankBranchDetails = new BE.BankBranch();
                DataContext = unit;
                HostingUnitKey_block.DataContext = unit.HostingUnitKey;
                HostingUnitKey_block.Text = unit.HostingUnitKey;

                HostKey_Block.DataContext = unit.Owner.HostKey;
                HostKey_Block.Text = unit.Owner.HostKey;
                refreshHostingUnitCombo();
                refreshHostCombo();

                hostingUnitNameTextBox.ClearValue(TextBox.TextProperty);
                privateNameTextBox.ClearValue(TextBox.TextProperty);
                familyNameTextBox.ClearValue(TextBox.TextProperty);
                phoneNumberTextBox.ClearValue(TextBox.TextProperty);
                mailAddressTextBox.ClearValue(TextBox.TextProperty);
                collectionClearanceCheckBox.IsChecked = false;
                bankAccountNumberTextBox.ClearValue(TextBox.TextProperty);
                BankNamecomboBox.ClearValue(ComboBox.TextProperty);
                bankNumberTextBox.ClearValue(TextBox.TextProperty);
                branchAddressTextBox.ClearValue(TextBox.TextProperty);
                branchCityTextBox.ClearValue(TextBox.TextProperty);
                BranchNumbercomboBox.ClearValue(ComboBox.TextProperty);
                areaComboBox.ClearValue(ComboBox.TextProperty);
                subAreaComboBox.ClearValue(ComboBox.TextProperty);
                typeComboBox.ClearValue(ComboBox.TextProperty);

                ExistingHost_checkBox.IsChecked = false;
            }
            //catch(DuplicatedKeyInBLException ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            catch (CanNotLoadFileInBlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (EmptyInputException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (NotValidException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch(DoubleKeyException ex)
            {
                MessageBox.Show(ex.Message);

                unit = new BE.HostingUnit();
                unit.Owner = new BE.Host();
                unit.Owner.BankBranchDetails = new BE.BankBranch();
                DataContext = unit;
                HostingUnitKey_block.DataContext = unit.HostingUnitKey;
                HostingUnitKey_block.Text = unit.HostingUnitKey;

                HostKey_Block.DataContext = unit.Owner.HostKey;
                HostKey_Block.Text = unit.Owner.HostKey;
                refreshHostingUnitCombo();
                refreshHostCombo();

                hostingUnitNameTextBox.ClearValue(TextBox.TextProperty);
                privateNameTextBox.ClearValue(TextBox.TextProperty);
                familyNameTextBox.ClearValue(TextBox.TextProperty);
                phoneNumberTextBox.ClearValue(TextBox.TextProperty);
                mailAddressTextBox.ClearValue(TextBox.TextProperty);
                collectionClearanceCheckBox.IsChecked = false;
                bankAccountNumberTextBox.ClearValue(TextBox.TextProperty);
                BankNamecomboBox.ClearValue(ComboBox.TextProperty);
                bankNumberTextBox.ClearValue(TextBox.TextProperty);
                branchAddressTextBox.ClearValue(TextBox.TextProperty);
                branchCityTextBox.ClearValue(TextBox.TextProperty);
                BranchNumbercomboBox.ClearValue(ComboBox.TextProperty);
                areaComboBox.ClearValue(ComboBox.TextProperty);
                subAreaComboBox.ClearValue(ComboBox.TextProperty);
                typeComboBox.ClearValue(ComboBox.TextProperty);

                UpdateUnit_checkBox.IsChecked = false;
                ExistingHost_checkBox.IsChecked = false;
            }
        }


        private void UpdateUnit_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {                                  
                if (UpdateUnit_checkBox.IsChecked == false)
                    throw new EmptyInputException("you must confirm your will to update a unit first!");

                if (UnitToUpdade_comboBox.SelectedIndex == -1)
                    throw new EmptyInputException("you must select the unit you would like to update first!");

                if (hostingUnitNameTextBox.Text == "" || areaComboBox.SelectedIndex == -1 || subAreaComboBox.SelectedIndex == -1 || typeComboBox.SelectedIndex == -1
                    || privateNameTextBox.Text == "" || familyNameTextBox.Text == "" || phoneNumberTextBox.Text == "" || mailAddressTextBox.Text == "" 
                     || bankAccountNumberTextBox.Text == "" || BankNamecomboBox.SelectedIndex == -1 || bankNumberTextBox.Text == "" ||
                    branchAddressTextBox.Text == "" || branchCityTextBox.Text == "" || BranchNumbercomboBox.SelectedIndex == -1)
                    throw new EmptyInputException("one of the fields is empty!");

                //checks phone number
                #region regex
                Regex regex = new Regex(@"[0-9]");
                Match match = regex.Match(phoneNumberTextBox.Text.ToString());

                if (!match.Success)
                {
                    throw new EmptyInputException("invalid phone number of Host ");
                }

                //checks email
                string strR = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

                Regex r = new Regex(strR, RegexOptions.IgnoreCase);

                if (!r.IsMatch(mailAddressTextBox.Text.ToString()))
                {
                    throw new EmptyInputException("invalid Email Address of Host ");
                }

                //checks number bankAccountNumberTextBox
                 regex = new Regex(@"[0-9]");
                 match = regex.Match(bankAccountNumberTextBox.Text.ToString());
                if (!match.Success)
                {
                    throw new NotValidException("Bank Account number should be number and not Letters ");
                }


                if (int.Parse(bankAccountNumberTextBox.Text.ToString()) < 0)
                {
                    throw new NotValidException("Bank Account number must be positive ");
                }

                //checks bankNumberTextBox
                match = regex.Match(bankNumberTextBox.Text.ToString());
                if (!match.Success)
                {
                    throw new NotValidException("Bank number should be number and not Letters ");
                }


                if (int.Parse(bankAccountNumberTextBox.Text.ToString()) < 0)
                {
                    throw new NotValidException("Bank number must be positive ");
                }

                //checks branchNumberTextBox
                match = regex.Match(BranchNumbercomboBox.Text.ToString());
                if (!match.Success)
                {
                    throw new NotValidException("Branch number should be number and not Letters ");
                }

                if (int.Parse(BranchNumbercomboBox.Text.ToString()) < 0)
                {
                    throw new NotValidException("Branch number must be positive ");
                }
                #endregion

                unit.hostingUnitKey = long.Parse(HostingUnitKey_block.Text);
                unit.HostingUnitName = hostingUnitNameTextBox.Text;
                unit.Owner.hostKey = long.Parse(HostKey_Block.Text);
                unit.Owner.PrivateName = privateNameTextBox.Text;
                unit.Owner.FamilyName = familyNameTextBox.Text;
                unit.Owner.phoneNumber = long.Parse(phoneNumberTextBox.Text);
                unit.Owner.MailAddress = mailAddressTextBox.Text;
                unit.Owner.CollectionClearance = (bool)collectionClearanceCheckBox.IsChecked;
                unit.Owner.bankAccountNumber = long.Parse(bankAccountNumberTextBox.Text);
                unit.Owner.BankBranchDetails.BankName = BankNamecomboBox.SelectedValue.ToString();
                unit.Owner.BankBranchDetails.bankNumber = long.Parse(bankNumberTextBox.Text);
                unit.Owner.BankBranchDetails.BranchAddress = branchAddressTextBox.Text;
                unit.Owner.BankBranchDetails.BranchCity = branchCityTextBox.Text;
                unit.Owner.BankBranchDetails.branchNumber = long.Parse(BranchNumbercomboBox.Text);

                unit.Area = (BE.Enum.AreaOfV)areaComboBox.SelectedIndex;
                unit.SubArea = (BE.Enum.subArea)subAreaComboBox.SelectedIndex;
                unit.Type = (BE.Enum.UnitType)typeComboBox.SelectedIndex;


                bl.update_HostingUnit(unit);
                unit = new BE.HostingUnit();
                unit.Owner = new BE.Host();
                unit.Owner.BankBranchDetails = new BE.BankBranch();
                this.DataContext = unit;
                HostingUnitKey_block.DataContext = unit.HostingUnitKey;
                HostKey_Block.DataContext = unit.Owner.HostKey;

                HostingUnitKey_block.Text = unit.HostingUnitKey;
                HostKey_Block.Text = unit.Owner.HostKey;

                UpdateUnit_checkBox.IsChecked = false;
                ExistingHost_checkBox.IsChecked = false;
                UnitToUpdade_comboBox.SelectedIndex = -1;
                ExistingHost_comboBox.SelectedIndex = -1;
                BranchNumbercomboBox.SelectedIndex = -1;
                BankNamecomboBox.SelectedIndex = -1;

                hostingUnitNameTextBox.ClearValue(TextBox.TextProperty);
                privateNameTextBox.ClearValue(TextBox.TextProperty);
                familyNameTextBox.ClearValue(TextBox.TextProperty);
                phoneNumberTextBox.ClearValue(TextBox.TextProperty);
                mailAddressTextBox.ClearValue(TextBox.TextProperty);
                collectionClearanceCheckBox.IsChecked = false;
                bankAccountNumberTextBox.ClearValue(TextBox.TextProperty);
                BankNamecomboBox.ClearValue(ComboBox.TextProperty);
                bankNumberTextBox.ClearValue(TextBox.TextProperty);
                branchAddressTextBox.ClearValue(TextBox.TextProperty);
                branchCityTextBox.ClearValue(TextBox.TextProperty);
                BranchNumbercomboBox.ClearValue(ComboBox.TextProperty);
                areaComboBox.ClearValue(ComboBox.TextProperty);
                subAreaComboBox.ClearValue(ComboBox.TextProperty);
                typeComboBox.ClearValue(ComboBox.TextProperty);

                refreshHostingUnitCombo();
                refreshHostCombo();
            }
            catch(DoesNotExistException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch(EmptyInputException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch(CanNotRemoveCollectionClearanceException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (CanNotLoadFileInBlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void deleteHostingUnit_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (UpdateUnit_checkBox.IsChecked == false)
                    throw new EmptyInputException("you must confirm your will to delete a unit first!");

                if (UnitToUpdade_comboBox.SelectedIndex == -1)
                    throw new EmptyInputException("you must select the unit you would like to delete first!");

                if (hostingUnitNameTextBox.Text == "" || areaComboBox.SelectedIndex == -1 || subAreaComboBox.SelectedIndex == -1 || typeComboBox.SelectedIndex == -1
                    || privateNameTextBox.Text == "" || familyNameTextBox.Text == "" || phoneNumberTextBox.Text == "" || mailAddressTextBox.Text == "" ||
                    collectionClearanceCheckBox.IsChecked == false || bankAccountNumberTextBox.Text == "" || BankNamecomboBox.SelectedIndex == -1 || bankNumberTextBox.Text == "" ||
                    branchAddressTextBox.Text == "" || branchCityTextBox.Text == "" || BranchNumbercomboBox.SelectedIndex == -1)
                    throw new EmptyInputException("one of the fields is empty!");

                unit.hostingUnitKey = long.Parse(HostingUnitKey_block.Text);
                unit.HostingUnitName = hostingUnitNameTextBox.Text;
                unit.Owner.hostKey = long.Parse(HostKey_Block.Text);
                unit.Owner.PrivateName = privateNameTextBox.Text;
                unit.Owner.FamilyName = familyNameTextBox.Text;
                unit.Owner.phoneNumber = long.Parse(phoneNumberTextBox.Text);
                unit.Owner.MailAddress = mailAddressTextBox.Text;
                unit.Owner.CollectionClearance = (bool)collectionClearanceCheckBox.IsChecked;
                unit.Owner.bankAccountNumber = long.Parse(bankAccountNumberTextBox.Text);
                unit.Owner.BankBranchDetails.BankName = BankNamecomboBox.SelectedValue.ToString();
                unit.Owner.BankBranchDetails.bankNumber = long.Parse(bankNumberTextBox.Text);
                unit.Owner.BankBranchDetails.BranchAddress = branchAddressTextBox.Text;
                unit.Owner.BankBranchDetails.BranchCity = branchCityTextBox.Text;
                unit.Owner.BankBranchDetails.branchNumber = long.Parse(BranchNumbercomboBox.Text);

                unit.Area = (BE.Enum.AreaOfV)areaComboBox.SelectedIndex;
                unit.SubArea = (BE.Enum.subArea)subAreaComboBox.SelectedIndex;
                unit.Type = (BE.Enum.UnitType)typeComboBox.SelectedIndex;

                bl.delete_HostingUnit(unit);
                unit = new BE.HostingUnit();
                unit.Owner = new BE.Host();
                unit.Owner.BankBranchDetails = new BE.BankBranch();
                this.DataContext = unit;
                HostingUnitKey_block.DataContext = unit.HostingUnitKey;
                HostKey_Block.DataContext = unit.Owner.HostKey;

                HostingUnitKey_block.Text = unit.HostingUnitKey;
                HostKey_Block.Text = unit.Owner.HostKey;

                UpdateUnit_checkBox.IsChecked = false;
                ExistingHost_checkBox.IsChecked = false;
                UnitToUpdade_comboBox.SelectedIndex = -1;
                ExistingHost_comboBox.SelectedIndex = -1;

                hostingUnitNameTextBox.ClearValue(TextBox.TextProperty);
                privateNameTextBox.ClearValue(TextBox.TextProperty);
                familyNameTextBox.ClearValue(TextBox.TextProperty);
                phoneNumberTextBox.ClearValue(TextBox.TextProperty);
                mailAddressTextBox.ClearValue(TextBox.TextProperty);
                collectionClearanceCheckBox.IsChecked = false;
                bankAccountNumberTextBox.ClearValue(TextBox.TextProperty);
                BankNamecomboBox.ClearValue(ComboBox.TextProperty);
                bankNumberTextBox.ClearValue(TextBox.TextProperty);
                branchAddressTextBox.ClearValue(TextBox.TextProperty);
                branchCityTextBox.ClearValue(TextBox.TextProperty);
                BranchNumbercomboBox.ClearValue(ComboBox.TextProperty);
                areaComboBox.ClearValue(ComboBox.TextProperty);
                subAreaComboBox.ClearValue(ComboBox.TextProperty);
                typeComboBox.ClearValue(ComboBox.TextProperty);

                refreshHostingUnitCombo();
                refreshHostCombo();

                MessageBox.Show("Unit Deleted Successfully");
            }
            catch (CanNotLoadFileInBlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (CanNotDeleteException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch(EmptyInputException ex)
            {
                MessageBox.Show(ex.Message);
            }
             catch(DoesNotExistException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private BE.HostingUnit GetUnitToUpdate_Or_Delete()
        {
            BE.HostingUnit h = UnitToUpdade_comboBox.SelectedValue as BE.HostingUnit;

            if (h == null)
                throw new EmptyInputException("you must select a unit first");

            return h;
        }

        private void refreshHostingUnitCombo()
        {
            UnitToUpdade_comboBox.ItemsSource = bl.get_All_HostingUnits();
        }

        private void UpdateUnit_checkBox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {                
                if (bl.get_All_HostingUnits().Count == 0 && UpdateUnit_checkBox.IsChecked == true)
                {
                    UpdateUnit_checkBox.IsChecked = false;
                    throw new DoesNotExistException("there are no units, therefore you can not update or delete one!");
                }

                if (bl.get_All_HostingUnits().Count != 0 && UpdateUnit_checkBox.IsChecked == true)
                {
                    UnitToUpdate_label.Visibility = System.Windows.Visibility.Visible;
                    UnitToUpdade_comboBox.Visibility = System.Windows.Visibility.Visible;                  
                }
            }
            catch(DoesNotExistException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (EmptyInputException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void UpdateUnit_checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            UnitToUpdade_comboBox.SelectedIndex = -1;
            UnitToUpdate_label.Visibility = System.Windows.Visibility.Hidden;
            UnitToUpdade_comboBox.Visibility = System.Windows.Visibility.Hidden;

            HostingUnitKey_block.Text = unit.HostingUnitKey;
            HostKey_Block.Text = unit.Owner.HostKey;

            hostingUnitNameTextBox.ClearValue(TextBox.TextProperty);
            privateNameTextBox.ClearValue(TextBox.TextProperty);
            familyNameTextBox.ClearValue(TextBox.TextProperty);
            phoneNumberTextBox.ClearValue(TextBox.TextProperty);
            mailAddressTextBox.ClearValue(TextBox.TextProperty);
            collectionClearanceCheckBox.IsChecked = false;
            bankAccountNumberTextBox.ClearValue(TextBox.TextProperty);
            BankNamecomboBox.ClearValue(ComboBox.TextProperty);
            bankNumberTextBox.ClearValue(TextBox.TextProperty);
            branchAddressTextBox.ClearValue(TextBox.TextProperty);
            branchCityTextBox.ClearValue(TextBox.TextProperty);
            BranchNumbercomboBox.ClearValue(ComboBox.TextProperty);
            areaComboBox.ClearValue(ComboBox.TextProperty);
            subAreaComboBox.ClearValue(ComboBox.TextProperty);
            typeComboBox.ClearValue(ComboBox.TextProperty);



        }

        private void UnitToUpdade_comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UnitToUpdade_comboBox.SelectedIndex == -1)
                return;

            BE.HostingUnit H = new BE.HostingUnit();
            H.Owner = new BE.Host();
            H.Owner.BankBranchDetails = new BE.BankBranch();

            H = GetUnitToUpdate_Or_Delete();          
            HostingUnitOptionsGrid.DataContext = H;

            HostingUnitKey_block.Text = H.HostingUnitKey;
            hostingUnitNameTextBox.Text = H.HostingUnitName;
            areaComboBox.Text = (H.Area).ToString();
            subAreaComboBox.Text = (H.SubArea).ToString();
            typeComboBox.Text = (H.Type).ToString();
            HostKey_Block.Text = H.Owner.HostKey;
            privateNameTextBox.Text = H.Owner.PrivateName;
            familyNameTextBox.Text = H.Owner.FamilyName;
            phoneNumberTextBox.Text = H.Owner.PhoneNumber;
            mailAddressTextBox.Text = H.Owner.MailAddress;
            collectionClearanceCheckBox.IsChecked = H.Owner.CollectionClearance;
            bankAccountNumberTextBox.Text = H.Owner.BankAccountNumber;
            BankNamecomboBox.Text = H.Owner.BankBranchDetails.BankName;
            bankNumberTextBox.Text = H.Owner.BankBranchDetails.BankNumber;
            branchAddressTextBox.Text = H.Owner.BankBranchDetails.BranchAddress;
            branchCityTextBox.Text = H.Owner.BankBranchDetails.BranchCity;

            int index = bl.GetBrachNumbers(H.Owner.BankBranchDetails.BankName).FindIndex(ln => ln == H.Owner.BankBranchDetails.branchNumber);

            BranchNumbercomboBox.SelectedIndex = index;

            //BranchNumbercomboBox.Text = H.Owner.BankBranchDetails.BranchNumber;
        }
        private BE.Host GetHostExisting()
        {
            BE.Host h = ExistingHost_comboBox.SelectedValue as BE.Host;

            if (h == null)
                throw new EmptyInputException("you must select a Host first");

            return h;
        }

        private void refreshHostCombo()
        {
            ExistingHost_comboBox.ItemsSource = bl.get_All_Host();
        }


        private void ExistingHost_checkBox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (bl.get_All_Host().Count == 0 && ExistingHost_checkBox.IsChecked == true)//if it was set as there are, but the list is empty
                {
                    ExistingHost_checkBox.IsChecked = false;//initialize
                    throw new DoesNotExistException("there are no Hosts, therefore you can not choose!");
                }

                if (bl.get_All_Host().Count != 0 && ExistingHost_checkBox.IsChecked == true)
                {
                    chooseExistingHost_Label.Visibility = System.Windows.Visibility.Visible;
                    ExistingHost_comboBox.Visibility = System.Windows.Visibility.Visible;
                }
            }
            catch (DoesNotExistException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (EmptyInputException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ExistingHost_checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            chooseExistingHost_Label.Visibility = System.Windows.Visibility.Hidden;
            ExistingHost_comboBox.Visibility = System.Windows.Visibility.Hidden;

            privateNameTextBox.ClearValue(TextBox.TextProperty);
            familyNameTextBox.ClearValue(TextBox.TextProperty);
            phoneNumberTextBox.ClearValue(TextBox.TextProperty);
            mailAddressTextBox.ClearValue(TextBox.TextProperty);
            collectionClearanceCheckBox.IsChecked = false;
            bankAccountNumberTextBox.ClearValue(TextBox.TextProperty);
            BankNamecomboBox.ClearValue(ComboBox.TextProperty);
            bankNumberTextBox.ClearValue(TextBox.TextProperty);
            branchAddressTextBox.ClearValue(TextBox.TextProperty);
            branchCityTextBox.ClearValue(TextBox.TextProperty);
            BranchNumbercomboBox.ClearValue(ComboBox.TextProperty);

            unit = new BE.HostingUnit();
            unit.Owner = new BE.Host();
            unit.Owner.BankBranchDetails = new BE.BankBranch();
            this.DataContext = unit;
            HostingUnitKey_block.DataContext = unit.HostingUnitKey;
            HostKey_Block.DataContext = unit.Owner.HostKey;

            HostingUnitKey_block.Text = unit.HostingUnitKey;
            HostKey_Block.Text = unit.Owner.HostKey;
            ExistingHost_comboBox.SelectedIndex = -1;
            BranchNumbercomboBox.SelectedIndex = -1;
        }

        private void ExistingHost_comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ExistingHost_comboBox.SelectedIndex == -1)
                return;        

            BE.Host h = new BE.Host();
            h.BankBranchDetails = new BE.BankBranch();
            HostingUnitOptionsGrid.DataContext = h;
            h = GetHostExisting();

            List<long> branchNumbers = bl.GetBrachNumbers(h.BankBranchDetails.BankName);
            BranchNumbercomboBox.ItemsSource = branchNumbers;

            HostKey_Block.Text = h.HostKey;
            privateNameTextBox.Text = h.PrivateName;
            familyNameTextBox.Text = h.FamilyName;
            phoneNumberTextBox.Text = h.PhoneNumber;
            mailAddressTextBox.Text = h.MailAddress;
            collectionClearanceCheckBox.IsChecked = h.CollectionClearance;
            bankAccountNumberTextBox.Text = h.BankAccountNumber;

            BankNamecomboBox.Text = h.BankBranchDetails.BankName;
            bankNumberTextBox.Text = h.BankBranchDetails.BankNumber;
            branchAddressTextBox.Text = h.BankBranchDetails.BranchAddress;
            branchCityTextBox.Text = h.BankBranchDetails.BranchCity;
            BranchNumbercomboBox.Text = h.BankBranchDetails.BranchNumber;
            BranchNumbercomboBox.SelectedValue= h.BankBranchDetails.BranchNumber;

            int index = bl.GetBrachNumbers(h.BankBranchDetails.BankName).FindIndex(ln => ln == h.BankBranchDetails.branchNumber);

            BranchNumbercomboBox.SelectedIndex = index;


        }

        private void BankNamecomboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BankNamecomboBox.SelectedIndex == -1)
                return;
            try
            {
                List<long> branchNumbers = bl.GetBrachNumbers(BankNamecomboBox.SelectedValue as string);
                BranchNumbercomboBox.ItemsSource = branchNumbers;

                bankNumberTextBox.ClearValue(TextBox.TextProperty);
                branchAddressTextBox.ClearValue(TextBox.TextProperty);
                branchCityTextBox.ClearValue(TextBox.TextProperty);
            }
            catch(DoesNotExistException ex)
            {
                MessageBox.Show(ex.Message);
            }                     
        }

        private void BranchNumbercomboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (BranchNumbercomboBox.SelectedIndex == -1)
                    return;

                long numberOfBranch = long.Parse(BranchNumbercomboBox.SelectedValue.ToString());
                string name = BankNamecomboBox.SelectedValue as string; 

                if (name == null)
                {
                    BranchNumbercomboBox.ClearValue(ComboBox.TextProperty);
                    BranchNumbercomboBox.SelectedIndex = -1;
                    throw new DoesNotExistException("the name of bank was not chosen, therefore you can not select a branch, choose a bank name first!");                                   
                }

                BE.BankBranch branch = bl.GetBranchByNumberAndName(numberOfBranch, name);
                bankNumberTextBox.Text = branch.BankNumber;
                branchAddressTextBox.Text = branch.BranchAddress;
                branchCityTextBox.Text = branch.BranchCity;
            }          
            catch (DoesNotExistException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
