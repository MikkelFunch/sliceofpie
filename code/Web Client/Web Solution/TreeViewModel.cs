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
        private static TreeViewModel instance;
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
        /// id,name,parent
        /// </summary>
        /// <param name="items"></param>
        /// <param name="foldersAndFiles"></param>
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
        /// 0:id, 1:name, 2:parentfolderid
        /// </summary>
        /// <param name="folders"></param>
        /// <param name="items"></param>
        public void InsertFolder(string[] folder, ItemCollection items)
        {
            TreeViewItem folderItem = new TreeViewItem();
            folderItem.Header = folder[1];
            folderItem.Opacity = 0.5;
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
                if (int.Parse(((object[])item.Tag)[0].ToString()) == int.Parse(folder[2]))
                {//add to specified folder
                    item.Items.Add(folderItem);
                    return;
                }
                InsertFolderHelper(folder, item.Items);
            }
        }

        /// <summary>
        /// 0:document id,1:folderid,2:name
        /// </summary>
        /// <param name="document"></param>
        /// <param name="items"></param>
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

        public string GetRelativePath(int folderId, ItemCollection items)
        {
            StringBuilder relativePath = new StringBuilder();
            
            while (folderId != Session.GetInstance().RootFolderID)
            {
                TreeViewItem folderItem = GetFolderTag(folderId,items);
                relativePath.Insert(0, "\\" + folderItem.Header);
                folderId = (int)((object[])folderItem.Tag)[1];
            }

            relativePath.Append(Session.GetInstance().Email + "\\");
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

        public void RemoveDocument(int documentId, ItemCollection items)
        {
            foreach (TreeViewItem item in items)
            {
                object[] tag = (object[])item.Tag;
                if()
            }
        }
    }

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