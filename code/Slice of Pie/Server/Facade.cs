﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    public class Facade
    {
        //Singleton instance of facade
        private static Facade instance;
        private DAO dao;
        private FileSystemHandler fsh;

        /// <summary>
        /// Private constructor to ensure that Facade is not created outside this class.
        /// </summary>
        private Facade()
        {
            dao = DAO.GetInstance();
            fsh = FileSystemHandler.GetInstance();
        }

        /// <summary>
        /// Accessor method for accessing the single instance of facade.
        /// </summary>
        /// <returns>The only instance of facade </returns>
        public static Facade GetInstance()
        {
            if (instance == null)
            {
                instance = new Facade();
            }
            return instance;
        }

        public void AddDocument(String name, int userId, int folderId, String content)
        {
            String documentPath = fsh.GetDocumentPath(userId, folderId);
            fsh.WriteToFile(documentPath, content);
            dao.AddDocument(name, userId, documentPath);
        }

        public void AddDocumentRevision(int editorId, int documentId, String content)
        {
            DateTime creationTime = DateTime.UtcNow;
            Document document = dao.GetDocument(documentId);
            String filepath = String.Format("{0}\\{1}_revision_{2}.txt", document.path, document.name, creationTime.ToString().Replace(':', '.'));
            fsh.WriteToFile(filepath, content);
            dao.AddDocumentRevision(creationTime, editorId, documentId, filepath);
        }

        public void AddUser(String email, String password)
        {
            dao.AddUser(email, password);
        }

        public void AddFolder(String name, int parentFolderId)
        {
            dao.AddFolder(name, parentFolderId);
        }

        public User GetUser(int userId)
        {
            return dao.GetUser(userId);
        }

        public int GetUser(String email, String pass)
        {
            return dao.GetUser(email, pass);
        }

        public User GetUser(String email)
        {
            return dao.GetUser(email);
        }

        public Document GetDocument(int documentId)
        {
            return dao.GetDocument(documentId);
        }

        public Document GetDocument(String name)
        {
            return dao.GetDocument(name);
        }

        public Folder GetFolder(int folderId)
        {
            return dao.GetFolder(folderId);
        }

        public Folder GetFolder(String name)
        {
            return dao.GetFolder(name);
        }

        public void DeleteFolder(int folderId)
        {
            dao.DeleteFolder(folderId);
        }

        public void DeleteDocumentReference(int userId, int documentId)
        {
            dao.DeleteDocumentReference(userId, documentId);
        }

        public void DeleteDocument(int documentId)
        {
            dao.DeleteDocument(documentId);
        }

        public List<Document> GetAllDocumentsByUserId(int userId)
        {
            return dao.GetAllDocumentsByUserId(userId);
        }

        public String GetDocumentContent(String path)
        {
            return dao.GetDocumentContent(path);
        }

        public void AddUserDocument(int userId, int documentId, int folderId)
        {
            dao.AddUserDocument(userId, documentId, folderId);
        }

        public Userdocument GetUserdocument(int userId, int documentId)
        {
            return dao.GetUserdocument(userId, documentId);
        }

        public List<Documentrevision> GetDocumentRevisions(int documentId)
        {
            return dao.GetDocumentRevisions(documentId);
        }

        public List<Documentrevision> GetLatestDocumentRevision(int documentId, int count)
        {
            return dao.GetLatestDocumentRevision(documentId, count);
        }

        public List<Folder> GetFoldersByRootId(int parentId)
        {
            return dao.GetFoldersByRootId(parentId);
        }
    }
}
