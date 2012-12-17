using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web_Solution
{
    class Session
    {
        private static Session _instance;

        private Session()
        {

        }

        /// <summary>
        /// Accessor method for accessing the single instance of controller.
        /// </summary>
        /// <returns>The only instance of controller</returns>
        public static Session GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Session();
            }
            return _instance;
        }

        /// <summary>
        /// ID of the user currently logged in
        /// <para>-1 if no user is online</para>
        /// </summary>
        public int UserID
        {
            get;
            set;
        }

        public int CurrentDocumentID
        {
            get;
            set;
        }

        public string CurrentDocumentPath
        {
            get;
            set;
        }

        public string RootFolderPath
        {
            get;
            set;
        }

        public string CurrentDocumentTitle
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public DateTime CurrentDocumentTimeStampMetadata
        {
            get;
            set;
        }

        public int RootFolderID
        {
            get;
            set;
        }

        public string NewlyCreatedFolderName
        {
            get;
            set;
        }

        public int NewlyCreatedFolderParentId
        {
            get;
            set;
        }

        public int FolderID 
        { 
            get; 
            set; 
        }

        public System.Windows.Controls.ItemCollection RevisionItems
        {
            get;
            set;
        }
    }
}
