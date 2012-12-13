using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    public partial class Document
    {
        public static explicit operator ServiceReference.ServiceDocument(Document d)
        {
            ServiceReference.ServiceDocument doc = new ServiceReference.ServiceDocument();
            doc.creationTime = d.creationTime;
            doc.creatorId = d.creatorId;
            doc.id = d.id;
            doc.name = d.name;
            doc.path = d.path;
            return doc;
        }

        public static explicit operator Server.Document(ServiceReference.ServiceDocument sd)
        {
            Server.Document doc = new Server.Document();
            doc.creationTime = sd.creationTime;
            doc.creatorId = sd.creatorId;
            doc.id = sd.id;
            doc.name = sd.name;
            doc.path = sd.path;
            return doc;
        }
    }
}