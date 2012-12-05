using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        /// <returns>The only instance of model</returns>
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
        /// <param name="password">The non-encrypted password of the user</param>
        public Boolean AddUser(String email, String password)
        {
            try
            {
                DAO.GetInstance().AddUser(email, password);
            }
            catch(System.Data.UpdateException e)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Adds a folder to the database.
        /// </summary>
        /// <param name="name">The name of the folder</param>
        /// <param name="parentFolderId">The id of the parent folder. Null if it is a root folder.</param>
        public void AddFolder(String name, int parentFolderId)
        {

            DAO.GetInstance().AddFolder(name, parentFolderId);
        }

        /// <summary>
        /// Adds a document to the database.
        /// </summary>
        /// <param name="name">The name of the document</param>
        /// <param name="userId">The id of the user that creates the document</param>
        public void AddDocument(String name, int userId)
        {
            DAO.GetInstance().AddDocument(name, userId);
        }

        public int GetUser(String email, String pass)
        {
            return DAO.GetInstance().GetUser(email,pass);
        }

        public User GetUser(int userId)
        {
            return DAO.GetInstance().GetUser(userId);
        }

        public User GetUser(String email)
        {
            return DAO.GetInstance().GetUser(email);
        }

        public Document GetDocument(int documentId)
        {
            return DAO.GetInstance().GetDocument(documentId);
        }

        public Document GetDocument(String name)
        {
            return DAO.GetInstance().GetDocument(name);
        }

        public Folder GetFolder(int folderId)
        {
            return DAO.GetInstance().GetFolder(folderId);
        }

        public void DeleteFolder(int folderId)
        {
            DAO.GetInstance().DeleteFolder(folderId);
        }

        public void DeleteDocumentReference(int userId, int documentId)
        {
            DAO.GetInstance().DeleteDocumentReference(userId, documentId);
        }

        public void DeleteDocument(int documentId)
        {
            DAO.GetInstance().DeleteDocument(documentId);
        }
    }
}