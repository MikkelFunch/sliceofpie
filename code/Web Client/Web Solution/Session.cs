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

        /// <summary>
        /// ID of the document currently in focus.
        /// </summary>
        public int CurrentDocumentID
        {
            get;
            set;
        }

        /// <summary>
        /// The path of the local document currently in focus.
        /// </summary>
        public string CurrentDocumentPath
        {
            get;
            set;
        }

        /// <summary>
        /// The path to the root folder on the local file system.
        /// </summary>
        public string RootFolderPath
        {
            get;
            set;
        }

        /// <summary>
        /// The path of the local document currently in focus.
        /// </summary>
        public string CurrentDocumentTitle
        {
            get;
            set;
        }

        /// <summary>
        /// The title of the document currently in focus.
        /// </summary>
        public string Email
        {
            get;
            set;
        }

        /// <summary>
        /// The time of creation for the current document
        /// </summary>
        public DateTime CurrentDocumentTimeStampMetadata
        {
            get;
            set;
        }

        /// <summary>
        /// The id of the user's root folder
        /// </summary>
        public int RootFolderID
        {
            get;
            set;
        }

        /// <summary>
        /// The name of the last created folder
        /// </summary>
        public string NewlyCreatedFolderName
        {
            get;
            set;
        }

        /// <summary>
        /// The id of the containing folder of the last createst folder.
        /// </summary>
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
