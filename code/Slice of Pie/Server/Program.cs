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
            String email = "heyho";
            String password = "gedig";
            Controller.GetInstance().AddUser(email, password);
        }
    }
}