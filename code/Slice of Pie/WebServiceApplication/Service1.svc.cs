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
            Controller.GetInstance().AddUser(email, password);
        }

        public void AddDocument(String name, int userId)
        {
            Controller.GetInstance().AddDocument(name, userId);
        }

        public void AddFolder(String name, int parentFolderId)
        {
            Controller.GetInstance().AddFolder(name, parentFolderId);
        }
    }
}
