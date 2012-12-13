using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    class PersistentStorage
    {
        //Singleton instance of PersistentStorage
        private static PersistentStorage instance;

        /// <summary>
        /// Private constructor to insure that PersistentStorage is not created outside this class.
        /// </summary>
        private PersistentStorage()
        {

        }

        /// <summary>
        /// Accessor method for accessing the single instance of PersistentStorage.
        /// </summary>
        /// <returns>The only instance of PersistentStorage</returns>
        public static PersistentStorage GetInstance()
        {
            if (instance == null)
            {
                instance = new PersistentStorage();
            }
            return instance;
        }
    }
}
