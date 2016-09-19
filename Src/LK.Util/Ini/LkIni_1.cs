using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace LK.Util
{
    /// <summary>
    /// Ini file operation(Instance)
    /// </summary>
    public class LkIni
    {

        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="fileName">Winform請用:System.AppDomain.CurrentDomain.BaseDirectory+FileName,Web請給相對路徑</param>
        public LkIni(string fileName)
        {
            _fileName = fileName;
        }

        private string _fileName;
        /// <summary>
        /// 設定ini檔的路徑(Winform請用:System.AppDomain.CurrentDomain.BaseDirectory+FileName,Web請給相對路徑)
        /// </summary>
        public string FileName
        {
            get
            {
                if (string.IsNullOrEmpty(_fileName))
                    _fileName = "config.ini";
                return _fileName;
            }
            set
            {
                _fileName = value;
            }
        }

        private string FilePath()
        {
            FileInfo iniFile = new FileInfo(FileName);

            if (iniFile.Exists)
            {
                return iniFile.FullName;
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// 取得 Key 相對的 Value 值
        /// </summary>
        /// <param name="section">Section</param>
        /// <param name="key">Key</param>       
        public string GetKeyValue(string section, string key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, "", temp, 255, FilePath());
            return temp.ToString();
        }


        /// <summary>
        /// 取得 Key 相對的 Value 值 ， 沒有就用Default值
        /// </summary>
        /// <param name="section">Section</param>
        /// <param name="key">Key</param>       
        /// <param name="defaultValue">Default Value</param>       
        public string GetKeyValue(string section, string key, string defaultValue)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, "", temp, 255, FilePath());
            return (string.IsNullOrEmpty(temp.ToString()) ? defaultValue : temp.ToString());
        }

        /// <summary>
        /// 設定 KeyValue 值。
        /// </summary>
        /// <param name="section">Section。</param>
        /// <param name="key">Key。</param>
        /// <param name="value">Value。</param>
        public void SetKeyValue(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, FilePath());
        }


    }
}
