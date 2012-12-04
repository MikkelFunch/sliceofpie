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
            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                //Delete all folders
                var folders = context.Folders;
                foreach (Folder f in folders)
                {
                    context.Folders.DeleteObject(f);
                }

                //Delete all users
                var users = context.Users;
                foreach (User u in users)
                {
                    context.Users.DeleteObject(u);
                }

                //Delete all documents
                var documents = context.Documents;
                foreach (Document d in documents)
                {
                    context.Documents.DeleteObject(d);
                }

                //Delete all documentRevision
                var documentRevisions = context.Documentrevisions;
                foreach (Documentrevision d in documentRevisions)
                {
                    context.Documentrevisions.DeleteObject(d);
                }

                //Delete all userDocuments
                var userdocuments = context.Userdocuments;
                foreach (Userdocument ud in userdocuments)
                {
                    context.Userdocuments.DeleteObject(ud);
                }
                context.SaveChanges();
            }
        }

        /// <summary>
        ///A test for AddUser
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

        [TestMethod()]
        public void AddDocumentGetDocumentTest()
        {

        }
    }
}