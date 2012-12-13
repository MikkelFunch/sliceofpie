using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    public partial class User
    {
        public static explicit operator ServiceReference.ServiceUser(Server.User u)
        {
            ServiceReference.ServiceUser user = new ServiceReference.ServiceUser();
            user.email = u.email;
            user.id = u.id;
            user.password = u.password;
            user.rootFolderId = u.rootFolderId;
            return user;
        }

        public static explicit operator Server.User(ServiceReference.ServiceUser su)
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
