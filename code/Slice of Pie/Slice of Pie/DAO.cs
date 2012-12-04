using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slice_of_Pie
{
    public class DAO
    {
        //Singleton instance of DAO
        private static DAO instance;
        /// <summary>
        /// Private constructor to insure that DAO is not created outside this class.
        /// </summary>
        private DAO()
        {
        }

        public static DAO GetInstance()
        {
            if (instance == null)
            {
                instance = new DAO();
            }
            return instance;
        }


        /// <summary>
        /// Adds a user to the database
        /// </summary>
        /// <param name="user">The email and password for the user</param>
        public void AddUser(String email, String password)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                User user = new User();
                user.email = email;
                user.password = password;
                Folder folder = new Folder();
                folder.name = "Root Folder";
                user.Folder = folder;
                context.Users.AddObject(user);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Add a folder to the database
        /// </summary>
        /// <param name="folder">The name of the folder and id of parent folder</param>
        public void AddFolder(String name, int parentFolderId)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                Folder folder = new Folder();
                folder.name = name;
                folder.parentFolderId = parentFolderId;
                context.Folders.AddObject(folder);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Add a document to the database
        /// </summary>
        /// <param name="document">The name of the document and the id of the user creator</param>
        public void AddDocument(String name, int userId)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                Document document = new Document();
                document.name = name;
                document.creatorId = userId;
                document.creationTime = DateTime.UtcNow;
                context.Documents.AddObject(document);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Get a user from the database
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>The user with the given id. Null if no user has that id</returns>
        public User GetUser(int userId)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                var users = from u in context.Users
                            where u.id == userId
                            select u;
                User user = null;
                if (users.Count<User>() > 0)
                {
                    user = users.First<User>();
                }
                return user;
            }
        }

        /// <summary>
        /// Get a document from the database
        /// </summary>
        /// <param name="documentId">The id of the document</param>
        /// <returns>The document with the given id. Null if no document has that id</returns>
        public Document GetDocument(int documentId)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                var documents = from d in context.Documents
                                where d.id == documentId
                                select d;
                Document document = null;
                if (documents.Count<Document>() > 0)
                {
                    document = documents.First<Document>();
                }
                return document;
            }
        }

        /// <summary>
        /// Get a folder from the database
        /// </summary>
        /// <param name="folderId">The id of the folder</param>
        /// <returns>The folder with the given id. Null if no folder has that id</returns>
        public Folder GetFolder(int folderId)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                var folders = from f in context.Folders
                              where f.id == folderId
                              select f;
                Folder folder = null;
                if (folders.Count<Folder>() > 0)
                {
                    folder = folders.First<Folder>();
                }
                return folder;
            }
        }

        /// <summary>
        /// Delete a folder from the database
        /// </summary>
        /// <param name="folderId">The id of the folder to delete</param>
        public void DeleteFolder(int folderId)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                var folders = from f in context.Folders
                              where f.id == folderId
                              select f;
                if (folders.Count<Folder>() > 0)
                {
                    Folder folder = folders.First<Folder>();
                    context.Folders.DeleteObject(folder);
                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Delete a users reference to a document
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="documentId"></param>
        public void DeleteDocumentReference(int userId, int documentId)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                var userDocuments = from dr in context.Userdocuments
                                    where dr.documentId == documentId & dr.userId == userId
                                    select dr;
                if (userDocuments.Count<Userdocument>() > 0)
                {
                    Userdocument userDocument = userDocuments.First<Userdocument>();
                    context.Userdocuments.DeleteObject(userDocument);
                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Delete a document from the database
        /// </summary>
        /// <param name="documentId">The id of the document to delete</param>
        public void DeleteDocument(int documentId)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                var documents = from d in context.Documents
                                where d.id == documentId
                                select d;
                if (documents.Count<Document>() > 0)
                {
                    Document document = documents.First<Document>();
                    context.Documents.DeleteObject(document);
                    context.SaveChanges();
                }
            }
        }
    }
}