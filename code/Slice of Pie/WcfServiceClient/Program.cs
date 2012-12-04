using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WcfServiceLibrary;

namespace WcfServiceClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WcfServiceLibrary.ServiceClient proxy = new WcfServiceLibrary.ServiceClient())
            {
                proxy.AddUser("markasdasdt", "oajiq3");
            }
        }
    }
}
