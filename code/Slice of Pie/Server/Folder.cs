using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    public partial class Folder
    {
        public static explicit operator ServiceReference.ServiceFolder(Server.Folder f)
        {
            ServiceReference.ServiceFolder folder = new ServiceReference.ServiceFolder();
            folder.id = f.id;
            folder.name = f.name;
            folder.parentFolderId = f.parentFolderId;
            return folder;
        }

        public static explicit operator Server.Folder(ServiceReference.ServiceFolder sf)
        {
            Server.Folder folder = new Server.Folder();
            folder.id = sf.id;
            folder.name = sf.name;
            folder.parentFolderId = sf.parentFolderId;
            return folder;
        }
    }
}
