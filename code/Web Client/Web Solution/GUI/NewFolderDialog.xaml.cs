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
    public partial class NewFolderDialog : ChildWindow
    {
        public NewFolderDialog()
        {
            InitializeComponent();
        }

        public String FolderTitle
        {
            get;
            set;
        }

        public TreeViewItem SelectedItem
        {
            get;
            set;
        }

        private void buttonCreate_Click(object sender, RoutedEventArgs e)
        {
            if (textboxTitle.Text.Length > 0)
            {
                FolderTitle = textboxTitle.Text;
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("Enter folder name");
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            textboxTitle.Focus();
        }
    }
}
