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
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.IO;

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for ExplorerView.xaml
    /// </summary>
    public partial class ExplorerView : UserControl
    {
        public ExplorerView()
        {
            InitializeComponent();
        }

        private object nullItem = null; //in order to lazy load files and folders - Makes folders expandable

        /// <summary>
        /// Method being run when the view has been loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExplorerView_Loaded(object sender, RoutedEventArgs e)
        {
            //Get path to the current users files
            String folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\sliceofpie\\" + Model.Username;
            //Create a DirectoryInfo for that folder
            DirectoryInfo dir = new DirectoryInfo(folderPath);

            //Insert dictionaries and files from the folder as items in treeview
            insertDirectoryIntoDirectory(ExplorerTree.Items, dir);
            insertFilesIntoDirectory(ExplorerTree.Items, dir);
        }

        /// <summary>
        /// Method being ran when a folder is expanded in the treeview
        /// </summary>
        /// <param name="sender">The item being expanded</param>
        /// <param name="e"></param>
        private void folderExpanded(object sender, RoutedEventArgs e)
        {
            //Get the treeviewitem and create a DirectoryInfo for the folder which it represent
            TreeViewItem item = (TreeViewItem)sender;
            DirectoryInfo dir = new DirectoryInfo(item.Tag.ToString());

            //If the folder being expanded has never been expanded before
            if (item.Items.Count == 1 && item.Items[0] == nullItem)
            {
                //Clear the items(remove nullItem)
                item.Items.Clear();
                //Insert dictionaries and files from the folder as subitems in the item
                insertDirectoryIntoDirectory(item.Items, dir);
                insertFilesIntoDirectory(item.Items, dir);

                //If no itmes are found, insert the nullItem, so that the folder still looks expanable
                if (item.Items.Count == 0)
                {
                    item.Items.Add(nullItem);
                    item.IsExpanded = false;
                }
            }
        }

        /// <summary>
        /// Inserts all directories in the given directory into the given collection
        /// </summary>
        /// <param name="collection">Collection which should contain the directories</param>
        /// <param name="dir">DirectoryInfo for the directory which should be explored</param>
        private void insertDirectoryIntoDirectory(ItemCollection collection, DirectoryInfo dir)
        {
            //For each directory in the given directory
            foreach (DirectoryInfo dInfo in dir.GetDirectories())
            {
                //Create a treeviewitem to represent the directory
                TreeViewItem subItem = new TreeViewItem();
                //Set herder to the folders name
                subItem.Header = dInfo.Name;
                //Set tag to the full path to the folder
                subItem.Tag = dInfo.FullName;
                //add a nullItem to the folder, in roder to make it expandable
                subItem.Items.Add(nullItem);
                //When the Expand event occurs, call folderExpanded
                subItem.Expanded += new RoutedEventHandler(folderExpanded);
                //Add the item to the collection
                collection.Add(subItem);
            }
        }

        /// <summary>
        /// Inserts all files in the given directory into the given collection
        /// </summary>
        /// <param name="collection">Collection which should contain the files</param>
        /// <param name="dir">DirectoryInfo for the directory which should be explored</param>
        private void insertFilesIntoDirectory(ItemCollection collection, DirectoryInfo dir)
        {
            //For each file in the given directory
            foreach (FileInfo file in dir.GetFiles("*.txt"))
            {
                //Create a treeviewitem to represent the file
                TreeViewItem subItem = new TreeViewItem();
                //Set the header to be the name of the file
                subItem.Header = file.Name;
                //Add the file to the collction
                collection.Add(subItem);
            }
        }
    }
}
