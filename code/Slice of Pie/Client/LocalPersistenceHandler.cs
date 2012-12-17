using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.IO;
using System.Windows.Media.Imaging;

namespace Client
{
    class LocalPersistenceHandler
    {
        private static LocalPersistenceHandler _instance;
        private Session session;

        private LocalPersistenceHandler()
        {
            session = Session.GetInstance();
        }

        public static LocalPersistenceHandler GetInstance()
        {
            if (_instance == null)
            {
                _instance = new LocalPersistenceHandler();
            }
            return _instance;
        }

        /// <summary>
        /// Create a new file for a new document when it is created
        /// </summary>
        /// <param name="documentTitle"></param>
        /// <param name="document"></param>
        public void CreateNewDocumentFile(string documentTitle, FlowDocument document)
        {
            session.CurrentDocumentPath = session.RootFolderPath + "\\" + documentTitle + ".txt";
            File.Create(session.CurrentDocumentPath).Close();
            String metadata = Metadata.GenerateMetadataStringForNewFile();
            SaveDocumentToFile(document, metadata);
        }

        /// <summary>
        /// Save a flowdocument to a file.
        /// </summary>
        /// <param name="document">Document to be saved</param>
        public void SaveDocumentToFile(FlowDocument document)
        {
            Object[] metadata = RetrieveMetadataFromFile(session.CurrentDocumentPath);
            String metadataString;
            if (metadata != null)
                metadataString = Metadata.GetMetadataStringFromObjectArray(metadata);
            else
                metadataString = Metadata.GenerateMetadataStringForNewFile();
            SaveDocumentToFile(document, metadataString);
        }

        /// <summary>
        /// Saves to the current document path in session - currently opened document
        /// </summary>
        /// <param name="document"></param>
        /// <param name="metadata"></param>
        public void SaveDocumentToFile(FlowDocument document, string metadata)
        {
            //build the file content
            StringBuilder content = new StringBuilder();
            content.Append(metadata); //insert metadata
            content.AppendLine(); //blank line
            content.Append(System.Windows.Markup.XamlWriter.Save(document)); //insert xaml


            SaveDocumentToFile(content.ToString(), session.CurrentDocumentPath);
        }

        public void SaveDocumentToFile(string content, string path)
        {
            //Create the file and write the content to it.
            using (StreamWriter sw = new StreamWriter(path, false)) //when no document has been chosen/opened, path is not set
            {
                sw.Write(content.ToString());
            }
        }



        /// <summary>
        /// Creates a folder with the given name at at the given path
        /// If path is null, the folder is created in the users root folder
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="path"></param>
        public void CreateFolder(string folderName, string path)
        {
            if (path == null)
            {
                Directory.CreateDirectory(session.RootFolderPath + "\\" + folderName);
            }
            else
            {
                Directory.CreateDirectory(path + "\\" + folderName);
            }
        }

        /// <summary>
        /// Create a flowdocument directly from a files content
        /// </summary>
        /// <param name="content">The raw file content, containing metadata and xaml</param>
        /// <returns>A flowdocument created from the content, without the metadata</returns>
        public FlowDocument CreateFlowDocumentWithoutMetadata(String content)
        {
            content = content.Substring(content.IndexOf('<'));
            return (FlowDocument)System.Windows.Markup.XamlReader.Parse(content);
        }

        /// <summary>
        /// Get metadata + xamlcontent from the given filepath
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string GetFileContent(string filePath)
        {
            String content = "";
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(File.OpenRead(filePath)))
                {
                    content = reader.ReadToEnd();
                }
            }

            return content;
        }

        public void ReplaceMetadataStringInFile(string filePath, string metadata)
        {
            string fileContent = GetFileContent(filePath);
            fileContent = fileContent.Substring(fileContent.IndexOf("<"));
            SaveDocumentToFile(metadata + fileContent, filePath);
        }

        public void MoveFileToFolder(string fromPath, string toPath)
        {
            File.Move(fromPath, toPath);
            Object[] metadataArray = RetrieveMetadataFromFile(toPath);
            String metadataString = Metadata.GetMetadataStringFromObjectArray(metadataArray);
            ReplaceMetadataStringInFile(toPath, metadataString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">Path to the file for which you want metadata</param>
        /// <returns></returns>
        public static object[] RetrieveMetadataFromFile(String path)
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

                return new object[] { documentID, userID, baseDocumentCreationTime };
            }
            else
            { //throw en exception, file corrupted
                return null;
            }
        }

        /// <summary>
        /// Retreive the local metadata from the given file
        /// </summary>
        /// <param name="directoryPath">Path to the files directoy</param>
        /// <param name="filename">The filename withouth .txt ending</param>
        /// <returns>Object array containing document id, user id and timestamp</returns>
        public static Object[] RetrieveMetadataFromFile(String directoryPath, String filename)
        {
            return (RetrieveMetadataFromFile(directoryPath + "\\" + filename + ".txt"));
        }

        /// <summary>
        /// Delete file at given filepath
        /// </summary>
        /// <param name="filePath">Path to the file which should be deleted</param>
        public void DeleteFile(string filePath)
        {
            File.Delete(filePath);
        }

        /// <summary>
        /// Method for handling newly donwloaded images. The images are stored in the local file system and URL's added to the containing document.
        /// </summary>
        /// <param name="image">BitmapImage which have been fully donwloaded</param>
        public void DownloadImage(BitmapImage image)
        {
            String url = image.UriSource.ToString();
            String picsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\sliceofpie\\pics\\";
            int indexStart = url.LastIndexOf('/') + 1;
            String fileName = url.Substring(indexStart);
            if (!File.Exists(picsPath + fileName))
            {
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();

                Directory.CreateDirectory(picsPath);
                String photolocation = picsPath + fileName; //file name

                encoder.Frames.Add(BitmapFrame.Create(image));

                using (var filestream = new FileStream(photolocation, FileMode.Create))
                {
                    encoder.Save(filestream);
                }
            }
        }
    }
}
