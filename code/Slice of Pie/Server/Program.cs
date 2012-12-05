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
            string email = "test@test.test";
            string password = "test123";
            DAO.GetInstance().AddUser(email, password);
            User u = DAO.GetInstance().GetUser(email);
            string name = "TestDoc";
            DAO.GetInstance().AddDocument("TestDoc", u.id);
            Document d = DAO.GetInstance().GetDocument(name);

            Console.Out.WriteLine(Controller.GetInstance().GetUser(email).email);
            Console.In.ReadLine();
        }
    }
}