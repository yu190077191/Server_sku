using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WF.Framework.Helper;
using WF.Model;

namespace WF.BusinessRule
{
    public class WordRule
    {
        public static string CreateWord(string templateFolder, string targetPath, List<WordParameters> list)
        {
            if(File.Exists(HttpContext.Current.Server.MapPath("~/" + targetPath)))
            {
                return targetPath;
            }
            string serverMapFolder = "Word/" + DateTime.Now.ToString("yyyy-MM-dd") + "/" + Guid.NewGuid().ToString();
            string targetFolder = HttpContext.Current.Server.MapPath("~/" + serverMapFolder + "/");
            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }

            CopyFolder(System.Web.HttpContext.Current.Server.MapPath("~/" + templateFolder), targetFolder);
            RewriteWordDocument(list, targetFolder);
            ZipHelper.ZipDir(targetFolder, new string[] { }, string.Empty, 9, false);
            targetFolder = targetFolder.Remove(targetFolder.Length - 1);
            System.IO.File.Move(targetFolder + ".zip", HttpContext.Current.Server.MapPath("~/" + targetPath));
            DeleteFolder(targetFolder);

            return targetPath;
        }

        private static void CopyFolder(string from, string to)
        {
            if (from.Replace("/", "\\").LastIndexOf("\\") < from.Length - 1)
            {
                from += "\\";
            }

            List<string> fileList = new System.Collections.Generic.List<string>();
            CommonRule.ScanDir(from, ref fileList);
            foreach (string path in fileList)
            {
                string targetPath = Path.Combine(to, path.Substring(from.Length));
                string targetFolder = targetPath.Substring(0, targetPath.LastIndexOf("\\"));
                if (!Directory.Exists(targetFolder))
                {
                    Directory.CreateDirectory(targetFolder);
                }

                System.IO.File.Copy(path, targetPath, true);
            }
        }

        public static void DeleteFolder(string dir)
        {
            if (Directory.Exists(dir))
            {
                foreach (string d in Directory.
                GetFileSystemEntries(dir))
                {
                    if (System.IO.File.Exists(d))
                        System.IO.File.Delete(d);
                    else
                        DeleteFolder(d);
                }

                Directory.Delete(dir);
            }
        }

        public static void RewriteWordDocument(List<WordParameters> list, string targetFolder)
        {
            List<string> pathList = list.Select(k => k.Path).Distinct().ToList();
            foreach (string path in pathList)
            {
                string fileFullPath = targetFolder + path;
                List<WordParameters> li = list.Where(k => k.Path == path).ToList();
                System.Text.Encoding coding = EncodingHelper.GetType(fileFullPath);
                string xml = System.IO.File.ReadAllText(fileFullPath, coding);
                string newXml = xml;
                foreach (WordParameters item in li)
                {
                    newXml = newXml.Replace(item.Code, item.Value);
                }

                System.IO.File.WriteAllText(fileFullPath, newXml, coding);
            }
        }
    }
}
