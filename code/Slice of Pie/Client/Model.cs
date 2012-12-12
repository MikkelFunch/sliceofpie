using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Threading;

namespace Client
{
    class Model
    {
        private static Model instance;

        private Model()
        {
            RootFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\sliceofpie\\";
            UserID = -1;
        }

        /// <summary>
        /// path to the current users documents folder on the system
        /// </summary>
        private String RootFolder
        {
            get;
            set;
        }

        /// <summary>
        /// Path to currently open document
        /// including .txt
        /// </summary>
        public String CurrentDocumentPath
        {
            get;
            set;
        }

        /// <summary>
        /// Title of the currently opened document
        /// </summary>
        public String CurrentDocumentTitle
        {
            get;
            set;
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

        /// <summary>
        /// Id of the users rootfolder on server
        /// </summary>
        private static int RootFolderID
        {
            get;
            set;
        }

        public static String Email
        {
            get;
            private set;
        }

        public Boolean RegisterUser(string email, string passUnencrypted)
        {
            Boolean successful = false;
            string pass = Security.EncryptPassword(passUnencrypted);
            using (ServiceReference.Service1Client proxy = new ServiceReference.Service1Client())
            {
                successful = proxy.AddUser(email, pass);
            }
            return successful;
        }

        public int LoginUser(string email, string pass)
        {
            int id = -1;
            using (ServiceReference.Service1Client proxy = new ServiceReference.Service1Client())
            {
                id = proxy.GetUserByEmailAndPass(email, pass);
                RootFolderID = proxy.GetRootFolderId(id);
            }
            if (id != -1)
            {
                //User logged in
                UserID = id;
                Email = email;
                RootFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\sliceofpie\\" + Email;
                Directory.CreateDirectory(RootFolder);
            }
            return id;
        }

        public void CreateDocument(String title, FlowDocument document)
        {
            CurrentDocumentPath = RootFolder + "\\" + title + ".txt";
            File.Create(CurrentDocumentPath).Close();
            SaveDocument(document);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        /// <returns>
        /// 0: int docid -> docid 11
        /// 1: int userid -> userid 11
        /// 2: DateTime timestamp -> timestamp 12-12-2012 12:18:19
        /// 3: folderid -> fid 52
        /// </returns>
        public object[] RetrieveMetadata()
        {
            String content;
            using (StreamReader stream = new StreamReader(File.OpenRead(CurrentDocumentPath)))
            {
                content = stream.ReadToEnd();
            }
            if (content.StartsWith("["))
            {
                String[] metadata = content.Split(new String[] { "]" }, StringSplitOptions.None)[0].Split(new String[] { "|", "]" }, StringSplitOptions.None);

                int documentID = int.Parse(metadata[0].Substring(7));
                int userID = int.Parse(metadata[1].Substring(7));
                DateTime baseDocumentCreationTime = DateTime.Parse(metadata[2].Substring(10));
                int folderID = int.Parse(metadata[3].Substring(4));

                return new object[] { documentID, userID, baseDocumentCreationTime, folderID };
            }
            else
            { //throw en exception, file corrupted
                return null;
            }
        }

        private String GetMetadata()
        {
            object[] metadata = RetrieveMetadata();
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            sb.Append("docid " + (int)metadata[0]);
            sb.Append("|");
            sb.Append("userid " + (int)metadata[1]);
            sb.Append("|");
            sb.Append("timestamp " + (DateTime)metadata[2]);
            sb.Append("|");
            sb.Append("fid " + (int)metadata[3]);
            sb.Append("]");

            return sb.ToString();
        }

        private String GenerateMetadata()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            sb.Append("docid 0");
            sb.Append("|");
            sb.Append("userid " + UserID);
            sb.Append("|");
            sb.Append("timestamp " + DateTime.UtcNow);
            sb.Append("|");
            sb.Append("fid " + RootFolderID);
            sb.Append("]");

            return sb.ToString();
        }

        public FlowDocument CreateDocumentWithoutMetadata(String content)
        {
            content = content.Substring(content.IndexOf('<'));
            return (FlowDocument)System.Windows.Markup.XamlReader.Parse(content);
        }

        public void SaveDocument(FlowDocument document)
        {
            String metadata;

            //check if document contains metadata
            if (new TextRange(document.ContentStart, document.ContentEnd).Text.StartsWith("["))
            { //contains metadata
                metadata = GetMetadata();
            }
            else //does not containt metadata
            {
                metadata = GenerateMetadata();
            }

            StringBuilder content = new StringBuilder();
            content.Append(metadata); //metadata
            content.AppendLine(); //blank line
            content.Append(System.Windows.Markup.XamlWriter.Save(document)); //xaml

            //Create the document and write the content to it.
            using (StreamWriter sw = new StreamWriter(File.OpenWrite(CurrentDocumentPath))) //when no document has been chosen/opened, path is not set
            {
                sw.Write(content.ToString());
            }
        }

        public void DownloadComplete(BitmapImage image)
        {
            String url = image.UriSource.ToString();
            String picsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\sliceofpie\\pics";
            String fileName = Security.EncryptPassword(url) + ".jpg";
            if (File.Exists(picsPath + fileName))
            {
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();

                Directory.CreateDirectory(picsPath);
                String photolocation = picsPath + fileName; //file name

                encoder.Frames.Add(BitmapFrame.Create(image));

                using (var filestream = new FileStream(photolocation, FileMode.Create))
                    encoder.Save(filestream);
            }
        }

        public void SyncAllDocuments()
        {
            if (UserID != -1)
            {
                using (ServiceReference.Service1Client proxy = new ServiceReference.Service1Client())
                {
                    ServiceReference.ServiceDocument[] documents = proxy.GetAllDocumentsByUserId(UserID);
                    if (documents != null)
                    {
                        foreach (ServiceReference.ServiceDocument currentDoc in documents)
                        {
                            int index = currentDoc.path.IndexOf("sliceofpie");
                            String path = currentDoc.path.Substring(index);
                            String fullPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + path;
                            if (!File.Exists(fullPath + "\\" + currentDoc.name))
                            {
                                Directory.CreateDirectory(fullPath);
                                using (FileStream stream = File.Create(fullPath + "\\" + currentDoc.name + ".txt"))
                                {
                                    String content = proxy.GetDocumentContent(currentDoc.path, currentDoc.name);
                                    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                                    byte[] byteContent = encoding.GetBytes(content);
                                    stream.Write(byteContent, 0, byteContent.Length);
                                }
                            }
                            else
                            {
                                //Document already exits.
                                //Make user single sync this document.
                            }
                        }
                    }
                    else
                    {
                        //No documents found by this userId
                    }
                }
            }
            else
            {
                //not logged in
            }
        }

        public void SyncDocument(FlowDocument document)
        {
            //Metadata
            //0: docid -> docid 11
            //1: userid -> userid 11
            //2: timestamp -> timestamp 12-12-2012 12:18:19
            object[] metadata = RetrieveMetadata();

            int documentID = (int)metadata[0];
            DateTime baseDocumentCreationTime = (DateTime)metadata[2];
            
            StringBuilder sb = new StringBuilder();
            sb.Append(GetMetadata());
            sb.AppendLine();
            sb.Append(System.Windows.Markup.XamlWriter.Save(document));


            using (ServiceReference.Service1Client proxy = new ServiceReference.Service1Client())
            {
                ServiceReference.ServiceDocumentrevision responseDocument = proxy.SyncDocument(UserID, documentID, RootFolderID, baseDocumentCreationTime, sb.ToString(), CurrentDocumentTitle);
                if (responseDocument == null)
                {
                    //save document with new metadata - basedocument
                    SaveDocument(document);
                }
                else
                {
                    //conflict
                    //merge da shiat
                }
            }
        }
    }
}
