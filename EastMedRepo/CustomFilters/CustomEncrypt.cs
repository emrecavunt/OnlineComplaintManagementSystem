using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;


namespace EastMedRepo.CustomFilters
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


//public static byte[] GenerateRandomNumber(int length)
//{
//    // 256-bit şifreleme için
//    // Length = 32 (32 byte * 8 bit = 256 bit)
//    using (var randomNumberGenerator = new RNGCryptoServiceProvider())
//    {
//        var randomNumber = new byte[length];
//        randomNumberGenerator.GetBytes(randomNumber);

//        return randomNumber;
//    }
////}
//static void Main(string[] args)
//{
//    for (int i = 0; i < 10; i++)
//        // Base64 işlemi daha okunabilir hale gelmesi için uygulanıyor
//        Console.WriteLine($"Rastgele Numara {i + 1}: {Convert.ToBase64String(GenerateRandomNumber(32))}");

//    Console.ReadLine();
//}
//static void Main(string[] args)
//{
//    string password = "Kompleks Şifre Örneği";

//    var key = GenerateRandomNumber(32);
//    var iv = GenerateRandomNumber(16);

//    var encrypted = Encrypt(Encoding.UTF8.GetBytes(password), key, iv);
//    var decrypted = Encoding.UTF8.GetString(Decrypt(encrypted, key, iv));

//    Console.WriteLine($"Orijinal hali : {password}");
//    Console.WriteLine($"Şifrelenmiş hali : {Convert.ToBase64String(encrypted)}");
//    Console.WriteLine($"Şifresi çözülmüş hali : {decrypted}");

//    Console.Read();
//}

//private static byte[] GenerateRandomNumber(int length)
//{
//    using (RNGCryptoServiceProvider randomNumberGenerator = new RNGCryptoServiceProvider())
//    {
//        var randomNumber = new byte[length];
//        randomNumberGenerator.GetBytes(randomNumber);

//        return randomNumber;
//    }
//}

//private static byte[] Encrypt(byte[] dataToEncrypt, byte[] key, byte[] iv)
//{
//    using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
//    {
//        aes.Key = key;
//        aes.IV = iv;

//        using (MemoryStream memoryStream = new MemoryStream())
//        {
//            CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(),
//                                        CryptoStreamMode.Write);

//            cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
//            cryptoStream.FlushFinalBlock();

//            return memoryStream.ToArray();
//        }
//    }
//}

//private static byte[] Decrypt(byte[] dataToDecrypt, byte[] key, byte[] iv)
//{
//    using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
//    {
//        aes.Key = key;
//        aes.IV = iv;

//        using (MemoryStream memoryStream = new MemoryStream())
//        {
//            CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(),
//                                        CryptoStreamMode.Write);

//            cryptoStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);
//            cryptoStream.FlushFinalBlock();

//            return memoryStream.ToArray();
//        }
//    }
//}