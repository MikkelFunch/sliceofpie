using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Server;

namespace WcfServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService" in both code and config file together.
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        Boolean AddUser(String email, String password);

        [OperationContract]
        void AddFolder(String name, int parentFolderId);

        [OperationContract]
        void AddDocument(String name, int userId, int folderId, String content);

        [OperationContract]
        User GetUserById(int userId);

        [OperationContract]
        User GetUserByEmail(String email);

        [OperationContract]
        Document GetDocumentById(int documentId);

        [OperationContract]
        Document GetDocumentByName(String name);

        [OperationContract]
        Folder GetFolder(int folderId);

        [OperationContract]
        void DeleteFolder(int folderId);

        [OperationContract]
        void DeleteDocumentReference(int userId, int documentId);

        [OperationContract]
        void DeleteDocument(int documentId);
    }
}