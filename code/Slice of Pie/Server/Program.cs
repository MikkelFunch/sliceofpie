using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceReference.Service1Client proxy = new ServiceReference.Service1Client())
            {
                proxy.SyncDocument(202, 0, 300, DateTime.UtcNow, "<FlowDocument PagePadding=\"5,0,5,0\" AllowDrop=\"True\" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"><Paragraph><Run xml:lang=\"da-dk\">vir</Run></Paragraph></FlowDocument>", "Works three");
            }
        }
    }
}