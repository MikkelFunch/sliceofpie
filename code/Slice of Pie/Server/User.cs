using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    public partial class User
    {
        public static explicit operator WcfServiceLibrary.ServiceUser(Server.User u)
        {
            WcfServiceLibrary.ServiceUser user = new WcfServiceLibrary.ServiceUser();
            user.email = u.email;
            user.id = u.id;
            user.password = u.password;
            user.rootFolderId = u.rootFolderId;
            return user;
        }

        public static explicit operator Server.User(WcfServiceLibrary.ServiceUser su)
        {
            Server.User user = new Server.User();
            user.email = su.email;
            user.id = su.id;
            user.password = su.password;
            user.rootFolderId = su.rootFolderId;
            return user;
        }
    }
}
