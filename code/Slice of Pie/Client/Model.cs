using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    class Model
    {
        private static Model instance;

        private Model()
        {
            UserID = 1;
        }

        public static Model GetInstance()
        {
            if (instance == null)
            {
                instance = new Model();
            }
            return instance;
        }

        public static int UserID
        {
            get;
            set;
        }

        public Boolean RegisterUser(string email, string passUnencrypted)
        {
            Boolean successful = false;
            string pass = Security.EncryptPassword(passUnencrypted);
            using (WcfServiceReference.ServiceClient proxy = new WcfServiceReference.ServiceClient())
            {
                successful = proxy.AddUser(email, pass);
            }
            return successful;
        }

        public int LoginUser(string email, string pass)
        {
            int id = -1;
            using (WcfServiceReference.ServiceClient proxy = new WcfServiceReference.ServiceClient())
            {
                id = proxy.GetUserByEmailAndPass(email, pass);
            }
            if (id != -1)
            {
                //User logged in
                UserID = id;
            }
            return id;
        }
        /*
        public Boolean DocumentExists(int documentId)
        {
            using (WcfServiceReference.ServiceClient proxy = new WcfServiceReference.ServiceClient())
            {
                return proxy.GetDocumentById(documentId).id != 0;
            }
        }

        public void SaveDocument(String name, int folderId, String content)
        {
            using (WcfServiceReference.ServiceClient proxy = new WcfServiceReference.ServiceClient())
            {
                proxy.AddDocument(name, UserID, folderId, content);
            }
        }

        public void SaveDocumentRevision(int int documentId, String content)
        {
            using (WcfServiceReference.ServiceClient proxy = new WcfServiceReference.ServiceClient())
            {
                proxy.AddDocumentRevision(editorId, documentId, content);
            }
        }
        */
    }
}
