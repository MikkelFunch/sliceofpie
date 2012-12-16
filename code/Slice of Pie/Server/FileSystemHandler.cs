using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Server
{
    class FileSystemHandler
    {
        //Singleton instance of FileSystemHandler
        private static FileSystemHandler instance;

        /// <summary>
        /// Private constructor to ensure that FileSystemHandler is not created outside this class
        /// </summary>
        private FileSystemHandler() { }

        /// <summary>
        /// Accessor method for accessing the single instance of FileSystemHandler.
        /// </summary>
        /// <returns>The only instance of FileSystemHandler</returns>
        public static FileSystemHandler GetInstance()
        {
            if (instance == null)
            {
                instance = new FileSystemHandler();
            }
            return instance;
        }

        public String GetDocumentPath(int userId, int folderId)
        {
            StringBuilder sb = new StringBuilder();
            Folder folder = PersistentStorage.GetInstance().GetFolder(folderId);
            while (folder != null || folder.parentFolderId != null)
            {
                if (folder.parentFolderId == null)
                {
                    break;
                }
                else
                {
                    sb.Insert(0, "\\" + folder.name);
                }
                folder = PersistentStorage.GetInstance().GetFolder((int)folder.parentFolderId);
            }
            String userEmail = PersistentStorage.GetInstance().GetUserById(userId).email;
            sb.Insert(0, "D:\\SliceOfPieDocuments\\sliceofpie\\" + userEmail);
            return sb.ToString();
        }

        /// <summary>
        /// Saves a file to the server file system as a txt file with the specified content and the documentId as metadata.
        /// </summary>
        /// <param name="filepath">The file path to point to the document</param>
        /// <param name="content">The textual content of the document</param>
        /// <param name="documentId">The docId to place in metadata</param>
        public void WriteToFile(String filepath, String content, int documentId)
        {
            int directoryLength = filepath.LastIndexOf("\\");
            String directoryPath = filepath.Substring(0, directoryLength);
            if (!File.Exists(filepath))
            {
                CreateDirectory(directoryPath);
                CreateFile(filepath);
            }
            int startIndex = content.IndexOf("docid") + 5;
            int endIndex = content.IndexOf("|");
            content = content.Remove(startIndex, endIndex - startIndex);
            content = content.Insert(startIndex, " " + documentId.ToString());
            using (StreamWriter sw = new StreamWriter(filepath))
            {
                sw.Write(content);
            }
        }

        /// <summary>
        /// Creates a file at the location that the path specifies. The file to be created is the end of the file path.
        /// </summary>
        /// <param name="filePath">The file path specifying the location and the file name</param>
        public void CreateFile(String filePath)
        {
            File.Create(filePath).Close();
        }
        
        /// <summary>
        /// Creates a directory at the location that the path specifies.
        /// </summary>
        /// <param name="directoryPath">The directory path specifying the location and the directory name</param>
        public void CreateDirectory(String directoryPath)
        {
            Directory.CreateDirectory(directoryPath);
        }
        
        /// <summary>
        /// Gets the textual content of the file specified with the file name at in the directory specified by the directory path.
        /// </summary>
        /// <param name="directoryPath">The path specifying the directory</param>
        /// <param name="filename">The file to get content from</param>
        /// <returns></returns>
        public String GetDocumentContent(String directoryPath, String filename)
        {
            return GetDocumentContent(directoryPath + "\\" + filename + ".txt");
        }

        /// <summary>
        /// Gets the textual content of the file specified with the filepath.
        /// </summary>
        /// <param name="filepath">The filepath pointing to the file</param>
        /// <returns>The textual content of the file</returns>
        public string GetDocumentContent(string filepath)
        {
            String content = null;
            using (StreamReader reader = new StreamReader(filepath))
            {
                content = reader.ReadToEnd();
            }
            return content;
        }
    }
}
