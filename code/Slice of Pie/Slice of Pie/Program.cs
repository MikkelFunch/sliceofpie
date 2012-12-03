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
            User user = new User();
            user.password = "Password";
            user.email = "Email";
            DAO.AddUser(user);
        }
    }
}
