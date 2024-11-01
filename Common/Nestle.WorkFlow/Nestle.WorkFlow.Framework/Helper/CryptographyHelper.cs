using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Nestle.WorkFlow.Framework.Helper
{
    public static class CryptographyHelper
    {
        private const string FaceKey = "@WSX3edc";

        public static byte[] Encrypt(string plaintext)
        {
            ////byte[] plaintextData = Encoding.Unicode.GetBytes(plaintext);
            ////return ProtectedData.Protect(plaintextData, null, DataProtectionScope.CurrentUser);

            return EncryptByte(plaintext, FaceKey);
        }

        public static string Decrypt(byte[] encryptedData)
        {
            ////string plaintext = string.Empty;

            ////try
            ////{
            ////    byte[] plaintextData = ProtectedData.Unprotect(encryptedData, null, DataProtectionScope.CurrentUser);
            ////    plaintext = Encoding.Unicode.GetString(plaintextData);
            ////}
            ////catch (CryptographicException)
            ////{
            ////    plaintext = null;
            ////}

            ////return plaintext;

            return DecryptByte(encryptedData, FaceKey);
        }

        public static string Encrypt(string stringToEncrypt, string key)
        {
            var des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Unicode.GetBytes(stringToEncrypt);
            des.Key = Encoding.UTF8.GetBytes(key);
            des.IV = Encoding.UTF8.GetBytes(key);
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            var ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }

            ret.ToString();
            return ret.ToString();
        }

        public static string Decrypt(string stringToDecrypt, string key)
        {
            var des = new DESCryptoServiceProvider();
            var inputByteArray = new byte[stringToDecrypt.Length / 2];
            for (int x = 0; x < stringToDecrypt.Length / 2; x++)
            {
                int i = Convert.ToInt32(stringToDecrypt.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }

            des.Key = Encoding.UTF8.GetBytes(key);
            des.IV = Encoding.UTF8.GetBytes(key);
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            var ret = new StringBuilder();
            return Encoding.Unicode.GetString(ms.ToArray());
        }

        public static byte[] EncryptByte(string stringToEncrypt, string key)
        {
            var des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Unicode.GetBytes(stringToEncrypt);
            des.Key = Encoding.UTF8.GetBytes(key);
            des.IV = Encoding.UTF8.GetBytes(key);
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return ms.ToArray();
        }

        public static string DecryptByte(byte[] inputByteArray, string key)
        {
            try
            {
                var des = new DESCryptoServiceProvider();
                des.Key = Encoding.UTF8.GetBytes(key);
                des.IV = Encoding.UTF8.GetBytes(key);
                var ms = new MemoryStream();
                var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Encoding.Unicode.GetString(ms.ToArray());
            }
            catch
            {
                return null;
            }
        }
    }
}