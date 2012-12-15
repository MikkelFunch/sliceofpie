using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

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
        /// <param name="pass">The md5'ed password of the user</param>
        /// <returns>The user with the given email of password</returns>
        public int GetUserByEmailAndPass(String email, String pass)
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
        /// <param name="latestUserFileContent">The xaml content of the user latest document</param>
        /// <param name="title">The title of the document</param>
        /// <param name="latest">The "pure" content of the document. One line per index in the array</param>
        /// <returns>Null if there's no mergeconflict.
        /// If there is a mergeconflict the returned is like this:
        /// Array[0] = the merged document
        /// Array[1] = insertions, same length as Array[0]
        /// Array[2] = deletions, same length as Array[3]
        /// Array[3] = the original document (server version)</returns>
        public String[][] SyncDocument(int editorId, int documentId, String filepath, String latestUserFileContent, String title, String[] latest)
        {
            PersistentStorage ps = PersistentStorage.GetInstance();
            //Document found with the given id
            if (GetDocumentById(documentId) != null)
            {
                bool hasRevisions = ps.DocumentHasRevision(documentId);

                if (!hasRevisions)
                {
                    //No conflict
                    return ps.SyncNoConflict(editorId, documentId, filepath, latestUserFileContent);
                }

                Documentrevision latestUserDocumentRevision = ps.GetLatestDocumentRevisionByUserId(editorId, documentId);
                Documentrevision latestServerDocumentRevision = ps.GetLatestDocumentRevisions(documentId)[0];
                String latestUserDocumentContent = ps.GetDocumentRevisionContent(latestUserDocumentRevision);
                String latestServerDocumentContent = ps.GetDocumentRevisionContent(latestServerDocumentRevision);
                if (latestUserDocumentContent == latestServerDocumentContent)
                {
                    //No conflict
                    return ps.SyncNoConflict(editorId, documentId, filepath, latestUserFileContent);
                }
                else
                {
                    //Conflict
                    return Model.GetInstance().SyncConflict(documentId, latest);
                }
            }
            //No document found with the given id.
            else
            {
                AddDocumentWithUserDocument(title, editorId, filepath, latestUserFileContent);
                return null;
            }
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

        public string GetLatestDocumentContent(int documentId)
        {
            return PersistentStorage.GetInstance().GetLatestDocumentContent(documentId);
        }

        public int FolderExists(int parentFolderId, string name)
        {
            return PersistentStorage.GetInstance().FolderExists(parentFolderId, name);
        }

        public void AddUserDocumentInRoot(int userId, int documentId)
        {
            PersistentStorage.GetInstance().AddUserDocumentInRoot(userId, documentId);
        }

        public List<Documentrevision> GetAllDocumentRevisionsByDocumentId(int documentId)
        {
            return PersistentStorage.GetInstance().GetLatestDocumentRevisions(documentId);
        }
    }
}