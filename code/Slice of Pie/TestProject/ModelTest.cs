using Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace TestProject
{
    /// <summary>
    ///This is a test class for ModelTest and is intended
    ///to contain all Unit tests for Model
    ///</summary>
    [TestClass()]
    public class ModelTest
    {
        /// <summary>
        /// Testing the append function.
        /// </summary>
        [TestMethod()]
        public void MergeDocumentsTestAppend()
        {
            Model target = Model.GetInstance();
            string[] original = {"A", "B", "C", "D"};
            string[] latest = {"A", "B", "C", "D", "E"};
            string[] expected = {"A", "B", "C", "D", "E"};
            string[] actual;
            actual = target.MergeDocuments(original, latest);
            Assert.AreEqual(ArrayToString(expected), ArrayToString(actual));
        }

        /// <summary>
        /// Testing the end of document delete function
        /// </summary>
        [TestMethod()]
        public void MergeDocumentsTestDeleteAtEnd()
        {
            Model target = Model.GetInstance();
            string[] original = { "A", "B", "C", "D" };
            string[] latest = { "A", "B", "C" };
            string[] expected = { "A", "B", "C"};
            string[] actual;
            actual = target.MergeDocuments(original, latest);
            Assert.AreEqual(ArrayToString(expected), ArrayToString(actual));
        }
        
        /// <summary>
        /// 
        /// </summary>
        [TestMethod()]
        public void MergeDocumentsTestDelete()
        {
            Model target = Model.GetInstance();
            string[] original = { "A", "B", "C", "D" };
            string[] latest = { "A", "C", "D" };
            string[] expected = { "A", "C", "D" };
            string[] actual;
            actual = target.MergeDocuments(original, latest);
            Assert.AreEqual(ArrayToString(expected), ArrayToString(actual));
        }

        [TestMethod()]
        public void MergeDocumentsTestInsert()
        {
            Model target = Model.GetInstance();
            string[] original = { "A", "B", "D", "E" };
            string[] latest = { "A", "B", "C", "D", "E" };
            string[] expected = { "A", "B", "C", "D", "E" };
            string[] actual;
            actual = target.MergeDocuments(original, latest);
            Assert.AreEqual(ArrayToString(expected), ArrayToString(actual));
        }

        [TestMethod()]
        public void MergeDocumentsTestMultipleEdits1()
        {
            Model target = Model.GetInstance();
            string[] original = { "A", "B", "D", "E", "F"};
            string[] latest = { "A", "B", "C", "D", "E" };
            string[] expected = { "A", "B", "C", "D", "E" };
            string[] actual;
            actual = target.MergeDocuments(original, latest);
            Assert.AreEqual(ArrayToString(expected), ArrayToString(actual));
        }

        [TestMethod()]
        public void MergeDocumentsTestMultipleEdits2()
        {
            Model target = Model.GetInstance();
            string[] original = { "A", "B", "D", "E", "F" };
            string[] latest = { "A", "B", "C", "C", "C", "C", "D", "E" };
            string[] expected = { "A", "B", "C", "C", "C", "C", "D", "E" };
            string[] actual;
            actual = target.MergeDocuments(original, latest);
            Assert.AreEqual(ArrayToString(expected), ArrayToString(actual));
        }

        private String ArrayToString(String[] sa)
        {
            StringBuilder sb = new StringBuilder();

            foreach (string s in sa)
            {
                sb.Append(s);
            }
            return sb.ToString();
        }
    }
}
