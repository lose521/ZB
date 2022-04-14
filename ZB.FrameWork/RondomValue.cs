using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ZB.FrameWork
{
    public class RondomValue
    {
        public static string CreateApiKey()
        {
            using (var cryptoProvider = new RNGCryptoServiceProvider())
            {
                var secretByteArray = new byte[32];
                cryptoProvider.GetBytes(secretByteArray);
                return Convert.ToBase64String(secretByteArray);
            }
        }

        public static string CreateAppId()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
