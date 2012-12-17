using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Text.RegularExpressions;
using System.Windows;
using System.IO;
using System.Windows.Media.Imaging;

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
            if (email != null && email.Length > 0)
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
            if (email != null && email.Length > 0 && unencrytedPass != null && unencrytedPass.Length > 0)
            {
                //encrypt passowrd
                String pass = Security.EncryptString(unencrytedPass);
                ServiceReference.ServiceUser user = null;
                //connect to webservice
                using (ServiceReference.Service1Client proxy = new ServiceReference.Service1Client())
                {
                    //get the user id - -1 if no user exists
                    user = proxy.GetUserByEmailAndPass(email, pass);
                }
                if (user != null) //login successful
                {
                    //User logged in
                    session.UserID = user.id;
                    session.Email = email;
                    session.RootFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\sliceofpie\\" + email;
                    Directory.CreateDirectory(session.RootFolderPath);

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

        /// <summary>
        /// Logout user
        /// </summary>
        public void LogoutUser()
        {
            session.RootFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\sliceofpie\\";
            session.UserID = -1;

            SetOpenDocument(System.Windows.Markup.XamlWriter.Save(new FlowDocument()), "", "");
            UpdateExplorerView();
        }

        /// <summary>
        /// Method to share the currently opened document with another user
        /// </summary>
        /// <param name="email">The email of the sue rwhich you want to share the doument</param>
        public void ShareDocument(string email)
        {
            if (email != null && email.Length > 0 && session.CurrentDocumentPath.Length > 0 && session.CurrentDocumentID != -1)
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
                        proxy.AddUserDocumentInRoot(shareUser.id, session.CurrentDocumentID);
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

        /// <summary>
        /// Creates a document file in the local file system.
        /// </summary>
        /// <param name="title">The title of the document</param>
        public void CreateNewDocumentFile(String title)
        {
            if (title != null && title.Length > 0)
            {
                FlowDocument emptyDoc = new FlowDocument();
                localPersistence.CreateNewDocumentFile(title, emptyDoc);
                String documentPath = session.RootFolderPath + "\\" + title + ".txt";
                SetOpenDocument(System.Windows.Markup.XamlWriter.Save(emptyDoc), title, documentPath);
                UpdateExplorerView();
            }
        }

        /// <summary>
        /// Saves a document from the userinterface to the local persistence.
        /// </summary>
        /// <param name="document">The file as format interpretable by Rich Text Box</param>
        public void SaveDocumentToFile(FlowDocument document)
        {
            if (session.CurrentDocumentPath != null)
            {
                localPersistence.SaveDocumentToFile(document);
                UpdateExplorerView();
            }
            else
            {
                MessageBox.Show("No document open", "Document save error");
            }
        }

        /// <summary>
        /// Creates a folder with the specified name at the location that the path points at.
        /// </summary>
        /// <param name="folderName">The name of the folder</param>
        /// <param name="path">The location where the folder is created</param>
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
            DownloadAllImagesFrom(new TextRange(doc.ContentStart, doc.ContentEnd ).Text);

            gui.richTextBox.Document = doc;
            gui.labelOpenDocument.Content = "Current document: " + title;
            session.CurrentDocumentTitle = title;
            session.CurrentDocumentPath = documentPath;
            if (title.Length > 0)
            {
                string fileContent = localPersistence.GetFileContent(documentPath);
                session.CurrentDocumentID = Metadata.FetchDocumentIDFromFileContent(fileContent);
                gui.richTextBox.Visibility = Visibility.Visible;
            }
            else
            {
                session.CurrentDocumentID = 0;
                gui.richTextBox.Visibility = Visibility.Hidden;
            }
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

        /// <summary>
        /// Synchronizes all documents. This includes downloading of files from the server and overwritting of eventual exisiting local saves that can cause conflicts.
        /// Locally absent folders from documents downloaded from the server are created locally.
        /// </summary>
        public void SyncAllDocuments()
        {
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
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
                            //Connect to webservice
                            using (ServiceReference.Service1Client proxy = new ServiceReference.Service1Client())
                            {
                                //Get the content of the file on the server
                                content = proxy.GetLatestDocumentContent(documentReference.id);
                                //add a document revision in order to be able to detect merge conflicts later
                                proxy.AddDocumentRevision(session.UserID, documentReference.id, content);
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
                        foreach (String file in Directory.GetFiles(session.RootFolderPath))
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
            else
            {
                MessageBox.Show("No internet connection", "Internet connection error");
            }
        }

        /// <summary>
        /// Gets the file path that a user's document is locally stored at.
        /// </summary>
        /// <param name="doc">Userdocument from the server</param>
        /// <returns>The file path to the document from the local root directory</returns>
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
            //fetch filename
            String fileName = filePath.Substring(filePath.LastIndexOf("\\") + 1, (filePath.IndexOf(".txt") - filePath.LastIndexOf("\\") - 1));

            //Connect to webservice
            using (ServiceReference.Service1Client proxy = new ServiceReference.Service1Client())
            {
                //add document online
                proxy.AddDocumentWithUserDocument(fileName, session.UserID, filePath, content);
            }
        }

        /// <summary>
        /// Synchronizes a specific document to push changes to the server. If another user made changes meanwhile, a conflict arises and a difference view is displayed.
        /// </summary>
        /// <param name="document">The document content from the UI</param>
        public void SyncDocument(FlowDocument document)
        {
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                //Metadata
                //0: docid -> docid 11
                //1: userid -> userid 11
                //2: timestamp -> timestamp 12-12-2012 12:18:19
                object[] metadata = LocalPersistenceHandler.RetrieveMetadataFromFile(session.CurrentDocumentPath);
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
                    responseArrays = proxy.SyncDocument(session.UserID, documentID, session.CurrentDocumentPath, sb.ToString(), session.CurrentDocumentTitle, content);
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
                else //if there is a conflict
                {
                    gui.SetupMergeView(responseArrays);
                }
            }
            else
            {
                //No web connection
            }
        }

        /// <summary>
        /// Pushes the merged document revision to the server to resolve the conflict.
        /// </summary>
        /// <param name="document">The document content from the UI</param>
        public void SaveMergedDocument(FlowDocument document)
        {
            Object[] oldMetadata = LocalPersistenceHandler.RetrieveMetadataFromFile(session.CurrentDocumentPath);
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

        /// <summary>
        /// Moves a file from one filepath to another filepath.
        /// </summary>
        /// <param name="fromPath">The filepath that points to the position of the file</param>
        /// <param name="toPath">The file path that points to the location where the file shall be stored</param>
        public void MoveFileToFolder(string fromPath, string toPath)
        {
            if (fromPath != null && fromPath.Length > 0 && toPath != null && toPath.Length > 0)
            {
                if (toPath == "Root folder") { toPath = ""; }
                localPersistence.MoveFileToFolder(fromPath, toPath);
                //In case you move the currently opened document
                if (fromPath == session.CurrentDocumentPath)
                {
                    session.CurrentDocumentPath = toPath;
                }

                UpdateExplorerView();
            }
        }

        /// <summary>
        /// Event handler for downloading of images for offline cache. Redirected to LocalPersistenceHandler
        /// </summary>
        /// <param name="sender">The image downloaded</param>
        /// <param name="ea">Event arguments. Not used.</param>
        public void ImageDownloadComplete(object sender, EventArgs ea)
        {
            localPersistence.DownloadImage((BitmapImage)sender);
        }

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
        public string[][] GetAllDocumentRevisionsWithContent(int documentId)
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
        }

        /// <summary>
        /// Loads all documents and folders in the item collection to the tree view.
        /// </summary>
        /// <param name="items"></param>
        public void LoadFilesAndFolders(System.Windows.Controls.ItemCollection items)
        {
            TreeViewModel.GetInstance().LoadFilesAndFolders(items);
        }

        /// <summary>
        /// Method to remove a users participation in a document - the document still lives on the server, but the currently online user can no longer access it
        /// </summary>
        /// <param name="item">TreeViewItem represetation of the item which is to be removed</param>
        /// <param name="items">An item collection which contains the soon to be removed document</param>
        public void DeleteDocument(System.Windows.Controls.TreeViewItem item, System.Windows.Controls.ItemCollection items)
        {
            //get document id for the file
            int documentId = Metadata.FetchDocumentIDFromFileContent(localPersistence.GetFileContent(item.Tag.ToString()));

            using(ServiceReference.Service1Client proxy = new ServiceReference.Service1Client())
            {
                //delete the users document reference
                proxy.DeleteDocumentReference(session.UserID, documentId);
            }
            //delete the file locally
            localPersistence.DeleteFile(item.Tag.ToString());
            UpdateExplorerView();
        }

        /// <summary>
        /// Method which finds and starts the download of all images in the given content
        /// </summary>
        /// <param name="content">Content which might contain images</param>
        public void DownloadAllImagesFrom(string content)
        {
            //for each loop the content with the image url and the content before that is removed
            while(content.Length > 0 && content.IndexOf("[IMAGE:") >= 0)
            {
                //remove content which was before the image
                content = content.Substring(content.IndexOf("[IMAGE:"));

                //start index of the url
                int startIndex = content.IndexOf("http");
                //end index of the url
                int endIndex = content.IndexOf("]");
                //get the url
                string url = content.Substring(startIndex, endIndex-startIndex);
                //set the new content variable
                content = content.Substring(endIndex);
                try
                {
                    //Create bitmapimage refrenceing the online link to download it
                    BitmapImage bitmap = new BitmapImage(new Uri(url, UriKind.RelativeOrAbsolute));
                    //bitmap.DownloadCompleted += controller.GetDownloadCompleteEventHandler();
                    bitmap.DownloadCompleted += new EventHandler(ImageDownloadComplete);
                }
                catch(Exception e)
                {
                    //image url error
                }
            }
        }
    }
}