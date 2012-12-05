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
            String email = textboxEmail.Text;
            String unencrytedPass = passwordBox.Password;

            if (unencrytedPass.Length > 0 && email.Length > 0)
            {
                String pass = Security.EncryptPassword(unencrytedPass);
                using (WcfServiceReference.ServiceClient proxy = new WcfServiceReference.ServiceClient())
                {
                    int userID = proxy.GetUserByEmailAndPass(email, pass);
                    if(userID != -1)
                    {
                        Model.UserID = userID;
                        Console.WriteLine("You did it, you gui bastard");
                        this.Close();

                        //Login event
                        if(LoginEvent != null)
                            LoginEvent();
                    }
                    else
                    {
                        MessageBox.Show("Wrong email or password", "Unable to login");
                    }
                }
            }
            else
            {
                MessageBox.Show("Enter email and password", "Login error");
            }
        }

        public static event Action LoginEvent;
    }
}
