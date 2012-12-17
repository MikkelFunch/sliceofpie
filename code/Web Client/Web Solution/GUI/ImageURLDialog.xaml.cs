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
    public partial class ImageURLDialog : ChildWindow
    {
        public ImageURLDialog()
        {
            InitializeComponent();
        }

        private void buttonInsert_Click(object sender, RoutedEventArgs e)
        {
            URLString = textBoxURL.Text;
            this.DialogResult = true;
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        public string URLString
        {
            get;
            private set;
        }
    }
}

