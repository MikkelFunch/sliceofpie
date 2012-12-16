using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Web_Solution.GUI
{
    public partial class LoginDialog : ChildWindow
    {
        public LoginDialog()
        {
            InitializeComponent();
        }

        public Boolean Online
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public string UnencryptedPass
        {
            get;
            set;
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            Email = textboxEmail.Text;
            UnencryptedPass = passwordBox.Password;

            this.Close();
        }
    }
}

