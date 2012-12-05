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

            //Controller.GetInstance().AddDocument("YoMommaDoc", 28, 82, "This is some dope ass content!");
            Controller.GetInstance().AddDocumentRevision(29, 12, "This is some new content");
            //Console.Out.WriteLine(Controller.GetInstance().GetUser(email).email);
        }
    }
}