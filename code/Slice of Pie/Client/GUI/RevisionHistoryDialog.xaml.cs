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
    /// Interaction logic for RevisionHistoryDialog.xaml
    /// </summary>
    public partial class RevisionHistoryDialog : Window
    {
        public RevisionHistoryDialog()
        {
            InitializeComponent();
        }

        public String[][] Revisions
        {
            get;
            set;
        }

        public string DocumentName
        {
            get;
            set;
        }

        public string EditorName
        {
            get;
            set;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            labelDocumentName.Content = 
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
