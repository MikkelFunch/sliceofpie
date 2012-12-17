using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.IO;

namespace Web_Solution
{
    static class Metadata
    {
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