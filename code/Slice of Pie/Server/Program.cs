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
            //Model.GetInstance().GetContentAsStringArray(120);
            //String s = "[docid 132|userid 213|timestamp 14-12-2012 15:53:00|fid 318]<FlowDocument PagePadding=\"5,0,5,0\" AllowDrop=\"True\" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"><Paragraph><Run xml:lang=\"da-dk\">hej</Run></Paragraph><Paragraph><Run xml:lang=\"da-dk\">fifty</Run></Paragraph></FlowDocument>";
            //String s2 = "hej\r\nfifty\r\n";
            //PersistentStorage.GetInstance().SyncDocument(213, 132, 318, new DateTime(2012, 12, 14, 15, 53, 00), s, "b", s2.Split(new string[] { "\r\n, \n" }, StringSplitOptions.None));
            using (ServiceReference.Service1Client proxy = new ServiceReference.Service1Client())
            {
                ServiceReference.ServiceDocumentrevision[] list = proxy.GetAllDocumentRevisionsByDocumentId(1);
                ServiceReference.ServiceDocumentrevision doc = list[0];
            }
        }
    }
}