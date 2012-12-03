using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace WcfServiceLibrary
{
    class Class1
    {
        public static void Main(String[] args) {
            using (ServiceHost host = new ServiceHost(typeof(Service)))
            {
                host.Open();
            }
        }
    }
}