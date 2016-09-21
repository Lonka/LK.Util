using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

namespace LK.Util
{
    internal interface IDatabase
    {
        string GetValue(string strSQL);
        string GetValue(string strSQL, List<DbParameter> parms);
        DataTable GetDataTable(string strSQL);
        DataTable GetDataTable(string strSQL, List<DbParameter> parms);
        DataTable GetStoredProcedure(string spName, List<DbParameter> parms);
        void ExecuteSQL(string strSQL,bool isTransaction=true);
        void ExecuteSQL(string strSQL, List<DbParameter> parms, bool isTransaction = true);
    }
}
