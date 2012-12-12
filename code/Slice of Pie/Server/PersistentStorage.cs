using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    public class PersistentStorage
    {
        //Singleton instance of facade
        private static PersistentStorage instance;
        private DAO dao;
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

        public void AddDocument(String name, int userId, int folderId, String content)
        {
            String documentPath = fsh.GetDocumentPath(userId, folderId);
            fsh.WriteToFile(documentPath, name, content);
            dao.AddDocument(name, userId, documentPath);
            int documentId = GetDocumentIdByPath(documentPath, name);
            AddUserDocument(userId, documentId, folderId);
        }

        public int GetDocumentIdByPath(String directoryPath, String filename)
        {
            return dao.GetDocumentIdByPath(directoryPath, filename);
        }

        public void AddDocumentRevision(int editorId, int documentId, String content)
        {
            DateTime creationTime = DateTime.UtcNow;
            Document document = dao.GetDocument(documentId);
            String filepath = document.path;
            String filename = document.name + "_revision_" + creationTime.ToString().Replace(':', '.');
            fsh.WriteToFile(filepath, filename, content);
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

        public String GetDocumentContent(String directoryPath, String filename)
        {
            return fsh.GetDocumentContent(directoryPath, filename);
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

        public bool DocumentHasRevision(int documentId)
        {
            return dao.DocumentHasRevision(documentId);
        }

        public String[][] SyncDocument(int editorId, int documentId, int folderId, DateTime baseDocCreationTime, String content, String title, String[] latest)
        {
            //No conflict
            if (!DocumentHasRevision(documentId))
            {
                AddDocument(title, editorId, folderId, content);
                return null;
            }
            //No conflict
            else if(GetLatestDocumentRevision(documentId, 1).First<Documentrevision>().creationTime == baseDocCreationTime)
            {
                AddDocumentRevision(editorId, documentId, content);
                return null;
            }
            //Conflict
            else
            {
                String[][] returnArray = new String[4][];
                String[] original = Model.GetInstance().GetContentAsStringArray(documentId);
                String[][] mergedLines = Model.GetInstance().MergeDocuments(original, latest);
                returnArray[0] = mergedLines[0];
                returnArray[1] = mergedLines[1];
                returnArray[2] = mergedLines[2];
                Documentrevision latestDoc = GetLatestDocumentRevision(documentId, 1).First<Documentrevision>();
                returnArray[3] = Model.GetInstance().GetContentAsStringArray(latestDoc.id);
                return returnArray;
            }
        }

        public int GetRootFolderId(int userId)
        {
            return dao.GetRootFolderId(userId);
        }

        public string GetDocumentContent(string filepath)
        {
            return fsh.GetDocumentContent(filepath);
        }
    }
}
