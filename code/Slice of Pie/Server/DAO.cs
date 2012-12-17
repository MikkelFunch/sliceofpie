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
        /// Adds a user to the database.
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="password">The encrypted password of the user</param>
        /// <returns>The id of the added user. -1 if a user with the given email already exists</returns>
        public int AddUser(String email, String password)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                User user = new User();
                user.email = email;
                user.password = password;
                //Add a root folder named "root"
                Folder folder = new Folder();
                folder.name = "root";
                user.Folder = folder;
                context.Users.AddObject(user);
                context.SaveChanges();
                return user.id;
            }
        }

        /// <summary>
        /// Adds a folder to the database.
        /// </summary>
        /// <param name="name">The name of the folder</param>
        /// <param name="parentFolderId">The id of the parent folder. Null if it's a root folder.</param>
        public int AddFolder(String name, int parentFolderId)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                Folder folder = new Folder();
                folder.name = name;
                folder.parentFolderId = parentFolderId;
                context.Folders.AddObject(folder);
                context.SaveChanges();
                return folder.id;
            }
        }

        /// <summary>
        /// Add a document to the database
        /// </summary>
        /// <param name="name">The name of the document</param>
        /// <param name="userId">The id of the user who created the document</param>
        /// <param name="filepath">The path to the file</param>
        /// <returns>The id of the created document</returns>
        public int AddDocument(String name, int userId, String directoryPath)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                Document document = new Document();
                document.name = name;
                document.creatorId = userId;
                document.creationTime = DateTime.UtcNow;
                document.path = directoryPath;
                context.Documents.AddObject(document);
                context.SaveChanges();
                return document.id;
            }
        }

        /// <summary>
        /// Adds a documentrevision to an already existing document
        /// </summary>
        /// <param name="editorId">The id of the user who made the revision</param>
        /// <param name="documentId">The id of the original document</param>
        /// <param name="content">The xaml + metadata content of the file</param>
        public void AddDocumentRevision(DateTime creationTime, int editorId, int documentId, String filepath)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                Documentrevision documentRevision = new Documentrevision();
                documentRevision.creationTime = creationTime;
                documentRevision.editorId = editorId;
                documentRevision.documentId = documentId;
                documentRevision.path = filepath;
                context.Documentrevisions.AddObject(documentRevision);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Adds a reference from a user to a document to the database.
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="documentId">The id of the document</param>
        /// <param name="directoryPath">The path of the directory in which the document lies</param>
        public void AddUserDocument(int userId, int documentId, String directoryPath)
        {
            int folderId = GetFolderIdByDirectoryPath(userId, directoryPath);
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
        /// Retrieve user from database using email and sha1 encrypted password
        /// </summary>
        /// <param name="email">user email</param>
        /// <param name="pass">corresponding password encrypted using sha1</param>
        /// <returns>return null if user does not exist and the corresponding User object if the user exists</returns>
        public User GetUserByEmailAndPass(String email, String pass)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                var user = from u in context.Users
                           where u.email == email && u.password == pass
                           select u;

                if (user.Count<User>() > 0)
                {
                    return user.First<User>();
                }
                return null;
            }
        }

        /// <summary>
        /// Get a user from the database
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>The user with the given id. Null if no user has that id</returns>
        public User GetUserById(int userId)
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
        public User GetUserByEmail(String email)
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
        /// Gets all document revisions from a single document.
        /// </summary>
        /// <param name="documentId">The id of the document</param>
        /// <returns>A list of all document revisions from the original document</returns>
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

        /// <summary>
        /// Gets the latest documents revisions from a document.
        /// </summary>
        /// <param name="documentId">The id of the document</param>
        /// <returns>The latest document revision</returns>
        public List<Documentrevision> GetLatestDocumentRevisions(int documentId)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                var documentRevisions = from dr in context.Documentrevisions
                                        where dr.documentId == documentId
                                        orderby dr.creationTime descending
                                        select dr;
                List<Documentrevision> returnList = documentRevisions.ToList<Documentrevision>();
                return returnList;
            }
        }


        /// <summary>
        /// Get a document from the database
        /// </summary>
        /// <param name="documentId">The id of the document</param>
        /// <returns>The document with the given id. Null if no document has that id</returns>
        public Document GetDocumentById(int documentId)
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
        /// Gets the folders with a specific parent id.
        /// </summary>
        /// <param name="parentId">The parent id of the folders</param>
        /// <returns>The folders with the parentId</returns>
        public List<Folder> GetFoldersByRootId(int parentId)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                var folders = from f in context.Folders
                              where f.parentFolderId == parentId
                              select f;
                List<Folder> folderList = new List<Folder>();
                foreach (Folder f in folders)
                {
                    folderList.Add(f);
                }
                return folderList;
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

        /// <summary>
        /// Get all userdocuments of a specific user
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>All userdocuments this user is subscribed to</returns>
        public List<Userdocument> GetAllUserDocumentsByUserId(int userId)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                var userdocs = from ud in context.Userdocuments
                               where ud.userId == userId
                               select ud;
                if (userdocs.Count<Userdocument>() > 0)
                {
                    return userdocs.ToList();
                }
                else
                {
                    //No documents found
                    return null;
                }
            }
        }

        /// <summary>
        /// States whether or not a document has any revisions
        /// </summary>
        /// <param name="documentId">The id of the document</param>
        /// <returns>False if the document has no revisions, otherwise true</returns>
        public bool DocumentHasRevision(int documentId)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                var documentRevisions = from dr in context.Documentrevisions
                                        where dr.documentId == documentId
                                        select dr;
                return documentRevisions.Count<Documentrevision>() != 0;
            }
        }

        /// <summary>
        /// Gets the RootFolderId of a User
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>The id of the RootFolder of the User</returns>
        public int GetRootFolderId(int userId)
        {
            return GetUserById(userId).rootFolderId;
        }

        /// <summary>
        /// Gets the id of a document by the directoryPath and filename
        /// </summary>
        /// <param name="directoryPath">The path of the directory</param>
        /// <param name="filename">The name of the file</param>
        /// <returns>The id of the document</returns>
        public int GetDocumentIdByPath(String directoryPath, String filename)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                var document = from d in context.Documents
                               where d.path == directoryPath && d.name == filename
                               select d;
                return document.First<Document>().id;
            }
        }

        /// <summary>
        /// Gets a document id by the id of the creator and the title of the document
        /// </summary>
        /// <param name="userId">the id of the user</param>
        /// <param name="title">the title of the document</param>
        /// <returns>The id of the document with the given creator and title</returns>
        public int GetDocumentId(int userId, string title)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                var document = from d in context.Documents
                               where d.creatorId == userId && d.name == title
                               select d;
                return document.First<Document>().id;
            }
        }

        /// <summary>
        /// Gets the latest documentrevision of a user
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="documentId">The id of the document</param>
        /// <returns>The latest documentrevision by that user</returns>
        public Documentrevision GetLatestDocumentRevisionByUserId(int userId, int documentId)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                var documentRevision = from dr in context.Documentrevisions
                                       where dr.editorId == userId && dr.documentId == documentId
                                       orderby dr.creationTime descending
                                       select dr;
                if (documentRevision.Count<Documentrevision>() > 0)
                {
                    return documentRevision.ToList<Documentrevision>()[0];
                }
                return null;
            }
        }

        /// <summary>
        /// Check if a folder exists
        /// </summary>
        /// <param name="parentFolderId">The id of the parentfolder</param>
        /// <param name="name">The name of the folder</param>
        /// <returns>The id of the folder if it exists, else -1</returns>
        public int FolderExists(int parentFolderId, string name)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                var folder = from f in context.Folders
                             where f.parentFolderId == parentFolderId && f.name == name
                             select f;
                if (folder.Count<Folder>() > 0)
                {
                    return folder.First<Folder>().id;
                }
                return -1;
            }
        }

        /// <summary>
        /// Alters a userdocument, effectively moving the document from one folder to another
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="documentId">The id of the document</param>
        /// <param name="filepath">the path to the document</param>
        public void AlterUserDocument(int userId, int documentId, String filepath)
        {
            int folderId = GetFolderIdByDirectoryPath(userId, filepath);
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                var userdocuments = from ud in context.Userdocuments
                                    where ud.userId == userId && ud.documentId == documentId
                                    select ud;
                Userdocument userdocument = userdocuments.First<Userdocument>();
                userdocument.folderId = folderId;
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Gets the id of the folder by a directory path
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="directoryPath">The path of the directory</param>
        /// <returns>The id of the folder</returns>
        private int GetFolderIdByDirectoryPath(int userId, String directoryPath)
        {
            directoryPath += "\\";
            User user = GetUserById(userId);
            int indexStart = directoryPath.IndexOf(user.email) + user.email.Length;
            int indexEnd = directoryPath.LastIndexOf("\\");
            String relativeDirPath = directoryPath.Substring(indexStart, indexEnd - indexStart);
            String[] folderNames = relativeDirPath.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
            int parentFolderId = user.rootFolderId;

            foreach (string s in folderNames)
            {
                int folderId = FolderExists(parentFolderId, s);
                if (folderId != -1)
                {
                    parentFolderId = folderId;
                }
                else
                {
                    parentFolderId = AddFolder(s, parentFolderId);
                }
            }
            return parentFolderId;
        }

        /// <summary>
        /// Returns the folder path for a user in the server from a user id and a base file path.
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="filepath">The base file path</param>
        /// <returns>The full path to the user's documents</returns>
        public String GetFullDirectoryPath(int userId, String filepath)
        {
            User user = GetUserById(userId);
            int indexStart = filepath.IndexOf(user.email) + user.email.Length;
            int indexEnd = filepath.LastIndexOf("\\");
            String relativeDirectory = filepath.Substring(indexStart, indexEnd - indexStart);
            String directoryPath = "D:\\SliceOfPieDocuments\\sliceofpie\\" + user.email + relativeDirectory;
            return directoryPath;
        }


        public String GetRootDirectoryPath(int userId, String filepath)
        {
            User user = GetUserById(userId);
            String directoryPath = "D:\\SliceOfPieDocuments\\sliceofpie\\" + user.email;
            return directoryPath;
        }

        /// <summary>
        /// Adds a userdocument in the givens users root folder
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="documentId">The id of the document</param>
        public void AddUserDocumentInRoot(int userId, int documentId)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                User user = GetUserById(userId);
                Userdocument userdocument = new Userdocument();
                userdocument.documentId = documentId;
                userdocument.folderId = user.rootFolderId;
                userdocument.userId = userId;
                context.Userdocuments.AddObject(userdocument);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Gets the latest revision of a specific document submitted by a specific user.
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="documentId">The id of the document</param>
        /// <returns>The revision of the specified document</returns>
        public Documentrevision GetUsersLatestDocumentRevision(int userId, int documentId)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                var documentrevisions = from dr in context.Documentrevisions
                                        where dr.editorId == userId && dr.documentId == documentId
                                        orderby dr.creationTime descending
                                        select dr;
                if (documentrevisions.Count<Documentrevision>() > 0)
                {
                    return documentrevisions.ToList<Documentrevision>()[0];
                }
                return null;
            }
        }

        /// <summary>
        /// Moves a document from one folder to another
        /// </summary>
        /// <param name="userId">The id of the user whos moving the document</param>
        /// <param name="documentId">The id of the document the user is moving</param>
        /// <param name="newFolderId">The id of the folder the user is moving the document to</param>
        public void MoveDocumentWeb(int userId, int documentId, int newFolderId)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                var userdocument = from ud in context.Userdocuments
                                   where ud.userId == userId && ud.documentId == documentId
                                   select ud;
                userdocument.First<Userdocument>().folderId = newFolderId;
                context.SaveChanges();
            }
        }
    }
}