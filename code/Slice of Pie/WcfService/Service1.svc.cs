using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Windows.Documents;

namespace WcfService
{
    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
    public class Service1 : IService1
    {
        /// <summary>
        /// Adds a user to the database.
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="password">The encrypted password of the user</param>
        /// <returns>The id of the added user. -1 if a user with the given email already exists</returns>
        public int AddUser(String email, String password)
        {
            return Server.Controller.GetInstance().AddUser(email, password);
        }

        /// <summary>
        /// Adds a folder to the database.
        /// </summary>
        /// <param name="name">The name of the folder</param>
        /// <param name="parentFolderId">The id of the parent folder. Null if it's a root folder.</param>
        public int AddFolder(String name, int parentFolderId)
        {
            return Server.Controller.GetInstance().AddFolder(name, parentFolderId);
        }

        /// <summary>
        /// Adds a document and links a user to the added document
        /// </summary>
        /// <param name="name">The name of the document</param>
        /// <param name="userId">The id of the user who created the document</param>
        /// <param name="folderId">The id of the folder in which the document lies</param>
        /// <param name="content">The xaml + metadata content of the document</param>
        /// <returns>The id of the document added</returns>
        public int AddDocumentWithUserDocument(String name, int userId, String filepath, String content)
        {
            return Server.Controller.GetInstance().AddDocumentWithUserDocument(name, userId, filepath, content);
        }

        /// <summary>
        /// Adds a documentrevision to an already existing document
        /// </summary>
        /// <param name="editorId">The id of the user who made the revision</param>
        /// <param name="documentId">The id of the original document</param>
        /// <param name="content">The xaml + metadata content of the file</param>
        public void AddDocumentRevision(int editorId, int documentId, String content)
        {
            Server.Controller.GetInstance().AddDocumentRevision(editorId, documentId, content);
        }

        /// <summary>
        /// Save a document that has been merged by the client
        /// </summary>
        /// <param name="editorId">The id of the user who did the merge</param>
        /// <param name="documentId">The id of the original document</param>
        /// <param name="content">The xaml + metadata content of the file</param>
        public void SaveMergedDocument(int editorId, int documentId, String content)
        {
            Server.Controller.GetInstance().SaveMergedDocument(editorId, documentId, content);
        }

        /// <summary>
        /// Gets a user from the database
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="pass">The sha1'ed password of the user</param>
        /// <returns>The user with the given email of password</returns>
        public ServiceUser GetUserByEmailAndPass(String email, String pass)
        {
            return (ServiceUser) Server.Controller.GetInstance().GetUserByEmailAndPass(email, pass);
        }

        /// <summary>
        /// Gets a user from the database
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>The user with the given id</returns>
        public ServiceUser GetUserById(int userId)
        {
            return (ServiceUser)Server.Controller.GetInstance().GetUserById(userId);
        }

        /// <summary>
        /// Gets a user from the database
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <returns>The user with the given email</returns>
        public ServiceUser GetUserByEmail(String email)
        {
            return (ServiceUser)Server.Controller.GetInstance().GetUserByEmail(email);
        }

        /// <summary>
        /// Gets a folder from the database
        /// </summary>
        /// <param name="folderId">The id of the folder</param>
        /// <returns>The folder with the given id</returns>
        public ServiceFolder GetFolder(int folderId)
        {
            return (ServiceFolder)Server.Controller.GetInstance().GetFolder(folderId);
        }

        /// <summary>
        /// Gets the RootFolderId of a User
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>The id of the RootFolder of the User</returns>
        public int GetRootFolderId(int userId)
        {
            return Server.Controller.GetInstance().GetRootFolderId(userId);
        }

        /// <summary>
        /// Gets a document from the database
        /// </summary>
        /// <param name="documentId">The id of the document</param>
        /// <returns>The document with the given id</returns>
        public ServiceDocument GetDocumentById(int documentId)
        {
            return (ServiceDocument)Server.Controller.GetInstance().GetDocumentById(documentId);
        }

        /// <summary>
        /// Delete a folder from the database/server
        /// </summary>
        /// <param name="folderId">The id of the folder to delete</param>
        public void DeleteFolder(int folderId)
        {
            Server.Controller.GetInstance().DeleteFolder(folderId);
        }

        /// <summary>
        /// Delete a users reference to a document
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="documentId">The id of the document</param>
        public void DeleteDocumentReference(int userId, int documentId)
        {
            Server.Controller.GetInstance().DeleteDocumentReference(userId, documentId);
        }

        /// <summary>
        /// Delete a document from the database/server
        /// </summary>
        /// <param name="documentId">The id of the document to delete</param>
        public void DeleteDocument(int documentId)
        {
            Server.Controller.GetInstance().DeleteDocument(documentId);
        }

        /// <summary>
        /// Get all userdocuments of a specific user
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>All userdocuments this user is subscribed to</returns>
        public List<ServiceUserdocument> GetAllUserDocumentsByUserId(int userId)
        {
            List<Server.Userdocument> serverList = Server.Controller.GetInstance().GetAllUserDocumentsByUserId(userId);
            List<ServiceUserdocument> returnList = null;
            if (serverList != null)
            {
                returnList = new List<ServiceUserdocument>();
                for (int i = 0; i < serverList.Count; i++)
                {
                    returnList.Add((ServiceUserdocument)serverList[i]);
                }
            }
            return returnList;
        }

        /// <summary>
        /// Get the xaml + metadata content of a document
        /// </summary>
        /// <param name="directoryPath">The path to the directory in which the document is located</param>
        /// <param name="filename">The name of the document</param>
        /// <returns>The xaml + metadata content of the document found at the filepath</returns>
        public String GetDocumentContent(String directoryPath, String filename)
        {
            return Server.Controller.GetInstance().GetDocumentContent(directoryPath, filename);
        }

        public String GetLatestDocumentContent(int documentId)
        {
            return Server.Controller.GetInstance().GetLatestDocumentContent(documentId);
        }

        /// <summary>
        /// Syncs a document with the server.
        /// </summary>
        /// <param name="editorId">The id of the user who's submitting his work</param>
        /// <param name="documentId">The id of the document</param>
        /// <param name="folderId">The folder in which the document lies</param>
        /// <param name="baseDocCreationTime">The creationTime of the document, this document is based on</param>
        /// <param name="content">The xaml content of the new document</param>
        /// <param name="title">The title of the document</param>
        /// <param name="latest">The "pure" content of the document. One line per index in the array</param>
        /// <returns>Null if there's no mergeconflict.
        /// If there is a mergeconflict the returned is like this:
        /// Array[0] = the merged document
        /// Array[1] = insertions, same length as Array[0]
        /// Array[2] = deletions, same length as Array[3]
        /// Array[3] = the original document (server version)</returns>
        public String[][] SyncDocument(int editorId, int documentId, String filepath, String fileContent, String title, String pureContent)
        {
            String[][] stringArray = Server.Controller.GetInstance().SyncDocument(editorId, documentId, filepath, fileContent, title, pureContent);
            return stringArray;
        }

        public String[][] SyncDocumentWeb(int editorId, int documentId, String filepath, String metadata, String title, String pureContent)
        {
            String[][] stringArray = Server.Controller.GetInstance().SyncDocumentWeb(editorId, documentId, filepath, metadata, title, pureContent);
            return stringArray;
        }

        /// <summary>
        /// Get the id of a document
        /// </summary>
        /// <param name="userId">The id of user which is subscribed to the document</param>
        /// <param name="title">The title of the document</param>
        /// <returns>The id of the document</returns>
        public int GetDocumentId(int userId, String title)
        {
            return Server.Controller.GetInstance().GetDocumentId(userId, title);
        }

        /// <summary>
        /// Subscribes a user to a document
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="documentId">The id of the document</param>
        /// <param name="folderId">The id of the folder</param>
        public void AddUserDocument(int userId, int documentId, String filepath)
        {
            Server.Controller.GetInstance().AddUserDocument(userId, documentId, filepath);
        }

        public int FolderExists(int parentFolderId, String name)
        {
            return Server.Controller.GetInstance().FolderExists(parentFolderId, name);
        }

        public void AddUserDocumentInRoot(int userId, int documentId)
        {
            Server.Controller.GetInstance().AddUserDocumentInRoot(userId, documentId);
        }

        public List<ServiceDocumentrevision> GetAllDocumentRevisionsByDocumentId(int documentId)
        {
            List<ServiceDocumentrevision> list = new List<ServiceDocumentrevision>();
            foreach (Server.Documentrevision doc in Server.Controller.GetInstance().GetAllDocumentRevisionsByDocumentId(documentId))
            {
                list.Add((ServiceDocumentrevision)doc);
            }
            return list;
        }

        public String[][][] GetAllFilesAndFoldersByUserId(int userId)
        {
            return Server.Controller.GetInstance().GetAllFilesAndFolderByUserId(userId);
        }

        public String GetLatestPureDocumentContent(int documentId)
        {
            return Server.Controller.GetInstance().GetLatestPureDocumentContent(documentId);
        }

        /// <summary>
        /// Share a document through the web interface
        /// </summary>
        /// <param name="documentId">The id of the document that's been shared</param>
        /// <param name="ownerId">The id of the user that shares the document</param>
        /// <param name="recieverId">the id of the user that is being shared the document</param>
        public void ShareDocumentWeb(int documentId, int ownerId, int recieverId)
        {
            Server.Controller.GetInstance().ShareDocumentWeb(documentId, ownerId, recieverId);
        }

        /// <summary>
        /// Add a document revision through the web site
        /// </summary>
        /// <param name="documentId">The id of the document, that were adding a revision to</param>
        /// <param name="userId">The id of the user who's adding the revision</param>
        /// <param name="pureContent">The pure content of the document</param>
        /// <param name="metadata">The metadata for the document</param>
        public void AddDocumentRevisionWeb(int documentId, int userId, String pureContent, String metadata)
        {
            Server.Controller.GetInstance().AddDocumentRevisionWeb(documentId, userId, pureContent, metadata);
        }

        /// <summary>
        /// Moves a document from one folder to another
        /// </summary>
        /// <param name="userId">The id of the user whos moving the document</param>
        /// <param name="documentId">The id of the document the user is moving</param>
        /// <param name="newFolderId">The id of the folder the user is moving the document to</param>
        public void MoveDocumentWeb(int userId, int documentId, int newFolderId)
        {
            Server.Controller.GetInstance().MoveDocumentWeb(userId, documentId, newFolderId);
        }
    }
}