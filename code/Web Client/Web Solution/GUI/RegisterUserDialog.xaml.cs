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
    public partial class RegisterUserDialog : ChildWindow
    {
        public RegisterUserDialog()
        {
            InitializeComponent();
        }

        public string Email
        {
            get;
            set;
        }

        public string PassUnencrypted1
        {
            get;
            set;
        }

        public string PassUnencrypted2
        {
            get;
            set;
        }
         

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            Email = textBoxEmail.Text;
            PassUnencrypted1 = passwordBox1.Password;
            PassUnencrypted2 = passwordBox2.Password;

            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}