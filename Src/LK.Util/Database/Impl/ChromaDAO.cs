using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.IO;

namespace LK.Util
{
    class ChromaDAO : IDatabase
    {
        public ChromaDAO()
        {
            if (ClientUtils.remoteObject == null)
            {
                string hostRoot = FindHostRoot(0);
                if (!string.IsNullOrEmpty(hostRoot))
                {
                    ClientUtils.remoteObject = (RemoteObject)Activator.GetObject(typeof(RemoteObject), GetAPServerRoot(hostRoot) + "/RemoteObject");
                    ClientUtils.url = GetAPServerRoot(hostRoot);
                }
            }
        }

        private string GetAPServerRoot(string hostRoot)
        {
            string result = string.Empty;
            DataSet ds = new DataSet();
            ds.ReadXml(string.Format(@"{0}\{1}", hostRoot, LkDAO.HostFile));
            if (ds.Tables["APServer"].Rows.Count > 0)
            {
                result = ds.Tables["APServer"].Rows[0]["ref"].ToString() + "://" + ds.Tables["APServer"].Rows[0]["IP"].ToString() + ":" + ds.Tables["APServer"].Rows[0]["Port"];
            }
            return result;
        }

        private string FindHostRoot(int layer)
        {
            string root = string.Empty;
            string upLayer = string.Empty;
            for (int i = 0; i < layer; i++)
            {
                upLayer += "..\\";
            }
            upLayer = (string.IsNullOrEmpty(upLayer) ? "." : upLayer);
            DirectoryInfo dir = new DirectoryInfo(upLayer);
            DirectoryInfo[] dirs = dir.GetDirectories();
            for (int i = 0; i < dirs.Length; i++)
            {
                if (dirs[i].Name == "Host")
                {
                    root = dirs[i].FullName;
                    break;
                }
            }
            if (layer < 10 && string.IsNullOrEmpty(root))
                root = FindHostRoot(layer + 1);
            return root;
        }

        #region IDatabase
        public string GetValue(string strSQL)
        {
            DataSet ds = ClientUtils.ExecuteSQL(strSQL);
            if (ds.Tables.Count == 0)
                return string.Empty;
            if (ds.Tables[0].Rows.Count == 0)
                return string.Empty;
            if (ds.Tables[0].Columns.Count == 0)
                return string.Empty;
            return ds.Tables[0].Rows[0][0].ToString();
        }

        public string GetValue(string strSQL, List<DbParameter> parms)
        {
            object[][] oParms = new object[parms.Count][];
            for (int i = 0; i < parms.Count; i++)
            {
                oParms[i] = new object[] { parms[i].Direction, parms[i].DbType, parms[i].ParameterName, parms[i].Value };
            }
            DataSet ds = ClientUtils.ExecuteSQL(strSQL, oParms);
            if (ds.Tables.Count == 0)
                return "Could not find table";
            if (ds.Tables[0].Rows.Count == 0)
                return "Could not find data row";
            if (ds.Tables[0].Columns.Count == 0)
                return "Could not find data column";
            return ds.Tables[0].Rows[0][0].ToString();
        }

        public DataTable GetDataTable(string strSQL)
        {
            DataSet ds = ClientUtils.ExecuteSQL(strSQL);
            if (ds.Tables.Count == 0)
                return new DataTable();
            return ds.Tables[0];
        }

        public DataTable GetDataTable(string strSQL, List<System.Data.Common.DbParameter> parms)
        {
            object[][] oParms = new object[parms.Count][];
            for (int i = 0; i < parms.Count; i++)
            {
                oParms[i] = new object[] { parms[i].Direction, parms[i].DbType, parms[i].ParameterName, parms[i].Value };
            }
            DataSet ds = ClientUtils.ExecuteSQL(strSQL, oParms);
            if (ds.Tables.Count == 0)
                return new DataTable();
            return ds.Tables[0];
        }

        public DataTable GetStoredProcedure(string spName, List<DbParameter> parms)
        {
            object[][] oParms = new object[parms.Count][];
            for (int i = 0; i < parms.Count; i++)
            {
                oParms[i] = new object[] { parms[i].Direction, parms[i].DbType, parms[i].ParameterName, parms[i].Value };
            }
            DataSet ds = ClientUtils.ExecuteProc(spName, oParms);
            if (ds.Tables.Count == 0)
                return new DataTable();
            return ds.Tables[0];
        }

        public void ExecuteSQL(string strSQL)
        {
            ClientUtils.ExecuteSQL(strSQL);
        }

        public void ExecuteSQL(string strSQL, List<System.Data.Common.DbParameter> parms)
        {
            object[][] oParms = new object[parms.Count][];
            for (int i = 0; i < parms.Count; i++)
            {
                oParms[i] = new object[] { parms[i].Direction, parms[i].DbType, parms[i].ParameterName, parms[i].Value };
            }
            ClientUtils.ExecuteSQL(strSQL,oParms);
        }

        #endregion
    }
}
