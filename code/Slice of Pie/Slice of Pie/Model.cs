using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slice_of_Pie
{
    class Model
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
        public String[] MergeDocuments(String[] original, String[] latest)
        {
            //index of the original document
            int o = 0;
            //index of the latest document
            int n = 0;
            //The final merged version
            List<String> merged = new List<String>();

            do
            {
                if (IsEndOfDocument(original, o) || IsEndOfDocument(latest, n))
                {
                    //Remaining lines are new in the latest document. Ends loop.
                    if (IsEndOfDocument(original, o) && !IsEndOfDocument(latest, n))
                    {
                        AppendLines(merged, latest, n, LastIndexOf(latest));
                        n = LastIndexOf(latest) + 1;
                    }

                    //Remaining lines are deleted in the latest document. Ends loop.
                    if (!IsEndOfDocument(original, o) && IsEndOfDocument(latest, n))
                    {
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
                            o++;
                        }
                        else
                        {
                            AppendLines(merged, latest, n, (int)i - 1);
                            n = (int)i;
                            o++;
                        }
                    }
                }
            }
            while (!(IsEndOfDocument(original, o) && IsEndOfDocument(latest, n)));
                  

            return merged.ToArray();
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
    }
}
