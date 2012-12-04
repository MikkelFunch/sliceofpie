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


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        /// <summary>
        /// Deletes all tuples in database.
        /// </summary>
        [TestInitialize()]
        public void CleanDataBase()
        {
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                //Delete all folders
                var folders = context.Folders;
                foreach (Folder f in folders)
                {
                    context.DeleteObject(f);
                }

                //Delete all users
                var users = context.Users;
                foreach (User u in users)
                {
                    context.DeleteObject(u);
                }

                //Delete all documents
                var documents = context.Documents;
                foreach (Document d in documents)
                {
                    context.DeleteObject(d);
                }

                //Delete all documentRevision
                var documentRevisions = context.Documentrevisions;
                foreach (Documentrevision d in documentRevisions)
                {
                    context.DeleteObject(d);
                }

                //Delete all userDocuments
                var userdocuments = context.Userdocuments;
                foreach(Userdocument ud in userdocuments)
                {
                    context.DeleteObject(ud);
                }

                context.SaveChanges();
            }
          
        }
        
        /// <summary>
        ///A test for AddUser
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Server.dll")]
        public void AddUserGetUserTest()
        {
                string email = "test@test.test";
                string password = "test123";
                DAO.GetInstance().AddUser(email, password);
                //TODO create getter with email
                User u = DAO.GetInstance().GetUser(email);
                Assert.AreEqual(u.email, email);
                Assert.AreEqual(u.password, password);

        }

        [TestMethod()]
        [DeploymentItem("Server.dll")]
        public void AddDocumentGetDocumentTest()
        {
        }
        
    }
    
}

