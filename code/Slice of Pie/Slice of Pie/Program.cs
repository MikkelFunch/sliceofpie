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

            using (PieFactoryEntities context = new PieFactoryEntities())
            {
                //Delete all folders
                var folders = context.Folders;
                foreach (Folder f in folders)
                {
                    context.DeleteObject(f);
                }
                context.SaveChanges();

                //Delete all users
                var users = context.Users;

            }
            String email = "Email";
            String password = "Password";
            Controller.GetInstance().AddUser(email, password);

        }
    }
}
