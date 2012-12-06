using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WcfServiceLibrary.ServiceClient proxy = new WcfServiceLibrary.ServiceClient())
            {
                Document doc = (Document)proxy.GetDocumentById(32);
                User user = (User)proxy.GetUserById(76);
                Folder folder = (Folder)proxy.GetFolder(139);
                Console.ReadLine();
            }
        }
    }
}