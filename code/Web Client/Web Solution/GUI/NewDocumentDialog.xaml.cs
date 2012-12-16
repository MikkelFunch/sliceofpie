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
    public partial class NewDocumentDialog : ChildWindow
    {
        public NewDocumentDialog()
        {
            InitializeComponent();
        }

        public String DocumentTitle
        {
            get;
            private set;
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            //if anything in
            if (textBoxTitle.Text.Length > 0)
            {
                DocumentTitle = textBoxTitle.Text;
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("Enter document name");
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            textBoxTitle.Focus();
        }
    }
}

