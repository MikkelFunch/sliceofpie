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
        /// Adds a document to the database.
        /// </summary>
        /// <param name="name">The name of the document</param>
        /// <param name="userId">The id of the user that creates the document</param>
        public void AddDocument(String name, int userId)
        {
            DAO.GetInstance().AddDocument(name, userId);
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
        /// Adds a user to the database.
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="password">The non-encrypted password of the user</param>
        public void AddUser(String email, String password)
        {
            DAO.GetInstance().AddUser(email, password);
        }
    }
}
