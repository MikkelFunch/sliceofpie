using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Web_Solution
{
    class TreeViewModel
    {
        //Singleton instance of tree view model.
        private static TreeViewModel instance;

        /// <summary>
        /// Accessor method to get the single instance of tree view.
        /// </summary>
        /// <returns>The single instance of tree view</returns>
        public static TreeViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new TreeViewModel();
            }
            return instance;
        }

        private object nullItem = null; //in order to lazy load files and folders - Makes folders expandable

        /// <summary>
        /// Creates the tree view with the specified files and folders.
        /// </summary>
        /// <param name="items">The UI componenets the tree view shall consist of</param>
        /// <param name="foldersAndFiles">The files and folders to be displayed in the tree view</param>
        public void LoadFilesAndFolders(ItemCollection items, string[][][] foldersAndFiles)
        {
            items.Clear();
            string[][] folders = foldersAndFiles[0];
            string[][] files = foldersAndFiles[1];

            foreach (string[] folder in folders)
            {
                InsertFolder(folder, items);
            }
            foreach (string[] file in files)
            {
                InsertDocument(file, items);
            }
        }

        /// <summary>
        /// Inserts a folder into the specified UI components in the tree view.
        /// </summary>
        /// <param name="folder">The folder to be inserted</param>
        /// <param name="items">The items that the tree view consists of</param>
        public void InsertFolder(string[] folder, ItemCollection items)
        {
            TreeViewItem folderItem = new TreeViewItem();
            folderItem.Header = folder[1];
            folderItem.Tag = new object[] { folder[0], folder[2], true};

            InsertFolderHelper(folder, items);
            
            //add to root
            if(int.Parse(((object[])(folderItem.Tag))[1].ToString()) == Session.GetInstance().RootFolderID)
                items.Add(folderItem);
        }

        private void InsertFolderHelper(string[] folder, ItemCollection items)
        {
            TreeViewItem folderItem = new TreeViewItem();
            folderItem.Header = folder[1];
            folderItem.Tag = new object[] { folder[0], folder[2], true };

            foreach (TreeViewItem item in items)
            {
                object[] tag = (object[])item.Tag;

                if ((bool)tag[2] == true && int.Parse(((object[])item.Tag)[0].ToString()) == int.Parse(folder[2]))
                {//add to specified folder
                    item.Items.Add(folderItem);
                    return;
                }
                InsertFolderHelper(folder, item.Items);
            }
        }

        /// <summary>
        /// Inserts a document into the specified UI components in the tree view.
        /// </summary>
        /// <param name="document">The document to be inserted</param>
        /// <param name="items">The items that the tree view consists of</param>
        public void InsertDocument(string[] document, ItemCollection items)
        {
            TreeViewItem documentItem = new TreeViewItem();
            documentItem.Header = document[2];
            documentItem.Tag = new object[] { document[0], document[1], false }; //0:document id, 1:folderid,2false->not a folder
            documentItem.MouseLeftButtonUp += new MouseButtonEventHandler(documentItem_MouseLeftButtonUp);

            InsertDocumentHelper(document, items);

            //add to root
            if (int.Parse(((object[])(documentItem.Tag))[1].ToString()) == Session.GetInstance().RootFolderID)
                items.Add(documentItem);
        }

        private void InsertDocumentHelper(string[] document, ItemCollection items)
        {
            TreeViewItem documentItem = new TreeViewItem();
            documentItem.Header = document[2];
            documentItem.Tag = new object[] { document[0], document[1], false }; //0:document id, 1:folderid,2false->not a folder
            documentItem.MouseLeftButtonUp += new MouseButtonEventHandler(documentItem_MouseLeftButtonUp);

            foreach (TreeViewItem item in items)
            {       //check if the item is a folder
                if ((bool)((object[])item.Tag)[2] == true)
                {
                    //check if the document item lies in the folder
                    if (int.Parse(((object[])item.Tag)[0].ToString()) == int.Parse(document[1]))
                    {
                        item.Items.Add(documentItem);
                        return;
                    }
                    InsertDocumentHelper(document, item.Items);
                }
            }
        }

        /// <summary>
        /// Event handler for mouse event on document selection. Specifies the document to be opened.
        /// </summary>
        /// <param name="sender">The tree view item getting selected</param>
        /// <param name="e">Event arguments</param>
        public void documentItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            bool doubleClick = MouseButtonHelper.IsDoubleClick(sender, e);
            if (doubleClick)
            {
                TreeViewItem item = (TreeViewItem)sender;
                Controller.GetInstance().SetOpenDocument(int.Parse(((object[])item.Tag)[0].ToString()), item.Header.ToString(),int.Parse(((object[])item.Tag)[1].ToString()));
                //open file in view
            }
        }

        /// <summary>
        /// Gets the path from the root folder to a specific folder.
        /// </summary>
        /// <param name="folderId">The id of the folder to retrieve the path from</param>
        /// <param name="items">The UI components the tree view consists of</param>
        /// <returns>The relative path from the root folder to the specified folder</returns>
        public string GetRelativePath(int folderId, ItemCollection items)
        {
            StringBuilder relativePath = new StringBuilder();
            
            while (folderId != Session.GetInstance().RootFolderID)
            {
                TreeViewItem folderItem = GetFolderTag(folderId,items);
                relativePath.Insert(0, "\\" + folderItem.Header);
                folderId = int.Parse(((object[])folderItem.Tag)[1].ToString());
            }

            relativePath.Insert(0, Session.GetInstance().Email);
            relativePath.Append("\\");
            return relativePath.ToString();
        }

        private TreeViewItem GetFolderTag(int folderID, ItemCollection items)
        {
            foreach (TreeViewItem item in items)
            {
                //is the item a folder
                if ((bool)((object[])item.Tag)[2] == true)
                {
                    //is it the folder with given id
                    if (int.Parse(((object[])item.Tag)[0].ToString()) == folderID)
                    {
                        //return the tag
                        return item;
                        
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Removes a document from the tree view.
        /// </summary>
        /// <param name="documentId">The id of the document to be removed</param>
        /// <param name="items">The UI components the tree view consists of</param>
        public void RemoveDocument(int documentId, ItemCollection items)
        {
            foreach (TreeViewItem item in items)
            {
                object[] tag = (object[])item.Tag;
                if ((bool)tag[2] == false && int.Parse(tag[0].ToString()) == documentId)
                {
                    items.Remove(item);
                    return;
                }
                else if ((bool)tag[2] == true)
                {
                    RemoveDocument(documentId, item.Items);
                }
            }
        }
    }

    /// <summary>
    /// Imported class to detect double clicks.
    /// </summary>
    public static class MouseButtonHelper
    {
        private const long k_DoubleClickSpeed = 500;
        private const double k_MaxMoveDistance = 10;

        private static long _LastClickTicks = 0;
        private static Point _LastPosition;
        private static WeakReference _LastSender;

        internal static bool IsDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(null);
            long clickTicks = DateTime.Now.Ticks;
            long elapsedTicks = clickTicks - _LastClickTicks;
            long elapsedTime = elapsedTicks / TimeSpan.TicksPerMillisecond;
            bool quickClick = (elapsedTime <= k_DoubleClickSpeed);
            bool senderMatch = (_LastSender != null && sender.Equals(_LastSender.Target));

            if (senderMatch && quickClick && position.Distance(_LastPosition) <= k_MaxMoveDistance)
            {
                // Double click!
                _LastClickTicks = 0;
                _LastSender = null;
                return true;
            }

            // Not a double click
            _LastClickTicks = clickTicks;
            _LastPosition = position;
            if (!quickClick)
                _LastSender = new WeakReference(sender);
            return false;
        }

        private static double Distance(this Point pointA, Point pointB)
        {
            double x = pointA.X - pointB.X;
            double y = pointA.Y - pointB.Y;
            return Math.Sqrt(x * x + y * y);
        }
    }
}