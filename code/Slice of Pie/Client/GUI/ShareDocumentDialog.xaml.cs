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

namespace Client.GUI
{
    /// <summary>
    /// Interaction logic for ShareDocumentDialog.xaml
    /// </summary>
    public partial class ShareDocumentDialog : Window
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
                this.Close();
            }
            else
            {
                MessageBox.Show("Enter email address", "Email error");
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
