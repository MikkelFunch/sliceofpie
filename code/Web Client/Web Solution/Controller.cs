using System;
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
                MessageBox.Show("Wrong email or password");
            }
        }

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
            UpdateExplorerView();

            System.Windows.MessageBox.Show("Logged in successfully");
        }

        #endregion

        #region Logout

        public void LogoutUser()
        {
            session.UserID = -1;
            session.CurrentDocumentID = -1;
            session.CurrentDocumentTitle = "";
            session.Email = "";

            SetOpenDocument("", "", "");
            gui.SetLoginView(false);
            gui.Items.Clear();
            UpdateExplorerView();
        }

        #endregion

        public void ShareDocument(string email)//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!HANDLE DOCUMENT NOT BEING SYNCED FIRST(NOT IN THE DATABASE)
        {
            if (email != null && email.Length > 0 && session.CurrentDocumentPath.Length > 0 && session.CurrentDocumentID != -1)
            {
                ServiceReference.Service1Client proxy = new ServiceReference.Service1Client();
                proxy.GetUserByEmailAsync(email);
                proxy.GetUserByEmailCompleted += new EventHandler<ServiceReference.GetUserByEmailCompletedEventArgs>(proxy_GetUserByEmailCompleted);
            }
        }

        private void proxy_GetUserByEmailCompleted(Object sender, ServiceReference.GetUserByEmailCompletedEventArgs args)
        {
            ServiceReference.ServiceUser shareUser = args.Result;

            if (shareUser != null)
            {
                ServiceReference.Service1Client proxy = new ServiceReference.Service1Client();
                proxy.AddUserDocumentInRootAsync(shareUser.id, session.CurrentDocumentID);
                MessageBox.Show("Document shared with " + shareUser.email);
            }
            else
            {
                MessageBox.Show("User does not exist");
            }
        }

        #endregion

        #region LocalPersistence

        public void CreateNewDocumentFile(String title)
        {
            if (title != null && title.Length > 0)
            {
                string emptyDocumentXaml = "<FlowDocument xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" />";
                string metadata = Metadata.GenerateMetadataStringForNewFile();
                string fileContent = metadata + emptyDocumentXaml;

                session.CurrentDocumentTitle = title;

                ServiceReference.Service1Client proxy = new ServiceReference.Service1Client();
                proxy.AddDocumentWithUserDocumentAsync(title, session.UserID, session.RootFolderPath, fileContent);
                proxy.AddDocumentWithUserDocumentCompleted += new EventHandler<ServiceReference.AddDocumentWithUserDocumentCompletedEventArgs>(proxy_AddDocumentWithUserDocumentCompleted);
            }
        }

        private void proxy_AddDocumentWithUserDocumentCompleted(object sender, ServiceReference.AddDocumentWithUserDocumentCompletedEventArgs args)
        {
            int documentId = args.Result;
            session.CurrentDocumentID = documentId;

            string[] documentData = new string[]{documentId.ToString(),session.RootFolderID.ToString(),session.CurrentDocumentTitle};

            TreeViewModel.GetInstance().InsertDocument(documentData, gui.ExplorerTree.Items);
            gui.richTextBox.Selection.Text = "";
            UpdateExplorerView();
        }

        public void CreateFolder(String folderName, int parentFolderId)
        {
            if(parentFolderId == -1) parentFolderId = session.RootFolderID;

            ServiceReference.Service1Client proxy = new ServiceReference.Service1Client();
            proxy.AddFolderAsync(folderName,parentFolderId);
            proxy.AddFolderCompleted +=new EventHandler<ServiceReference.AddFolderCompletedEventArgs>(proxy_AddFolderCompleted);
        }

        private void proxy_AddFolderCompleted(object sender, ServiceReference.AddFolderCompletedEventArgs args)
        {
            TreeViewModel.GetInstance().InsertFolder(new string[] { args.Result.ToString(), session.NewlyCreatedFolderName, session.NewlyCreatedFolderParentId.ToString() }, gui.ExplorerTree.Items);
            UpdateExplorerView();
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
                gui.richTextBox.Visibility = Visibility.Visible;
            }
            else
            {
                session.CurrentDocumentID = 0;
                gui.richTextBox.Visibility = Visibility.Collapsed;
            }
        }

        public void SetOpenDocument(int documentId, string documentTitle)
        {
            ServiceReference.Service1Client proxy = new ServiceReference.Service1Client();
            proxy.GetLatestDocumentContentAsync(documentId);
            proxy.GetLatestDocumentContentCompleted += new EventHandler<ServiceReference.GetLatestDocumentContentCompletedEventArgs>(proxy_GetLatestDocumentContentCompleted);
            session.CurrentDocumentID = documentId;
            session.CurrentDocumentTitle = documentTitle;
        }

        public void proxy_GetLatestDocumentContentCompleted(Object sender, ServiceReference.GetLatestDocumentContentCompletedEventArgs args)
        {
            string fileContent = args.Result;
            string xaml = Metadata.RemoveMetadataFromFileContent(fileContent);
            gui.richTextBox.Xaml = xaml;
        }

        /// <summary>
        /// Update the explorerview
        /// </summary>
        public void UpdateExplorerView()
        {
            /*gui.ExplorerTree.Items.Clear();
            foreach (TreeViewItem item in gui.Items)
            {
                gui.ExplorerTree.Items.Add(item);
            }*/
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

        public void SyncDocument(string textBoxXaml, string pureContent)
        {
            int documentId = session.CurrentDocumentID;
            DateTime baseDocumentCreationTime = session.CurrentDocumentTimeStampMetadata;

            string metadata = Metadata.GenerateMetadataString(documentId, session.UserID, DateTime.UtcNow);
            string fileContent = metadata + textBoxXaml;

            ServiceReference.Service1Client proxy = new ServiceReference.Service1Client();
            proxy.SyncDocumentAsync(session.UserID, documentId, "", fileContent, session.CurrentDocumentTitle, pureContent);
            proxy.SyncDocumentCompleted += new EventHandler<ServiceReference.SyncDocumentCompletedEventArgs>(proxy_SyncDocumentCompleted);
        }

        private void proxy_SyncDocumentCompleted(Object sender, ServiceReference.SyncDocumentCompletedEventArgs args)
        {
            string[][] responseArrays = new string[4][];
            for (int i = 0; i < args.Result.Count; i++)
            {
                responseArrays[i] = args.Result[i].ToArray();
            }

            if (responseArrays == null) //if there is no conflict
            {
                if (session.CurrentDocumentID == 0)
                {
                    //connect to the websevice
                    ServiceReference.Service1Client proxy = new ServiceReference.Service1Client();
                    proxy.GetDocumentIdAsync(session.UserID, session.CurrentDocumentTitle);
                    proxy.GetDocumentIdCompleted +=new EventHandler<ServiceReference.GetDocumentIdCompletedEventArgs>(proxy_GetDocumentIdCompleted);
                    proxy.GetDocumentIdCompleted += new EventHandler<ServiceReference.GetDocumentIdCompletedEventArgs>(proxy_GetDocumentIdCompleted);
                }
            }
            else //if there is a conflict
            {
                gui.SetupMergeView(responseArrays);
            }
        }

        private void proxy_GetDocumentIdCompleted(Object sender, ServiceReference.GetDocumentIdCompletedEventArgs args)
        {
            session.CurrentDocumentID = args.Result;
        }

        public void SaveMergedDocument(string fileXamlContent)
        {
            int documentid = session.CurrentDocumentID;
            int userid = session.UserID;
            DateTime timestamp = DateTime.UtcNow;

            String metadata = Metadata.GenerateMetadataString(documentid, userid, timestamp);

            String content = metadata + fileXamlContent;

            ServiceReference.Service1Client proxy = new ServiceReference.Service1Client();
            proxy.AddDocumentRevisionAsync(session.UserID, documentid, content);
            proxy.CloseAsync();
        }

        #endregion

        public void MoveFileToFolder(string fromPath, string toPath)
        {
            if (fromPath != null && fromPath.Length > 0 && toPath != null && toPath.Length > 0)
            {
                //make move method in treeview model
                //In case you move the currently opened document
                if (fromPath == session.CurrentDocumentPath)
                {
                    session.CurrentDocumentPath = toPath;
                }

                UpdateExplorerView();
            }
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