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
            String email = "Email";
            String password = "Password";
            Controller.GetInstance().AddUser(email, password);
        }
    }
}