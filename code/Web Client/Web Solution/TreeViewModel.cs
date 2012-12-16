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

            foreach (string[] folder in folders)
            {
                InsertFolder(folder, items);
            }
        }

        /// <summary>
        /// 0:id, 1:name, 2:parentfodlerid
        /// </summary>
        /// <param name="folders"></param>
        /// <param name="items"></param>
        public void InsertFolder(string[] folder, ItemCollection items)
        {
            TreeViewItem folderItem = new TreeViewItem();
            folderItem.Header = folder[1];
            folderItem.Tag = new object[] { folder[0], folder[2], true }; 

            foreach (TreeViewItem item in items)
            {
                if ((int)((object[])item.Tag)[0] == int.Parse(folder[2]))
                {//add to specified folder
                    item.Items.Add(folderItem);
                    return;
                }
	        }
            //add to root
            items.Add(folderItem);
        }

        /// <summary>
        /// document id,folderid,name
        /// </summary>
        /// <param name="document"></param>
        /// <param name="items"></param>
        public void InsertDocument(string[] document, ItemCollection items)
        {
            TreeViewItem documentItem = new TreeViewItem();
            documentItem.Header = document[2];
            documentItem.Tag = new object[] { document[0], document[1], false }; //0:document id, 1:folderid,2false->not a folder
            documentItem.MouseLeftButtonDown += new MouseButtonEventHandler(documentItem_MouseLeftButtonDown);

            foreach (TreeViewItem item in items)
            {       //check if the item is a folder             check if the document item lies in the folder
                if ((bool)((object[])item.Tag)[2] == true && (int)((object[])item.Tag)[0] == int.Parse(document[1]))
                {
                    item.Items.Add(documentItem);
                    return;
                }
            }

            //add to root
            items.Add(documentItem);
        }

        public void documentItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            bool doubleClick = MouseButtonHelper.IsDoubleClick(sender, e);
            if (doubleClick)
            {
                TreeViewItem item = (TreeViewItem)sender;
                Controller.GetInstance().SetOpenDocument((int)((object[])item.Tag)[0], item.Header.ToString());
                //open file in view
            }
        }

        /*private void OpenDocment(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            String text = File.ReadAllText(item.Tag.ToString()); //IOException, if file is used by another program
            Controller.GetInstance().SetOpenDocument(text, item.Header.ToString(), //item.Tag.ToString());
        }*/

        /*public void AddItem(string header, object tag, ItemCollection items, bool folder)
        {
            TreeViewItem subItem = new TreeViewItem();
            subItem.Header = header;
            subItem.Tag = tag;

            if (folder)
            {
                subItem.Items.Add(nullItem);
                subItem.Expanded += new RoutedEventHandler(FolderExpanded);
            }
            else
            {
                subItem.MouseLeftButtonDown += new MouseButtonEventHandler(OpenDocment);
            }

            items.Add(subItem);
        }*/
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