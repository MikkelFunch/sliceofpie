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
            Folder folder = Facade.GetInstance().GetFolder(folderId);
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
                folder = Facade.GetInstance().GetFolder((int)folder.parentFolderId);
            }
            String userEmail = Facade.GetInstance().GetUser(userId).email;
            sb.Insert(0, "D:\\SliceOfPieDocuments\\sliceofpie\\" + userEmail);
            return sb.ToString();
        }

        public void WriteToFile(String directoryPath, String filename, String content)
        {
            String filepath = directoryPath + "\\" + filename + ".txt";
            if (!File.Exists(directoryPath))
            {
                CreateDirectory(directoryPath);
                CreateFile(filepath);
            }
            using(StreamWriter sw = new StreamWriter(File.OpenWrite(filepath)))
                {
                    sw.Write(content);
                }
        }

        public void CreateFile(String filePath)
        {
            File.Create(filePath).Close();
        }

        public void CreateDirectory(String directoryPath)
        {
            Directory.CreateDirectory(directoryPath);
        }
    }
}
