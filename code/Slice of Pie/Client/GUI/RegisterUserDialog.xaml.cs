using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Interaction logic for RegisterUserDialog.xaml
    /// </summary>
    public partial class RegisterUserDialog : Window
    {
        public RegisterUserDialog()
        {
            InitializeComponent();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            string username = textBoxUser.Text;
            string password = textBoxPass1.Text;

            /*using (WcfServiceLibrary.ServiceClient proxy = new WcfServiceLibrary.ServiceClient())
            {
                proxy.AddUser(username, password);
            }*/
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
