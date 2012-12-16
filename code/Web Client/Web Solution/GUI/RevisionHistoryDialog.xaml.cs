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
    public partial class RevisionHistoryDialog : ChildWindow
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
            labelDocumentName.Text= DocumentName;
            labelEditor.Text = "Editor: " + EditorName;
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void CreateTreeView()
        {
            foreach (string[] item in Revisions)
            {
                TreeViewItem treeItem = new TreeViewItem();
                treeItem.Header = item[0];
                treeItem.Tag = item;
                treeItem.MouseLeftButtonDown += new MouseButtonEventHandler(ItemLeftMouseDownClick);
                treeViewRevisions.Items.Add(treeItem);
            }
        }

        private void ItemLeftMouseDownClick(object sender, MouseButtonEventArgs e)
        {
            bool doubleClick = MouseButtonHelper.IsDoubleClick(sender, e);
            if (doubleClick)
            {/*
                //Get the treeviewitem and create a DirectoryInfo for the folder which it represent
                TreeViewItem item = (TreeViewItem)sender;
                richTextBoxCurrentRevision.Selection.Xaml = Controller.GetInstance().GetDocumentContentFromId((int)item.Tag);
                labelEditor.Text = "Editor: " + ((string[])item.Tag)[1];
                labelCurrentTimeStamp.Text = "Current time: " + item.Header;*/
            }
        }
    }
}
