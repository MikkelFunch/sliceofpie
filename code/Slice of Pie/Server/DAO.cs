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
        /// <param name="name">The name of the folder</param>
        /// <param name="parentFolderId">The id of the parentfolder</param>
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

        /*
        /// <summary>
        /// Adds a document to the database
        /// </summary>
        /// <param name="name">The name of the document</param>
        /// <param name="userId">The id of the creator</param>
        /// <param name="folderId">The id of the folder in which the document is located</param>
        /// <param name="content">The content of the document</param>
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
        }*/

        public void AddDocument(String name, int userId, String documentPath)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                Document document = new Document();
                document.name = name;
                document.creatorId = userId;
                document.creationTime = DateTime.UtcNow;
                document.path = documentPath;
                context.Documents.AddObject(document);
                context.SaveChanges();
            }
        }

        /*
        /// <summary>
        /// Adds an edition/revision of an existing document.
        /// </summary>
        /// <param name="editorId">The id of the editor</param>
        /// <param name="documentId">The id of the document that has been edited</param>
        /// <param name="content">The content of the document</param>
        public void AddDocumentRevision(int editorId, int documentId, String content)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                Documentrevision documentRevision = new Documentrevision();
                documentRevision.creationTime = DateTime.UtcNow;
                documentRevision.editorId = editorId;
                documentRevision.documentId = documentId;

                Document originalDocument = GetDocument(documentId);
                String folderPath = originalDocument.path;

                String filepath = String.Format("{0}\\{1}_revision_{2}.txt", folderPath,
                    originalDocument.name, documentRevision.creationTime.ToString().Replace(':', '.'));
                //Create the document and write the content to it.
                using (StreamWriter sw = new StreamWriter(File.Create(filepath)))
                {
                    sw.Write(content);
                }

                documentRevision.path = filepath;
                context.Documentrevisions.AddObject(documentRevision);
                context.SaveChanges();
            }
        }*/

        public void AddDocumentRevision(DateTime creationTime, int editorId, int documentId, String filepath) {
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
        /// /// <param name="count">The number of document revisions to return</param>
        /// <returns>The latest document revision</returns>
        public List<Documentrevision> GetLatestDocumentRevision(int documentId, int count)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                var documentRevisions = from dr in context.Documentrevisions
                                        where dr.documentId == documentId
                                        orderby dr.creationTime descending
                                        select dr;

                List<Documentrevision> documentRevisionList = new List<Documentrevision>();
                int i = 1;
                foreach (Documentrevision dr in documentRevisions)
                {
                    if (i <= count)
                    {
                        documentRevisionList.Add(dr);
                        i++;
                    }
                    else
                    {
                        break;
                    }
                }

                return documentRevisionList;
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

        public List<Document> GetAllDocumentsByUserId(int userId)
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                var docs = from ud in context.Userdocuments
                                join d in context.Documents on ud.documentId equals d.id
                                select d;
                if (docs.Count<Document>() > 0)
                {
                    return docs.ToList();
                }
                else
                {
                    //No documents found
                    return null;
                }
            }
        }

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

        public int GetRootFolderId(int userId)
        {
            return GetUser(userId).rootFolderId;
        }

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
    }
}