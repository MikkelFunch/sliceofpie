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
                context.SaveChanges();

                //Delete all users
                var users = context.Users;
                foreach (User u in users)
                {
                    context.DeleteObject(u);
                }
                context.SaveChanges();

                //Delete all documents
                var documents = context.Documents;
                foreach (Document d in documents)
                {
                    context.DeleteObject(d);
                }
                context.SaveChanges();

                //Delete all documentRevision
                var documentRevision = context.Documentrevisions;
                foreach (Document d in documents)
                {
                    context.DeleteObject(d);
                }
                context.SaveChanges();
            }
          
        }
        
        /// <summary>
        ///A test for AddUser
        ///</summary>
        [TestMethod()]
        public void AddUserTest()
        {
            DAO_Accessor target = new DAO_Accessor(); // TODO: Initialize to an appropriate value
            string email = string.Empty; // TODO: Initialize to an appropriate value
            string password = string.Empty; // TODO: Initialize to an appropriate value
            target.AddUser(email, password);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
        
    }
    
}
