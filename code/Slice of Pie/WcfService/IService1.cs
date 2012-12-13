using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Windows.Documents;

namespace WcfService
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        int AddUser(String email, String password);

        [OperationContract]
        int AddFolder(String name, int parentFolderId);

        [OperationContract]
        int AddDocumentWithUserDocument(String name, int userId, int folderId, String content);

        [OperationContract]
        void AddDocumentRevision(int editorId, int documentId, String content);

        [OperationContract]
        void SaveMergedDocument(int editorId, int documentId, String content);

        [OperationContract]
        int GetUserByEmailAndPass(String email, String pass);

        [OperationContract]
        ServiceUser GetUserById(int userId);

        [OperationContract]
        ServiceUser GetUserByEmail(String email);

        [OperationContract]
        ServiceFolder GetFolder(int folderId);

        [OperationContract]
        int GetRootFolderId(int userId);

        [OperationContract]
        ServiceDocument GetDocumentById(int documentId);

        [OperationContract]
        void DeleteFolder(int folderId);

        [OperationContract]
        void DeleteDocumentReference(int userId, int documentId);

        [OperationContract]
        void DeleteDocument(int documentId);

        [OperationContract]
        List<ServiceDocument> GetAllDocumentsByUserId(int userId);

        [OperationContract]
        String GetDocumentContent(String directoryPath, String filename);

        [OperationContract]
        String[][] SyncDocument(int editorId, int documentId, int folderId, DateTime baseDocCreationTime, String content, String title, String[] original);

        [OperationContract]
        int GetDocumentId(int userId, String title);
    }

    [DataContract]
    public class ServiceDocument
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string path { get; set; }

        [DataMember]
        public DateTime creationTime { get; set; }

        [DataMember]
        public int creatorId { get; set; }

        public static explicit operator ServiceDocument(Server.Document d)
        {
            ServiceDocument doc = new ServiceDocument();
            doc.creationTime = d.creationTime;
            doc.creatorId = d.creatorId;
            doc.id = d.id;
            doc.name = d.name;
            doc.path = d.path;
            return doc;
        }

        public static explicit operator Server.Document(ServiceDocument sd)
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

    [DataContract]
    public class ServiceDocumentrevision
    {
        public int id { get; set; }

        public int documentId { get; set; }

        public DateTime creationTime { get; set; }

        public String path { get; set; }

        public int editorId { get; set; }

        public static explicit operator ServiceDocumentrevision(Server.Documentrevision dr)
        {
            ServiceDocumentrevision doc = new ServiceDocumentrevision();
            doc.creationTime = dr.creationTime;
            doc.documentId = dr.documentId;
            doc.editorId = dr.editorId;
            doc.id = dr.id;
            doc.path = dr.path;
            return doc;
        }

        public static explicit operator Server.Documentrevision(ServiceDocumentrevision sdr)
        {
            Server.Documentrevision doc = new Server.Documentrevision();
            doc.creationTime = sdr.creationTime;
            doc.documentId = sdr.documentId;
            doc.editorId = sdr.editorId;
            doc.id = sdr.id;
            doc.path = sdr.path;
            return doc;
        }
    }

    [DataContract]
    public class ServiceUser
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public String email { get; set; }

        [DataMember]
        public String password { get; set; }

        [DataMember]
        public int rootFolderId { get; set; }

        public static explicit operator ServiceUser(Server.User u)
        {
            ServiceUser user = new ServiceUser();
            user.email = u.email;
            user.id = u.id;
            user.password = u.password;
            user.rootFolderId = u.rootFolderId;
            return user;
        }

        public static explicit operator Server.User(ServiceUser su)
        {
            Server.User user = new Server.User();
            user.email = su.email;
            user.id = su.id;
            user.password = su.password;
            user.rootFolderId = su.rootFolderId;
            return user;
        }
    }

    [DataContract]
    public class ServiceFolder
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public int? parentFolderId { get; set; }

        public static explicit operator ServiceFolder(Server.Folder f)
        {
            ServiceFolder folder = new ServiceFolder();
            folder.id = f.id;
            folder.name = f.name;
            folder.parentFolderId = f.parentFolderId;
            return folder;
        }

        public static explicit operator Server.Folder(ServiceFolder sf)
        {
            Server.Folder folder = new Server.Folder();
            folder.id = sf.id;
            folder.name = sf.name;
            folder.parentFolderId = sf.parentFolderId;
            return folder;
        }
    }
}