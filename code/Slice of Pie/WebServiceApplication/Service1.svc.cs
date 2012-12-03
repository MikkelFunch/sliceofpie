using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Slice_of_Pie;

namespace WebServiceApplication
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class Service1 : IService1
    {
        public void AddUser(String email, String password)
        {
            User user = new User();
            user.email = email;
            user.password = password;
            DAO.AddUser(user);
            Console.WriteLine("hey you");
            Console.WriteLine("tester mester");
        }

        public void AddDocument(String name, int userId)
        {
            Document document = new Document();
            document.name = name;
            document.creatorId = userId;
            DAO.AddDocument(document);
        }

        public void AddFolder(String name, int parentFolderId)
        {
            Folder folder = new Folder();
            folder.name = name;
            folder.parentFolderId = parentFolderId;
            DAO.AddFolder(folder);
        }
    }
}
