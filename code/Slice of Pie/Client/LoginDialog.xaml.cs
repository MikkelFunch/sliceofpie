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
    /// Interaction logic for LoginDialog.xaml
    /// </summary>
    public partial class LoginDialog : Window
    {
        public LoginDialog()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            String user = textboxEmail.Text;
            String unencrytedPass = passwordBox.Password;

            if (unencrytedPass.Length > 0 && user.Length > 0)
            {
                String pass = Security.EncryptPassword(unencrytedPass);
                using (WcfServiceReference.ServiceClient proxy = new WcfServiceReference.ServiceClient())
                {
                    //int successful = proxy
                }
            }
            
        }
    }
}
