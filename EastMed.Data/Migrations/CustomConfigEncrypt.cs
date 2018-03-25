using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
// Custom Encrytion for the password using aes encryption wih byte declerations.
namespace EastMed.Data.Migrations
{
   public class CustomConfigEncrypt
    {
        public class CustomEncrypt
        {
            public static string passwordEncrypt(string inText, string key)
            {
                byte[] bytesBuff = Encoding.Unicode.GetBytes(inText);
                using (Aes aes = Aes.Create())
                {
                    Rfc2898DeriveBytes crypto = new Rfc2898DeriveBytes(key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    aes.Key = crypto.GetBytes(32);
                    aes.IV = crypto.GetBytes(16);
                    using (MemoryStream mStream = new MemoryStream())
                    {
                        using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cStream.Write(bytesBuff, 0, bytesBuff.Length);
                            cStream.Close();
                        }
                        inText = Convert.ToBase64String(mStream.ToArray());
                    }
                }
                return inText;
            }
        }
    }
}
