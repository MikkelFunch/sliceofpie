using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    class Model
    {
        private MainWindow mw;

        public Model(MainWindow mw)
        {
            this.mw = mw;
        }

        public static int UserID
        {
            get;
            set;
        }

        public void LoginSuccessful(int UserID)
        {

        }
    }
}
