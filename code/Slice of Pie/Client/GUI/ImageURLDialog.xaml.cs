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
    /// Interaction logic for ImageURLDialog.xaml
    /// </summary>
    public partial class ImageURLDialog : Window
    {
        public ImageURLDialog()
        {
            InitializeComponent();
        }

        private void buttonInsert_Click(object sender, RoutedEventArgs e)
        {
            URLString = textBoxURL.Text;
            this.Close();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public string URLString
        {
            get;
            private set;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            textBoxURL.Focus();
        }
    }
}