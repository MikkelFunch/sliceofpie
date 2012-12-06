using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    public partial class Folder
    {
        public static explicit operator WcfServiceLibrary.ServiceFolder(Server.Folder f)
        {
            WcfServiceLibrary.ServiceFolder folder = new WcfServiceLibrary.ServiceFolder();
            folder.id = f.id;
            folder.name = f.name;
            folder.parentFolderId = f.parentFolderId;
            return folder;
        }

        public static explicit operator Server.Folder(WcfServiceLibrary.ServiceFolder sf)
        {
            Server.Folder folder = new Server.Folder();
            folder.id = sf.id;
            folder.name = sf.name;
            folder.parentFolderId = sf.parentFolderId;
            return folder;
        }
    }
}
