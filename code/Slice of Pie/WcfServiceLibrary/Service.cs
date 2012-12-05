using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Server;

namespace WcfServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in both code and config file together.
    public class Service : IService
    {
        public Boolean AddUser(String email, String password)
        {
            return Controller.GetInstance().AddUser(email, password);
        }

        public void AddFolder(String name, int parentFolderId)
        {
            Controller.GetInstance().AddFolder(name, parentFolderId);
        }

        public void AddDocument(String name, int userId, int folderId, String content)
        {
            Controller.GetInstance().AddDocument(name, userId, folderId, content);
        }

        public User GetUserById(int userId)
        {
            return Controller.GetInstance().GetUser(userId);
        }

        public User GetUserByEmail(String email)
        {
            return Controller.GetInstance().GetUser(email);
        }

        public Document GetDocumentById(int documentId)
        {
            return Controller.GetInstance().GetDocument(documentId);
        }

        public Document GetDocumentByName(String name)
        {
            return Controller.GetInstance().GetDocument(name);
        }

        public Folder GetFolder(int folderId)
        {
            return Controller.GetInstance().GetFolder(folderId);
        }

        public void DeleteFolder(int folderId)
        {
            Controller.GetInstance().DeleteFolder(folderId);
        }

        public void DeleteDocumentReference(int userId, int documentId)
        {
            Controller.GetInstance().DeleteDocumentReference(userId, documentId);
        }

        public void DeleteDocument(int documentId)
        {
            Controller.GetInstance().DeleteDocument(documentId);
        }
    }
}
