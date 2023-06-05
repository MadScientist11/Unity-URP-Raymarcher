using System;
using System.Security.Cryptography;
using System.Text;

namespace Raymarcher
{
    static internal class HelperFunctions
    {
        public static int Hash(string input)
        {
            if (string.IsNullOrEmpty(input)) return -1;

            MD5 md5Hasher = MD5.Create();
            var hashed = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToInt32(hashed, 0);
        }
    }
}