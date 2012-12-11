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
            Facade.GetInstance().AddUser(email, password);
            User u = Facade.GetInstance().GetUser(email);
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
            string password = "098F6BCD4621D373CADE4E832627B4F6";
            Facade.GetInstance().AddUser(email, password);
            User u = Facade.GetInstance().GetUser(email);
            string name = "TestDoc";
            Facade.GetInstance().AddDocument(name, u.id, u.rootFolderId, "testcontent");
            Document d = Facade.GetInstance().GetDocument(name);
            Assert.AreEqual(d.name, name);
        }

        /// <summary>
        /// Test for AddFolder and GetFolder. Has AddUserGetUserTest as dependency.
        /// </summary>
        [TestMethod()]
        public void AddFolderGetFolderTest()
        {
            string email = "test";
            string password = "098F6BCD4621D373CADE4E832627B4F6";
            Facade.GetInstance().AddUser(email, password);
            User u = Facade.GetInstance().GetUser(email);
            string name = "testFolder";
            Facade.GetInstance().AddFolder(name, u.rootFolderId);
            Folder f = Facade.GetInstance().GetFolder(name);
            Assert.AreEqual(f.name, name);
        }

        /// <summary>
        /// Tests AddUserdocument and GetUserdocument. Has AddUserGetUserTest and AddDocumentGetDocumentTest as dependency.
        /// </summary>
        [TestMethod()]
        public void AddUserdocumentGetUserdocumentTest()
        {
            ///User 1
            string email1 = "test1";
            string password1 = "098F6BCD4621D373CADE4E832627B4F6";
            Facade.GetInstance().AddUser(email1, password1);
            User u1 = Facade.GetInstance().GetUser(email1);
            //User 2
            string email2 = "test2";
            string password2 = "098F6BCD4621D373CADE4E832627B4F6";
            Facade.GetInstance().AddUser(email2, password2);
            User u2 = Facade.GetInstance().GetUser(email2);
            //Document
            string name = "TestDoc";
            Facade.GetInstance().AddDocument(name, u1.id, u1.rootFolderId, "testcontent");
            Document d = Facade.GetInstance().GetDocument(name);
            //Userdocument
            Facade.GetInstance().AddUserDocument(u2.id, d.id, u2.rootFolderId);
            Userdocument ud = Facade.GetInstance().GetUserdocument(u2.id, d.id);
            Assert.AreEqual(ud.folderId, u2.rootFolderId);
            Assert.AreEqual(ud.documentId, d.id);
            Assert.AreEqual(ud.userId, u2.id);
        }

        /// <summary>
        /// Tests AddDocumentRevision and GetDocumentRevision. Has AddUserGetUserTest and AddDocumentGetDocumentTest as dependency.
        /// </summary>
        [TestMethod()]
        public void AddDocumentRevisionGetDocumentRevisionsTest()
        {
            //User
            string email = "test";
            string password = "098F6BCD4621D373CADE4E832627B4F6";
            Facade.GetInstance().AddUser(email, password);
            User u = Facade.GetInstance().GetUser(email);
            //Document
            string name = "TestDoc";
            Facade.GetInstance().AddDocument(name, u.id, u.rootFolderId, "testcontent");
            Document d = Facade.GetInstance().GetDocument(name);
            //DocumentRevision
            Facade.GetInstance().AddDocumentRevision(u.id, d.id, "Newtestcontent");
            List<Documentrevision> drlist = Facade.GetInstance().GetDocumentRevisions(d.id);
            Assert.AreEqual(drlist[0].documentId, d.id);
            Assert.AreEqual(drlist[0].editorId, u.id);
        }

        /// <summary>
        /// Tests AddDocumentRevision and GetLatestDocumentRevision. Has AddUserGetUserTest and AddDocumentGetDocumentTest as dependency.
        /// </summary>
        [TestMethod()]
        public void GetLatestDocumentRevisionTest()
        {
            //User
            string email = "test";
            string password = "098F6BCD4621D373CADE4E832627B4F6";
            Facade.GetInstance().AddUser(email, password);
            User u = Facade.GetInstance().GetUser(email);
            string email1 = "test1";
            string password1 = "098F6BCD4621D373CADE4E832627B4F6";
            Facade.GetInstance().AddUser(email1, password1);
            User u1 = Facade.GetInstance().GetUser(email);
            string email2 = "test2";
            string password2 = "098F6BCD4621D373CADE4E832627B4F6";
            Facade.GetInstance().AddUser(email2, password2);
            User u2 = Facade.GetInstance().GetUser(email);
            //Document
            string name = "TestDoc";
            Facade.GetInstance().AddDocument(name, u.id, u.rootFolderId, "testcontent");
            Document d = Facade.GetInstance().GetDocument(name);
            //DocumentRevision
            Facade.GetInstance().AddDocumentRevision(u.id, d.id, "Newtestcontent");
            Facade.GetInstance().AddDocumentRevision(u1.id, d.id, "Newtestcontentedited");
            Facade.GetInstance().AddDocumentRevision(u2.id, d.id, "Newtestcontenteditedagain");
            List<Documentrevision> drlist = Facade.GetInstance().GetLatestDocumentRevision(d.id, 1);
            Documentrevision dr = drlist[0];
            Assert.AreEqual(dr.editorId, u2.id);
        }

        /// <summary>
        /// Tests GetFolderByParentId. Has AddUserGetUserTest and AddFolderGetFolder as dependency.
        /// </summary>
        [TestMethod()]
        public void GetFolderByIdTest()
        {
            //User
            string email = "test";
            string password = "098F6BCD4621D373CADE4E832627B4F6";
            Facade.GetInstance().AddUser(email, password);
            User u = Facade.GetInstance().GetUser(email);
            //Folders
            string name1 = "testFolder1";
            string name2 = "testFolder2";
            string name3 = "testFolder3";
            Facade.GetInstance().AddFolder(name1, u.rootFolderId);
            Facade.GetInstance().AddFolder(name2, u.rootFolderId);
            Facade.GetInstance().AddFolder(name3, u.rootFolderId);
            List<Folder> folders = Facade.GetInstance().GetFoldersByRootId(u.rootFolderId);
            Assert.IsTrue(folders.Count == 3);
            Assert.IsTrue(name1.Equals(folders[0].name) || name1.Equals(folders[1].name) || name1.Equals(folders[2].name));
            Assert.IsTrue(name2.Equals(folders[0].name) || name2.Equals(folders[1].name) || name2.Equals(folders[2].name));
            Assert.IsTrue(name3.Equals(folders[0].name) || name3.Equals(folders[1].name) || name3.Equals(folders[2].name));
        }
    }
}