﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Slice_of_Pie;


namespace WcfServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in both code and config file together.
    public class Service : IService
    {
        public void AddUser(String email, String password)
        {
<<<<<<< HEAD
            User user = new User();
            user.email = email;
            user.password = password;
            Controller.GetInstance();
=======
            Controller.GetInstance().AddUser(email, password);
>>>>>>> d8861aed014c9d0834d26b636bdb3c928e5be09a
        }
    }
}
