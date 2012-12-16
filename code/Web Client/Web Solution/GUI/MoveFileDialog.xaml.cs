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
    public partial class MoveFileDialog : ChildWindow
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
            populateComboBox(Source);

            labelDocumentName.Text = SelectedItem.Header.ToString();
        }

        private void populateComboBox(ItemCollection Items)
        {
            foreach (TreeViewItem item in Items)
            {
                ComboBoxItem comboItem = new ComboBoxItem();
                comboItem.Content = item.Header;
                comboBoxFolders.Items.Add(comboItem);
                if (item.Items.Count > 0)
                {
                    populateComboBox(item.Items);
                }
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

        public ItemCollection Source
        {
            get;
            set;
        }

        private void buttonMove_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxFolders.SelectedItem != null)
            {
                FromPath = SelectedItem.Tag.ToString();
                if (((ComboBoxItem)comboBoxFolders.SelectedValue).Content.ToString() == "Root folder")
                {
                    ToPath = Session.GetInstance().RootFolderPath + "\\" + SelectedItem.Header + ".txt";
                }
                else
                {
                    ToPath = Session.GetInstance().RootFolderPath + "\\" + ((ComboBoxItem)comboBoxFolders.SelectedValue).Content.ToString() + "\\" + SelectedItem.Header + ".txt";
                }

                this.Close();
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
