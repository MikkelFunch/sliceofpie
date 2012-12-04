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
            string uname = textBoxUser.Text;
            string passUnencrypted = passwordBox1.Password;

            if (uname.Length > 0)
            {
                if (passUnencrypted != null && passUnencrypted.Length > 0 && passUnencrypted == passwordBox2.Password)
                {
                    string pass = Security.EncryptPassword(passUnencrypted);
                    using (WcfServiceReference.ServiceClient proxy = new WcfServiceReference.ServiceClient())
                    {
                        Boolean successful = proxy.AddUser(uname, pass);
                        if (!successful)
                        {
                            MessageBox.Show("User exsists", "Creation error");
                        }
                    }
                }
                else //the passwords does not match
                {
                    MessageBox.Show("User could not be created./nEntered passwords does not match", "Creation error");
                }
            }
            else
            {
                MessageBox.Show("Enter username", "Creation error");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
