using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Threading;

namespace Client
{
    class Model
    {
        private static Model instance;

        private Model()
        {

        }

        public static Model GetInstance()
        {
            if (instance == null)
            {
                instance = new Model();
            }
            return instance;
        }
    }
}
