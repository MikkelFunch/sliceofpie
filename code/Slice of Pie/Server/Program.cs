using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            //PersistentStorage.GetInstance().AddDocumentWithUserDocument("somename", 207, 305, "[docid 0|userid 207|timestamp 13-12-2012 11:10:16|fid 305]<FlowDocument PagePadding=\"5,0,5,0\" AllowDrop=\"True\" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"><Paragraph><Run xml:lang=\"da-dk\">virk nu for satan!</Run></Paragraph><Paragraph><Run xml:lang=\"da-dk\">!</Run></Paragraph><Paragraph><Run xml:lang=\"da-dk\">!</Run></Paragraph></FlowDocument>");
            //DAO.GetInstance().GetLatestDocumentRevisions(109);
            Model.GetInstance().GetContentAsStringArray(120);
        }
    }
}