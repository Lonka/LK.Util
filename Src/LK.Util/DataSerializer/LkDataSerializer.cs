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
        //private static readonly string extensionName = "temp";

        private static string _defaultDirectory = string.Empty;
        public static string DefaultDirectory
        {
            get
            {
                return _defaultDirectory;
            }
            set
            {
                _defaultDirectory = value;
            }
        }

        public static bool DataTableSerialize(string fileName, DataTable dt, bool isEncrypt = false, int backupSec = 0)
        {
            bool result = false;
            try
            {
                if (string.IsNullOrEmpty(dt.TableName))
                {
                    dt.TableName = fileName;
                }
                DateTime fileUpdateTime = DateTime.MinValue;
                string filePath = LkCommonUtil.GetFilePath(Path.Combine(DefaultDirectory, fileName));

                string dirName = new FileInfo(filePath).DirectoryName;
                if (!Directory.Exists(dirName))
                {
                    Directory.CreateDirectory(dirName);
                }
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
                    if (isEncrypt)
                    {
                        string temp = string.Empty;
                        if (LkEncryptDecrypt.Encrypt(data.ToString(), out temp))
                        {
                            File.WriteAllText(filePath, temp);
                            result = true;
                        }
                    }
                    else
                    {
                        File.WriteAllText(filePath, data.ToString());
                        result = true;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        public static bool DataTableDeserialize(string fileName, out DataTable dt, bool isEncrypt = false)
        {
            bool result = false;
            dt = null;
            try
            {
                string filePath = LkCommonUtil.GetFilePath(Path.Combine(DefaultDirectory, fileName));
                if (File.Exists(filePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(DataTable));
                    string data = File.ReadAllText(filePath);
                    string temp = string.Empty;
                    if (isEncrypt && LkEncryptDecrypt.Decrypt(data, out temp))
                    {
                        data = temp;
                    }
                    StringReader dataSr = new StringReader(data);
                    XmlReader reader = XmlReader.Create(dataSr);
                    dt = serializer.Deserialize(reader) as DataTable;
                    result = true;

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        public static bool ObjectSerialize(string fileName, object obj, bool isEncrypt = false, int backupSec = 0)
        {
            bool result = false;
            try
            {
                DateTime fileUpdateTime = DateTime.MinValue;
                string filePath = LkCommonUtil.GetFilePath(Path.Combine(DefaultDirectory, fileName));
                if (File.Exists(filePath))
                {
                    fileUpdateTime = File.GetLastWriteTime(filePath);
                }
                if (DateTime.Now.Subtract(fileUpdateTime).TotalSeconds > backupSec)
                {
                    Polenter.Serialization.SharpSerializer serializer = new Polenter.Serialization.SharpSerializer();
                    serializer.Serialize(obj, filePath);
                    if (isEncrypt)
                    {
                        string temp = string.Empty;
                        if (LkEncryptDecrypt.Encrypt(File.ReadAllText(filePath), out temp))
                        {
                            File.WriteAllText(filePath, temp);
                            result = true;
                        }
                    }
                    else
                    {
                        result = true;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        public static bool ObjectDeserialize(string fileName, out object obj, bool isEncrypt = false)
        {
            bool result = false;
            obj = null;
            try
            {
                string filePath = LkCommonUtil.GetFilePath(Path.Combine(DefaultDirectory, fileName));
                if (File.Exists(filePath))
                {
                    Polenter.Serialization.SharpSerializer serializer = new Polenter.Serialization.SharpSerializer();

                    if (isEncrypt)
                    {
                        string temp = string.Empty;
                        if (LkEncryptDecrypt.Decrypt(File.ReadAllText(filePath), out temp))
                        {
                            File.WriteAllText(filePath + ".temp", temp);
                            obj = serializer.Deserialize(filePath + ".temp");
                            File.Delete(filePath + ".temp");
                            result = true;
                        }
                    }
                    else
                    {
                        obj = serializer.Deserialize(filePath);
                        result = true;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }
    }
}
