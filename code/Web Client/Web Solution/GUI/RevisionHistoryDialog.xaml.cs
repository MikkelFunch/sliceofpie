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

        public ItemCollection Items
        {
            get;
            set;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            labelDocumentName.Text = DocumentName;
            labelEditor.Text = "Editor: " + EditorName;
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        public void CreateTreeView()
        {
            treeViewRevisions.Items.Clear();
            foreach (TreeViewItem item in Items)
            {
                treeViewRevisions.Items.Add(item);
            }
        }
    }
}
