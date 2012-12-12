using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace Server
{
    public class Model
    {
        //Singleton instance of model
        private static Model instance;

        /// <summary>
        /// Private constructor to insure that Model is not created outside this class.
        /// </summary>
        private Model()
        {
        }

        /// <summary>
        /// Accessor method for accessing the single instance of model.
        /// </summary>
        /// <returns>The only instance of model</returns>
        public static Model GetInstance()
        {
            if(instance == null)
            {
                instance = new Model();
            }
            return instance;
        }

        /// <summary>
        /// Merges two documents while prioritizing the latest.
        /// </summary>
        /// <param name="original">The document prior to the latest</param>
        /// <param name="latest">The latest version of the document</param>
        /// <returns>The merged version of two documents</returns>
        public String[][] MergeDocuments(String[] original, String[] latest)
        {
            //index of the original document
            int o = 0;
            //index of the latest document
            int n = 0;
            //The array which contains the resukting string arrays.
            String[][] result = new String[3][];
            //The final merged version
            List<String> merged = new List<String>();
            //Array containing trace of deletions.
            String[] deletions = new String[original.Length];
            //Array containing trace of insertions.
            String[] insertions = new String[latest.Length];

            do
            {
                if (IsEndOfDocument(original, o) || IsEndOfDocument(latest, n))
                {
                    //Remaining lines are new in the latest document. Ends loop.
                    if (IsEndOfDocument(original, o) && !IsEndOfDocument(latest, n))
                    {
                        fillIntervalWithText(insertions, n, latest.Length-1, "i");

                        AppendLines(merged, latest, n, LastIndexOf(latest));
                        n = LastIndexOf(latest) + 1;
                    }

                    //Remaining lines are deleted in the latest document. Ends loop.
                    if (!IsEndOfDocument(original, o) && IsEndOfDocument(latest, n))
                    {
                        fillIntervalWithText(deletions, o, original.Length-1, "d");

                        o = LastIndexOf(original) + 1;
                    }
                }
                else
                {
                    //The two lines are equal, hence it has not changed.
                    if (original[o] == latest[n])
                    {
                        merged.Add(latest[n]);
                        o++;
                        n++;
                    }
                    //The two lines are not equal, investigate whether it is removed or lines have been inserted.
                    else if (original[o] != latest[n])
                    {
                        int? i = FindAppearanceOfLine(latest, original[o], n + 1, LastIndexOf(latest));
                        if (i == null)
                        {
                            deletions[o] = "d";
                            o++;
                        }
                        else
                        {
                            fillIntervalWithText(insertions, n, (int)i - 1, "i");
                            AppendLines(merged, latest, n, (int)i - 1);
                            n = (int)i;
                            //o++;
                        }
                    }
                }
            }
            while (!(IsEndOfDocument(original, o) && IsEndOfDocument(latest, n)));
                  
            result[0] = merged.ToArray();
            result[1] = insertions;
            result[2] = deletions;

            return result;
        }
        
        private void fillIntervalWithText(String[] s, int start, int end, string text)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (i >= start && i <= end)
                {
                    s[i] = text;
                }
            }
        }


        private int LastIndexOf(String[] s)
        {
            return s.Length - 1;
        }

        private Boolean IsEndOfDocument(String[] document, int index)
        {
            return (document.Length == index);
        }

        private void AppendLines(List<String> target, String[] source, int startIndex, int endIndex)
        {
            for (int i = startIndex; i <= endIndex; i++)
            {
                target.Add(source[i]);
            }
        }

        private int? FindAppearanceOfLine(String[] latest, String originalLine, int startIndex, int endIndex)
        {
            for (int i = startIndex; i <= endIndex; i++)
            {
                if (latest[i] == originalLine) return i;
            }

            return null;
        }

        public string[] GetContentAsStringArray(int documentId)
        {
            Documentrevision latestDoc = PersistentStorage.GetInstance().GetLatestDocumentRevision(documentId, 1).First<Documentrevision>();
            String content = PersistentStorage.GetInstance().GetDocumentContent(latestDoc.path);
            content = content.Substring(content.IndexOf('<')); //Remove metadata
            FlowDocument flowDoc = (FlowDocument)System.Windows.Markup.XamlReader.Parse(content);
            TextRange textRange = new TextRange(flowDoc.ContentStart, flowDoc.ContentEnd);
            String pureContent = textRange.Text; //The "pure" content of the flowdocument i.e. what the user has written
            return pureContent.Split(new String[] {"\r\n", "\n"}, StringSplitOptions.None);
        }
    }
}
