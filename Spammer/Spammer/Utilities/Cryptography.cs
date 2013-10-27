using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Spammer
{
    public static class Cryptography
    {
        public static string CalculateSHA1(string username, string password)
        {
            byte[] buffer = Encoding.Default.GetBytes(username + password);

            SHA1CryptoServiceProvider cryptoTransformSHA1 =
                new SHA1CryptoServiceProvider();

            string hash = BitConverter.ToString(
                cryptoTransformSHA1.ComputeHash(buffer)).Replace("-", "");

            return hash;
        }
    }
}
