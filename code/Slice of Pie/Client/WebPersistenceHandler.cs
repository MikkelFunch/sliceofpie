using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    class WebPersistenceHandler
    {
        private static WebPersistenceHandler _instance;
        private Session session;

        private WebPersistenceHandler()
        {
            session = Session.GetInstance();
        }

        public static WebPersistenceHandler GetInstance()
        {
            if (_instance == null)
            {
                _instance = new WebPersistenceHandler();
            }
            return _instance;
        }

        /*public void SyncAllDocuments(ServiceReference.ServiceDocument[] documents)
        {

        }

        public void AddDocumentToServer(ServiceReference.Service1Client proxy, String content)
        {

        }

        public String[][] SyncDocument(FlowDocument document)
        {
            
        }

        public void SaveMergedDocument(FlowDocument document)
        {

        }*/
    }
}
