using System;
using System.IO;
using System.Collections;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using System.Collections.Generic;

namespace WF.Framework.Helper
{
    public class ZipHelper
    {
        public static int BufferSize = 20480;
        private static int _DifferBackupRecentDays = 2;

        public static int DifferBackupRecentDays
        {
            get
            {
                return _DifferBackupRecentDays;
            }
            set
            {
                _DifferBackupRecentDays = value;
            }
        }

        public static string IISBackupPath = @"C:\WebLocal\IISBackup\";

        /// <summary>
        /// 压缩单个文件
        /// </summary>
        /// <param name="FileToZip">被压缩的文件名称(包含文件路径)</param>
        /// <param name="ZipedFile">压缩后的文件名称(包含文件路径)</param>
        /// <param name="CompressionLevel">压缩率0（无压缩）-9（压缩率最高）</param>
        /// Zip.ZipFile(@"C:\Software\cn_visual_studio_premium_2013_x86_dvd_3175272.iso", @"C:\Projects\vs.zip", 6);
        public static void ZipFile(string FileToZip, string ZipedFile, int CompressionLevel, bool isOverrideExistingZip = true)
        {
            FileToZip = FileToZip.Replace("/", "\\");
            ZipedFile = ZipedFile.Replace("/", "\\");

            if (File.Exists(ZipedFile))
            {
                if (isOverrideExistingZip)
                {
                    File.Delete(ZipedFile);
                }
                else
                    return;
            }

            //如果文件没有找到，则报错 
            if (!File.Exists(FileToZip))
                throw new System.IO.FileNotFoundException("File：" + FileToZip + " not found！");

            if (ZipedFile == string.Empty)
                ZipedFile = Path.GetFileNameWithoutExtension(FileToZip) + ".zip";

            if (Path.GetExtension(ZipedFile) != ".zip")
                ZipedFile = ZipedFile + ".zip";

            //如果指定位置目录不存在，创建该目录
            string zipedDir = ZipedFile.Substring(0, ZipedFile.LastIndexOf("\\"));
            if (!Directory.Exists(zipedDir))
                Directory.CreateDirectory(zipedDir);

            //被压缩文件名称
            string filename = FileToZip.Substring(FileToZip.LastIndexOf('\\') + 1);

            System.IO.FileStream StreamToZip = new System.IO.FileStream(FileToZip, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.FileStream ZipFile = System.IO.File.Create(ZipedFile);
            ZipOutputStream ZipStream = new ZipOutputStream(ZipFile);
            ZipEntry ZipEntry = new ZipEntry(filename);
            ZipEntry.IsUnicodeText = true;
            ZipStream.PutNextEntry(ZipEntry);
            ZipStream.SetLevel(CompressionLevel);
            byte[] buffer = new byte[BufferSize];
            int readCount;
            try
            {
                while (true)
                {
                    readCount = StreamToZip.Read(buffer, 0, buffer.Length);
                    if (readCount == 0)
                        break;
                    ZipStream.Write(buffer, 0, readCount);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                ZipStream.Finish();
                ZipStream.Close();
                StreamToZip.Close();
            }
        }

        /// <summary>
        /// 压缩文件夹的方法
        /// </summary>
        /// Zip.ZipDir(@"C:\Software", @"C:\Projects\abc.zip", 6);            
        public static void ZipDir(string RootDirToZip, string[] subFoldersFilter, string ZipedFile, int CompressionLevel, bool isOnlyZipUpdatedFiles, bool isOverrideExistingZip = true)
        {
            try
            {
                if (File.Exists(ZipedFile))
                {
                    if (isOverrideExistingZip)
                        File.Delete(ZipedFile);
                    else
                        return;
                }

                RootDirToZip = RootDirToZip.TrimEnd('\\');

                //压缩文件为空时默认与压缩文件夹同一级目录
                if (ZipedFile == string.Empty)
                {
                    ZipedFile = RootDirToZip.Substring(RootDirToZip.LastIndexOf("\\") + 1);
                    ZipedFile = RootDirToZip.Substring(0, RootDirToZip.LastIndexOf("\\")) + "\\" + ZipedFile + ".zip";
                }

                if (Path.GetExtension(ZipedFile) != ".zip")
                    ZipedFile = ZipedFile + ".zip";

                emptyFolders.Clear();
                fileList.Clear();
                if (subFoldersFilter.Length > 0 && !string.IsNullOrEmpty(subFoldersFilter[0]))
                {
                    foreach (var subFolder in subFoldersFilter)
                    {
                        GetAllDirectories(Path.Combine(RootDirToZip, subFolder), isOnlyZipUpdatedFiles);
                    }
                }
                else
                {
                    GetAllDirectories(RootDirToZip, isOnlyZipUpdatedFiles);
                }

                var file = File.Create(ZipedFile);

                using (ZipOutputStream zipoutputstream = new ZipOutputStream(file))
                {
                    zipoutputstream.SetLevel(CompressionLevel);
                    Crc32 crc = new Crc32();
                    //Hashtable fileList = getAllFies(DirToZip);                
                    string rootMark = RootDirToZip + "\\";// RootDirToZip.Substring(0, RootDirToZip.LastIndexOf("\\") + 1);
                    foreach (DictionaryEntry item in fileList)
                    {
                        //FileStream fs = File.OpenRead(item.Key.ToString());
                        //byte[] buffer = new byte[fs.Length];
                        //fs.Read(buffer, 0, buffer.Length);
                        //ZipEntry entry = new ZipEntry(item.Key.ToString().Substring(DirToZip.Length + 1));
                        //entry.DateTime = (DateTime)item.Value;
                        //entry.Size = fs.Length;
                        //fs.Close();
                        //crc.Reset();
                        //crc.Update(buffer);
                        //entry.Crc = crc.Value;
                        //zipoutputstream.PutNextEntry(entry);
                        //zipoutputstream.Write(buffer, 0, buffer.Length);

                        FileStream StreamToZip = null;
                        try
                        {
                            StreamToZip = new FileStream(item.Key.ToString(), FileMode.Open, FileAccess.Read);
                            //ZipEntry entry = new ZipEntry(item.Key.ToString().Substring(DirToZip.Length + 1));
                            ZipEntry entry = new ZipEntry(item.Key.ToString().Replace(rootMark, string.Empty));
                            entry.IsUnicodeText = true;
                            entry.DateTime = (DateTime)item.Value;
                            entry.Size = StreamToZip.Length;

                            //TODO: Check
                            //crc.Reset();
                            //crc.Update(buffer);
                            //entry.Crc = crc.Value;

                            zipoutputstream.PutNextEntry(entry);
                            zipoutputstream.SetLevel(CompressionLevel);

                            byte[] buffer = new byte[BufferSize];
                            int readCount;

                            while (true)
                            {
                                readCount = StreamToZip.Read(buffer, 0, buffer.Length);
                                if (readCount == 0)
                                    break;
                                zipoutputstream.Write(buffer, 0, readCount);
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //throw ex;
                            Console.WriteLine("zipoutputstream: " + ex.Message);
                        }
                        finally
                        {
                            if (StreamToZip != null)
                            {
                                try
                                {
                                    StreamToZip.Close();
                                }
                                catch
                                { }
                            }
                        }
                    }
                    fileList.Clear();

                    foreach (string emptyPath in emptyFolders)
                    {
                        ZipEntry entry = new ZipEntry(emptyPath.Replace(rootMark, string.Empty) + "\\");
                        entry.IsUnicodeText = true;
                        zipoutputstream.PutNextEntry(entry);
                    }
                    emptyFolders.Clear();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ZipDir_1: " + e.Message);
            }
        }

        /// <summary>
        /// 功能：解压zip格式的文件。
        /// </summary>
        /// <param name="zipFilePath">压缩文件路径</param>
        /// <param name="unZipDir">解压文件存放路径,为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹</param>
        /// <param name="err">出错信息</param>
        /// <returns>解压是否成功</returns>
        /// Zip.UnZip(@"C:\Projects\abc.zip", @"C:\Projects\UnZip");
        public static void UnZip(string zipFilePath, string unZipDir)
        {
            zipFilePath = zipFilePath.Replace("/", "\\");
            unZipDir = unZipDir.Replace("/", "\\");

            if (zipFilePath == string.Empty)
                throw new Exception("zipFilePath cannot be NULL！");

            if (!File.Exists(zipFilePath))
                throw new System.IO.FileNotFoundException("zipFilePath is not existing!");

            //解压文件夹为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹
            if (unZipDir == string.Empty)
                unZipDir = zipFilePath.Replace(Path.GetFileName(zipFilePath), Path.GetFileNameWithoutExtension(zipFilePath));

            if (!unZipDir.EndsWith("\\"))
                unZipDir += "\\";

            if (!Directory.Exists(unZipDir))
                Directory.CreateDirectory(unZipDir);

            using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath)))
            {

                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);
                    
                    if (directoryName.Length > 0)
                        Directory.CreateDirectory(unZipDir + directoryName);

                    if (!directoryName.EndsWith("\\"))
                        directoryName += "\\";

                    if (fileName != String.Empty)
                    {
                        using (FileStream streamWriter = File.Create(unZipDir + theEntry.Name))
                        {

                            int size = BufferSize;
                            byte[] data = new byte[size];
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                    streamWriter.Write(data, 0, size);
                                else
                                    break;
                            }
                        }
                    }
                }
            }
        }

        #region Get File List
        /// <summary>
        /// 获取所有文件
        /// </summary>
        /// <returns></returns>
        private static Hashtable getAllFies(string dir)
        {
            Hashtable FilesList = new Hashtable();
            DirectoryInfo fileDire = new DirectoryInfo(dir);
            if (!fileDire.Exists)
            {
                throw new System.IO.FileNotFoundException("目录:" + fileDire.FullName + "没有找到!");
            }

            getAllDirFiles(fileDire, FilesList);
            getAllDirsFiles(fileDire.GetDirectories(), FilesList);
            return FilesList;
        }

        /// <summary>
        /// 获取一个文件夹下的所有文件夹里的文件
        /// </summary>
        /// <param name="dirs"></param>
        /// <param name="filesList"></param>
        private static void getAllDirsFiles(DirectoryInfo[] dirs, Hashtable filesList)
        {
            foreach (DirectoryInfo dir in dirs)
            {
                foreach (FileInfo file in dir.GetFiles())
                {
                    filesList.Add(file.FullName, file.LastWriteTime);
                }
                getAllDirsFiles(dir.GetDirectories(), filesList);
            }
        }

        /// <summary>
        /// 获取一个文件夹下的文件
        /// </summary>
        /// <param name="strDirName">目录名称</param>
        /// <param name="filesList">文件列表HastTable</param>
        private static void getAllDirFiles(DirectoryInfo dir, Hashtable filesList)
        {
            foreach (FileInfo file in dir.GetFiles())
            {
                filesList.Add(file.FullName, file.LastWriteTime);
            }
        }
        #endregion

        #region Support Empty Folder
        private static Hashtable fileList = new Hashtable();    //all files
        private static List<string> emptyFolders = new List<string>();    //all empty folders
        private static void GetAllDirectories(string rootPath, bool isOnlyZipUpdatedFiles)
        {
            try
            {
                if (!Directory.Exists(rootPath))
                    return;
                string[] subPaths = Directory.GetDirectories(rootPath);
                foreach (string path in subPaths)
                    GetAllDirectories(path, isOnlyZipUpdatedFiles);
                var dirInfo = new DirectoryInfo(rootPath);
                FileInfo[] filesArray = dirInfo.GetFiles();
                foreach (var fileInfo in filesArray)
                {
                    if (isOnlyZipUpdatedFiles)
                    {
                        if (fileInfo.FullName.Contains(IISBackupPath) || fileInfo.LastWriteTime.Date > DateTime.Now.Date.AddDays(-DifferBackupRecentDays))
                            fileList.Add(fileInfo.FullName, fileInfo.LastWriteTime);
                    }
                    else
                    {
                        fileList.Add(fileInfo.FullName, fileInfo.LastWriteTime); //put all files in current folder into list
                    }
                }
                if (!isOnlyZipUpdatedFiles && subPaths.Length == filesArray.Length && filesArray.Length == 0) //if it is an empty folder
                    emptyFolders.Add(rootPath);//add it to the list
            }
            catch (Exception e)
            {
                Console.WriteLine("GetAllDirectories: " + e.Message);
            }
        }
        #endregion
    }
}
