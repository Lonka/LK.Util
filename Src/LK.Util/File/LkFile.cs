using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using LK.Util;

namespace LK.Util
{
    /// <summary>
    /// 檔案處理
    /// </summary>
    public class LkFile
    {

        private static string _targetRoot = string.Empty;
        /// <summary>
        /// Copy檔案到的目標端(//192.168.166.1/)
        /// </summary>
        private static string TargetRoot
        {
            get
            {
                return _targetRoot.Replace("\\", "/").TrimEnd('/') + "/";
            }
            set
            {
                _targetRoot = value;
            }
        }

        private static string _sourceRoot = string.Empty;
        /// <summary>
        /// Copy檔案的來源位置(//127.0.0.1/)
        /// </summary>
        private static string SourceRoot
        {
            get
            {
                return _sourceRoot.Replace("\\", "/").TrimEnd('/');
            }
            set
            {
                _sourceRoot = value;
            }
        }

        private static List<string> _getCopyFolderList = new List<string>();
        /// <summary>
        /// 要Copy的目錄列表(沒設定即Copy整個資料夾內的資料)
        /// </summary>
        private static List<string> GetCopyFolderList
        {
            get
            {

                return _getCopyFolderList;
            }
            set
            {
                _getCopyFolderList = value;
            }
        }
        private static List<string> _getCopyFileList = new List<string>();
        /// <summary>
        /// 要Copy的檔案列表，請加上副檔名(沒設定即Copy整個資料夾內的資料)
        /// </summary>
        private static List<string> GetCopyFileList
        {
            get
            {

                return _getCopyFileList;
            }
            set
            {
                _getCopyFileList = value;
            }
        }
        private static bool _isDelExtraFile = false;
        /// <summary>
        /// 是否要刪除額外的檔案
        /// </summary>
        private static bool IsDelExtraFile
        {
            get
            {
                return _isDelExtraFile;
            }
            set
            {
                _isDelExtraFile = value;
            }
        }

        private static bool _isDelExtraFolder = false;
        /// <summary>
        /// 是否要刪除額外的資料夾
        /// </summary>
        private static bool IsDelExtraFolder
        {
            get
            {
                return _isDelExtraFolder;
            }
            set
            {
                _isDelExtraFolder = value;
            }
        }

        private static string backupFolder = string.Empty;
        #region Folder
        /// <summary>
        /// 執行更新資料夾
        /// </summary>
        /// <param name="sourceRoot">Copy檔案的來源位置(//127.0.0.1/)</param>
        /// <param name="targetRoot">Copy檔案到的目標端(//192.168.166.1/)</param>
        /// <param name="copyFolderList">要Copy的目錄列表(沒設定即Copy整個資料夾內的資料)</param>
        /// <param name="sourceAccount">來源位置帳號</param>
        /// <param name="sourcePassword">來源位置密碼</param>
        /// <param name="targetAccount">目標位置帳號</param>
        /// <param name="targetPassword">目標位置密碼</param>
        /// <param name="compressPassword">壓縮密碼</param>
        /// <param name="delExtraFile">是否要刪除額外的檔案</param>
        /// <param name="delExtraFolder">是否要刪除額外的資料夾</param>
        public static void DoFolderCopy(string sourceRoot, string targetRoot, List<string> copyFolderList, string sourceAccount, string sourcePassword, string targetAccount, string targetPassword, string compressPassword, bool delExtraFile, bool delExtraFolder)
        {
            SourceRoot = sourceRoot;
            TargetRoot = targetRoot;
            GetCopyFolderList = copyFolderList;
            IsDelExtraFile = delExtraFile;
            IsDelExtraFolder = delExtraFolder;
            if (string.IsNullOrEmpty(SourceRoot))
            {
                throw new Exception("未設定來源路徑");
            }
            else if (string.IsNullOrEmpty(TargetRoot))
            {
                throw new Exception("未設定目標路徑");
            }
            LkNetUse sourceNetUse = null;
            LkNetUse targetNetuse = null;
            if (!string.IsNullOrEmpty(sourceAccount))
            {
                sourceNetUse = new LkNetUse(SourceRoot.Substring(0, (SourceRoot + "/").IndexOf('/', 3)) + "/", sourceAccount, sourcePassword);
                Console.WriteLine("Connection Source Remote...");
                sourceNetUse.Connect();
            }
            if (!string.IsNullOrEmpty(targetAccount))
            {
                targetNetuse = new LkNetUse(TargetRoot.Substring(0, (TargetRoot).IndexOf('/', 3)) + "/", targetAccount, targetPassword);
                Console.WriteLine("Connection Target Remote...");
                targetNetuse.Connect();
            }

            Console.WriteLine("Start Copy");
            if (targetRoot.Contains(sourceRoot))
            {
                backupFolder = targetRoot.Replace(sourceRoot+"\\", string.Empty).TrimStart().
                    Substring(0, targetRoot.Replace(sourceRoot + "\\", string.Empty).TrimStart().IndexOf("\\"));
            }
            if (string.IsNullOrEmpty(compressPassword))
            {
                CopyFolder(SourceRoot, 0);
            }
            else
            {
                CompressAndCopyFolder(compressPassword);
            }
            Console.WriteLine("Complete");
            if (!string.IsNullOrEmpty(sourceAccount))
            {
                Console.WriteLine("Disconnection Source Remote");
                sourceNetUse.DisConnect();
            }
            if (!string.IsNullOrEmpty(targetAccount))
            {
                Console.WriteLine("Disconnection Target Remote...");
                targetNetuse.DisConnect();
            }

        }

        /// <summary>
        /// 執行更新資料夾
        /// </summary>
        /// <param name="sourceRoot">Copy檔案的來源位置(//127.0.0.1/)</param>
        /// <param name="targetRoot">Copy檔案到的目標端(//192.168.166.1/)</param>
        /// <param name="copyFolderList">要Copy的目錄列表(沒設定即Copy整個資料夾內的資料)</param>
        /// <param name="sourceAccount">來源位置帳號</param>
        /// <param name="sourcePassword">來源位置密碼</param>
        /// <param name="targetAccount">目標位置帳號</param>
        /// <param name="targetPassword">目標位置密碼</param>
        public static void DoFolderCopy(string sourceRoot, string targetRoot, List<string> copyFolderList, string sourceAccount, string sourcePassword, string targetAccount, string targetPassword)
        {
            DoFolderCopy(sourceRoot, targetRoot, copyFolderList, sourceAccount, sourcePassword, targetAccount, targetPassword, string.Empty, false, false);
        }

        /// <summary>
        /// 執行更新資料夾
        /// </summary>
        /// <param name="sourceRoot">Copy檔案的來源位置(//127.0.0.1/)</param>
        /// <param name="targetRoot">Copy檔案到的目標端(//192.168.166.1/)</param>
        /// <param name="copyFolderList">要Copy的目錄列表(沒設定即Copy整個資料夾內的資料)</param>
        /// <param name="sourceAccount">來源位置帳號</param>
        /// <param name="sourcePassword">來源位置密碼</param>
        /// <param name="targetAccount">目標位置帳號</param>
        /// <param name="targetPassword">目標位置密碼</param>
        /// <param name="compressPassword">壓縮密碼</param>
        public static void DoFolderCopy(string sourceRoot, string targetRoot, List<string> copyFolderList, string sourceAccount, string sourcePassword, string targetAccount, string targetPassword, string compressPassword)
        {
            DoFolderCopy(sourceRoot, targetRoot, copyFolderList, sourceAccount, sourcePassword, targetAccount, targetPassword, compressPassword, false, false);
        }


        /// <summary>
        /// 執行更新資料夾
        /// </summary>
        /// <param name="sourceRoot">Copy檔案的來源位置(//127.0.0.1/)</param>
        /// <param name="targetRoot">Copy檔案到的目標端(//192.168.166.1/)</param>
        /// <param name="copyFolderList">要Copy的目錄列表(沒設定即Copy整個資料夾內的資料)</param>
        public static void DoFolderCopy(string sourceRoot, string targetRoot, List<string> copyFolderList)
        {
            DoFolderCopy(sourceRoot, targetRoot, copyFolderList, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false, false);
        }

        /// <summary>
        /// 執行更新資料夾
        /// </summary>
        /// <param name="sourceRoot">Copy檔案的來源位置(//127.0.0.1/)</param>
        /// <param name="targetRoot">Copy檔案到的目標端(//192.168.166.1/)</param>
        /// <param name="copyFolderList">要Copy的目錄列表(沒設定即Copy整個資料夾內的資料)</param>
        /// <param name="compressPassword">壓縮密碼</param>
        public static void DoFolderCopy(string sourceRoot, string targetRoot, List<string> copyFolderList, string compressPassword)
        {
            DoFolderCopy(sourceRoot, targetRoot, copyFolderList, string.Empty, string.Empty, string.Empty, string.Empty, compressPassword, false, false);
        }


        /// <summary>
        /// 壓縮跟備份資料夾
        /// </summary>
        /// <param name="compressPassword">壓縮密碼</param>
        private static void CompressAndCopyFolder(string compressPassword)
        {
            //取得目錄資訊
            DirectoryInfo dir = new DirectoryInfo(SourceRoot);
            //取得該目錄下的所有目錄
            DirectoryInfo[] dirInfo = dir.GetDirectories();
            if (!Directory.Exists(TargetRoot))
            {
                Directory.CreateDirectory(TargetRoot);
            }
            for (int i = 0; i < dirInfo.Length; i++)
            {
                //判斷是否有要下載的目錄但這只有在第一層時才判斷
                if (IsCopyFolderListExist(dirInfo[i].Name))
                {
                    //壓縮備份
                    LkCompress.ZipDir(SourceRoot + "/" + dirInfo[i].Name, TargetRoot + dirInfo[i].Name, compressPassword);
                }
            }
        }
        /// <summary>
        /// 找出所有要更新的目錄
        /// </summary>
        /// <param name="path">要找的目錄path</param>
        /// <param name="layer">該目錄的第幾層(初使請給0)</param>
        private static void CopyFolder(string path, int layer)
        {
            //取得目錄資訊
            DirectoryInfo dir = new DirectoryInfo(path);
            //取得該目錄下的所有目錄
            DirectoryInfo[] dirInfo = dir.GetDirectories();
            for (int i = 0; i < dirInfo.Length; i++)
            {
                //判斷是否有要下載的目錄但這只有在第一層時才判斷
                if (!IsCopyFolderListExist(dirInfo[i].Name) && layer == 0)
                    continue;
                //下載
                CopyFile(path, dirInfo[i], layer);
            }
        }
        /// <summary>
        /// 實際執行更新
        /// </summary>
        /// <param name="path">記錄目前目錄來找尋是否還有子目錄</param>
        /// <param name="dirInfo">目前目錄的資訊</param>
        /// <param name="layer">目前在第幾層</param>
        private static void CopyFile(string path, DirectoryInfo dirInfo, int layer)
        {
            string saveFolder = GetDirRoot(path, dirInfo.Name);
            //判斷要下載的目錄檔案是否在host中有目錄
            if (!Directory.Exists(saveFolder))
                Directory.CreateDirectory(saveFolder);
            //收尋該目錄底下是否還有其它目錄
            DirectoryInfo[] dirInfos = dirInfo.GetDirectories();
            if (dirInfos.Length > 0)
            {
                CopyFolder(path + "\\" + dirInfo.Name, layer + 1);
            }
            FileInfo[] fileInfo = dirInfo.GetFiles();
            for (int i = 0; i < fileInfo.Length; i++)
            {
                try
                {
                    fileInfo[i].CopyTo(saveFolder + "\\" + fileInfo[i].Name, CheckVersion(saveFolder + "\\" + fileInfo[i].Name, fileInfo[i]));
                    //Console.WriteLine(fileInfo[i].Name + "-更新");
                }
                catch
                {
                    //Console.WriteLine(fileInfo[i].Name + "-存在");
                }
            }
            //copy 完後再由目前目錄比對來源目錄刪除多的
            if (IsDelExtraFile)
            {
                DirectoryInfo hostDir = new DirectoryInfo(saveFolder);
                FileInfo[] hostFiles = hostDir.GetFiles();
                foreach (FileInfo file in hostFiles)
                {
                    if (IsExtraFile(fileInfo, file))
                    {
                        File.Delete(file.FullName);
                        //Console.WriteLine(file.Name + "-刪除");
                    }
                }
            }
            if (IsDelExtraFolder)
            {
                DirectoryInfo hostDir = new DirectoryInfo(saveFolder);
                DirectoryInfo[] hostDirInfo = hostDir.GetDirectories();
                foreach (DirectoryInfo folder in hostDirInfo)
                {
                    if (IsExtraFolder(dirInfos, folder))
                    {
                        Directory.Delete(folder.FullName);
                        //Console.WriteLine(folder.Name + "-刪除");
                    }
                }
            }
        }
        /// <summary>
        /// 判斷是否為多出來的檔案
        /// </summary>
        /// <param name="remoteFiles">遠端的檔案區</param>
        /// <param name="hostFile">本機檔案</param>
        /// <returns></returns>
        private static bool IsExtraFile(FileInfo[] remoteFiles, FileInfo hostFile)
        {
            foreach (FileInfo file in remoteFiles)
            {
                if (file.Name == hostFile.Name)
                    return false;
            }
            return true;
        }
        /// <summary>
        /// 判斷是否為多出來的資料夾
        /// </summary>
        /// <param name="remoteDirInfo">遠端的資料夾區</param>
        /// <param name="hostFile">本機資料夾</param>
        /// <returns></returns>
        private static bool IsExtraFolder(DirectoryInfo[] remoteDirInfo, DirectoryInfo hostFolder)
        {
            foreach (DirectoryInfo folder in remoteDirInfo)
            {
                if (folder.Name == hostFolder.Name)
                    return false;
            }
            return true;
        }
        /// <summary>
        /// 是否在更新目錄的清單
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private static bool IsCopyFolderListExist(string source)
        {
            if (GetCopyFolderList.Count == 0 && source != backupFolder)
                return true;
            else return GetCopyFolderList.Contains(source);
        }
        #endregion

        #region File
        /// <summary>
        /// 執行更新檔案
        /// </summary>
        /// <param name="sourceRoot">Copy檔案的來源位置(//127.0.0.1/)</param>
        /// <param name="targetRoot">Copy檔案到的目標端(//192.168.166.1/)</param>
        /// <param name="copyFileList">要Copy的檔案列表(請給副檔名</param>
        /// <param name="sourceAccount">來源位置帳號</param>
        /// <param name="sourcePassword">來源位置密碼</param>
        /// <param name="targetAccount">目標位置帳號</param>
        /// <param name="targetPassword">目標位置密碼</param>
        /// <param name="compressFileName">壓縮檔名(不用副檔名)</param>
        /// <param name="compressPassword">壓縮密碼</param>
        public static void DoFileCopy(string sourceRoot, string targetRoot, List<string> copyFileList, string sourceAccount, string sourcePassword, string targetAccount, string targetPassword, string compressFileName, string compressPassword)
        {
            SourceRoot = sourceRoot;
            TargetRoot = targetRoot;
            GetCopyFileList = copyFileList;
            if (string.IsNullOrEmpty(SourceRoot))
            {
                throw new Exception("未設定來源路徑");
            }
            else if (string.IsNullOrEmpty(TargetRoot))
            {
                throw new Exception("未設定目標路徑");
            }
            LkNetUse sourceNetUse = null;
            LkNetUse targetNetuse = null;
            if (!string.IsNullOrEmpty(sourceAccount))
            {
                sourceNetUse = new LkNetUse(SourceRoot.Substring(0, (SourceRoot + "/").IndexOf('/', 3)) + "/", sourceAccount, sourcePassword);
                Console.WriteLine("Connection Source Remote...");
                sourceNetUse.Connect();
            }
            if (!string.IsNullOrEmpty(targetAccount))
            {
                targetNetuse = new LkNetUse(TargetRoot.Substring(0, (TargetRoot).IndexOf('/', 3)) + "/", targetAccount, targetPassword);
                Console.WriteLine("Connection Target Remote...");
                targetNetuse.Connect();
            }

            Console.WriteLine("Start Copy");
            List<string> fullPathFile = new List<string>();
            foreach (string fileRoot in GetCopyFileList)
            {
                if (!File.Exists(SourceRoot + "/" + fileRoot))
                {
                    throw new Exception("要Copy的「" + SourceRoot + "/" + fileRoot + "」檔案不存在");
                }
                fullPathFile.Add(SourceRoot + "/" + fileRoot);
            }
            if (!Directory.Exists(TargetRoot))
                Directory.CreateDirectory(TargetRoot);
            if (string.IsNullOrEmpty(compressFileName))
            {
                foreach (string fileRoot in GetCopyFileList)
                {
                    FileInfo file = new FileInfo(SourceRoot + "/" + fileRoot);
                    file.CopyTo(TargetRoot + file.Name, CheckVersion(TargetRoot + file.Name, file));
                }
            }
            else
            {
                LkCompress.ZipFiles(fullPathFile, TargetRoot + compressFileName, compressPassword);
            }
            Console.WriteLine("Complete");
            if (!string.IsNullOrEmpty(sourceAccount))
            {
                Console.WriteLine("Disconnection Source Remote");
                sourceNetUse.DisConnect();
            }
            if (!string.IsNullOrEmpty(targetAccount))
            {
                Console.WriteLine("Disconnection Target Remote...");
                targetNetuse.DisConnect();
            }
        }

        /// <summary>
        /// 執行更新檔案
        /// </summary>
        /// <param name="sourceRoot">Copy檔案的來源位置(//127.0.0.1/)</param>
        /// <param name="targetRoot">Copy檔案到的目標端(//192.168.166.1/)</param>
        /// <param name="copyFileList">要Copy的檔案列表(請給副檔名</param>
        /// <param name="sourceAccount">來源位置帳號</param>
        /// <param name="sourcePassword">來源位置密碼</param>
        /// <param name="targetAccount">目標位置帳號</param>
        /// <param name="targetPassword">目標位置密碼</param>
        public static void DoFileCopy(string sourceRoot, string targetRoot, List<string> copyFileList, string sourceAccount, string sourcePassword, string targetAccount, string targetPassword)
        {
            DoFileCopy(sourceRoot, targetRoot, copyFileList, sourceAccount, sourcePassword, targetAccount, targetPassword, string.Empty, string.Empty);
        }

        /// <summary>
        /// 執行更新檔案
        /// </summary>
        /// <param name="sourceRoot">Copy檔案的來源位置(//127.0.0.1/)</param>
        /// <param name="targetRoot">Copy檔案到的目標端(//192.168.166.1/)</param>
        /// <param name="copyFileList">要Copy的檔案列表</param>
        public static void DoFileCopy(string sourceRoot, string targetRoot, List<string> copyFileList)
        {
            DoFileCopy(sourceRoot, targetRoot, copyFileList, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
        }

        /// <summary>
        /// 執行更新檔案
        /// </summary>
        /// <param name="sourceRoot">Copy檔案的來源位置(//127.0.0.1/)</param>
        /// <param name="targetRoot">Copy檔案到的目標端(//192.168.166.1/)</param>
        /// <param name="copyFileList">要Copy的檔案列表</param>
        /// <param name="compressFileName">壓縮檔名(不用副檔名)</param>
        /// <param name="compressPassword">壓縮密碼</param>
        public static void DoFileCopy(string sourceRoot, string targetRoot, List<string> copyFileList, string compressFileName, string compressPassword)
        {
            DoFileCopy(sourceRoot, targetRoot, copyFileList, string.Empty, string.Empty, string.Empty, string.Empty, compressFileName, compressPassword);
        }
        #endregion

        /// <summary>
        /// 取得要更新的目錄root
        /// </summary>
        /// <param name="path">目前目錄，單純為了Replace留子目錄</param>
        /// <param name="dirInfoName">目錄名稱</param>
        /// <returns></returns>
        private static string GetDirRoot(string path, string dirInfoName)
        {
            return TargetRoot.TrimEnd('\\') + path.Replace(SourceRoot, string.Empty) + "\\" + dirInfoName;
        }
        /// <summary>
        /// 判斷最後寫入時間決定是否要更新檔案
        /// </summary>
        /// <param name="hostPath">主機的檔案位置</param>
        /// <param name="sourceFile">遠端的檔案位置</param>
        /// <returns></returns>
        private static bool CheckVersion(string hostPath, FileInfo sourceFile)
        {
            FileInfo hostFile = new FileInfo(hostPath);
            if (hostFile.Exists)
            {
                if (sourceFile.LastWriteTime > hostFile.LastWriteTime)
                    return true;
                else
                    return false;
            }
            else
                return true;
        }

    }
}
