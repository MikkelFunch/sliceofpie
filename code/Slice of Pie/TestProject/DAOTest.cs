using Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
        ///Test for AddUser and GetUser. Has no explicit dependencies.
        ///</summary>
        [TestMethod()]
        public void AddUserGetUserTest()
        {
            string email = "test@test.test";
            string password = "test123";
            DAO.GetInstance().AddUser(email, password);
            User u = DAO.GetInstance().GetUser(email);
            Assert.AreEqual(u.email, email);
            Assert.AreEqual(u.password, password);
        }

        /// <summary>
        /// Test for AddDocument and GetDocument. Has AddUserGetUserTest as dependency.
        /// </summary>
        [TestMethod()]
        public void AddDocumentGetDocumentTest()
        {
            string email = "test@test.test";
            string password = "test123";
            DAO.GetInstance().AddUser(email, password);
            User u = DAO.GetInstance().GetUser(email);
            string name = "TestDoc";
            DAO.GetInstance().AddDocument("TestDoc", u.id);
            Document d = DAO.GetInstance().GetDocument(name);
            Assert.AreEqual(d.name, name);
        }

        /// <summary>
        /// Test for AddFolder and GetFolder. Has AddUserGetUserTest as dependency.
        /// </summary>
        [TestMethod()]
        public void AddFolderGetFolderTest()
        {
            string email = "test@test.test";
            string password = "test123";
            DAO.GetInstance().AddUser(email, password);
            User u = DAO.GetInstance().GetUser(email);
            string name = "testFolder";
            DAO.GetInstance().AddFolder(name, u.rootFolderId);
            Folder f = DAO.GetInstance().GetFolder(name);
            Assert.AreEqual(f.name, name);
        }


    }
}