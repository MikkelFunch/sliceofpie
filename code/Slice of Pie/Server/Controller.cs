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
        public Boolean AddUser(String email, String password)
        {
            try
            {
                PersistentStorage.GetInstance().AddUser(email, password);
            }
            catch (System.Data.UpdateException)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Adds a folder to the database.
        /// </summary>
        /// <param name="name">The name of the folder</param>
        /// <param name="parentFolderId">The id of the parent folder. Null if it's a root folder.</param>
        public void AddFolder(String name, int parentFolderId)
        {

            PersistentStorage.GetInstance().AddFolder(name, parentFolderId);
        }

        /// <summary>
        /// Adds a document to the database.
        /// </summary>
        /// <param name="name">The name of the document</param>
        /// <param name="userId">The id of the user that creates the document</param>
        /// <param name="folderId">The id of the folder in which the document is located</param>
        /// <param name="content">The content of the document</param>
        public void AddDocumentWithUserDocument(String name, int userId, int folderId, String content)
        {
            PersistentStorage.GetInstance().AddDocumentWithUserDocument(name, userId, folderId, content);
        }

        public void AddDocumentRevision(int editorId, int documentId, String content)
        {
            PersistentStorage.GetInstance().AddDocumentRevision(editorId, documentId, content);
        }

        public void SaveMergedDocument(int editorId, int documentId, string content)
        {
            PersistentStorage.GetInstance().SaveMergedDocument(editorId, documentId, content);
        }

        public int GetUser(String email, String pass)
        {
            return PersistentStorage.GetInstance().GetUser(email, pass);
        }

        public User GetUser(int userId)
        {
            return PersistentStorage.GetInstance().GetUser(userId);
        }

        public User GetUser(String email)
        {
            return PersistentStorage.GetInstance().GetUser(email);
        }

        public Document GetDocument(int documentId)
        {
            return PersistentStorage.GetInstance().GetDocument(documentId);
        }

        public Document GetDocument(String name)
        {
            return PersistentStorage.GetInstance().GetDocument(name);
        }

        public Folder GetFolder(int folderId)
        {
            return PersistentStorage.GetInstance().GetFolder(folderId);
        }

        public void DeleteFolder(int folderId)
        {
            PersistentStorage.GetInstance().DeleteFolder(folderId);
        }

        public void DeleteDocumentReference(int userId, int documentId)
        {
            PersistentStorage.GetInstance().DeleteDocumentReference(userId, documentId);
        }

        public void DeleteDocument(int documentId)
        {
            PersistentStorage.GetInstance().DeleteDocument(documentId);
        }

        public List<Document> GetAllDocumentsByUserId(int userId)
        {
            return PersistentStorage.GetInstance().GetAllDocumentsByUserId(userId);
        }

        public String GetDocumentContent(String directoryPath, String filename)
        {
            return PersistentStorage.GetInstance().GetDocumentContent(directoryPath, filename);
        }

        public String[][] SyncDocument(int editorId, int documentId, int folderId, DateTime baseDocCreationTime, String content, String title, String[] original)
        {
            return PersistentStorage.GetInstance().SyncDocument(editorId, documentId, folderId, baseDocCreationTime, content, title, original);
        }

        public int GetRootFolderId(int userId)
        {
            return PersistentStorage.GetInstance().GetRootFolderId(userId);
        }

        public int GetDocumentId(int userId, string title)
        {
            return PersistentStorage.GetInstance().GetDocumentId(userId, title);
        }
    }
}