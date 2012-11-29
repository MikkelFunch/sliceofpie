using Slice_of_Pie;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestProject
{
    
    
    /// <summary>
    ///This is a test class for ModelTest and is intended
    ///to contain all Unit tests for Model
    ///</summary>
    [TestClass()]
    public class ModelTest
    {
        [TestMethod()]
        public void MergeDocumentsTestAppend()
        {
            Model_Accessor target = new Model_Accessor();
            string[] original = {"A", "B", "C", "D"};
            string[] latest = {"A", "B", "C", "D", "E"};
            string[] expected = {"A", "B", "C", "D", "E"};
            string[] actual;
            actual = target.MergeDocuments(original, latest);
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod()]
        public void MergeDocumentsTestDeleteAtEnd()
        {
            Model_Accessor target = new Model_Accessor();
            string[] original = { "A", "B", "C", "D" };
            string[] latest = { "A", "B", "C" };
            string[] expected = { "A", "B", "C"};
            string[] actual;
            actual = target.MergeDocuments(original, latest);
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod()]
        public void MergeDocumentsTestDelete()
        {
            Model_Accessor target = new Model_Accessor();
            string[] original = { "A", "B", "C", "D" };
            string[] latest = { "A", "C", "D" };
            string[] expected = { "A", "B", "C" };
            string[] actual;
            actual = target.MergeDocuments(original, latest);
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod()]
        public void MergeDocumentsTestInsert()
        {
            Model_Accessor target = new Model_Accessor();
            string[] original = { "A", "B", "D", "E" };
            string[] latest = { "A", "B", "C", "D", "E" };
            string[] expected = { "A", "B", "C", "D", "E" };
            string[] actual;
            actual = target.MergeDocuments(original, latest);
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod()]
        public void MergeDocumentsTestMultipleEdits1()
        {
            Model_Accessor target = new Model_Accessor();
            string[] original = { "A", "B", "D", "E", "F"};
            string[] latest = { "A", "B", "C", "D", "E" };
            string[] expected = { "A", "B", "C", "D", "E" };
            string[] actual;
            actual = target.MergeDocuments(original, latest);
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod()]
        public void MergeDocumentsTestMultipleEdits2()
        {
            Model_Accessor target = new Model_Accessor();
            string[] original = { "A", "B", "D", "E", "F" };
            string[] latest = { "A", "B", "C", "C", "C", "C", "D", "E" };
            string[] expected = { "A", "B", "C", "C", "C", "C", "D", "E" };
            string[] actual;
            actual = target.MergeDocuments(original, latest);
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }
    }
}
