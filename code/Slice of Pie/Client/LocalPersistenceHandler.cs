using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.IO;

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
        /// IS THIS ONE NECESSARY? MAKE A SINGLE METHOD OUT OF THIS ONE AND SaveDocumentToFile(FlowDocument document, string metadata) MAYBE
        /// Save a flowdocument to root folder
        /// </summary>
        /// <param name="document"></param>
        public void SaveDocumentToFile(FlowDocument document)
        {
            Object[] metadata = Metadata.RetrieveMetadataFromFile(session.CurrentDocumentPath);
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

            //Create the file and write the content to it.
            /*using (StreamWriter sw = new StreamWriter(File.OpenWrite(session.CurrentDocumentPath))) //when no document has been chosen/opened, path is not set
            {
                sw.Write(content.ToString());
            }*/
        }

        public void SaveDocumentToFile(string content, string path)
        {
            //Create the file and write the content to it.
            using (StreamWriter sw = new StreamWriter(File.OpenWrite(path))) //when no document has been chosen/opened, path is not set
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


        /////////////////////////////////////////////////////////////////// HØRER DEN HER TIL I METADATA ELLER HER!?!??!?!
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
        /// Get content from the given filepath
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
    }
}
