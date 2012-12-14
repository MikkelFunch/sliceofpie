using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Text.RegularExpressions;
using System.Windows;
using System.IO;

namespace Client
{
    public class Controller
    {
        #region Fields

        //Singleton instance of controller
        private static Controller instance;
        //refrence to model
        private Model model;
        //refrence to gui
        private MainWindow gui;
        //refrence to session
        private Session session;
        //refrence to localpersistence handler
        private LocalPersistenceHandler localPersistence;
        //refrence to webpersistence handler
        private WebPersistenceHandler webPersistenceHandler;

        #endregion

        #region Singleton

        /// <summary>
        /// Private constructor to insure that Controller is not created outside this class.
        /// </summary>
        private Controller()
        {
            model = Model.GetInstance();
            session = Session.GetInstance();
            localPersistence = LocalPersistenceHandler.GetInstance();
            webPersistenceHandler = WebPersistenceHandler.GetInstance();

            //Default when  not logged on
            session.RootFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\sliceofpie\\";
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
        public void SetGui(MainWindow gui)
        {
            this.gui = gui;
        }

        #endregion

        #region UserActions

        /// <summary>
        /// Method to register a new user in the database
        /// Using an email and 2 identical passwords, in order protect against typoes
        /// </summary>
        /// <param name="email">The users email</param>
        /// <param name="passUnencrypted1">unencrypted first password</param>
        /// <param name="passUnencrypted2">unencrypted second password</param>
        /// <returns>Wheter the creation of the user was successful - return -1 if it was unsuccesful</returns>
        public int RegisterUser(string email, string passUnencrypted1, string passUnencrypted2)
        {
            //boolean which will be returned
            int successful = -1;

            //Check if something have been entered as email - WE DO NOT CHECK THAT IT IS AN EMAIL
            if (email.Length > 0)
            {
                //Check that something has been entered as been entered as passwords and check that the two passwords are identical
                if (passUnencrypted1 != null && passUnencrypted1.Length > 0 && passUnencrypted2 != null && passUnencrypted1 == passUnencrypted2)
                {
                    //encrypt password
                    string pass = Security.EncryptString(passUnencrypted1);
                    //connect to webservice
                    using (ServiceReference.Service1Client proxy = new ServiceReference.Service1Client())
                    {
                        //register user
                        successful = proxy.AddUser(email, pass);
                    }

                    if (successful == -1) //if the user already exsist
                    {
                        System.Windows.MessageBox.Show("User aldready exsists", "Creation error");
                    }
                    else //the user has been created
                    {
                        System.Windows.MessageBox.Show("User with email: " + email + " have been successfully created", "Successful");
                    }
                }
                else //the passwords does not match
                {
                    System.Windows.MessageBox.Show("User could not be created. Entered passwords does not match", "Creation error");
                }
            }
            else //no email was entered
            {
                System.Windows.MessageBox.Show("Enter email address", "Creation error");
            }
            return successful; //return the result
        }

        public bool LoginUser(string email, string unencrytedPass)
        {
            bool successfulLogin = false;
            if (unencrytedPass.Length > 0 && email.Length > 0)
            {
                //encrypt passowrd
                String pass = Security.EncryptString(unencrytedPass);
                int userid = -1;
                //connect to webservice
                using (ServiceReference.Service1Client proxy = new ServiceReference.Service1Client())
                {
                    //get the user id - -1 if no user exists
                    userid = proxy.GetUserByEmailAndPass(email, pass);
                }
                if (userid != -1) //login successful
                {
                    //User logged in
                    session.UserID = userid;
                    session.Email = email;
                    session.RootFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\sliceofpie\\" + email;
                    Directory.CreateDirectory(session.RootFolderPath);

                    /*using (ServiceReference.Service1Client proxy = new ServiceReference.Service1Client())
                    {
                        session.RootFolderID = proxy.GetRootFolderId(userid);
                    }*/
                }
                if (userid != -1)
                {
                    System.Windows.MessageBox.Show("Logged in successfully", "Login");
                    successfulLogin = true;
                    UpdateExplorerView();
                }
                else
                {
                    MessageBox.Show("Wrong email or password", "Unable to login");
                }
            }
            else
            {
                MessageBox.Show("Enter email and password", "Login error");
            }
            return successfulLogin;
        }

        public void LogoutUser()
        {
            session.RootFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\sliceofpie\\";
            session.UserID = -1;

            SetOpenDocument(System.Windows.Markup.XamlWriter.Save(new FlowDocument()), "", "");
            UpdateExplorerView();
        }

        public void ShareDocument(string email)
        {
            if (email != null && email.Length > 0)
            {
                ServiceReference.ServiceUser shareUser = null;
                using (ServiceReference.Service1Client proxy = new ServiceReference.Service1Client())
                {
                    shareUser = proxy.GetUserByEmail(email);
                }
                if (shareUser != null)
                {
                    using (ServiceReference.Service1Client proxy = new ServiceReference.Service1Client())
                    {
                        proxy.AddUserDocument(shareUser.id, session.CurrentDocumentID, session.CurrentDocumentPath);
                    }
                    MessageBox.Show("Document shared with " + shareUser.email, "Share success");
                }
                else
                {
                    MessageBox.Show("User does not exist", "Email error");
                }
            }
        }

        #endregion

        #region LocalPersistence

        public void CreateNewDocumentFile(String title)
        {
            FlowDocument emptyDoc = new FlowDocument();
            localPersistence.CreateNewDocumentFile(title, emptyDoc);
            String documentPath = session.RootFolderPath + "\\" + title + ".txt";
            SetOpenDocument(System.Windows.Markup.XamlWriter.Save(emptyDoc), title, documentPath);
            UpdateExplorerView();
        }

        public void SaveDocumentToFile(FlowDocument document)
        {
            if (session.UserID != -1 && session.CurrentDocumentPath != null)
            {
                localPersistence.SaveDocumentToFile(document);
                UpdateExplorerView();
            }
            else
            {
                MessageBox.Show("No document open", "Document save error");
            }
        }

        public void CreateFolder(String folderName, String path)
        {
            localPersistence.CreateFolder(folderName, path);
            UpdateExplorerView();
        }

        #endregion

        #region ControllerMethods

        /// <summary>
        /// Set the currently opened document
        /// </summary>
        /// <param name="content"></param>
        /// <param name="title"></param>
        /// <param name="documentPath"></param>
        public void SetOpenDocument(String content, String title, String documentPath) //lav en overloaded metode som tager et flowdocument, title og documentpath - eller bare title og documentpath - eller bare documentpath
        {
            //try catch in case of corrupted file
            FlowDocument doc = localPersistence.CreateFlowDocumentWithoutMetadata(content);

            gui.richTextBox.Document = doc;
            gui.labelOpenDocument.Content = "Current document: " + title;
            session.CurrentDocumentTitle = title;
            session.CurrentDocumentPath = documentPath;
        }

        /// <summary>
        /// Update the explorerview
        /// </summary>
        public void UpdateExplorerView()
        {
            TreeViewModel.GetInstance().LoadFilesAndFolders(gui.ExplorerTree.Items);
        }

        #endregion

        #region SyncMethods

        public void SyncAllDocuments()
        {
            if (session.UserID != -1) //Check if user is logged in
            {
                ServiceReference.ServiceUserdocument[] documents;
                //Connect to webservice
                using (ServiceReference.Service1Client proxy = new ServiceReference.Service1Client())
                {
                    //Retrieve all documents the users documents
                    documents = proxy.GetAllUserDocumentsByUserId(session.UserID);
                }
                if (documents != null) //check if any documents is found
                {
                    //For each document found
                    foreach (ServiceReference.ServiceUserdocument currentDoc in documents)
                    {
                        String relativeDirPath = FetchRelativeFilePath(currentDoc);
                        String dirPath = session.RootFolderPath + "\\" + relativeDirPath;
                        ServiceReference.ServiceDocument documentReference = null;

                        using (ServiceReference.Service1Client proxy = new ServiceReference.Service1Client())
                        {
                            //Get the original document from the server
                            documentReference = proxy.GetDocumentById(currentDoc.documentId);
                        }
                        String filePath = dirPath + "\\" + documentReference.name + ".txt";

                        String content;
                        //Connect to webservæice
                        using (ServiceReference.Service1Client proxy = new ServiceReference.Service1Client())
                        {
                            //Get the content of the file on the server
                            content = proxy.GetLatestDocumentContent(documentReference.id);
                        }
                        //Create the directories needed?
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                        //Create a document locally with the content
                        localPersistence.SaveDocumentToFile(content, filePath);
                    }
                }
                else //No documents found by this userId
                { //loop through all the users folders and add all files to the server
                    List<String> files = new List<String>();
                    foreach (String file in Directory.GetFiles(session.RootFolderPath)) //!!!!!!!!!!!!!!!!If the rootfolder doesn't exist, an exception is thrown
                    {
                        AddDocumentToServer(file);
                    }

                    foreach (String dir in Directory.GetDirectories(session.RootFolderPath))
                    {
                        foreach (String file in Directory.GetFiles(dir))
                        {
                            AddDocumentToServer(file);
                        }
                    }
                }
            }
            else
            {
                //not logged in
            }

            UpdateExplorerView();
        }

        private string FetchRelativeFilePath(ServiceReference.ServiceUserdocument doc)
        {
            StringBuilder sb = new StringBuilder();
            ServiceReference.ServiceFolder folder = null;

            using (ServiceReference.Service1Client proxy = new ServiceReference.Service1Client())
            {
                folder = proxy.GetFolder(doc.folderId);
            }

            while (folder != null || folder.parentFolderId != null)
            {
                if (folder.parentFolderId == null)
                {
                    break;
                }
                else
                {
                    sb.Insert(0, "\\" + folder.name);
                }
                using (ServiceReference.Service1Client proxy = new ServiceReference.Service1Client())
                {
                    folder = proxy.GetFolder((int)folder.parentFolderId);
                }
            }

            String userEmail = session.Email;
            sb.Insert(0, "");

            return sb.ToString();
        }

        /// <summary>
        /// Add file to server
        /// </summary>
        /// <param name="filePath">Path to the file which should be added to the server</param>
        private void AddDocumentToServer(String filePath)
        {
            //load file content
            String content = localPersistence.GetFileContent(filePath);
            //get file metadata
            //Object[] metadata = Metadata.RetrieveMetadataFromFile(file);
            //fetch filename
            String fileName = filePath.Substring(filePath.LastIndexOf("\\") + 1, (filePath.IndexOf(".txt") - filePath.LastIndexOf("\\") - 1));

            //Connect to webservice
            using (ServiceReference.Service1Client proxy = new ServiceReference.Service1Client())
            {
                //add document online
                proxy.AddDocumentWithUserDocument(fileName, session.UserID, filePath, content);
            }
        }

        public void SyncDocument(FlowDocument document)
        {
            //Metadata
            //0: docid -> docid 11
            //1: userid -> userid 11
            //2: timestamp -> timestamp 12-12-2012 12:18:19
            object[] metadata = Metadata.RetrieveMetadataFromFile(session.CurrentDocumentPath);
            int documentID = (int)metadata[0];
            DateTime baseDocumentCreationTime = (DateTime)metadata[2];
            //int folderID = (int)metadata[3];

            StringBuilder sb = new StringBuilder();
            string metadataString = Metadata.GenerateMetadataString(documentID, session.UserID, DateTime.UtcNow);//, folderID);
            sb.Append(metadataString);
            sb.AppendLine();
            //generate xaml for the document
            String xaml = System.Windows.Markup.XamlWriter.Save(document);
            sb.Append(xaml);

            //get the text from the document
            String content = new TextRange(document.ContentStart, document.ContentEnd).Text;

            String[][] responseArrays = null;
            //connect to the websevice
            using (ServiceReference.Service1Client proxy = new ServiceReference.Service1Client())
            {
                //push the current document
                responseArrays = proxy.SyncDocument(session.UserID, documentID, session.CurrentDocumentPath, baseDocumentCreationTime, sb.ToString(), session.CurrentDocumentTitle, content.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None));
            }

            if (responseArrays == null) //if there is no conflict
            {
                if (documentID == 0)
                {
                    //connect to the websevice
                    using (ServiceReference.Service1Client proxy = new ServiceReference.Service1Client())
                    {
                        documentID = proxy.GetDocumentId(session.UserID, session.CurrentDocumentTitle);
                    }
                }
                //save document with new metadata - basedocument
                localPersistence.SaveDocumentToFile(document, Metadata.ReplaceDocumentIDInMetadata(metadataString, documentID));
                session.CurrentDocumentID = documentID;
            }
            else if (responseArrays.Length == 1)
            {
                localPersistence.SaveDocumentToFile(responseArrays[0][0], session.CurrentDocumentPath);
            }
            else //if there is a conflict
            {
                gui.SetupMergeView(responseArrays);
            }
        }

        public void SaveMergedDocument(FlowDocument document)
        {
            Object[] oldMetadata = Metadata.RetrieveMetadataFromFile(session.CurrentDocumentPath);
            int documentid = (int)oldMetadata[0];
            int userid = session.UserID;
            DateTime timestamp = DateTime.UtcNow;
            //int folderid = (int)oldMetadata[3];

            String metadata = Metadata.GenerateMetadataString(documentid, userid, timestamp);//, folderid);
            String xamlContent = System.Windows.Markup.XamlWriter.Save(document);
            String content = metadata + xamlContent;

            using (ServiceReference.Service1Client proxy = new ServiceReference.Service1Client())
            {
                proxy.AddDocumentRevision(session.UserID, documentid, content);
            }
            localPersistence.SaveDocumentToFile(content, session.CurrentDocumentPath);
        }

        #endregion

        public void MoveFileToFolder(string fromPath, string toPath)
        {
            if (fromPath != null && fromPath.Length > 0 && toPath != null && toPath.Length > 0)
            {
                localPersistence.MoveFileToFolder(fromPath, toPath);

                //In case you move the currently opened document
                if (fromPath == session.CurrentDocumentPath)
                {
                    session.CurrentDocumentPath = toPath;
                }

                UpdateExplorerView();
            }
        }
    }
}