using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    interface IPersitence
    {
        void RegisterUser();
        void LoginUser();
        void Logout();

        void CreateDocument();
        void SaveDocumentToFile();

        void RetrieveMetaData();
        void GetMetaData();
        void GenerateMetaData();
        void GenerateNewMetaData();

        void CreateFlowDocumenWithoutMetadata();

        void SyncDocument();
        void SyncAllDocuments();

        //void DownloadComplete();
    }
}
