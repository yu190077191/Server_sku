using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF.BusinessRule
{
    public class MD5SHA1Rule
    {
        public static string GetMd5Hash(string pathName)
        {
            string strResult = "";
            string strHashData = "";
            byte[] arrbytHashValue;

            System.IO.FileStream oFileStream = null;

            System.Security.Cryptography.MD5CryptoServiceProvider oMD5Hasher =
                new System.Security.Cryptography.MD5CryptoServiceProvider();

            oFileStream = new System.IO.FileStream(pathName, System.IO.FileMode.Open,
                                                   System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);

            arrbytHashValue = oMD5Hasher.ComputeHash(oFileStream); //计算指定Stream 对象的哈希值

            oFileStream.Close();

            //由以连字符分隔的十六进制对构成的String，其中每一对表示value 中对应的元素；例如“F-2C-4A”

            strHashData = System.BitConverter.ToString(arrbytHashValue);

            //替换-
            strHashData = strHashData.Replace("-", "");

            strResult = strHashData;

            return strResult;
        }

        public static string GetSHA1Hash(string pathName)
        {
            string strResult = "";
            string strHashData = "";
            byte[] arrbytHashValue;

            System.IO.FileStream oFileStream = null;

            System.Security.Cryptography.SHA1CryptoServiceProvider SHA1Hasher =
                new System.Security.Cryptography.SHA1CryptoServiceProvider();

            oFileStream = new System.IO.FileStream(pathName, System.IO.FileMode.Open,
                                                   System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);

            arrbytHashValue = SHA1Hasher.ComputeHash(oFileStream); //计算指定Stream 对象的哈希值

            oFileStream.Close();

            //由以连字符分隔的十六进制对构成的String，其中每一对表示value 中对应的元素；例如“F-2C-4A”

            strHashData = System.BitConverter.ToString(arrbytHashValue);

            //替换-
            strHashData = strHashData.Replace("-", "");

            strResult = strHashData;

            return strResult;
        }
    }
}
