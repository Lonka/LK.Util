using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
namespace LK.Util
{
    /// <summary>
    /// 寫完後才發現XPath的好用，如：XmlDocument.SelectNodes("/root/info[contains(@name,'Lonka') and @sex='M' and @age>11]") 找出info節點中屬性name like Lonka和屬性sex = 'M'和屬性age > 11。找出來後再處理
    /// </summary>
    public class LkXml
    {
        private XmlDocument doc;
        private XmlElement currentElement;
        private string _saveName = string.Empty;
        /// <summary>
        /// 儲存的檔案路徑
        /// </summary>
        public string SaveName
        {
            get
            {
                if (string.IsNullOrEmpty(_saveName))
                {
                    _saveName = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
                }
                return _saveName;
            }
            set
            {
                _saveName = value;
            }
        }

        /// <summary>
        /// 如要讀檔請呼叫LoadXml(請先using System.Xml)
        /// </summary>
        private LkXml()
        {
            doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", ""));//將宣告節點加入document中        
        }

        /// <summary>
        /// 讀Xml檔
        /// </summary>
        /// <param name="fileRoot">檔名(請給副檔名)</param>
        public void LoadXml(string fileRoot)
        {
            doc.Load(fileRoot);

            for (int i = 0; i < doc.ChildNodes.Count; i++)
            {
                if (doc.ChildNodes[i].NodeType == XmlNodeType.XmlDeclaration)
                {
                    continue;
                }
                else if (doc.ChildNodes[i].NodeType == XmlNodeType.Element)
                {
                    currentElement = doc.ChildNodes[i] as XmlElement;
                    break;
                }
            }
        }

        #region Select Element

        #region Single

        /// <summary>
        /// 找單個Element,返回第一個找到的Element
        /// </summary>
        /// <param name="elementName">Element Name</param>
        /// <returns></returns>
        public XmlElement SelElement(string elementName)
        {
            XmlElement result = null;
            XmlNode node = doc.SelectSingleNode(elementName);
            if (node == null)
            {
                result = RecursiveSelElement(doc.DocumentElement, elementName);
            }
            else
            {
                result = node as XmlElement;
            }
            return result;
        }

        /// <summary>
        /// 遞迴去找出Element
        /// </summary>
        /// <param name="source">Element</param>
        /// <param name="searchElementName">目標的Element Name</param>
        /// <returns></returns>
        private XmlElement RecursiveSelElement(XmlElement source, string searchElementName)
        {
            XmlElement result = null;
            foreach (XmlElement element in source.ChildNodes)
            {
                if (element.Name == searchElementName)
                {
                    result = element;
                    return result;
                }
                else if (element.ChildNodes.Count > 0)
                {
                    result = RecursiveSelElement(element, searchElementName);
                    if (result != null)
                        return result;
                }
            }
            return result;
        }
        #endregion

        #region Multiple and one condition

        /// <summary>
        /// 找多個Element
        /// </summary>
        /// <param name="elementName">Element Name</param>
        /// <returns></returns>
        public List<XmlElement> SelElements(string elementName)
        {
            return DoSelElements(elementName, string.Empty, string.Empty, WhereMode.Equal);
        }

        /// <summary>
        /// 找多個Element
        /// </summary>
        /// <param name="elementName">Element Name</param>
        /// <param name="whereKey">要查詢的Key</param>
        /// <param name="whereValue">要符合條件的Value</param>
        /// <param name="whereMode">比對條件</param>
        /// <returns></returns>
        public List<XmlElement> SelElements(string elementName, string whereKey, string whereValue, WhereMode whereMode)
        {
            return DoSelElements(elementName, whereKey, whereValue, whereMode);
        }

        /// <summary>
        /// 實際執行找Element
        /// </summary>
        /// <param name="elementName">Element Name</param>
        /// <param name="whereKey">要查詢的Key</param>
        /// <param name="whereValue">要符合條件的Value</param>
        /// <param name="whereMode">比對條件</param>
        /// <returns></returns>
        private List<XmlElement> DoSelElements(string elementName, string whereKey, string whereValue, WhereMode whereMode)
        {
            List<XmlElement> result = new List<XmlElement>();
            XmlNodeList nodes = doc.SelectNodes(elementName);
            if (nodes.Count == 0)
            {
                return RecursiveSelElements(doc.DocumentElement, elementName, whereKey, whereValue, whereMode);
            }
            else
            {
                foreach (XmlNode node in nodes)
                {
                    if (string.IsNullOrEmpty(whereKey))
                    {
                        result.Add(node as XmlElement);
                    }
                    else if (node.Attributes[whereKey] == null)
                    {
                        throw new Exception(whereKey + "-無法對應的Key值");
                    }
                    else if (CheckWhere(node.Attributes[whereKey].Value, whereValue, whereMode))
                    {
                        result.Add(node as XmlElement);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 遞迴找Element
        /// </summary>
        /// <param name="source">來源的Element</param>
        /// <param name="searchElementName">目標的Element Name</param>
        /// <param name="whereKey">要查詢的Key</param>
        /// <param name="whereValue">要符合條件的Value</param>
        /// <param name="whereMode">比對條件</param>
        /// <returns></returns>
        private List<XmlElement> RecursiveSelElements(XmlElement source, string searchElementName, string whereKey, string whereValue, WhereMode whereMode)
        {
            List<XmlElement> result = new List<XmlElement>();
            foreach (XmlElement element in source.ChildNodes)
            {
                if (element.Name == searchElementName)
                {
                    if (string.IsNullOrEmpty(whereKey))
                    {
                        result.Add(element);
                    }
                    else if (element.Attributes[whereKey] == null)
                    {
                        throw new Exception(whereKey + "-無法對應的Key值");
                    }
                    else if (CheckWhere(element.Attributes[whereKey].Value, whereValue, whereMode))
                    {
                        result.Add(element);
                    }
                }
                else if (element.ChildNodes.Count > 0)
                {
                    List<XmlElement> nodesResult = RecursiveSelElements(element, searchElementName, whereKey, whereValue, whereMode);
                    foreach (XmlElement xe in nodesResult)
                        result.Add(xe);
                }
            }
            return result;
        }

        /// <summary>
        /// 判斷兩數是否有符合運算子
        /// </summary>
        /// <param name="keyValue">來源值</param>
        /// <param name="whereValue">比較值</param>
        /// <param name="whereMode">運算子種類</param>
        /// <returns></returns>
        private bool CheckWhere(string keyValue, string whereValue, WhereMode whereMode)
        {
            Xml.IOperator iOperator = Xml.Expression.ConstructOperator(whereMode);
            return iOperator.IsCompare(keyValue, whereValue);
        }
        #endregion

        #region Multiple and more condition
        /// <summary>
        /// 找多個Element
        /// </summary>
        /// <param name="elementName">Element Name</param>
        /// <param name="wheres">比對條件</param>
        /// <returns></returns>
        public List<XmlElement> SelElements(string elementName, List<WhereCondition> wheres)
        {
            List<XmlElement> result = new List<XmlElement>();
            XmlNodeList nodes = doc.SelectNodes(elementName);
            if (nodes.Count == 0)
            {
                return RecursiveSelElements(doc.DocumentElement, elementName, wheres);
            }
            else
            {
                foreach (XmlNode node in nodes)
                {
                    CheckAttributes(node as XmlElement, wheres);
                    if (CheckWhere(node as XmlElement, wheres))
                    {
                        result.Add(node as XmlElement);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 遞迴找Element
        /// </summary>
        /// <param name="source">來源Element</param>
        /// <param name="searchElementName">目標的Element Name</param>
        /// <param name="wheres">比對條件</param>
        /// <returns></returns>
        private List<XmlElement> RecursiveSelElements(XmlElement source, string searchElementName, List<WhereCondition> wheres)
        {
            List<XmlElement> result = new List<XmlElement>();
            foreach (XmlElement element in source.ChildNodes)
            {
                if (element.Name == searchElementName)
                {
                    CheckAttributes(element, wheres);
                    if (CheckWhere(element, wheres))
                    {
                        result.Add(element);
                    }
                }
                else if (element.ChildNodes.Count > 0)
                {
                    List<XmlElement> nodesResult = RecursiveSelElements(element, searchElementName, wheres);
                    foreach (XmlElement xe in nodesResult)
                        result.Add(xe);
                }
            }
            return result;
        }

        /// <summary>
        /// 判斷Attribute名中是否都有條件值的Attribute西
        /// </summary>
        /// <param name="sourceElement">來源 Element</param>
        /// <param name="wheres">條件值</param>
        /// <returns></returns>
        private bool CheckAttributes(XmlElement sourceElement, List<WhereCondition> wheres)
        {
            foreach (WhereCondition where in wheres)
            {
                if (sourceElement.Attributes[where.Key] == null)
                {
                    throw new Exception(where.Key + "-無法對應的Key值");
                }
            }
            return true;
        }

        /// <summary>
        /// 判斷兩數是否有符合運算子
        /// </summary>
        /// <param name="sourceElement">來源 Element</param>
        /// <param name="wheres">條件值</param>
        /// <returns></returns>
        private bool CheckWhere(XmlElement sourceElement, List<WhereCondition> wheres)
        {
            foreach (WhereCondition where in wheres)
            {
                if (!CheckWhere(sourceElement.Attributes[where.Key].Value, where.Value, where.Mode))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #endregion

        #region Add Element

        /// <summary>
        /// 新增初始節點 [root /]
        /// </summary>
        /// <param name="elementName"></param>
        public void AddRootElement(string elementName)
        {
            currentElement = doc.CreateElement(elementName);
            doc.AppendChild(currentElement);
        }

        /// <summary>
        /// 新增節點 [node /]
        /// </summary>
        /// <param name="rootElementName">父項節點</param>
        /// <param name="elementName">要新增的節點名</param>
        public void AddElement(string rootElementName, string elementName)
        {
            if (currentElement == null)
            {
                AddRootElement(rootElementName);
            }
            else if (currentElement.Name != rootElementName)
            {
                currentElement = SelElement(rootElementName);
            }
            XmlElement element = doc.CreateElement(elementName);
            currentElement.AppendChild(element);
        }

        /// <summary>
        /// 新增節點含一組屬性 [node key=value /]
        /// </summary>
        /// <param name="rootElementName"></param>
        /// <param name="elementName"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddElement(string rootElementName, string elementName, string key, string value)
        {
            if (currentElement == null)
            {
                AddRootElement(rootElementName);
            }
            else if (currentElement.Name != rootElementName)
            {
                currentElement = SelElement(rootElementName);
            }
            XmlElement element = doc.CreateElement(elementName);
            element.SetAttribute(key, value);
            currentElement.AppendChild(element);
        }

        /// <summary>
        /// 新增節點含多組屬性 [node key1=value1 key2=value2 /]
        /// </summary>
        /// <param name="rootElementName"></param>
        /// <param name="elementName"></param>
        /// <param name="attribute"></param>
        public void AddElement(string rootElementName, string elementName, Dictionary<string, string> attribute)
        {
            if (currentElement == null)
            {
                AddRootElement(rootElementName);
            }
            else if (currentElement.Name != rootElementName)
            {
                currentElement = SelElement(rootElementName);
            }
            XmlElement element = doc.CreateElement(elementName);
            foreach (KeyValuePair<string, string> dic in attribute)
            {
                element.SetAttribute(dic.Key, dic.Value);
            }
            currentElement.AppendChild(element);
        }

        /// <summary>
        /// 新增多筆節點含多組屬性 [node1 key1(column1)=value1(row1[column1]) key2(column2)=value2(row1[column2]) /] [node2 key1(column1)=value1(row2[column1]) key2(column2)=value2(row2[column2]) /]
        /// </summary>
        /// <param name="rootElementName"></param>
        /// <param name="elements"></param>
        public void AddElement(string rootElementName, DataTable elements)
        {
            if (currentElement == null)
            {
                AddRootElement(rootElementName);
            }
            else if (currentElement.Name != rootElementName)
            {
                currentElement = SelElement(rootElementName);
            }
            foreach (DataRow dr in elements.Rows)
            {
                XmlElement element = doc.CreateElement(elements.TableName);
                foreach (DataColumn dc in elements.Columns)
                {
                    element.SetAttribute(dc.ColumnName, dr[dc.ColumnName].ToString());
                }
                currentElement.AppendChild(element);
            }
        }

        #endregion

        /// <summary>
        /// 存Xml檔
        /// </summary>
        /// <param name="fileRoot">檔名(請給副檔名)</param>
        public void SaveXml(string fileName)
        {
            _saveName = fileName;
            doc.Save(SaveName);
        }

        /// <summary>
        /// 存Xml檔
        /// </summary>
        public void SaveXml()
        {
            doc.Save(SaveName);
        }

    }
}
