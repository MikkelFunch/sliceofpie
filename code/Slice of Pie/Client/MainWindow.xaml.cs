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
            if (url != null && (url.StartsWith("http://") || url.StartsWith("https://")))
            {
                TextRange tr = new TextRange(
                richTextBox.Selection.Start,
                richTextBox.Selection.End);

                tr.Text = "";
                Run run = new Run("[" + url + "]");
                Hyperlink hlink = new Hyperlink(run, tr.Start);
                hlink.NavigateUri = new Uri(url);
            }


            /// Show an image rather than a text with the link
            /*
            string url = null;

            ImageURLDialog imDialog = new ImageURLDialog();
            imDialog.ShowDialog();

            //get inserted url
            url = imDialog.URLString;
>>>>>>> c201143b2ac7301d07aa9f99523ecc7a966da4b4
             

            //check if it is a valid url
            if (url != null && (url.StartsWith("http://") || url.StartsWith("https://")))
            {
                //Create bitmapimage refrenceing the online link to download it
                BitmapImage bitmap = new BitmapImage(new Uri(url, UriKind.RelativeOrAbsolute));
                
                //bitmap.DownloadCompleted += controller.GetDownloadCompleteEventHandler();
                bitmap.DownloadCompleted += new EventHandler(controller.DownloadComplete);
                /*
                //Create a new source refrenceing the image locally
                Uri newSource = new Uri(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\sliceofpie\\pics\\" + Security.EncryptPassword(url) + ".jpg", UriKind.RelativeOrAbsolute);
                
                
                //create bitmapimage refrencing the local source
                BitmapImage localImage = new BitmapImage(newSource);
                //Create the image which will be shown in gui
                Image image = new Image();
                //set the source of the image
                image.Source = localImage;
                image.Width = 130;
                image.Height = 130;
                image.Tag = newSource;
                
                //insert the image
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
                ChangeActiveButtons(true);

                menuItemLogin.Header = "Log out";
                menuItemLogin.Click -= LoginItem_Click;
                menuItemLogin.Click += new RoutedEventHandler(LogoutItem_Click);
            }
        }

        private void LogoutItem_Click(object sender, RoutedEventArgs e)
        {
            controller.Logout();
            BitmapImage bitmap = new BitmapImage(new Uri("Resources\\redndot.png", UriKind.Relative));
            OnlineImage.Source = bitmap;
            ChangeActiveButtons(false);

            menuItemLogin.Header = "Log in";
            menuItemLogin.Click -= LogoutItem_Click;
            menuItemLogin.Click += new RoutedEventHandler(LoginItem_Click);
            richTextBox.Document = new FlowDocument();
        }

        /// <summary>
        /// Change active buttons on login and logout
        /// </summary>
        /// <param name="loggedin">Wheter the user is logged in</param>
        private void ChangeActiveButtons(Boolean loggedin)
        {
            buttonDelete.IsEnabled = loggedin;
            buttonHistory.IsEnabled = loggedin;
            buttonNewFolder.IsEnabled = loggedin;
            buttonSync.IsEnabled = loggedin;
            buttonSyncAll.IsEnabled = loggedin;
            buttonImage.IsEnabled = loggedin;
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

            String title = docDia.DocumentTitle;
            if (title != null && title.Length > 0)
            {
                controller.CreateDocument(title);
            }
        }

        private void buttonSaveDocument_Click(object sender, RoutedEventArgs e)
        {
            controller.SaveDocument(richTextBox.Document);
        }

        private void buttonSync_Click(object sender, RoutedEventArgs e)
        {
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                controller.SyncDocument(richTextBox.Document);
            }
            else
            {
                //some error
            }
        }

        private void buttonSyncAll_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                controller.SyncAllDocuments();
            }
            else
            {
                //no network
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response">0:server document, 1:merged document, 2:string[] with 3 arrays?</param>
        public void SetupMergeView(String[][] response)
        {
            double centerGridWidth = (this.Width / 9) * 7;
            double documentViewWidth = centerGridWidth * 0.92;
            richTextBox.Width = (documentViewWidth) / 2;
            richTextBoxMerged.Width = (documentViewWidth) / 2;
            richTextBoxMerged.Visibility = Visibility.Visible;
            Grid.SetRow(richTextBox, 1);
            Grid.SetRow(richTextBoxMerged, 1);
            richTextBox.IsReadOnly = true;

            labelMerge.Visibility = Visibility.Visible;
            labelServer.Visibility = Visibility.Visible;

            richTextBox.Document = InsertIntoRichtextbox(response[3],response[2]) ;
            richTextBoxMerged.Document = InsertIntoRichtextbox(response[0], response[1]);

            buttonSaveDocument.Click -= buttonSaveDocument_Click;
            buttonSaveDocument.Click += new RoutedEventHandler(buttonSaveMergedDocument_Click);
            buttonSaveDocument.Content = "Save merged";
        }


        private FlowDocument InsertIntoRichtextbox(String[] content, String[] lineChanges)
        {
            FlowDocument doc = new FlowDocument();
            for(int i = 0; i < content.Length; i++)
            {
                Paragraph p = new Paragraph(new Run(content[i]));
                switch (lineChanges[i])
                {
                    case "i": 
                        p.Background = new SolidColorBrush(Colors.Green);
                        break;
                    case "d": 
                        p.Background = new SolidColorBrush(Colors.Red);
                        break;
                    default:
                        p.Background = new SolidColorBrush(Colors.White);
                        break;
                }
                doc.Blocks.Add(p);
            }
            doc.Blocks.Add(new Paragraph(new Run("")));
            return doc;
        }

        private void buttonSaveMergedDocument_Click(object sender, RoutedEventArgs e)
        {
            TextRange textRange = new TextRange(richTextBoxMerged.Document.ContentStart, richTextBoxMerged.Document.ContentEnd);
            String[] pureContent = textRange.Text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            FlowDocument mergedDocument = new FlowDocument();
            foreach (String s in pureContent)
            {
                Paragraph p = new Paragraph(new Run(s));
                mergedDocument.Blocks.Add(p);
            }

            controller.SaveMergedDocument(mergedDocument);
            richTextBox.Document = mergedDocument;

            richTextBox.Width = double.NaN;
            richTextBoxMerged.Visibility = Visibility.Hidden;
            Grid.SetRow(richTextBox, 0);
            Grid.SetRow(richTextBoxMerged, 0);
            richTextBox.IsReadOnly = false;

            labelMerge.Visibility = Visibility.Hidden;
            labelServer.Visibility = Visibility.Hidden;

            buttonSaveDocument.Click -= buttonSaveMergedDocument_Click;
            buttonSaveDocument.Click += new RoutedEventHandler(buttonSaveDocument_Click);
            buttonSaveDocument.Content = "Save document";
        }
    }
}