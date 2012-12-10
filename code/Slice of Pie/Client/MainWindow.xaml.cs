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
using System.Threading;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Controller controller;
        Point _lastMouseDown;
        TreeViewItem draggedItem, _target;

        public MainWindow()
        {
            InitializeComponent();
            controller = Controller.GetInstance();
            controller.SetGui(this);
        }

        private void buttonImage_Click(object sender, RoutedEventArgs e)
        {
            string url = null;

            ImageURLDialog imDialog = new ImageURLDialog();
            imDialog.ShowDialog();

            //get inserted url
            url = imDialog.URLString;

            //check if it is a valid url
            /*if (url != null && (url.StartsWith("http://") || url.StartsWith("https://")))
            {
                richTextBox.Document
            }*/


            /// Show an image rather than a text with the link
            /*
            string url = null;

            ImageURLDialog imDialog = new ImageURLDialog();
            imDialog.ShowDialog();

            //get inserted url
            url = imDialog.URLString;
             

            //check if it is a valid url
            if (url != null && (url.StartsWith("http://") || url.StartsWith("https://")))
            {
             * Create bitmapimage refrenceing the online link to download it
                BitmapImage bitmap = new BitmapImage(new Uri(url, UriKind.RelativeOrAbsolute));
                
                //bitmap.DownloadCompleted += controller.GetDownloadCompleteEventHandler();
                bitmap.DownloadCompleted += new EventHandler(controller.DownloadComplete);
             * 
             * //Create a new source refrenceing the image locally
                Uri newSource = new Uri(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\sliceofpie\\pics\\" + Security.EncryptPassword(url) + ".jpg", UriKind.RelativeOrAbsolute);

             
             * create bitmapimage refrencing the local source
                BitmapImage localImage = new BitmapImage(newSource);
             * * Create the image which will be shown in gui
                Image image = new Image();
             *  set the source of the image
                image.Source = localImage;
                image.Width = 130;
                image.Height = 130;
                image.Tag = newSource;
                
             * insert the image
                Paragraph p = (Paragraph) richTextBox.Document.Blocks.LastBlock;
                p.Inlines.Add(image);
                richTextBox.Document.Blocks.Add(p);
            }*/
        }



        private void LoginItem_Click(object sender, RoutedEventArgs e)
        {
            LoginDialog logDialog = new LoginDialog();
            logDialog.ShowDialog();
            if (logDialog.Online)
            {
                BitmapImage bitmap = new BitmapImage(new Uri("Resources\\greendot.png",UriKind.Relative));
                OnlineImage.Source = bitmap;
            }
        }

        private void RegisterItem_Click(object sender, RoutedEventArgs e)
        {
            RegisterUserDialog userdialog = new RegisterUserDialog();
            userdialog.ShowDialog();
        }

        /// <summary>
        /// Method being run when the explorerview has been loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExplorerTree_Loaded(object sender, RoutedEventArgs e)
        {
            TreeViewModel.GetInstance().LoadFilesAndFolders(ExplorerTree.Items);
        }

        private void NewDocumentItem_Click(object sender, RoutedEventArgs e)
        {
            NewDocumentDialog docDia = new NewDocumentDialog();
            docDia.ShowDialog();

            String title = docDia.Title;

            controller.CreateDocument(title);
        }

        private void buttonSaveDocument_Click(object sender, RoutedEventArgs e)
        {
            controller.SaveDocument(richTextBox.Document);
        }

        private void saveDocumentButton_Click(object sender, RoutedEventArgs e)
        {
            //controller.SaveDocument("Title
        }
    }
}