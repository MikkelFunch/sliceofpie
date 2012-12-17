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
using System.Text;
using Web_Solution.GUI;

namespace Web_Solution
{
    public partial class MainPage : UserControl
    {
        Controller controller;

        public MainPage()
        {
            InitializeComponent();
            controller = Controller.GetInstance();
            controller.SetGui(this);
            richTextBox.Visibility = Visibility.Collapsed;
        }

        private void buttonImage_Click(object sender, RoutedEventArgs e)
        {
            ImageURLDialog imgDia = new ImageURLDialog();
            imgDia.Closed += new EventHandler(imgDia_Closed);
            imgDia.Show();
        }

        private void imgDia_Closed(object sender, EventArgs args)
        {
            ImageURLDialog imgDia = (ImageURLDialog)sender;

            //get inserted url
            string url = imgDia.URLString;

            //check if it is a valid url
            if (url != null && (url.StartsWith("http://") || url.StartsWith("https://")))
            {
                string urlString = "[IMAGE:" + url + "]";
                richTextBox.Selection.Text += urlString;
            }
        }

        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginDialog logDialog = new LoginDialog();
            logDialog.Closed += new EventHandler(logDialog_Closed);
            logDialog.Show();
        }

        private void logDialog_Closed(object sender, EventArgs args)
        {
            LoginDialog logDialog = (LoginDialog)sender;
            bool? result = logDialog.DialogResult;
            if (result == true)
            {
                controller.LoginUser(logDialog.Email, logDialog.UnencryptedPass);
            }
        }

        private void LogoutItem_Click(object sender, RoutedEventArgs e)
        {
            controller.LogoutUser();
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
            //buttonSyncAll.IsEnabled = loggedin;
            buttonImage.IsEnabled = loggedin;
            if (loggedin) textBlockOnline.Text = "Online";
            else textBlockOnline.Text = "Offline";
        }

        private void RegisterItem_Click(object sender, RoutedEventArgs e)
        {
            RegisterUserDialog userDialog = new RegisterUserDialog();
            userDialog.Closed += new EventHandler(userDialog_Closed);
            userDialog.Show();
        }

        private void userDialog_Closed(object sender, EventArgs args)
        {
            RegisterUserDialog userDialog = (RegisterUserDialog)sender;
            controller.RegisterUser(userDialog.Email, userDialog.PassUnencrypted1, userDialog.PassUnencrypted2);
        }

        private void buttonSync_Click(object sender, RoutedEventArgs e)
        {
            richTextBox.SelectAll();
            string a = richTextBox.Selection.Text;
            string b = richTextBoxMerged.Selection.Text;
            controller.SyncDocument(richTextBox.Selection.Text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response">0:new document, 1:insertions, 2:deletions, 3:old server document</param>
        public void SetupMergeView(String[][] response)
        {
            double centerGridWidth = (this.Width / 9) * 7;
            double documentViewWidth = centerGridWidth * 0.92;
            richTextBox.Width = (documentViewWidth) / 2;
            richTextBoxMerged.Width = (documentViewWidth) / 2;
            richTextBoxMerged.Visibility = Visibility.Visible;
            richTextBox.HorizontalAlignment = HorizontalAlignment.Left;
            Grid.SetRow(richTextBox, 1);
            Grid.SetRow(richTextBoxMerged, 1);
            richTextBox.IsReadOnly = true;

            labelMerge.Visibility = Visibility.Visible;
            labelServer.Visibility = Visibility.Visible;

            richTextBox.Selection.Text = InsertIntoRichtextbox(response[3], response[2]);
            richTextBoxMerged.Selection.Text = InsertIntoRichtextbox(response[0], response[1]);

            buttonSync.Click -= buttonSync_Click;
            buttonSync.Click += new RoutedEventHandler(buttonSaveMergedDocument_Click);
            buttonSync.Content = "Save merged";
        }


        private string InsertIntoRichtextbox(String[] content, String[] lineChanges)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < content.Length; i++)
            {
                sb.AppendLine(content[i]);
            }
            return sb.ToString();
        }

        private void buttonSaveMergedDocument_Click(object sender, RoutedEventArgs e)
        {
            //Get textselection from richtextbox
            richTextBoxMerged.SelectAll();
            TextSelection textSelection = richTextBoxMerged.Selection;
            //retrieve the xaml
            String pureContent = textSelection.Text;
            controller.SaveMergedDocument(pureContent);
            richTextBox.Selection.Text = pureContent;

            richTextBox.Width = double.NaN;
            richTextBoxMerged.Visibility = Visibility.Collapsed;
            Grid.SetRow(richTextBox, 0);
            Grid.SetRow(richTextBoxMerged, 0);
            richTextBox.IsReadOnly = false;
            richTextBox.HorizontalAlignment = HorizontalAlignment.Stretch;

            labelMerge.Visibility = Visibility.Collapsed;
            labelServer.Visibility = Visibility.Collapsed;

            buttonSync.Click -= buttonSaveMergedDocument_Click;
            buttonSync.Click += new RoutedEventHandler(buttonSync_Click);
            buttonSync.Content = "Sync";
        }

        private void buttonNewFolder_Click(object sender, RoutedEventArgs e)
        {
            NewFolderDialog newfDia = new NewFolderDialog();
            newfDia.Closed += new EventHandler(newfDia_Closed);
            newfDia.SelectedItem = (TreeViewItem)ExplorerTree.SelectedItem;
            newfDia.Show();
        }

        private void newfDia_Closed(object sender, EventArgs args)
        {
            NewFolderDialog newfDia = (NewFolderDialog)sender;
            int parentFolderId = -1;
            if (newfDia.SelectedItem != null) //an item is  chosen
            {
                TreeViewItem item = (TreeViewItem)newfDia.SelectedItem;

                //it is a folder, and not a file
                if ((bool)((object[])item.Tag)[2] == true)
                {
                    parentFolderId = int.Parse(((object[])item.Tag)[0].ToString());
                }
            }

            controller.CreateFolder(newfDia.FolderTitle, parentFolderId);
        }


        private void buttonShareDocument_Click(object sender, RoutedEventArgs e)
        {
            ShareDocumentDialog shareDia = new ShareDocumentDialog();
            shareDia.Closed += new EventHandler(shareDia_Closed);
            shareDia.Show();
        }

        private void shareDia_Closed(object sender, EventArgs args)
        {
            ShareDocumentDialog shareDia = (ShareDocumentDialog)sender;
            controller.ShareDocument(shareDia.Email);
        }

        private void buttonNewDocument_Click(object sender, RoutedEventArgs e)
        {
            NewDocumentDialog docDia = new NewDocumentDialog();
            docDia.Closed += new EventHandler(docDia_Closed);
            docDia.Show();
        }

        private void docDia_Closed(object sender, EventArgs args)
        {
            NewDocumentDialog docDia = (NewDocumentDialog)sender;
            controller.CreateNewDocumentFile(docDia.DocumentTitle);
        }

        private void buttonMoveDocument_Click(object sender, RoutedEventArgs e)
        {
            if (ExplorerTree.SelectedItem != null && ((bool)((object[])((TreeViewItem)ExplorerTree.SelectedItem).Tag)[2]) == false)
            {
                MoveFileDialog movDia = new MoveFileDialog();
                movDia.SelectedItem = (TreeViewItem)ExplorerTree.SelectedItem;
                movDia.Source = ExplorerTree.Items;
                movDia.Closed += new EventHandler(movDia_Closed);
                movDia.Show();
            }
        }

        private void movDia_Closed(object sender, EventArgs args)
        {
            MoveFileDialog movDia = (MoveFileDialog)sender;
            controller.MoveFileToFolder(movDia.FromId, movDia.ToId, int.Parse(((object[])movDia.SelectedItem.Tag)[0].ToString()),movDia.SelectedItem);
        }

        private void buttonHistory_Click(object sender, RoutedEventArgs e)
        {
            RevisionHistoryDialog revDia = new RevisionHistoryDialog();
            controller.PopulateHistory(revDia.Items);
            revDia.Show();


            /*RevisionHistoryDialog revDia = new RevisionHistoryDialog();

            revDia.DocumentName = Session.GetInstance().CurrentDocumentTitle;
            revDia.EditorName = "";
            revDia.Revisions = controller.GetAllDocumentRevisionsWithContent(Session.GetInstance().CurrentDocumentID);
            revDia.CreateTreeView();
            revDia.Show();*/
        }

        public void SetLoginView(bool active)
        {
            if(active)
            {
                richTextBox.Visibility = Visibility.Visible;
            }
            else
            {
                richTextBox.Visibility = Visibility.Collapsed;
            }

            buttonDelete.IsEnabled = active;
            buttonHistory.IsEnabled = active;
            buttonImage.IsEnabled = active;
            buttonMoveDocument.IsEnabled = active;
            buttonNewDocument.IsEnabled = active;
            buttonNewFolder.IsEnabled = active;
            buttonShareDocument.IsEnabled = active;
            buttonSync.IsEnabled = active;
            //buttonSyncAll.IsEnabled = active;
        }
    }
}
