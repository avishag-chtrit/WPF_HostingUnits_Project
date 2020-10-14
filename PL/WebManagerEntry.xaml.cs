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

namespace PL
{
    /// <summary>
    /// Interaction logic for WebManagerEntry.xaml
    /// </summary>
    public partial class WebManagerEntry : Window
    {
        public WebManagerEntry()
        {
            InitializeComponent();
        }

        private void Password_button_Click(object sender, RoutedEventArgs e)
        {
            //if(Password_textbox.ToString()=="6543")
            if (passwordbox.Password.ToString()=="6543")
            {
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Wrong Password. Please try again\n Look at the clue bellow");

            }
        }
    }
}
