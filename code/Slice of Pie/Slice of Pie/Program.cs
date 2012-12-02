using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slice_of_Pie
{
    class Program
    {
        static void Main(string[] args)
        {
            Folder folder = new Folder();
            folder.name = "TestFolder";
            DAO.AddFolder(folder);
            DAO.DeleteFolder(5);
        }
    }
}
