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
    public partial class ShareDocumentDialog : ChildWindow
    {
        public ShareDocumentDialog()
        {
            InitializeComponent();
        }

        public string Email
        {
            get;
            set;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            textBoxEmail.Focus();
        }

        private void buttonShare_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxEmail.Text.Length > 0)
            {
                Email = textBoxEmail.Text;
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("Enter email address");
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
