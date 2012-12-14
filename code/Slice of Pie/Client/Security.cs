using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;


namespace Client
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
            // calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(s);
            byte[] hash = md5.ComputeHash(inputBytes);

            // convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}