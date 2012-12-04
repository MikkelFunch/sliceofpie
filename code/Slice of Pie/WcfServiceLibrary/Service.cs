using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Server;

namespace WcfServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in both code and config file together.
    public class Service : IService
    {
        public void AddUser(String email, String password)
        {
            Controller.GetInstance().AddUser(email, password);
        }
    }
}
