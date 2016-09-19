using System;
using System.Collections.Generic;
using System.Text;

namespace LK.Util
{
    /// <summary>
    /// 把所有DbProviderFactory支援的種類加入dbInfo的List中
    /// </summary>
    internal class CommonDbInfo
    {
        public CommonType dataBaseType { get; set; }

        public string dbProvider { get; set; }

        private static List<CommonDbInfo> _dbInfo;

        /// <summary>
        /// 所有DbProviderFactory支援的種類的List，
        /// dataBaseType:type(Oracle)，
        /// dbProvider:Dll(System.Data.OracleClient)
        /// </summary>
        public static List<CommonDbInfo> dbInfo
        {
            get
            {
                if (_dbInfo == null)
                    _dbInfo = new List<CommonDbInfo>();
                _dbInfo.Add(new CommonDbInfo() { dataBaseType = CommonType.MySql, dbProvider = "MySql.Data.MySqlClient" });
                _dbInfo.Add(new CommonDbInfo() { dataBaseType = CommonType.SqlServer, dbProvider = "System.Data.SqlClient" });
                _dbInfo.Add(new CommonDbInfo() { dataBaseType = CommonType.Oracle, dbProvider = "System.Data.OracleClient" });
                _dbInfo.Add(new CommonDbInfo() { dataBaseType = CommonType.Teradata, dbProvider = "Teradata.Client.Provider" });
                _dbInfo.Add(new CommonDbInfo() { dataBaseType = CommonType.Sqlite, dbProvider = "System.Data.SQLite" });
                _dbInfo.Add(new CommonDbInfo() { dataBaseType = CommonType.OleDb, dbProvider = "System.Data.OleDb" });
                return _dbInfo;
            }
        }


        //providerInvariantNames.Add(DbProviderType.SqlServer, "System.Data.SqlClient");  
        //providerInvariantNames.Add(DbProviderType.OleDb, "System.Data.OleDb");  
        //providerInvariantNames.Add(DbProviderType.ODBC, "System.Data.ODBC");  
        //providerInvariantNames.Add(DbProviderType.Oracle, "Oracle.DataAccess.Client");  
        //providerInvariantNames.Add(DbProviderType.MySql, "MySql.Data.MySqlClient");  
        //providerInvariantNames.Add(DbProviderType.SQLite, "System.Data.SQLite");  
        //providerInvariantNames.Add(DbProviderType.Firebird, "FirebirdSql.Data.Firebird");  
        //providerInvariantNames.Add(DbProviderType.PostgreSql, "Npgsql");  
        //providerInvariantNames.Add(DbProviderType.DB2, "IBM.Data.DB2.iSeries");  
        //providerInvariantNames.Add(DbProviderType.Informix, "IBM.Data.Informix");  
        //providerInvariantNames.Add(DbProviderType.SqlServerCe, "System.Data.SqlServerCe"); 
    }
}
