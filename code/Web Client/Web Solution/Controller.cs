﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Text.RegularExpressions;
using System.Windows;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace Web_Solution
{
    public class Controller
    {
        #region Fields

        //Singleton instance of controller
        private static Controller instance;
        //reference to gui
        private MainPage gui;
        //reference to session
        private Session session;

        #endregion

        #region Singleton

        /// <summary>
        /// Private constructor to insure that Controller is not created outside this class.
        /// </summary>
        private Controller()
        {
            session = Session.GetInstance();
            session.UserID = -1;
        }

        /// <summary>
        /// Accessor method for accessing the single instance of controller.
        /// </summary>
        /// <returns>The only instance of controller</returns>
        public static Controller GetInstance()
        {
            if (instance == null)
            {
                instance = new Controller();
            }
            return instance;
        }

        #endregion

        #region GUI

        /// <summary>
        /// Method used to get the main gui window
        /// </summary>
        /// <param name="gui">Refrence of the main gui window</param>
        public void SetGui(MainPage gui)
        {
            this.gui = gui;
        }

        #endregion

        #region UserActions

        #region RegisterUser
        /// <summary>
        /// Method to register a new user in the database
        /// Using an email and 2 identical passwords, in order protect against typoes
        /// </summary>
        /// <param name="email">The users email</param>
        /// <param name="passUnencrypted1">unencrypted first password</param>
        /// <param name="passUnencrypted2">unencrypted second password</param>
        /// <returns>Wheter the creation of the user was successful - return -1 if it was unsuccesful</returns>
        public void RegisterUser(string email, string passUnencrypted1, string passUnencrypted2)
        {
            //Check if something have been entered as email - WE DO NOT CHECK THAT IT IS AN EMAIL
            if (email != null && email.Length > 0)
            {
                //Check that something has been entered as been entered as passwords and check that the two passwords are identical
                if (passUnencrypted1 != null && passUnencrypted1.Length > 0 && passUnencrypted2 != null && passUnencrypted1 == passUnencrypted2)
                {
                    //encrypt password
                    string pass = Security.EncryptString(passUnencrypted1);
                    //connect to webservice
                    ServiceReference.Service1Client proxy = new ServiceReference.Service1Client();
                    proxy.AddUserAsync(email, pass);
                    proxy.AddUserCompleted += new EventHandler<ServiceReference.AddUserCompletedEventArgs>(proxy_AddUserCompleted);
                    session.Email = email;
                }
                else //the passwords does not match
                {
                    System.Windows.MessageBox.Show("User could not be created. Entered passwords does not match");
                }
            }
            else //no email was entered
            {
                System.Windows.MessageBox.Show("Enter email address");
            }
        }

        /// <summary>
        /// Sets the session data and displays the correct dialog for the register operation.
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="args"></param>
        private void proxy_AddUserCompleted(Object sender, ServiceReference.AddUserCompletedEventArgs args)
        {
            //-1 == user exists
            int successful = args.Result;
            if (successful == -1) //if the user already exsist
            {
                System.Windows.MessageBox.Show("User aldready exsists");
                session.Email = "";
            }
            else //the user has been created
            {
                System.Windows.MessageBox.Show("User with email: " + session.Email + " have been successfully created");
                session.UserID = successful;
            }
        }

        #endregion

        #region login

        /// <summary>
        /// Logs a user in with an email and an unencrypted password.
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="unencrytedPass">The password of the user</param>
        public void LoginUser(string email, string unencrytedPass)
        {
            if (email != null && email.Length > 0 && unencrytedPass != null && unencrytedPass.Length > 0)
            {
                //encrypt passowrd
                String pass = Security.EncryptString(unencrytedPass);
                //connect to webservice
                ServiceReference.Service1Client proxy = new ServiceReference.Service1Client();
                //get the user id - -1 if no user exists
                proxy.GetUserByEmailAndPassAsync(email, pass);
                proxy.GetUserByEmailAndPassCompleted += new EventHandler<ServiceReference.GetUserByEmailAndPassCompletedEventArgs>(proxy_GetUserByEmailAndPassCompleted);
                session.Email = email;
            }
            else
            {
                MessageBox.Show("Enter email and password");
            }
        }

        /// <summary>
        /// Sets gui view and session data. Calls additional asynchronous calls to set update view and data.
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="args">The user credentials</param>
        private void proxy_GetUserByEmailAndPassCompleted(Object sender, ServiceReference.GetUserByEmailAndPassCompletedEventArgs args)
        {
            ServiceReference.ServiceUser user = args.Result;
            if (user != null) //login successful
            {
                //User logged in
                session.UserID = user.id;
                session.RootFolderID = user.rootFolderId;

                gui.SetLoginView(true);

                ServiceReference.Service1Client proxy = new ServiceReference.Service1Client();
                proxy.GetAllFilesAndFoldersByUserIdAsync(user.id);
                proxy.GetAllFilesAndFoldersByUserIdCompleted += new EventHandler<ServiceReference.GetAllFilesAndFoldersByUserIdCompletedEventArgs>(proxy_GetAllFilesAndFoldersByUserIdCompleted);
            }
            else
            {
                session.Email = "";
                gui.textBlockOnline.Text = "Offline";
                MessageBox.Show("Wrong email or password");
            }
        }

        /// <summary>
        /// Gets all the files and folders from a user, which are used to correctly update the view and data.
        /// </summary>
        /// <param name="sender">The sender ofthe event</param>
        /// <param name="args">The user data</param>
        private void proxy_GetAllFilesAndFoldersByUserIdCompleted(Object sender, ServiceReference.GetAllFilesAndFoldersByUserIdCompletedEventArgs args)
        {
            System.Collections.ObjectModel.ObservableCollection<System.Collections.ObjectModel.ObservableCollection<System.Collections.ObjectModel.ObservableCollection<string>>> foldersAndFiles = args.Result;
            string[][][] foldersAndFilesArrays = new string[2][][];

            for (int i = 0; i < 2; i++)
            {
                System.Collections.ObjectModel.ObservableCollection<string>[] temp = foldersAndFiles[i].ToArray();
                foldersAndFilesArrays[i] = new string[temp.Length][];
                for (int j = 0; j < temp.Length; j++)
                {
                    string[] tempfoldersorfiles = temp[j].ToArray();
                    foldersAndFilesArrays[i][j] = tempfoldersorfiles;
                }
            }

            TreeViewModel.GetInstance().LoadFilesAndFolders(gui.ExplorerTree.Items, foldersAndFilesArrays);
            gui.textBlockOnline.Text = "Online";
        }

        #endregion

        #region Logout

        /// <summary>
        /// Logs out the user, which is currently logged in.
        /// </summary>
        public void LogoutUser()
        {
            session.UserID = -1;
            session.CurrentDocumentID = -1;
            session.CurrentDocumentTitle = "";
            session.Email = "";

            //SetOpenDocument("", "", "");
            gui.SetLoginView(false);
            gui.ExplorerTree.Items.Clear();
        }

        #endregion

        /// <summary>
        /// Shares a document with another user.
        /// </summary>
        /// <param name="email">The email of the user</param>
        public void ShareDocument(string email)
        {
            if (email != null && email.Length > 0 && session.CurrentDocumentID != -1 && email != session.Email)
            {
                ServiceReference.Service1Client proxy = new ServiceReference.Service1Client();
                proxy.GetUserByEmailAsync(email);
                proxy.GetUserByEmailCompleted += new EventHandler<ServiceReference.GetUserByEmailCompletedEventArgs>(proxy_GetUserByEmailCompleted);
            }
        }

        /// <summary>
        /// Creates a userdocument for the invited user.
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="args">The email of the user</param>
        private void proxy_GetUserByEmailCompleted(Object sender, ServiceReference.GetUserByEmailCompletedEventArgs args)
        {
            ServiceReference.ServiceUser shareUser = args.Result;

            if (shareUser != null)
            {
                ServiceReference.Service1Client proxy = new ServiceReference.Service1Client();
                proxy.AddUserDocumentInRootAsync(shareUser.id, session.CurrentDocumentID);
                proxy.ShareDocumentWebAsync(session.CurrentDocumentID, session.UserID, shareUser.id);
                MessageBox.Show("Document shared with " + shareUser.email);
            }
            else
            {
                MessageBox.Show("User does not exist");
            }
        }

        #endregion

        #region LocalPersistence

        /// <summary>
        /// Calls the asynchrounous calls to create a document. The method sets session data and creates metadata.
        /// </summary>
        /// <param name="title">The title of the document</param>
        public void CreateNewDocumentFile(String title)
        {
            if (title != null && title.Length > 0)
            {
                string emptyDocumentXaml = "<FlowDocument xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" />";
                string metadata = Metadata.GenerateMetadataStringForNewFile();
                string fileContent = metadata + emptyDocumentXaml;

                session.CurrentDocumentTitle = title;

                ServiceReference.Service1Client proxy = new ServiceReference.Service1Client();
                proxy.AddDocumentWithUserDocumentAsync(title, session.UserID, session.Email + "\\", fileContent);
                proxy.AddDocumentWithUserDocumentCompleted += new EventHandler<ServiceReference.AddDocumentWithUserDocumentCompletedEventArgs>(proxy_AddDocumentWithUserDocumentCompleted);
            }
        }

        /// <summary>
        /// Sets session data, updates tree view and updates the open document view.
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="args">The id of the document</param>
        private void proxy_AddDocumentWithUserDocumentCompleted(object sender, ServiceReference.AddDocumentWithUserDocumentCompletedEventArgs args)
        {
            int documentId = args.Result;
            session.CurrentDocumentID = documentId;

            string[] documentData = new string[] { documentId.ToString(), session.RootFolderID.ToString(), session.CurrentDocumentTitle };

            TreeViewModel.GetInstance().InsertDocument(documentData, gui.ExplorerTree.Items);
            SetOpenDocument(documentId, session.CurrentDocumentTitle, session.RootFolderID);
        }

        /// <summary>
        /// Creates a folder by calling asynchrounous calls and setting session data.
        /// </summary>
        /// <param name="folderName">The name of the folder</param>
        /// <param name="parentFolderId">The id of the containing folder</param>
        public void CreateFolder(String folderName, int parentFolderId)
        {
            if (parentFolderId == -1) parentFolderId = session.RootFolderID;

            ServiceReference.Service1Client proxy = new ServiceReference.Service1Client();
            proxy.AddFolderAsync(folderName, parentFolderId);
            proxy.AddFolderCompleted += new EventHandler<ServiceReference.AddFolderCompletedEventArgs>(proxy_AddFolderCompleted);
            session.NewlyCreatedFolderName = folderName;
            session.NewlyCreatedFolderParentId = parentFolderId;
        }

        /// <summary>
        /// Updates the tree model to reflect the newly created document.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void proxy_AddFolderCompleted(object sender, ServiceReference.AddFolderCompletedEventArgs args)
        {
            TreeViewModel.GetInstance().InsertFolder(new string[] { args.Result.ToString(), session.NewlyCreatedFolderName, session.NewlyCreatedFolderParentId.ToString() }, gui.ExplorerTree.Items);
        }

        #endregion

        #region ControllerMethods

        /// <summary>
        /// Set the currently opened document
        /// </summary>
        /// <param name="fileXamlContent"></param>
        /// <param name="title"></param>
        /// <param name="documentPath"></param>
        public void SetOpenDocument(String fileContent, String title, String documentPath) //lav en overloaded metode som tager et flowdocument, title og documentpath - eller bare title og documentpath - eller bare documentpath
        {
            gui.richTextBox.Selection.Text = fileContent;
            gui.labelOpenDocument.Content = "Current document: " + title;
            session.CurrentDocumentTitle = title;
            session.CurrentDocumentPath = documentPath;
            if (title.Length > 0)
            {
                session.CurrentDocumentID = Metadata.FetchDocumentIDFromFileContent(fileContent);
                gui.labelOpenDocument.Content = "Open document: " + title;
                gui.richTextBox.Visibility = Visibility.Visible;
            }
            else
            {
                session.CurrentDocumentID = 0;
                gui.richTextBox.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Sets the session data and the current document view in the UI.
        /// </summary>
        /// <param name="documentId">The id of the current document</param>
        /// <param name="documentTitle">The title of the current document</param>
        /// <param name="folderId">The folder that contains the current document</param>
        public void SetOpenDocument(int documentId, string documentTitle, int folderId)
        {
            ServiceReference.Service1Client proxy = new ServiceReference.Service1Client();
            proxy.GetLatestPureDocumentContentAsync(documentId);
            proxy.GetLatestPureDocumentContentCompleted += new EventHandler<ServiceReference.GetLatestPureDocumentContentCompletedEventArgs>(proxy_GetLatestPureDocumentContentCompleted);
            session.CurrentDocumentID = documentId;
            session.CurrentDocumentTitle = documentTitle;
            session.FolderID = folderId;
            gui.labelOpenDocument.Content = "Current document: " + documentTitle;
        }

        /// <summary>
        /// Receives the latest textual content from a document.
        /// </summary>
        /// <param name="sender">The sender of the content</param>
        /// <param name="args">The textual content of the document</param>
        public void proxy_GetLatestPureDocumentContentCompleted(Object sender, ServiceReference.GetLatestPureDocumentContentCompletedEventArgs args)
        {
            string pureContent = args.Result;
            gui.richTextBox.SelectAll();
            gui.richTextBox.Selection.Text = pureContent;
        }

        #endregion

        #region SyncMethods

        #region Sync all

        /*public void SyncAllDocuments()
        {
            if (session.UserID != -1)
            {
                ServiceReference.Service1Client proxy = new ServiceReference.Service1Client();
                proxy.GetAllUserDocumentsByUserIdAsync(session.UserID);
                proxy.GetAllUserDocumentsByUserIdCompleted += new EventHandler<ServiceReference.GetAllUserDocumentsByUserIdCompletedEventArgs>(proxy_GetAllUserDocumentsByUserIdCompleted);
                
            }
        }

        private void proxy_GetAllUserDocumentsByUserIdCompleted(Object sender, ServiceReference.GetAllUserDocumentsByUserIdCompletedEventArgs args)
        {
            Web_Solution.ServiceReference.ServiceUserdocument[] documents = args.Result.ToArray();
            if (documents.Length > 0)
            {
                foreach (ServiceReference.ServiceUserdocument currentDoc in documents)
                {
                    ServiceReference.Service1Client proxy = new ServiceReference.Service1Client();
                    proxy.GetDocumentByIdAsync(currentDoc.documentId);
                    proxy.GetDocumentByIdCompleted += new EventHandler<ServiceReference.GetDocumentByIdCompletedEventArgs>(proxy_GetDocumentByIdCompleted);
                }
            }
            else //No documents found by this userId
            { //loop through all the users folders and add all files to the server
                foreach (TreeViewItem item in gui.Items)
                {
                    if (item.Items.Count == 0)//fil
                    {
                        AddNewDocumentToServer(item.Header, ((object[])item.Tag)[0], gui.richTextBox.Xaml, folderpath/id);
                    }
                }
            }
        }

        

        private void proxy_GetDocumentByIdCompleted(Object sender, ServiceReference.GetDocumentByIdCompletedEventArgs args)
        {
            Web_Solution.ServiceReference.ServiceDocument document = args.Result;
            TreeViewModel.GetInstance().AddItem(document.name, document.id, gui.Items, false);
            
            ServiceReference.Service1Client proxy = new ServiceReference.Service1Client();
            proxy.AddDocumentRevisionAsync(session.UserID, document.id, content); //lav en metode i serveren som selv finder content for den seneste revision der er det dokument id og indsætter det, ellers er vi nødt til at lave flere proxy kald og flere event handlers
            UpdateExplorerView();
        }

        /// <summary>
        /// Add file to server
        /// </summary>
        /// <param name="filePath">Path to the file which should be added to the server</param>
        private void AddNewDocumentToServer(string documentTitle, int documentId, string textBoxXaml, int folderId)
        {
            string metadataString = Metadata.GenerateMetadataStringForNewFile();
            string fileContent = metadataString + textBoxXaml;
            ServiceReference.Service1Client proxy = new ServiceReference.Service1Client();
            proxy.AddDocumentWithUserDocument(documentTitle, session.UserID, filepath, fileContent);
        }*/

        #endregion

        /// <summary>
        /// Syncs a document and calls the asynchronous calls, resolving the merge.
        /// </summary>
        /// <param name="pureContent">The content of the document being synchronized</param>
        public void SyncDocument(string pureContent)
        {
            int documentId = session.CurrentDocumentID;
            DateTime baseDocumentCreationTime = session.CurrentDocumentTimeStampMetadata;

            string metadata = Metadata.GenerateMetadataString(documentId, session.UserID, DateTime.UtcNow);
            string filePath = TreeViewModel.GetInstance().GetRelativePath(session.FolderID, gui.ExplorerTree.Items);

            ServiceReference.Service1Client proxy = new ServiceReference.Service1Client();
            proxy.SyncDocumentWebAsync(session.UserID, documentId, filePath, metadata, session.CurrentDocumentTitle, pureContent);
            proxy.SyncDocumentWebCompleted += new EventHandler<ServiceReference.SyncDocumentWebCompletedEventArgs>(proxy_SyncDocumentWebCompleted);
        }

        /// <summary>
        /// Receives a merge result and interpret whether it is a conflict. If a conflict exists the merge view is setup.
        /// </summary>
        /// <param name="sender">The merge result</param>
        /// <param name="args">Event arguments. Not used</param>
        private void proxy_SyncDocumentWebCompleted(Object sender, ServiceReference.SyncDocumentWebCompletedEventArgs args)
        {
            if (args.Result != null) //if there is a conflict
            {
                string[][] responseArrays = new string[4][];
                for (int i = 0; i < args.Result.Count; i++)
                {
                    responseArrays[i] = args.Result[i].ToArray();
                }

                gui.SetupMergeView(responseArrays);
            }
        }

        /// <summary>
        /// Saves the content specified in the merge view to the server.
        /// </summary>
        /// <param name="pureContent">Pure content of the text in the document</param>
        public void SaveMergedDocument(string pureContent)
        {
            int documentid = session.CurrentDocumentID;
            int userid = session.UserID;
            DateTime timestamp = DateTime.UtcNow;

            String metadata = Metadata.GenerateMetadataString(documentid, userid, timestamp);

            ServiceReference.Service1Client proxy = new ServiceReference.Service1Client();
            proxy.AddDocumentRevisionWebAsync(documentid, session.UserID, pureContent, metadata);
            proxy.CloseAsync();
        }

        #endregion

        public void MoveFileToFolder(int fromId, int toId, int documentId)
        {
            if (toId == -1) toId = session.RootFolderID;

            ServiceReference.Service1Client proxy = new ServiceReference.Service1Client();
            proxy.MoveDocumentWebAsync(session.UserID, documentId, toId);

            TreeViewModel.GetInstance().RemoveDocument(documentId, gui.ExplorerTree.Items);
        }

        /*public void SetContentFromDocumentId(int id, string title)
        {
            ServiceReference.Service1Client proxy = new ServiceReference.Service1Client();
            proxy.GetLatestDocumentContentAsync(id);
            proxy.GetLatestDocumentContentCompleted +=new EventHandler<ServiceReference.GetLatestDocumentContentCompletedEventArgs>(proxy_GetLatestDocumentContentCompleted);
            session.CurrentDocumentTitle = title;
        }

        private void proxy_GetLatestDocumentContentCompleted(Object sender, ServiceReference.GetLatestDocumentContentCompletedEventArgs args)
        {
            string resultContent = args.Result;
            if(resultContent != null)
            {
                SetOpenDocument(resultContent,session.CurrentDocumentTitle,?);
            }
        }*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns>
        /// [x]a new revision
        /// [x][0]timestamp
        /// [x][1]editor name
        /// [x][2]filecontent with metadata
        /// </returns>
        /*public string[][] GetAllDocumentRevisionsWithContent(int documentId)
        {
            string[][] returnArray = null;
            if (documentId > 0)
            {
                ServiceReference.ServiceDocumentrevision[] revisions = null;
                using (ServiceReference.Service1Client proxy = new ServiceReference.Service1Client())
                {
                    revisions = proxy.GetAllDocumentRevisionsByDocumentId(documentId);
                }

                //remove duplicates
                //here

                returnArray = new string[revisions.Length][];
                
                using(ServiceReference.Service1Client proxy = new ServiceReference.Service1Client())
                {
                    for(int i = 0; i < revisions.Length; i++)
                    {
                        ServiceReference.ServiceDocumentrevision doc = revisions[i];
                        ServiceReference.ServiceDocument originalDocument = proxy.GetDocumentById(doc.documentId);

                        String creationTime = doc.creationTime.ToString().Replace(":", ".");
                        String filename = originalDocument.name + "_revision_" + creationTime;
                        String content = proxy.GetDocumentContent(originalDocument.path, filename);

                        string[] item = new string[3];
                        item[0] = doc.creationTime.ToString();
                        item[1] = proxy.GetUserById(doc.editorId).email;
                        item[2] = Metadata.RemoveMetadataFromFileContent(content);
                        
                        returnArray[i] = item;
	                }
                }
            }

            return returnArray;
        }*/
    }
}