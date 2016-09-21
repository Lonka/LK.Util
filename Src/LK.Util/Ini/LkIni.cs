using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;

namespace LK.Util
{
    public class LkIni
    {
        private static DateTime m_lastCheckConfigTime = DateTime.MinValue;

        /// <summary>
        /// 請給絕對路徑
        /// </summary>
        public static string IniPath { get; set; }

        public static bool GetProfileBool(string section, string key, bool defaultValue)
        {
            return GetProfileBool(IniPath, section, key, defaultValue);
        }

        public static bool GetProfileBool(string iniFile, string section, string key, bool defaultValue)
        {
            string value = GetProfileString(iniFile, section, key, "").Trim();
            bool result = false;
            if (value == "1" || string.Compare(value, "true", true) == 0 || string.Compare(value, "on", true) == 0)
            {
                result = true;
            }
            else if (value == "0" || string.Compare(value, "false", true) == 0 || string.Compare(value, "off", true) == 0)
            {
                result = false;
            }
            else
            {
                result = defaultValue;
            }
            return result;
        }

        public static object GetProfileEnum(string iniFile, Type enumType, string section, string key, string defaultValue)
        {
            object result = null;
            string profileValue = GetProfileString(iniFile, section, key, defaultValue);
            try
            {
                result = Enum.Parse(enumType, profileValue);
            }
            catch
            {
                result = Enum.Parse(enumType, defaultValue);
            }
            return result;
        }


        /// <summary>
        /// 取得Setting.ini的Enum資料
        /// </summary>
        /// <param name="enumType">要取回Enum的Type</param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static object GetProfileEnum(Type enumType, string section, string key, string defaultValue)
        {
            return GetProfileEnum(IniPath, enumType, section, key, defaultValue);
        }

        public static int GetProfileInt(string iniFile, string section, string key, int defaultValue)
        {
            string value = GetProfileString(iniFile, section, key, "");
            int result = int.MinValue;
            if (!int.TryParse(value, out result))
            {
                result = defaultValue;
            }
            return result;
        }

        public static int GetProfileInt(string section, string key, int defaultValue)
        {
            return GetProfileInt(IniPath, section, key, defaultValue);
        }

        /// <summary>
        /// 取得INI的String資料
        /// </summary>
        /// <param name="iniFile">INI檔的Path</param>
        /// <param name="section">INI檔的Section</param>
        /// <param name="key">INI檔中Section的Key</param>
        /// <param name="defaultValue">取不到值的預設值</param>
        /// <returns></returns>
        public static string GetProfileString(string iniFile, string section, string key, string defaultValue)
        {
            StringBuilder result = new StringBuilder(512);
            GetPrivateProfileString(section, key, defaultValue, result, 512, iniFile);
            return (result.ToString());
        }

        /// <summary>
        /// 取得Setting.ini的String資料
        /// </summary>
        /// <param name="section">INI檔的Section</param>
        /// <param name="key">INI檔中Section的Key</param>
        /// <param name="defaultValue">取不到值的預設值</param>
        /// <returns></returns>
        public static string GetProfileString(string section, string key, string defaultValue)
        {
            return GetProfileString(IniPath, section, key, defaultValue);
        }

        /// <summary>
        /// 取得Setting.ini的Section-Key是否有值
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool CheckProfile(string section, string key)
        {
            StringBuilder result = new StringBuilder(512);
            GetPrivateProfileString(section, key, "", result, 512, IniPath);
            if (string.IsNullOrEmpty(result.ToString()))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool IsConfigChanged()
        {
            bool change = true;
            try
            {
                DateTime time = System.IO.File.GetLastWriteTime(IniPath);
                if (time == m_lastCheckConfigTime)
                {
                    change = false;
                }
                else
                {
                    m_lastCheckConfigTime = time;
                }
            }
            catch (Exception e)
            {
                LkLog.WriteLog(LogLevel.Error, e);
            }
            return change;
        }

        public static bool WriteProfileSection(string iniFile, string section, string value)
        {
            bool result = false;
            if (WritePrivateProfileSection(section, value, iniFile) > 0)
                result = true;
            return (result);
        }

        public static bool WriteProfileString(string iniFile, string section, string key, string value)
        {
            bool result = false;
            if (WritePrivateProfileString(section, key, value, iniFile) > 0)
                result = true;
            return (result);
        }

        /// <summary>
        /// 取得ini內的所有section
        /// </summary>
        /// <returns></returns>
        public static List<string> GetSections()
        {
            return GetSections(IniPath);
        }

        /// <summary>
        /// 取得ini內的所有section
        /// </summary>
        /// <param name="iniFiles"></param>
        /// <returns></returns>
        public static List<string> GetSections(string iniFile)
        {
            List<string> result = new List<string>();
            using (StreamReader sr = new StreamReader(iniFile, System.Text.Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (line.StartsWith("[") && line.EndsWith("]"))
                    {
                        result.Add(line.Replace("[", string.Empty).Replace("]", string.Empty));
                    }
                }
            }
            return result;
        }



        public static List<string> GetKeys(string section)
        {
            return new List<string>(GetKeyValues(IniPath, section).Keys);
        }

        public static List<string> GetKeys(string iniFile, string section)
        {
            return new List<string>(GetKeyValues(iniFile, section).Keys);
        }
        public static Dictionary<string, string> GetKeyValues(string section)
        {
            return GetKeyValues(IniPath, section);
        }
        public static Dictionary<string, string> GetKeyValues(string iniFile, string section)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            using (StreamReader sr = new StreamReader(iniFile, System.Text.Encoding.Default))
            {
                bool isSection = false;
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if(line.StartsWith("'"))
                    {
                        continue;
                    }
                    if (isSection)
                    {
                        string[] token = line.Split('=');
                        if (token.Length == 2 && !string.IsNullOrEmpty(token[0]))
                        {
                            result[token[0]] = token[1];
                        }
                    }

                    if (line.StartsWith("[" + section) && line.EndsWith("]"))
                    {
                        isSection = true;
                    }
                    else if (line.StartsWith("[") && line.EndsWith("]") && isSection)
                    {
                        break;
                    }
                }
            }
            return result;
        }

        [DllImport("KERNEL32.DLL", EntryPoint = "GetPrivateProfileString")]
        private static extern int GetPrivateProfileString(string appName,
                                                                     string keyName, string defaultString,
                                                                     StringBuilder returnedString, int size,
                                                                     string fileName);

        [DllImport("KERNEL32.DLL")]
        private static extern long WritePrivateProfileSection(string section,
                                                              string value,
                                                              string fileName);

        [DllImport("KERNEL32.DLL")]
        private static extern long WritePrivateProfileString(string section,
                                                             string key,
                                                             string value,
                                                             string fileName);
    }
}