using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;


namespace Web_Solution
{
    static public class Security
    {
        /// <summary>
        /// Method which encrypts the given string using md5
        /// </summary>
        /// <param name="pass">String to encrypt</param>
        /// <returns>Returns the string encrypted</returns>
        public static string EncryptString(string s)
        {
            //create new instance of md5
            SHA1 sha1 = new SHA1Managed();

            //convert the input text to array of bytes
            byte[] hashData = sha1.ComputeHash(Encoding.Default.GetBytes(s));

            //create new instance of StringBuilder to save hashed data
            StringBuilder returnValue = new StringBuilder();

            //loop for each byte and add it to StringBuilder
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }

            // return hexadecimal string
            return returnValue.ToString();
        }
    }
}