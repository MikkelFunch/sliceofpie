using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Controls;

namespace Server
{
    public class Controller
    {
        //Singleton instance of controller
        private static Controller instance;

        /// <summary>
        /// Private constructor to insure that Controller is not created outside this class.
        /// </summary>
        private Controller()
        {
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

        /// <summary>
        /// Adds a user to the database.
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="password">The encrypted password of the user</param>
        /// <returns>The id of the added user. -1 if a user with the given email already exists</returns>
        public int AddUser(String email, String password)
        {
            try
            {
                return PersistentStorage.GetInstance().AddUser(email, password);
            }
            catch (System.Data.UpdateException)
            {
                return -1;
            }
        }

        /// <summary>
        /// Adds a folder to the database.
        /// </summary>
        /// <param name="name">The name of the folder</param>
        /// <param name="parentFolderId">The id of the parent folder. Null if it's a root folder.</param>
        public int AddFolder(String name, int parentFolderId)
        {

            return PersistentStorage.GetInstance().AddFolder(name, parentFolderId);
        }

        /// <summary>
        /// Adds a document and links a user to the added document
        /// </summary>
        /// <param name="name">The name of the document</param>
        /// <param name="userId">The id of the user who created the document</param>
        /// <param name="folderId">The id of the folder in which the document lies</param>
        /// <param name="content">The xaml + metadata content of the document</param>
        public int AddDocumentWithUserDocument(String name, int userId, String filepath, String content)
        {
            return PersistentStorage.GetInstance().AddDocumentWithUserDocument(name, userId, filepath, content);
        }

        /// <summary>
        /// Adds a documentrevision to an already existing document
        /// </summary>
        /// <param name="editorId">The id of the user who made the revision</param>
        /// <param name="documentId">The id of the original document</param>
        /// <param name="content">The xaml + metadata content of the file</param>
        public void AddDocumentRevision(int editorId, int documentId, String content)
        {
            PersistentStorage.GetInstance().AddDocumentRevision(editorId, documentId, content);
        }

        /// <summary>
        /// Save a document that has been merged by the client
        /// </summary>
        /// <param name="editorId">The id of the user who did the merge</param>
        /// <param name="documentId">The id of the original document</param>
        /// <param name="content">The xaml + metadata content of the file</param>
        public void SaveMergedDocument(int editorId, int documentId, string content)
        {
            PersistentStorage.GetInstance().SaveMergedDocument(editorId, documentId, content);
        }

        /// <summary>
        /// Gets a user from the database
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="pass">The sha1'ed password of the user</param>
        /// <returns>The user with the given email of password</returns>
        public User GetUserByEmailAndPass(String email, String pass)
        {
            return PersistentStorage.GetInstance().GetUserByEmailAndPass(email, pass);
        }

        /// <summary>
        /// Gets a user from the database
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>The user with the given id</returns>
        public User GetUserById(int userId)
        {
            return PersistentStorage.GetInstance().GetUserById(userId);
        }

        /// <summary>
        /// Gets a user from the database
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <returns>The user with the given email</returns>
        public User GetUserByEmail(String email)
        {
            return PersistentStorage.GetInstance().GetUserByEmail(email);
        }

        /// <summary>
        /// Gets a folder from the database
        /// </summary>
        /// <param name="folderId">The id of the folder</param>
        /// <returns>The folder with the given id</returns>
        public Folder GetFolder(int folderId)
        {
            return PersistentStorage.GetInstance().GetFolder(folderId);
        }

        /// <summary>
        /// Gets the RootFolderId of a User
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>The id of the RootFolder of the User</returns>
        public int GetRootFolderId(int userId)
        {
            return PersistentStorage.GetInstance().GetRootFolderId(userId);
        }

        /// <summary>
        /// Gets a document from the database
        /// </summary>
        /// <param name="documentId">The id of the document</param>
        /// <returns>The document with the given id</returns>
        public Document GetDocumentById(int documentId)
        {
            return PersistentStorage.GetInstance().GetDocumentById(documentId);
        }

        /// <summary>
        /// Delete a folder from the database/server
        /// </summary>
        /// <param name="folderId">The id of the folder to delete</param>
        public void DeleteFolder(int folderId)
        {
            PersistentStorage.GetInstance().DeleteFolder(folderId);
        }

        /// <summary>
        /// Delete a users reference to a document
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="documentId">The id of the document</param>
        public void DeleteDocumentReference(int userId, int documentId)
        {
            PersistentStorage.GetInstance().DeleteDocumentReference(userId, documentId);
        }

        /// <summary>
        /// Delete a document from the database/server
        /// </summary>
        /// <param name="documentId">The id of the document to delete</param>
        public void DeleteDocument(int documentId)
        {
            PersistentStorage.GetInstance().DeleteDocument(documentId);
        }

        /// <summary>
        /// Get all userdocuments of a specific user
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>All userdocuments this user is subscribed to</returns>
        public List<Userdocument> GetAllUserDocumentsByUserId(int userId)
        {
            return PersistentStorage.GetInstance().GetAllUserDocumentsByUserId(userId);
        }

        /// <summary>
        /// Get the xaml + metadata content of a document
        /// </summary>
        /// <param name="directoryPath">The path to the directory in which the document is located</param>
        /// <param name="filename">The name of the document</param>
        /// <returns>The xaml + metadata content of the document found at the filepath</returns>
        public String GetDocumentContent(String directoryPath, String filename)
        {
            return PersistentStorage.GetInstance().GetDocumentContent(directoryPath, filename);
        }

        /// <summary>
        /// Syncs a document with the server.
        /// </summary>
        /// <param name="editorId">The id of the user who's submitting his work</param>
        /// <param name="documentId">The id of the document</param>
        /// <param name="filepath">The path to where the file lies on the client</param>
        /// <param name="fileContent">The xaml content of the document the user is syncing</param>
        /// <param name="title">The title of the document</param>
        /// <param name="pureContent">The "pure" content of the document. One line per index in the array</param>
        /// <returns>Null if there's no mergeconflict.
        /// If there is a mergeconflict the returned is like this:
        /// Array[0] = the merged document
        /// Array[1] = insertions, same length as Array[0]
        /// Array[2] = deletions, same length as Array[3]
        /// Array[3] = the original document (server version)</returns>
        public String[][] SyncDocument(int editorId, int documentId, String filepath, String fileContent, String title, String pureContent)
        {
            String[] latestAsArray = pureContent.Split(new String[] { "\r\n", "\n" }, StringSplitOptions.None);

            PersistentStorage ps = PersistentStorage.GetInstance();
            //Document found with the given id
            if (GetDocumentById(documentId) != null)
            {
                //Check if the document has any revisions
                bool hasRevisions = ps.DocumentHasRevision(documentId);

                if (!hasRevisions)
                {
                    //No conflict
                    return ps.SyncNoConflict(editorId, documentId, filepath, fileContent);
                }

                //Get the latest documentrevision by the user
                Documentrevision latestUserDocumentRevision = ps.GetLatestDocumentRevisionByUserId(editorId, documentId);
                //Get the latest documentrevision on the server
                Documentrevision latestServerDocumentRevision = ps.GetLatestDocumentRevisions(documentId)[0];
                //Get the content of the latest documentrevision by the user
                String latestUserDocumentContent = ps.GetDocumentRevisionContent(latestUserDocumentRevision);
                //Get the content of the latest documentrevision on the server
                String latestServerDocumentContent = ps.GetDocumentRevisionContent(latestServerDocumentRevision);

                //Check if the two contents are equal. If they are equal, there's no conflict
                if (latestUserDocumentContent == latestServerDocumentContent)
                {
                    //No conflict
                    return ps.SyncNoConflict(editorId, documentId, filepath, fileContent);
                }
                else
                {
                    //Conflict
                    return Model.GetInstance().SyncConflict(documentId, latestAsArray);
                }
            }
            else
            {
                //No document found with the given id.
                ps.AddDocumentWithUserDocument(title, editorId, filepath, fileContent);
                return null;
            }
        }

        /// <summary>
        /// Syncs a document with the server.
        /// </summary>
        /// <param name="editorId">The id of the user who's submitting his work</param>
        /// <param name="documentId">The id of the document</param>
        /// <param name="filepath">The path to where the file lies on the client</param>
        /// <param name="metadata">The metadata for the document being synchronized</param>
        /// <param name="title">The title of the document</param>
        /// <param name="pureContent">The "pure" content of the document. One line per index in the array</param>
        /// <returns>Null if there's no mergeconflict.
        /// If there is a mergeconflict the returned is like this:
        /// Array[0] = the merged document
        /// Array[1] = insertions, same length as Array[0]
        /// Array[2] = deletions, same length as Array[3]
        /// Array[3] = the original document (server version)</returns>
        public String[][] SyncDocumentWeb(int editorId, int documentId, String filepath, String metadata, String title, String pureContent)
        {
            String xaml = GetXamlFromPureContent(pureContent);
            String fileContent = metadata + xaml;
            return SyncDocument(editorId, documentId, filepath, fileContent, title, pureContent);
        }

        /// <summary>
        /// Get the id of a document
        /// </summary>
        /// <param name="userId">The id of user which is subscribed to the document</param>
        /// <param name="title">The title of the document</param>
        /// <returns>The id of the document</returns>
        public int GetDocumentId(int userId, string title)
        {
            return PersistentStorage.GetInstance().GetDocumentId(userId, title);
        }

        /// <summary>
        /// Subscribes a user to a document
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="documentId">The id of the document</param>
        /// /// <param name="filepath">The path to the file</param>
        public void AddUserDocument(int userId, int documentId, String filepath)
        {
            PersistentStorage.GetInstance().AddUserDocument(userId, documentId, filepath);
        }

        /// <summary>
        /// Get the content of the latest documentrevision by a documentId
        /// </summary>
        /// <param name="documentId">The id of the document</param>
        /// <returns>The content of the latest documentrevision</returns>
        public string GetLatestDocumentContent(int documentId)
        {
            return PersistentStorage.GetInstance().GetLatestDocumentContent(documentId);
        }

        /// <summary>
        /// Check if a folder exists
        /// </summary>
        /// <param name="parentFolderId">The id of the parentfolder</param>
        /// <param name="name">The name of the folder</param>
        /// <returns>The id of the folder if it exists, else -1</returns>
        public int FolderExists(int parentFolderId, string name)
        {
            return PersistentStorage.GetInstance().FolderExists(parentFolderId, name);
        }

        /// <summary>
        /// Adds a userdocument in the givens users root folder
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="documentId">The id of the document</param>
        public void AddUserDocumentInRoot(int userId, int documentId)
        {
            PersistentStorage.GetInstance().AddUserDocumentInRoot(userId, documentId);
        }

        /// <summary>
        /// Get all documentrevisions with the given documentId
        /// </summary>
        /// <param name="documentId">The documentId</param>
        /// <returns>All documentrevisions with the given documentId</returns>
        public List<Documentrevision> GetAllDocumentRevisionsByDocumentId(int documentId)
        {
            return PersistentStorage.GetInstance().GetLatestDocumentRevisions(documentId);
        }

        public String[][][] GetAllFilesAndFolderByUserId(int userId)
        {
            PersistentStorage ps = PersistentStorage.GetInstance();

            List<String[]> metadataListFolder = new List<String[]>();
            //Get the rootFolderId of the user
            int rootFolderId = ps.GetRootFolderId(userId);

            List<Folder> folders = new List<Folder>();
            folders.AddRange(ps.GetFoldersByRootId(rootFolderId));
            Folder currentFolder;
            for (int i = 0; i < folders.Count; i++)
            {
                currentFolder = folders[i];
                String[] metadata = new String[3];
                metadata[0] = currentFolder.id.ToString();
                metadata[1] = currentFolder.name;
                metadata[2] = currentFolder.parentFolderId.ToString();
                metadataListFolder.Add(metadata);
                folders.AddRange(ps.GetFoldersByRootId(currentFolder.id));
            }
            ////////////////////////////////////////////////////////////
            List<String[]> metadataListDocument = new List<String[]>();

            List<Userdocument> userdocs = new List<Userdocument>();
            List<Userdocument> userdocsFromServer = ps.GetAllUserDocumentsByUserId(userId);
            if (userdocsFromServer != null)
            {
                userdocs.AddRange(userdocsFromServer);
                Document currentDoc;
                foreach (Userdocument ud in userdocs)
                {
                    currentDoc = ps.GetDocumentById(ud.documentId);
                    String[] metadata = new String[3];
                    metadata[0] = ud.documentId.ToString();
                    metadata[1] = ud.folderId.ToString();
                    metadata[2] = currentDoc.name;
                    metadataListDocument.Add(metadata);
                }
            }
            String[][][] returnArray = new String[2][][];
            returnArray[0] = metadataListFolder.ToArray();
            returnArray[1] = metadataListDocument.ToArray();
            return returnArray;
        }

        /// <summary>
        /// Gets the textual content of a document from the document id.
        /// </summary>
        /// <param name="documentId">The document to get the content from</param>
        /// <returns>The textual content</returns>
        public string GetLatestPureDocumentContent(int documentId)
        {
            String metadataAndXaml = PersistentStorage.GetInstance().GetLatestDocumentContent(documentId);
            String xaml = metadataAndXaml.Substring(metadataAndXaml.IndexOf("<"));
            FlowDocument flowDocument = (FlowDocument)System.Windows.Markup.XamlReader.Parse(xaml);
            TextRange textRange = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd);
            return textRange.Text;
        }

        /// <summary>
        /// Share a document through the web interface
        /// </summary>
        /// <param name="documentId">The id of the document that's been shared</param>
        /// <param name="ownerId">The id of the user that shares the document</param>
        /// <param name="recieverId">the id of the user that is being shared the document</param>
        public void ShareDocumentWeb(int documentId, int ownerId, int recieverId)
        {
            String content = PersistentStorage.GetInstance().GetLatestDocumentContent(documentId);
            PersistentStorage.GetInstance().AddDocumentRevision(recieverId, documentId, content);
        }

        /// <summary>
        /// Add a document revision through the web site
        /// </summary>
        /// <param name="documentId">The id of the document, that were adding a revision to</param>
        /// <param name="userId">The id of the user who's adding the revision</param>
        /// <param name="pureContent">The pure content of the document</param>
        /// <param name="metadata">The metadata for the document</param>
        public void AddDocumentRevisionWeb(int documentId, int userId, string pureContent, string metadata)
        {
            String xaml = GetXamlFromPureContent(pureContent);
            String fileContent = metadata + xaml;
            PersistentStorage.GetInstance().AddDocumentRevision(userId, documentId, fileContent);
        }

        /// <summary>
        /// Convert a regular string to a xaml content
        /// </summary>
        /// <param name="pureContent">The pure content</param>
        /// <returns>The xaml'ed content</returns>
        private String GetXamlFromPureContent(String pureContent)
        {
            String[] splitPureContent = pureContent.Split(new String[] { "\r\n", "\n" }, StringSplitOptions.None);
            FlowDocument document = new FlowDocument();
            foreach (String s in splitPureContent)
            {
                Paragraph p = new Paragraph(new Run(s));
                document.Blocks.Add(p);
            }
            return System.Windows.Markup.XamlWriter.Save(document);
        }

        /// <summary>
        /// Moves a document from one folder to another
        /// </summary>
        /// <param name="userId">The id of the user whos moving the document</param>
        /// <param name="documentId">The id of the document the user is moving</param>
        /// <param name="newFolderId">The id of the folder the user is moving the document to</param>
        public void MoveDocumentWeb(int userId, int documentId, int newFolderId)
        {
            PersistentStorage.GetInstance().MoveDocumentWeb(userId, documentId, newFolderId);
        }

        /// <summary>
        /// Gets the pure content of a document revision
        /// </summary>
        /// <param name="documentRevisionId">The id of the document revision</param>
        /// <returns>The pure content of the document revision</returns>
        public string GetDocumentRevisionContentById(int documentRevisionId)
        {
            Documentrevision documentRevision = PersistentStorage.GetInstance().GetDocumentRevisionById(documentRevisionId);
            String fileContent = PersistentStorage.GetInstance().GetDocumentRevisionContent(documentRevision);
            String xaml = fileContent.Substring(fileContent.IndexOf("<"));
            FlowDocument flowDocument = (FlowDocument)System.Windows.Markup.XamlReader.Parse(xaml);
            TextRange textRange = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd);
            return textRange.Text;
        }
    }
}