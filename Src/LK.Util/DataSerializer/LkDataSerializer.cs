using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Data;
using System.IO;

namespace LK.Util
{
    /// <summary>
    /// 使用前一定要先設定LkEncryptDecrypt的IV(or IVStr)和Key(or KeyStr)
    /// </summary>
    public class LkDataSerializer
    {
        private static readonly string extensionName = "temp";
        public static bool DataTableSerialize(string fileName, DataTable dt, int backupSec = 3600)
        {
            bool result = false;
            try
            {
                DateTime fileUpdateTime = DateTime.MinValue;
                string filePath = LkCommonUtil.GetFilePath("TempFile\\" + fileName + "." + extensionName);
                if (File.Exists(filePath))
                {
                    fileUpdateTime = File.GetLastWriteTime(filePath);
                }
                if (DateTime.Now.Subtract(fileUpdateTime).TotalSeconds > backupSec)
                {
                    StringBuilder data = new StringBuilder();
                    XmlWriter writer = XmlWriter.Create(data);
                    XmlSerializer serializer = new XmlSerializer(typeof(DataTable));
                    serializer.Serialize(writer, dt);
                    string temp = string.Empty;
                    if (LkEncryptDecrypt.Encrypt(data.ToString(), out temp))
                    {
                        File.WriteAllText(filePath, temp);
                        result = true;
                    }
                }
            }
            catch (Exception e)
            {
                LkLog.WriteLog(LogLevel.Error, e);
            }
            return result;
        }

        public static bool DataTableDeserialize(string fileName, out DataTable dt)
        {
            bool result = false;
            dt = null;
            try
            {
                string filePath = LkCommonUtil.GetFilePath("TempFile\\" + fileName + "." + extensionName);
                if (File.Exists(filePath))
                {
                    string temp = string.Empty;
                    if (LkEncryptDecrypt.Decrypt(File.ReadAllText(filePath), out temp))
                    {
                        StringReader data = new StringReader(temp);
                        XmlReader reader = XmlReader.Create(data);
                        XmlSerializer serializer = new XmlSerializer(typeof(DataTable));
                        dt = serializer.Deserialize(reader) as DataTable;
                        result = true;
                    }
                }
            }
            catch (Exception e)
            {
                LkLog.WriteLog(LogLevel.Error, e);
            }
            return result;
        }

        public static bool ObjectSerialize(string fileName, object obj, int backupSec = 3600)
        {
            bool result = false;
            try
            {
                DateTime fileUpdateTime = DateTime.MinValue;
                string filePath = LkCommonUtil.GetFilePath("TempFile\\" + fileName + "." + extensionName);
                if (File.Exists(filePath))
                {
                    fileUpdateTime = File.GetLastWriteTime(filePath);
                }
                if (DateTime.Now.Subtract(fileUpdateTime).TotalSeconds > backupSec)
                {
                    Polenter.Serialization.SharpSerializer serializer = new Polenter.Serialization.SharpSerializer();
                    serializer.Serialize(obj, filePath);
                    string temp = string.Empty;
                    if (LkEncryptDecrypt.Encrypt(File.ReadAllText(filePath), out temp))
                    {
                        File.WriteAllText(filePath, temp);
                        result = true;
                    }
                }
            }
            catch (Exception e)
            {
                LkLog.WriteLog(LogLevel.Error, e);
            }
            return result;
        }

        public static bool ObjectDeserialize(string fileName, out object obj)
        {
            bool result = false;
            obj = null;
            try
            {
                string filePath = LkCommonUtil.GetFilePath("TempFile\\" + fileName + "." + extensionName);
                if (File.Exists(filePath))
                {
                    string temp = string.Empty;
                    if (LkEncryptDecrypt.Decrypt(File.ReadAllText(filePath), out temp))
                    {
                        File.WriteAllText(filePath + ".temp", temp);
                        Polenter.Serialization.SharpSerializer serializer = new Polenter.Serialization.SharpSerializer();
                        obj = serializer.Deserialize(filePath + ".temp");
                        File.Delete(filePath + ".temp");
                        result = true;
                    }
                }
            }
            catch (Exception e)
            {
                LkLog.WriteLog(LogLevel.Error, e);
            }
            return result;
        }
    }
}
