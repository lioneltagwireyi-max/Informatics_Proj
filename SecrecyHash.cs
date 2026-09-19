using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace PhoneFit
{
    public class SecrecyHash
    {
          
        public static string hashFunction(string password)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = md5.ComputeHash(bytes);

                return BitConverter.ToString(hash).Replace("-", "");
            }
        }
    }
}