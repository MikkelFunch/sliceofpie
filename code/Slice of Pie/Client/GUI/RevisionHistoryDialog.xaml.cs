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
            labelDocumentName.Content = DocumentName;
            labelEditor.Content = "Editor: " + EditorName;
            CreateTreeView();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CreateTreeView()
        {
            foreach (string[] item in Revisions)
            {
                TreeViewItem treeItem = new TreeViewItem();
                treeItem.Header = item[0];
                treeItem.Tag = item;
                treeItem.MouseDoubleClick += new MouseButtonEventHandler(ItemDoubleClick);
            }
        }

        private void ItemDoubleClick(object sender, RoutedEventArgs e)
        {
            //Get the treeviewitem and create a DirectoryInfo for the folder which it represent
            TreeViewItem item = (TreeViewItem)sender;

            richTextBoxCurrentRevision.Document = (FlowDocument)System.Windows.Markup.XamlReader.Parse(((string[])item.Tag)[2]);
            labelEditor.Content = ((string[])item.Tag)[1];
            labelCurrentTimeStamp.Content = ((string[])item.Tag)[0];
        }
    }
}
