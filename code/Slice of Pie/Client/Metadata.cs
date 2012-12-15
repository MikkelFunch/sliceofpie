using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.IO;

namespace Client
{
    static class Metadata
    {
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
        /*public static object[] RetrieveMetadata()
        {
            return RetrieveMetadataFromFile(CurrentDocumentPath);
        }*/

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

        public static Object[] RetrieveMetadataFromFile(String directoryPath, String filename)
        {
            return (RetrieveMetadataFromFile(directoryPath + "\\" + filename + ".txt"));
        }

        /// <summary>
        /// Generate metadata string from an object array containing metadata
        /// </summary>
        /// <param name="metadata">an object array containing metadata: [0](int)documentid,[1](int)userid,[2](DateTime)timestamp,[3](int)folderid</param>
        /// <returns></returns>
        public static String GetMetadataStringFromObjectArray(Object[] metadata)
        {
            return GenerateMetadataString((int)metadata[0], (int)metadata[1], (DateTime)metadata[2]);//, (int)metadata[3]);
        }

        /// <summary>
        /// Generates a string containing metadata for a newly created file on the local system.
        /// The data will be default to:
        /// <para>documentid: 0 </para>
        /// <para>userid: Currently online user</para>
        /// <para>timestamp: current time</para>
        /// <para>folderid: 0</para>
        /// Example return: [docid 0|userid 42|timestamp 13-12-2012 20:52:04|fid 0]
        /// </summary>
        /// <returns>Returns a metadata string for a completely new document in a format ready to be placed into a file</returns>
        public static String GenerateMetadataStringForNewFile()
        {
            return GenerateMetadataString(0, Session.GetInstance().UserID, DateTime.UtcNow);//, -1);
        }

        /// <summary>
        /// Generate a metadata string from raw data
        /// </summary>
        /// <param name="docid">Document id which will be inserted</param>
        /// <param name="userid">User id which will be inserted</param>
        /// <param name="timestamp">Timestamp which will be inserted</param>
        /// <param name="folderid">Folder id which will be inserted</param>
        /// <returns>Returns a metadata string containing the given data</returns>
        public static String GenerateMetadataString(int docid, int userid, DateTime timestamp)//, int folderid)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            sb.Append("docid " + docid);
            sb.Append("|");
            sb.Append("userid " + userid);
            sb.Append("|");
            sb.Append("timestamp " + timestamp);
            //sb.Append("|");
            //sb.Append("fid " + folderid);
            sb.Append("]");

            return sb.ToString();
        }

        /// <summary>
        /// Replace the document id in the given metadata
        /// </summary>
        /// <param name="metadata">A metadata string</param>
        /// <param name="documentid">The document id which should be inserted</param>
        /// <returns>Returns a metadata string with the new document id</returns>
        public static String ReplaceDocumentIDInMetadata(String metadata, int documentid)
        {
            int startIndex = metadata.IndexOf("docid") + 5;
            int endIndex = metadata.IndexOf("|");
            metadata = metadata.Remove(startIndex, endIndex - startIndex);
            metadata = metadata.Insert(startIndex, " " + documentid.ToString());
            return metadata;
        }

        public static String RemoveMetadataFromFileContent(string fileContent)
        {
            return fileContent.Substring(fileContent.IndexOf('<'));
        }

        public static int FetchDocumentIDFromFileContent(string fileContent)
        {
            int indexStart = fileContent.IndexOf("docid") + 5;
            int indexEnd = fileContent.IndexOf("|");
            int id = int.Parse(fileContent.Substring(indexStart,indexEnd - indexStart));
            return id;
        }
    }
}