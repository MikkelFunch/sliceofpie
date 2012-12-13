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
    /// Interaction logic for NewDocumentDialog.xaml
    /// </summary>
    public partial class NewDocumentDialog : Window
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
                this.Close();
            }
            else
            {
                MessageBox.Show("Enter document name", "Creation error");
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            textBoxTitle.Focus();
        }
    }
}
