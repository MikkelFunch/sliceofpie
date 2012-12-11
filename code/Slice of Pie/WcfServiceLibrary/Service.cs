using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in both code and config file together.
    public class Service : IService
    {
        public Boolean AddUser(String email, String password)
        {
            return Server.Controller.GetInstance().AddUser(email, password);
        }

        public void AddFolder(String name, int parentFolderId)
        {
            Server.Controller.GetInstance().AddFolder(name, parentFolderId);
        }

        /// <summary>
        /// Adds a document to the database.
        /// </summary>
        /// <param name="name">The name of the document</param>
        /// <param name="userId">The id of the user that creates the document</param>
        /// <param name="folderId">The id of the folder in which the document is located</param>
        /// <param name="content">The content of the document</param>
        public void AddDocument(String name, int userId, int folderId, String content)
        {
            Server.Controller.GetInstance().AddDocument(name, userId, folderId, content);
        }

        public void AddDocumentRevision(int editorId, int documentId, String content)
        {
            Server.Controller.GetInstance().AddDocumentRevision(editorId, documentId, content);
        }

        public int GetUserByEmailAndPass(String email, String pass)
        {
            return Server.Controller.GetInstance().GetUser(email, pass);
        }

        public ServiceUser GetUserById(int userId)
        {
            return (ServiceUser)Server.Controller.GetInstance().GetUser(userId);
        }

        public ServiceUser GetUserByEmail(String email)
        {
            return (ServiceUser)Server.Controller.GetInstance().GetUser(email);
        }

        public ServiceDocument GetDocumentById(int documentId)
        {
            return (ServiceDocument)Server.Controller.GetInstance().GetDocument(documentId);
        }

        public ServiceDocument GetDocumentByName(String name)
        {
            return (ServiceDocument)Server.Controller.GetInstance().GetDocument(name);
        }

        public ServiceFolder GetFolder(int folderId)
        {
            return (ServiceFolder)Server.Controller.GetInstance().GetFolder(folderId);
        }

        public void DeleteFolder(int folderId)
        {
            Server.Controller.GetInstance().DeleteFolder(folderId);
        }

        public void DeleteDocumentReference(int userId, int documentId)
        {
            Server.Controller.GetInstance().DeleteDocumentReference(userId, documentId);
        }

        public void DeleteDocument(int documentId)
        {
            Server.Controller.GetInstance().DeleteDocument(documentId);
        }

        public List<ServiceDocument> GetAllDocumentsByUserId(int userId)
        {
            List<Server.Document> serverDocuments =  Server.Controller.GetInstance().GetAllDocumentsByUserId(userId);
            if (serverDocuments != null)
            {
                List<ServiceDocument> documents = new List<ServiceDocument>();
                foreach (Server.Document currentDoc in serverDocuments)
                {
                    documents.Add((ServiceDocument)currentDoc);
                }
                return documents;
            }
            else
            {
                return null;
            }
        }

        public String GetDocumentContent(String path)
        {
            return Server.Controller.GetInstance().GetDocumentContent(path);
        }
    }
}