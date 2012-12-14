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
using System.IO;

namespace Client.GUI
{
    /// <summary>
    /// Interaction logic for MoveFileDialog.xaml
    /// </summary>
    public partial class MoveFileDialog : Window
    {
        public MoveFileDialog()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBoxItem item = new ComboBoxItem();
            item.Content = "Root folder";
            comboBoxFolders.Items.Add(item);
            populateComboBox(Session.GetInstance().RootFolderPath);

            labelDocumentName.Content = SelectedItem.Header;
        }

        private void populateComboBox(String path)
        {
            foreach (String dirPath in Directory.GetDirectories(path))
            {
                populateComboBox(dirPath);
                ComboBoxItem item = new ComboBoxItem();
                item.Content = dirPath.Remove(0, Session.GetInstance().RootFolderPath.Length);
                comboBoxFolders.Items.Add(item);
            }
        }

        public TreeViewItem SelectedItem
        {
            get;
            set;
        }

        public String FromPath
        {
            get;
            set;
        }

        public String ToPath
        {
            get;
            set;
        }

        private void buttonMove_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxFolders.SelectedItem != null)
            {
                FromPath = SelectedItem.Tag.ToString();
                ToPath = Session.GetInstance().RootFolderPath + "\\" + ((ComboBoxItem)comboBoxFolders.SelectedValue).Content.ToString() + "\\" + SelectedItem.Header + ".txt";

                this.Close();
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
