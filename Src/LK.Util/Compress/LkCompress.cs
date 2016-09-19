using System;
using System.Collections.Generic;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
namespace LK.Util
{
    public class LkCompress
    {
        /// <summary>
        /// 壓縮資料夾
        /// </summary>
        /// <param name="sourceDir">要壓縮的資料夾(請給絕對路徑)</param>
        /// <param name="targetFile">壓縮後的檔案名</param>
        /// <param name="password">壓縮密碼</param>
        /// <param name="backupOldFile">是否要備份原壓縮檔</param>
        public static void ZipDir(string sourceDir, string targetFile, string password, bool backupOldFile)
        {
            if (!Directory.Exists(sourceDir))
            {
                throw new Exception("要壓縮的資料夾不存在");
            }
            string compressRoot = string.Empty;
            if (sourceDir.TrimEnd('/').LastIndexOf('/') > -1)
            {
                compressRoot = sourceDir.Substring(0, sourceDir.TrimEnd('/').LastIndexOf('/')) + "/";
            }

            string saveRoot = targetFile.Replace(Path.GetFileNameWithoutExtension(targetFile), string.Empty);
            string saveFileName = Path.GetFileNameWithoutExtension(targetFile) + ".zip";
            FastZip zip = new FastZip();
            if (backupOldFile)
            {
                if (File.Exists(saveRoot + saveFileName))
                {
                    try
                    {
                        File.Copy(saveRoot + saveFileName, saveRoot + saveFileName + "-" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".back");
                        File.Delete(saveRoot + saveFileName);
                    }
                    catch
                    {
                        //檔案不存在或剛被刪除
                    }
                }
            }
            if (!string.IsNullOrEmpty(password))
            {
                zip.Password = password;
            }
            zip.CreateZip(compressRoot + saveFileName, sourceDir, true, string.Empty);
            if (compressRoot + saveFileName == saveRoot + saveFileName)
            {
                return;
            }
            else
            {
                File.Copy(compressRoot + saveFileName, saveRoot + saveFileName);
                File.Delete(compressRoot + saveFileName);
            }
        }

        /// <summary>
        /// 壓縮資料夾
        /// </summary>
        /// <param name="sourceDir">要壓縮的資料夾(請給絕對路徑)</param>
        /// <param name="targetFile">壓縮後的檔案名</param>
        /// <param name="password">壓縮密碼</param>
        public static void ZipDir(string sourceDir, string targetFile, string password)
        {
            ZipDir(sourceDir, targetFile, password, true);
        }

        /// <summary>
        /// 壓縮資料夾
        /// </summary>
        /// <param name="sourceDir">要壓縮的資料夾(請給絕對路徑)</param>
        /// <param name="targetFile">壓縮後的檔案名</param>
        public static void ZipDir(string sourceDir, string targetFile)
        {
            ZipDir(sourceDir, targetFile, "", true);
        }

        /// <summary>
        /// 壓縮檔案
        /// </summary>
        /// <param name="sourceFile">要壓縮的檔案(請給絕對路徑)</param>
        /// <param name="targetFile">壓縮後的檔案名</param>
        /// <param name="password">壓縮密碼</param>
        /// <param name="backupOldFile">是否要備份原壓縮檔</param>
        public static void ZipFiles(List<string> sourceFile, string targetFile, string password, bool backupOldFile)
        {
            if (sourceFile == null || sourceFile.Count == 0)
            {
                throw new Exception("請設定要壓縮的檔案");
            }
            foreach (string fileRoot in sourceFile)
            {
                if (!File.Exists(fileRoot))
                {
                    throw new Exception("要壓縮的「" + fileRoot + "」檔案不存在");
                }
            }

            string compressRoot = string.Empty;
            if (sourceFile[0].LastIndexOf('/') > -1)
            {
                compressRoot = sourceFile[0].Substring(0, sourceFile[0].LastIndexOf('/')) + "/";
            }
            string saveRoot = targetFile.Replace(Path.GetFileNameWithoutExtension(targetFile), string.Empty);
            string saveFileName = Path.GetFileNameWithoutExtension(targetFile) + ".zip";

            if (backupOldFile)
            {
                if (File.Exists(saveRoot + saveFileName))
                {
                    try
                    {
                        File.Copy(saveRoot + saveFileName, saveRoot + saveFileName + "-" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".back");
                        File.Delete(saveRoot + saveFileName);
                    }
                    catch
                    {
                        //檔案不存在或剛被刪除
                    }
                }
            }
            ZipOutputStream zs = new ZipOutputStream(File.Create(compressRoot + saveFileName));
            zs.SetLevel(9);
            if (!string.IsNullOrEmpty(password))
            {
                zs.Password = password;
            }
            foreach (string fileRoot in sourceFile)
            {
                FileStream file = File.OpenRead(fileRoot);
                byte[] buffer = new byte[file.Length];
                file.Read(buffer, 0, buffer.Length);
                ZipEntry entry = new ZipEntry(Path.GetFileName(fileRoot));
                entry.DateTime = DateTime.Now;
                entry.Size = file.Length;
                file.Close();
                zs.PutNextEntry(entry);
                zs.Write(buffer, 0, buffer.Length);
            }
            zs.Finish();
            zs.Close();
            if (compressRoot + saveFileName == saveRoot + saveFileName)
            {
                return;
            }
            else
            {
                File.Copy(compressRoot + saveFileName, saveRoot + saveFileName, true);
                File.Delete(compressRoot + saveFileName);
            }
        }

        /// <summary>
        /// 壓縮檔案
        /// </summary>
        /// <param name="sourceFile">要壓縮的檔案(請給絕對路徑)</param>
        /// <param name="targetFile">壓縮後的檔案名</param>
        /// <param name="password">壓縮密碼</param>
        public static void ZipFiles(List<string> sourceFile, string targetFile, string password)
        {
            ZipFiles(sourceFile, targetFile, password, true);
        }

        /// <summary>
        /// 壓縮檔案
        /// </summary>
        /// <param name="sourceFile">要壓縮的檔案(請給絕對路徑)</param>
        /// <param name="targetFile">壓縮後的檔案名</param>
        public static void ZipFiles(List<string> sourceFile, string targetFile)
        {
            ZipFiles(sourceFile, targetFile, string.Empty, true);
        }

        /// <summary>
        /// 壓縮單一檔案
        /// </summary>
        /// <param name="sourceFile">要壓縮的檔案(請給絕對路徑)</param>
        /// <param name="targetFile">壓縮後的檔案名</param>
        /// <param name="password">壓縮密碼</param>
        /// <param name="backupOldFile">是否要備份原壓縮檔</param>
        public static void ZipFile(string sourceFile, string targetFile, string password, bool backupOldFile)
        {
            List<string> files = new List<string>();
            files.Add(sourceFile);
            ZipFiles(files, targetFile, password, backupOldFile);
        }

        /// <summary>
        /// 壓縮單一檔案
        /// </summary>
        /// <param name="sourceFile">要壓縮的檔案(請給絕對路徑)</param>
        /// <param name="targetFile">壓縮後的檔案名</param>
        /// <param name="password">壓縮密碼</param>
        public static void ZipFile(string sourceFile, string targetFile, string password)
        {
            ZipFile(sourceFile, targetFile, password, true);
        }

        /// <summary>
        /// 壓縮單一檔案
        /// </summary>
        /// <param name="sourceFile">要壓縮的檔案(請給絕對路徑)</param>
        /// <param name="targetFile">壓縮後的檔案名</param>
        /// <returns></returns>
        public static void ZipFile(string sourceFile, string targetFile)
        {
            ZipFile(sourceFile, targetFile, "", true);
        }

        /// <summary>
        /// 解壓縮
        /// </summary>
        /// <param name="SourceFile">要解壓縮的Zip檔案(完整路徑檔名) </param>
        /// <param name="targetDir">解壓縮後存放的資料夾路徑(完整路徑) </param>
        /// <param name="password">密碼</param>
        public static void ExtractZip(string sourceFile, string targetDir, string password)
        {
            string sourceFileName = Path.GetFileNameWithoutExtension(sourceFile) + ".zip";
            FastZip zip = new FastZip();
            try
            {
                //判斷ZIP檔案是否存在
                if (File.Exists(sourceFileName) == false)
                {
                    throw new Exception("要解壓縮的檔案【" + sourceFileName + "】不存在，無法執行");
                }
                if (password != "")
                {
                    zip.Password = password;
                }
                zip.ExtractZip(sourceFileName, targetDir, "");
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 解壓縮
        /// </summary>
        /// <param name="SourceFile">要解壓縮的Zip檔案(完整路徑檔名) </param>
        /// <param name="TargetDir">解壓縮後存放的資料夾路徑(完整路徑) </param>
        public static void ExtractZip(string sourceFile, string targetDir)
        {
            ExtractZip(sourceFile, targetDir, "");
        }
    }
}
