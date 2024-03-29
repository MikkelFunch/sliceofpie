﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    public class PersistentStorage
    {
        //Singleton instance of facade
        private static PersistentStorage instance;
        //A reference to the dao
        private DAO dao;
        //A refenrence to the filesystemhandler
        private FileSystemHandler fsh;

        /// <summary>
        /// Private constructor to ensure that Facade is not created outside this class.
        /// </summary>
        private PersistentStorage()
        {
            dao = DAO.GetInstance();
            fsh = FileSystemHandler.GetInstance();
        }

        /// <summary>
        /// Accessor method for accessing the single instance of facade.
        /// </summary>
        /// <returns>The only instance of facade </returns>
        public static PersistentStorage GetInstance()
        {
            if (instance == null)
            {
                instance = new PersistentStorage();
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
            return dao.AddUser(email, password);
        }

        /// <summary>
        /// Adds a folder to the database.
        /// </summary>
        /// <param name="name">The name of the folder</param>
        /// <param name="parentFolderId">The id of the parent folder. Null if it's a root folder.</param>
        public int AddFolder(String name, int parentFolderId)
        {
            return dao.AddFolder(name, parentFolderId);
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
            String fullDirectoryPath = dao.GetFullDirectoryPath(userId, filepath);
            String rootDirectoryPath = dao.GetRootDirectoryPath(userId, filepath);
            CreateDirectoriesAndReturnLatestId(userId, fullDirectoryPath);
            int documentId = dao.AddDocument(name, userId, rootDirectoryPath);
            filepath = rootDirectoryPath + "\\" + name + ".txt";
            fsh.WriteToFile(filepath, content, documentId);

            AddUserDocument(userId, documentId, fullDirectoryPath);
            return documentId;
        }

        /// <summary>
        /// Adds a documentrevision to an already existing document
        /// </summary>
        /// <param name="editorId">The id of the user who made the revision</param>
        /// <param name="documentId">The id of the original document</param>
        /// <param name="content">The xaml + metadata content of the file</param>
        public void AddDocumentRevision(int editorId, int documentId, String content)
        {
            int startIndex = content.IndexOf("timestamp") + 10;
            int endIndex = content.IndexOf("]");
            DateTime creationTime = DateTime.Parse(content.Substring(startIndex, (endIndex - startIndex)));
            Document document = dao.GetDocumentById(documentId);
            String directoryPath = document.path;
            String filename = document.name + "_revision_" + creationTime.ToString().Replace(':', '.') + ".txt";
            String filepath = directoryPath +"\\" + filename;
            fsh.WriteToFile(filepath, content, documentId);
            dao.AddDocumentRevision(creationTime, editorId, documentId, directoryPath);
        }

        /// <summary>
        /// Save a document that has been merged by the client
        /// </summary>
        /// <param name="editorId">The id of the user who did the merge</param>
        /// <param name="documentId">The id of the original document</param>
        /// <param name="content">The xaml + metadata content of the file</param>
        public void SaveMergedDocument(int editorId, int documentId, string content)
        {
            int startIndex = content.IndexOf("timestamp") + 10;
            int endIndex = content.LastIndexOf("|");
            DateTime creationTime = DateTime.Parse(content.Substring(startIndex, (endIndex - startIndex)));
            Document document = dao.GetDocumentById(documentId);
            String directoryPath = document.path;
            String filename = document.name + "_revision_" + creationTime.ToString().Replace(':', '.');
            String filepath = directoryPath + "\\" + filename;
            fsh.WriteToFile(filepath, content, documentId);
            dao.AddDocumentRevision(creationTime, editorId, documentId, filepath);
        }

        /// <summary>
        /// Gets a user from the database
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="pass">The sha1'ed password of the user</param>
        /// <returns>The user with the given email of password</returns>
        public User GetUserByEmailAndPass(String email, String pass)
        {
            return dao.GetUserByEmailAndPass(email, pass);
        }

        /// <summary>
        /// Gets a user from the database
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>The user with the given id</returns>
        public User GetUserById(int userId)
        {
            return dao.GetUserById(userId);
        }

        /// <summary>
        /// Gets a user from the database
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <returns>The user with the given email</returns>
        public User GetUserByEmail(String email)
        {
            return dao.GetUserByEmail(email);
        }

        /// <summary>
        /// Gets a folder from the database
        /// </summary>
        /// <param name="folderId">The id of the folder</param>
        /// <returns>The folder with the given id</returns>
        public Folder GetFolder(int folderId)
        {
            return dao.GetFolder(folderId);
        }

        /// <summary>
        /// Gets the RootFolderId of a User
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>The id of the RootFolder of the User</returns>
        public int GetRootFolderId(int userId)
        {
            return dao.GetRootFolderId(userId);
        }

        /// <summary>
        /// Gets a document from the database
        /// </summary>
        /// <param name="documentId">The id of the document</param>
        /// <returns>The document with the given id</returns>
        public Document GetDocumentById(int documentId)
        {
            return dao.GetDocumentById(documentId);
        }

        /// <summary>
        /// Delete a folder from the database/server
        /// </summary>
        /// <param name="folderId">The id of the folder to delete</param>
        public void DeleteFolder(int folderId)
        {
            dao.DeleteFolder(folderId);
        }

        /// <summary>
        /// Delete a users reference to a document
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="documentId">The id of the document</param>
        public void DeleteDocumentReference(int userId, int documentId)
        {
            dao.DeleteDocumentReference(userId, documentId);
        }

        /// <summary>
        /// Delete a document from the database/server
        /// </summary>
        /// <param name="documentId">The id of the document to delete</param>
        public void DeleteDocument(int documentId)
        {
            dao.DeleteDocument(documentId);
        }

        /// <summary>
        /// Get all userdocuments of a specific user
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>All userdocuments this user is subscribed to</returns>
        public List<Userdocument> GetAllUserDocumentsByUserId(int userId)
        {
            return dao.GetAllUserDocumentsByUserId(userId);
        }

        /// <summary>
        /// Get the xaml + metadata content of a document
        /// </summary>
        /// <param name="directoryPath">The path to the directory in which the document is located</param>
        /// <param name="filename">The name of the document</param>
        /// <returns>The xaml + metadata content of the document found at the filepath</returns>
        public String GetDocumentContent(String directoryPath, String filename)
        {
            return fsh.GetDocumentContent(directoryPath, filename);
        }

        /// <summary>
        /// Syncs a document with the server, when there's no conflict
        /// </summary>
        /// <param name="editorId">The id of the editor</param>
        /// <param name="documentId">The id of the document</param>
        /// <param name="filepath">The path to where the file lies on the client</param>
        /// <param name="latestUserFileContent">The xaml content of the user latest document</param>
        /// <returns>null</returns>
        public String[][] SyncNoConflict(int editorId, int documentId, String filepath, String latestUserFileContent)
        {
            //No conflict
            int indexEnd = filepath.LastIndexOf("\\");
            filepath = filepath.Substring(0, indexEnd);
            AddDocumentRevision(editorId, documentId, latestUserFileContent);
            dao.AlterUserDocument(editorId, documentId, filepath);
            return null;
        }

        /// <summary>
        /// Get the id of a document
        /// </summary>
        /// <param name="userId">The id of user which is subscribed to the document</param>
        /// <param name="title">The title of the document</param>
        /// <returns>The id of the document</returns>
        public int GetDocumentId(int userId, string title)
        {
            return dao.GetDocumentId(userId, title);
        }

        /// <summary>
        /// Subscribe a user to a document.
        /// </summary>
        /// <param name="userId">The Id of the user</param>
        /// <param name="documentId">The id of the document</param>
        /// <param name="folderId">The id of the folder in which the document is located</param>
        public void AddUserDocument(int userId, int documentId, String filepath)
        {
            dao.AddUserDocument(userId, documentId, filepath);
        }

        /// <summary>
        /// Gets the userdocument that connects a document and a user.
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="documentId">the id of the document</param>
        /// <returns>The userdocument connecting the document and the user</returns>
        public Userdocument GetUserdocument(int userId, int documentId)
        {
            return dao.GetUserdocument(userId, documentId);
        }

        /// <summary>
        /// Gets all document revisions from a specified 
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        public List<Documentrevision> GetDocumentRevisions(int documentId)
        {
            return dao.GetDocumentRevisions(documentId);
        }

        /// <summary>
        /// Get all documentrevisions with the given documentId
        /// </summary>
        /// <param name="documentId">The documentId</param>
        /// <returns>All documentrevisions with the given documentId</returns>
        public List<Documentrevision> GetLatestDocumentRevisions(int documentId)
        {
            return dao.GetLatestDocumentRevisions(documentId);
        }

        /// <summary>
        /// Gets all folders contained in the root folder and folder contained in these folders.
        /// </summary>
        /// <param name="parentId">The id of the containing folder</param>
        /// <returns>A list of the child folders</returns>
        public List<Folder> GetFoldersByRootId(int parentId)
        {
            return dao.GetFoldersByRootId(parentId);
        }

        /// <summary>
        /// Returns whether the document with the specified id has a revision.
        /// </summary>
        /// <param name="documentId">The id of the specified document</param>
        /// <returns>Whether the document has a revision in the database</returns>
        public bool DocumentHasRevision(int documentId)
        {
            return dao.DocumentHasRevision(documentId);
        }

        /// <summary>
        /// Gets the content of the document in the specified file path.
        /// </summary>
        /// <param name="filepath">The file path of the document</param>
        /// <returns>The content of the document in the filepath</returns>
        public string GetDocumentContent(string filepath)
        {
            return fsh.GetDocumentContent(filepath);
        }

        /// <summary>
        /// Gets the content of the latest document revision made from a document.
        /// </summary>
        /// <param name="documentId">The id of the document the revision is based from</param>
        /// <returns>The content of the latest document revision</returns>
        public string GetLatestDocumentContent(int documentId)
        {
            List<Documentrevision> latestDocumentRevisions = GetLatestDocumentRevisions(documentId);
            Document originalDocument = GetDocumentById(documentId);
            String documentContent;
            if (latestDocumentRevisions.Count > 0)
            {
                Documentrevision latestDocumentRevision = latestDocumentRevisions[0];
                String creationTime = latestDocumentRevision.creationTime.ToString().Replace(":", ".");
                String filepath = originalDocument.path + "\\" + originalDocument.name + "_revision_" + creationTime + ".txt";
                documentContent = GetDocumentContent(filepath);
            }
            else
            {
                String creationTime = originalDocument.creationTime.ToString().Replace(":", ".");
                String filepath = originalDocument.path + "\\" + originalDocument.name + ".txt";
                documentContent = GetDocumentContent(filepath);
            }
            return documentContent;
        }

        /// <summary>
        /// Check if a folder exists
        /// </summary>
        /// <param name="parentFolderId">The id of the parentfolder</param>
        /// <param name="name">The name of the folder</param>
        /// <returns>The id of the folder if it exists, else -1</returns>
        public int FolderExists(int parentFolderId, string name)
        {
            return dao.FolderExists(parentFolderId, name);
        }

        /// <summary>
        /// Create the necessary directories to store a document.
        /// The int returned is the id of the last folder.
        /// folder1\folder2\folder3 would return the id of folder3.
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="directoryPath">The directory path that the method creates folders from</param>
        /// <returns>The id of the last folder in the directory path</returns>
        public int CreateDirectoriesAndReturnLatestId(int userId, String directoryPath)
        {
            User user = GetUserById(userId);
            int parentFolderId = user.rootFolderId;
            directoryPath = directoryPath.Substring(directoryPath.IndexOf(user.email) + user.email.Length);
            String[] folderNames = directoryPath.Split(new String[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
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
        /// Adds a userdocument in the givens users root folder
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="documentId">The id of the document</param>
        public void AddUserDocumentInRoot(int userId, int documentId)
        {
            dao.AddUserDocumentInRoot(userId, documentId);
        }

        /// <summary>
        /// Gets the content of a document revision from the file system.
        /// </summary>
        /// <param name="documentRevision">The documentrevision, which the method returns the content from</param>
        /// <returns>The content of the document revision</returns>
        public String GetDocumentRevisionContent(Documentrevision documentRevision)
        {
            Document originalDocument = GetDocumentById(documentRevision.documentId);
            String creationTime = documentRevision.creationTime.ToString().Replace(":", ".");
            String filepath = originalDocument.path + "\\" + originalDocument.name + "_revision_" + creationTime + ".txt";
            return fsh.GetDocumentContent(filepath);
        }

        /// <summary>
        /// Gets the latest document revision from a specific document submitted by a specific user.
        /// </summary>
        /// <param name="editorId">The id of the user that submitted the revision</param>
        /// <param name="documentId">The id of the document the revision is made from</param>
        /// <returns>The document revision</returns>
        public Documentrevision GetLatestDocumentRevisionByUserId(int editorId, int documentId)
        {
            return dao.GetLatestDocumentRevisionByUserId(editorId, documentId);
        }

        /// <summary>
        /// Moves a document from one folder to another
        /// </summary>
        /// <param name="userId">The id of the user whos moving the document</param>
        /// <param name="documentId">The id of the document the user is moving</param>
        /// <param name="newFolderId">The id of the folder the user is moving the document to</param>
        public void MoveDocumentWeb(int userId, int documentId, int newFolderId)
        {
            dao.MoveDocumentWeb(userId, documentId, newFolderId);
        }

        /// <summary>
        /// Gets a documentrevision by its id
        /// </summary>
        /// <param name="documentRevisionId">The id of the document revision</param>
        /// <returns>The documentrevision with the given id</returns>
        public Documentrevision GetDocumentRevisionById(int documentRevisionId)
        {
            return dao.GetDocumentRevisionById(documentRevisionId);
        }
    }
}