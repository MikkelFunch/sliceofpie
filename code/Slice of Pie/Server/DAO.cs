using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Server
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

        /// <summary>
        /// Accessor method for accessing the single instance of DAO.
        /// </summary>
        /// <returns>The only instance of DAO</returns>
        public static DAO GetInstance()
        {
            if (instance == null)
            {
                instance = new DAO();
            }
            return instance;
        }

        /// <summary>
        /// Removes all data from the database.
        /// </summary>
        public void DeleteAllData()
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                //Delete all folders
                var folders = context.Folders;
                foreach (Folder f in folders)
                {
                    context.Folders.DeleteObject(f);
                }

                //Delete all users
                var users = context.Users;
                foreach (User u in users)
                {
                    context.Users.DeleteObject(u);
                }

                //Delete all documents
                var documents = context.Documents;
                foreach (Document d in documents)
                {
                    context.Documents.DeleteObject(d);
                }

                //Delete all documentRevision
                var documentRevisions = context.Documentrevisions;
                foreach (Documentrevision d in documentRevisions)
                {
                    context.Documentrevisions.DeleteObject(d);
                }

                //Delete all userDocuments
                var userdocuments = context.Userdocuments;
                foreach (Userdocument ud in userdocuments)
                {
                    context.Userdocuments.DeleteObject(ud);
                }
                context.SaveChanges();
            }
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
                folder.name = "root";
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
        public void AddDocument(String name, int userId, int folderId, String content)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                Document document = new Document();
                document.name = name;
                document.creatorId = userId;
                document.creationTime = DateTime.UtcNow;
                
                //Find the path of this document
                StringBuilder sb = new StringBuilder();
                Folder folder = GetFolder(folderId);
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
                    folder = GetFolder((int)folder.parentFolderId);
                }
                String userEmail = GetUser(userId).email;
                sb.Insert(0, "\\sliceofpie\\" + userEmail);
                String folderPath = sb.ToString();

                document.path = String.Format("{0}{1}", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), folderPath);
                Directory.CreateDirectory(document.path);
                String filepath = String.Format("{0}\\{1}.txt", document.path, document.name);

                //Create the document and write the content to it.
                using (StreamWriter sw = new StreamWriter(File.Create(filepath)))
                {
                    sw.Write(content);
                }

                context.Documents.AddObject(document);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Adds a reference from a user to a document to the database.
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="documentId">The id of the document</param>
        /// <param name="folderId">the id of the folder</param>
        public void AddUserDocument(int userId, int documentId, int folderId)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                Userdocument userDocument = new Userdocument();
                userDocument.documentId = documentId;
                userDocument.userId = userId;
                userDocument.folderId = folderId;
                context.Userdocuments.AddObject(userDocument);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Retrieve user from database using email and md5 encrypted password
        /// </summary>
        /// <param name="email">user email</param>
        /// <param name="pass">corresponding password encrypted using md5</param>
        /// <returns>return null if user does not exist and the corresponding User object if the user exists</returns>
        public int GetUser(String email, String pass)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                var userid = from u in context.Users
                            where u.email == email && u.password == pass
                            select u.id;

                int returnID = -1;
                if (userid.Count<int>() > 0)
                {
                    returnID = userid.First<int>();
                }

                return returnID;
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
        /// Get a user from the database
        /// </summary>
        /// <param name="userId">The email of the user</param>
        /// <returns>The user with the given id. Null if no user has that id</returns>
        public User GetUser(String email)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                var users = from u in context.Users
                            where u.email == email
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
        /// Adds an edition/revision of an existing document.
        /// </summary>
        /// <param name="editorId">The id of the user editor</param>
        /// <param name="documentId">The id of the document that has been edited</param>
        public void AddDocumentRevision(int editorId, int documentId)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                Documentrevision dr = new Documentrevision();
                dr.creationTime = DateTime.UtcNow;
                dr.editorId = editorId;
                dr.documentId = documentId;
                dr.path = "";                                  //TODO Create that path!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                context.Documentrevisions.AddObject(dr);
                context.SaveChanges();
            }
        }

        public List<Documentrevision> GetDocumentRevisions(int documentId)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                var documentRevisions = from dr in context.Documentrevisions
                                        where dr.documentId == documentId
                                        select dr;

                List<Documentrevision> documentRevisionList = new List<Documentrevision>();
                foreach (Documentrevision dr in documentRevisions)
                {
                    documentRevisionList.Add(dr);
                }
                return documentRevisionList;
            }
        }

        public Documentrevision GetLatestDocumentRevision(int documentId)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                var documentRevisions = from dr in context.Documentrevisions
                                        where dr.documentId == documentId
                                        select dr;

                Documentrevision mostRecent = documentRevisions.First<Documentrevision>();
                foreach(Documentrevision dr in documentRevisions)
                {
                    if (dr.creationTime > mostRecent.creationTime)
                    {
                        mostRecent = dr;
                    }
                }
                return mostRecent;
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
        /// Get a document from the database
        /// </summary>
        /// <param name="documentId">The name of the document</param>
        /// <returns>The document with the given id. Null if no document has that id</returns>
        public Document GetDocument(String name)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                var documents = from d in context.Documents
                                where d.name == name
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
        /// Get a folder from the database
        /// </summary>
        /// <param name="folderId">The name of the folder</param>
        /// <returns>The folder with the given name. Null if no folder has that name</returns>
        public Folder GetFolder(String name)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                var folders = from f in context.Folders
                              where f.name == name
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
        /// <param name="userId">The id of the user</param>
        /// <param name="documentId">The id of the document</param>
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

        /// <summary>
        /// Get a userdocument from the database
        /// </summary>
        /// <param name="userId">The userId of the userdocument</param>
        /// <param name="documentId">The documentId of the userdocument</param>
        /// <returns>The Userdocument with the given userId and documentId. Null if none exists</returns>
        public Userdocument GetUserdocument(int userId, int documentId)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                var userdocuments = from ud in context.Userdocuments
                                    where ud.userId == userId && ud.documentId == documentId
                                    select ud;
                Userdocument userdocument = null;
                if (userdocuments.Count<Userdocument>() > 0)
                {
                    userdocument = userdocuments.First<Userdocument>();
                }
                return userdocument;
            }
        }
    }
}