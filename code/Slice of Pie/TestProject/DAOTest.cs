using Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;

namespace TestProject
{
    /// <summary>
    ///This is a test class for DAOTest and is intended
    ///to contain all DAOTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DAOTest
    {
        /// <summary>
        /// Deletes all tuples in database.
        /// </summary>
        [TestInitialize()]
        public void CleanDataBase()
        {
            DAO.GetInstance().DeleteAllData();
        }

        /// <summary>
        /// Deletes all tuples in database.
        /// </summary>
        [ClassCleanup()]
        public static void CleanDataBaseFinish()
        {
            //DAO.GetInstance().DeleteAllData();
        }

        /// <summary>
        ///Test for AddUser and GetUser. Has no explicit dependencies.
        ///</summary>
        [TestMethod()]
        public void AddUserGetUserTest()
        {
            string email = "test";
            string password = "098F6BCD4621D373CADE4E832627B4F6";
            PersistentStorage.GetInstance().AddUser(email, password);
            User u = PersistentStorage.GetInstance().GetUserByEmail(email);
            Assert.AreEqual(u.email, email);
            Assert.AreEqual(u.password, password);
        }

        /// <summary>
        /// Test for AddDocument and GetDocument. Has AddUserGetUserTest as dependency.
        /// </summary>
        [TestMethod()]
        public void AddDocumentGetDocumentTest()
        {
            string email = "test";
            string password = "169741432292041771551662876811521114523313515247187211";
            int userId = DAO.GetInstance().AddUser(email, password);
            User u = DAO.GetInstance().GetUserById(userId);
            string name = "TestDoc";
            int documentId = DAO.GetInstance().AddDocument(name, u.id, "testcontent");
            Document d = DAO.GetInstance().GetDocumentById(documentId);
            Assert.AreEqual(d.creatorId, u.id);
            Assert.AreEqual(d.name, name);
        }

        /// <summary>
        /// Test for AddFolder and GetFolder. Has AddUserGetUserTest as dependency.
        /// </summary>
        [TestMethod()]
        public void AddFolderGetFolderTest()
        {
            string email = "test";
            string password = "169741432292041771551662876811521114523313515247187211";
            DAO.GetInstance().AddUser(email, password);
            User u = DAO.GetInstance().GetUserByEmail(email);
            string name = "testFolder";
            int folderId = DAO.GetInstance().AddFolder(name, u.rootFolderId);
            Folder f = DAO.GetInstance().GetFolder(folderId);
            Assert.AreEqual(f.name, name);
        }

        /// <summary>
        /// Test for AddUserDocumentInRoot and GetUserDocument. Has AddUserGetUserTest and AddDocumentGetDocumentTest as dependency.
        /// </summary>
        [TestMethod()]
        public void AddUserDocumentInRootGetUserDocument()
        {
            //User
            string email1 = "test";
            string password = "169741432292041771551662876811521114523313515247187211";
            int userId1 = DAO.GetInstance().AddUser(email1, password);
            string email2 = "test2";
            int userId2 = DAO.GetInstance().AddUser(email2, password);
            User user = DAO.GetInstance().GetUserById(userId2);
            //Document
            int docId = DAO.GetInstance().AddDocument("title", userId1, "testpath");
            //Userdocument
            DAO.GetInstance().AddUserDocumentInRoot(userId2, docId);
            Userdocument ud = DAO.GetInstance().GetUserdocument(userId2, docId);
            Assert.AreEqual(ud.documentId, docId);
            Assert.AreEqual(ud.folderId, user.rootFolderId);
        }
        

        /// <summary>
        /// Tests AddDocumentRevision and GetLatestDocumentRevision. Has AddUserGetUserTest and AddDocumentGetDocumentTest as dependency.
        /// </summary>
        [TestMethod()]
        public void GetDocumentRevisionsAddDocumentRevisionTest()
        {
            //User
            string email1 = "test";
            string password = "169741432292041771551662876811521114523313515247187211";
            int userId = DAO.GetInstance().AddUser(email1, password);
            //Document
            string name = "TestDoc";
            int docId = DAO.GetInstance().AddDocument(name, userId, "testpath");
            //DocumentRevision
            DAO.GetInstance().AddDocumentRevision(DateTime.MinValue, userId, docId, "testpath");
            DAO.GetInstance().AddDocumentRevision(DateTime.UtcNow, userId, docId, "testpath");
            List<Documentrevision> drlist = DAO.GetInstance().GetLatestDocumentRevisions(docId);
            Documentrevision dr1 = drlist[0];
            Documentrevision dr2 = drlist[1];
            Assert.AreEqual(dr1.editorId, userId);
            Assert.AreEqual(dr2.editorId, userId);
            Assert.AreEqual(dr1.documentId, docId);
            Assert.AreEqual(dr2.documentId, docId);
        }

        /// <summary>
        /// Tests GetUsersLatestDocumentRevision. Has GetDocumentRevisionsAddDocumentRevisionTest as dependency.
        /// </summary>
        [TestMethod()]
        public void GetUsersLatestDocumentRevision()
        {
            //User
            string email = "test";
            string password = "169741432292041771551662876811521114523313515247187211";
            int userId = DAO.GetInstance().AddUser(email, password);
            //Document
            string name = "TestDoc";
            int docId = DAO.GetInstance().AddDocument(name, userId, "testpath");
            //DocumentRevision
            DAO.GetInstance().AddDocumentRevision(DateTime.UtcNow, userId, docId, "testpath");
            DAO.GetInstance().AddDocumentRevision(DateTime.UtcNow, userId, docId, "testpath");
            List<Documentrevision> drlist = DAO.GetInstance().GetLatestDocumentRevisions(docId);
            Documentrevision dr1 = drlist[0];
            Documentrevision dr2 = drlist[1];
            Documentrevision dr = DAO.GetInstance().GetUsersLatestDocumentRevision(userId, docId);
            Assert.AreEqual(dr.documentId, dr1.documentId);

        }

        /// <summary>
        /// Tests GetFolderByParentId. Has AddUserGetUserTest and AddFolderGetFolder as dependency.
        /// </summary>
        [TestMethod()]
        public void GetFolderByIdTest()
        {
            //User
            string email = "test";
            string password = "169741432292041771551662876811521114523313515247187211";
            DAO.GetInstance().AddUser(email, password);
            User u = DAO.GetInstance().GetUserByEmail(email);
            //Folders
            string name1 = "testFolder1";
            string name2 = "testFolder2";
            string name3 = "testFolder3";
            DAO.GetInstance().AddFolder(name1, u.rootFolderId);
            DAO.GetInstance().AddFolder(name2, u.rootFolderId);
            DAO.GetInstance().AddFolder(name3, u.rootFolderId);
            List<Folder> folders = PersistentStorage.GetInstance().GetFoldersByRootId(u.rootFolderId);
            Assert.IsTrue(folders.Count == 3);
            Assert.IsTrue(name1.Equals(folders[0].name) || name1.Equals(folders[1].name) || name1.Equals(folders[2].name));
            Assert.IsTrue(name2.Equals(folders[0].name) || name2.Equals(folders[1].name) || name2.Equals(folders[2].name));
            Assert.IsTrue(name3.Equals(folders[0].name) || name3.Equals(folders[1].name) || name3.Equals(folders[2].name));
        }
    }
}