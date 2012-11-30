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
                Folder folder = new Folder();
                //folder.id = 1;
                folder.name = "FolderNameMikkel";
                //folder.parentFolderId = 1;
                context.Folders.AddObject(folder);
                context.SaveChanges();
            }
        }
    }
}
