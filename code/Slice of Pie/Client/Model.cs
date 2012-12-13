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
        public String RootFolder
        {
            get;
            private set;
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

        public int RegisterUser(string email, string passUnencrypted)
        {
            int successful = -1;
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
            }
            if (id != -1)
            {
                //User logged in
                UserID = id;
                Email = email;
                RootFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\sliceofpie\\" + Email;
                Directory.CreateDirectory(RootFolder);

                using (ServiceReference.Service1Client proxy = new ServiceReference.Service1Client())
                {
                    RootFolderID = proxy.GetRootFolderId(id);
                }
            }
            return id;
        }

        public void CreateDocument(String title, FlowDocument document)
        {
            CurrentDocumentPath = RootFolder + "\\" + title + ".txt";
            File.Create(CurrentDocumentPath).Close();
            SaveDocumentToFile(document);
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
            return RetrieveMetadataFromFile(CurrentDocumentPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">Path to the file for which you want metadata</param>
        /// <returns></returns>
        public object[] RetrieveMetadataFromFile(String path)
        {
            String content;
            using (StreamReader stream = new StreamReader(File.OpenRead(path)))
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


        private Object[] RetrieveMetadataFromFile(String directorypath, String filename)
        {
            return (RetrieveMetadataFromFile(directorypath + "\\" + filename + ".txt"));
        }

        private String GetMetadataFromObjectArray(Object[] metadata)
        {
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

        public FlowDocument CreateFlowDocumentWithoutMetadata(String content)
        {
            content = content.Substring(content.IndexOf('<'));
            return (FlowDocument)System.Windows.Markup.XamlReader.Parse(content);
        }

        public void SaveDocumentToFile(FlowDocument document, String metadata)
        {
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

        /// <summary>
        /// When a file is being saved
        /// </summary>
        /// <param name="document"></param>
        public void SaveDocumentToFile(FlowDocument document)
        {
            Object[] metadata = RetrieveMetadata();
            String metadataString;
            if(metadata != null)
                metadataString = GetMetadataFromObjectArray(metadata);
            else
                metadataString = GenerateMetadata();
            SaveDocumentToFile(document, metadataString);
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
                            {//file does not exsits - save file locally
                                
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
                                //currentDoc.creationTime;
                                //get local corresponsing document




                                //check if it is same base
                                //if it is -> check if online version is newer
                                //// if it is -> user online version
                                //// if it is not -> sync local version
                                //if it is not -> single sync the document
                                //Document already exits.
                                //Make user single sync this document.
                            }
                        }
                    }
                    else
                    {
                        //No documents found by this userId
                        List<String> files = new List<String>();
                        foreach (String file in Directory.GetFiles(RootFolder))
                        {
                            AddDocumentToServer(proxy, RootFolder, file);
                        }

                        foreach (String dir in Directory.GetDirectories(RootFolder))
                        {
                            foreach (String file in Directory.GetFiles(dir))
                            {
                                AddDocumentToServer(proxy, dir, file);
                            }
                        }
                    }
                }
            }
            else
            {
                //not logged in
            }
        }

        private void AddDocumentToServer(ServiceReference.Service1Client proxy, String dir, String file)
        {
            String content;
            using (StreamReader reader = new StreamReader(file))
            {
                content = reader.ReadToEnd();
            }
            Object[] metadata = RetrieveMetadataFromFile(file);
            int index = file.IndexOf(dir + "\\");
            String filename = file.Substring(file.LastIndexOf("\\") + 1, (file.IndexOf(".txt") - file.LastIndexOf("\\") -1));
            proxy.AddDocumentWithUserDocument(filename, UserID, (int)metadata[3], content);
        }

        public String[][] SyncDocument(FlowDocument document)
        {
            //Metadata
            //0: docid -> docid 11
            //1: userid -> userid 11
            //2: timestamp -> timestamp 12-12-2012 12:18:19
            object[] metadata = RetrieveMetadata();

            int documentID = (int)metadata[0];
            DateTime baseDocumentCreationTime = (DateTime)metadata[2];

            StringBuilder sb = new StringBuilder();
            String newMetadata = GenerateNewMetadata(documentID, UserID, RootFolderID);
            sb.Append(newMetadata);
            sb.AppendLine();
            String content = System.Windows.Markup.XamlWriter.Save(document);
            sb.Append(content);

            using (ServiceReference.Service1Client proxy = new ServiceReference.Service1Client())
            {
                String pureContent = new TextRange(document.ContentStart, document.ContentEnd).Text;
                String[][] responseArrays = proxy.SyncDocument(UserID, documentID, RootFolderID, baseDocumentCreationTime, sb.ToString(), CurrentDocumentTitle, pureContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None));
                if (responseArrays == null)
                {
                    if (documentID == 0)
                    {
                        documentID = proxy.GetDocumentId(UserID, CurrentDocumentTitle);
                    }
                    //save document with new metadata - basedocument
                    SaveDocumentToFile(document, ReplaceDocumentIDInMetadata(newMetadata,documentID));
                }
                else
                {
                    return responseArrays;
                }
            }
            return null;
        }

        private String ReplaceDocumentIDInMetadata(String metadata, int documentid)
        {
            int startIndex = metadata.IndexOf("docid") + 5;
            int endIndex = metadata.IndexOf("|");
            metadata = metadata.Remove(startIndex, endIndex - startIndex);
            metadata = metadata.Insert(startIndex, " " + documentid.ToString());
            return metadata;
        }

        private String GenerateNewMetadata(int docid, int userid, int folderid)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            sb.Append("docid " + docid);
            sb.Append("|");
            sb.Append("userid " + userid);
            sb.Append("|");
            sb.Append("timestamp " + DateTime.UtcNow);
            sb.Append("|");
            sb.Append("fid " + folderid);
            sb.Append("]");

            return sb.ToString();
        }

        private String GenerateMetadataString(int docid, int userid, int folderid, DateTime timestamp)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            sb.Append("docid " + docid);
            sb.Append("|");
            sb.Append("userid " + userid);
            sb.Append("|");
            sb.Append("timestamp " + timestamp);
            sb.Append("|");
            sb.Append("fid " + folderid);
            sb.Append("]");

            return sb.ToString();
        }


        public void LogoutUser()
        {
            RootFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\sliceofpie\\";
            UserID = -1;
        }

        public void SaveMergedDocument(FlowDocument document)
        {
            Object[] oldMetadata = RetrieveMetadataFromFile(CurrentDocumentPath);
            int documentid = (int)oldMetadata[0];
            int userid = UserID;
            DateTime timestamp = DateTime.UtcNow;
            int folderid = (int)oldMetadata[3];

            String metadata = GenerateMetadataString(documentid, userid, folderid, timestamp);
            String xamlContent = System.Windows.Markup.XamlWriter.Save(document);
            String content = metadata + xamlContent;

            using (ServiceReference.Service1Client proxy = new ServiceReference.Service1Client())
            {
                proxy.AddDocumentRevision(UserID, documentid, content);
            }
        }

        public void CreateFolder(string folderName, string path)
        {
            if (path == null)
            {
                Directory.CreateDirectory(RootFolder + "\\" + folderName);
            }
            else
            {
                Directory.CreateDirectory(path + "\\" + folderName);
            }
        }
    }
}
