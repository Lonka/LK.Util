using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;

namespace LK.Util
{
    class CommonDb : IDatabase
    {
        private string _connectionString;
        private CommonType _type;
        public CommonDb(string connectionString,CommonType type)
        {
            _connectionString = connectionString;
            _type = type;
        }



        private  DbProviderFactory _dbProviderFactory;
        private  DbProviderFactory dbProviderFactory
        {
            get
            {
                if (_dbProviderFactory == null)
                {
                    for (int i = 0; i < CommonDbInfo.dbInfo.Count; i++)
                    {
                        if (_type  == CommonDbInfo.dbInfo[i].dataBaseType)
                        {
                            _dbProviderFactory = DbProviderFactories.GetFactory(CommonDbInfo.dbInfo[i].dbProvider);
                            break;
                        }
                    }
                    if (_dbProviderFactory == null)
                        throw new Exception("specify DatabaseMode can not be supported.");
                }
                return _dbProviderFactory;
            }
        }
        private string GetConnectionString()
        {
            return _connectionString;
        }

        #region IDatabase
        public string GetValue(string strSQL)
        {
            string result = string.Empty;
            using (DbConnection dbConnection = dbProviderFactory.CreateConnection())
            {
                dbConnection.ConnectionString = GetConnectionString();
                dbConnection.Open();
                using (DbCommand dbCommand = dbProviderFactory.CreateCommand())
                {
                    dbCommand.Connection = dbConnection;
                    dbCommand.CommandText = strSQL;
                    DbDataReader dbDataReader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            result = dbDataReader[0].ToString();
                            break;
                        }
                    }
                    //http://www.dotblogs.com.tw/jeff-yeh/archive/2009/12/04/12286.aspx
                    dbCommand.Cancel();
                    dbDataReader.Close();
                }
            }
            return result;
        }

        public string GetValue(string strSQL, List<DbParameter> parms)
        {
            string result = string.Empty;
            using (DbConnection dbConnection = dbProviderFactory.CreateConnection())
            {
                dbConnection.ConnectionString = GetConnectionString();
                dbConnection.Open();
                using (DbCommand dbCommand = dbProviderFactory.CreateCommand())
                {
                    dbCommand.Connection = dbConnection;
                    dbCommand.CommandText = strSQL;
                    for (int i = 0; i < parms.Count; i++)
                    {
                        DbParameter dbParms = dbCommand.CreateParameter();
                        dbParms.Direction = parms[i].Direction;
                        dbParms.DbType = parms[i].DbType;
                        dbParms.ParameterName = parms[i].ParameterName;
                        dbParms.Value = parms[i].Value;
                        dbCommand.Parameters.Add(dbParms);
                    }
                    DbDataReader dbDataReader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            result = dbDataReader[0].ToString();
                            break;
                        }
                    }
                    //http://www.dotblogs.com.tw/jeff-yeh/archive/2009/12/04/12286.aspx
                    dbCommand.Cancel();
                    dbDataReader.Close();
                }
            }
            return result;
        }

        public DataTable GetDataTable(string strSQL)
        {
            DataTable result = new DataTable();
            using (DbConnection dbConnection = dbProviderFactory.CreateConnection())
            {
                dbConnection.ConnectionString = GetConnectionString();
                dbConnection.Open();
                using (DbCommand dbCommand = dbProviderFactory.CreateCommand())
                {
                    dbCommand.Connection = dbConnection;
                    dbCommand.CommandText = strSQL;
                    DbDataAdapter dbDataAdapter = dbProviderFactory.CreateDataAdapter();
                    dbDataAdapter.SelectCommand = dbCommand;
                    dbDataAdapter.Fill(result);
                    //http://www.dotblogs.com.tw/jeff-yeh/archive/2009/12/04/12286.aspx
                    dbCommand.Cancel();
                }
            }
            return result;
        }

        public DataTable GetDataTable(string strSQL, List<DbParameter> parms)
        {
            DataTable result = new DataTable();
            using (DbConnection dbConnection = dbProviderFactory.CreateConnection())
            {
                dbConnection.ConnectionString = GetConnectionString();
                dbConnection.Open();
                using (DbCommand dbCommand = dbProviderFactory.CreateCommand())
                {
                    dbCommand.Connection = dbConnection;
                    dbCommand.CommandText = strSQL;
                    for (int i = 0; i < parms.Count; i++)
                    {
                        DbParameter dbParms = dbCommand.CreateParameter();
                        dbParms.Direction = parms[i].Direction;
                        dbParms.DbType = parms[i].DbType;
                        dbParms.ParameterName = parms[i].ParameterName;
                        dbParms.Value = parms[i].Value;
                        dbCommand.Parameters.Add(dbParms);
                    }
                    DbDataAdapter dbDataAdapter = dbProviderFactory.CreateDataAdapter();
                    dbDataAdapter.SelectCommand = dbCommand;
                    dbDataAdapter.Fill(result);
                    //http://www.dotblogs.com.tw/jeff-yeh/archive/2009/12/04/12286.aspx
                    dbCommand.Cancel();
                }

            }
            return result;
        }

        public DataTable GetStoredProcedure(string spName, List<DbParameter> parms)
        {
            DataTable result = new DataTable();
            bool hasOutput = false;
            using (DbConnection dbConnection = dbProviderFactory.CreateConnection())
            {
                dbConnection.ConnectionString = GetConnectionString();
                dbConnection.Open();
                using (DbCommand dbCommand = dbProviderFactory.CreateCommand())
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Connection = dbConnection;
                    dbCommand.CommandText = spName;
                    for (int i = 0; i < parms.Count; i++)
                    {
                        DbParameter dbParms = dbCommand.CreateParameter();
                        dbParms.Direction = parms[i].Direction;
                        dbParms.DbType = parms[i].DbType;
                        dbParms.ParameterName = parms[i].ParameterName;
                        dbParms.Value = parms[i].Value;
                        dbParms.Size = parms[i].Size;
                        if (parms[i].Direction == ParameterDirection.Output)
                        {
                            dbParms.Size = 4000;
                            result.Columns.Add(parms[i].ParameterName);
                            hasOutput = true;
                        }
                        dbCommand.Parameters.Add(dbParms);
                    }
                    DbDataReader dbDataReader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
                    //http://www.dotblogs.com.tw/jeff-yeh/archive/2009/12/04/12286.aspx
                    dbCommand.Cancel();
                    dbDataReader.Close();
                    if (hasOutput)
                    {
                        DataRow dr = result.NewRow();
                        foreach (DbParameter dbParms in dbCommand.Parameters)
                        {
                            if (dbParms.Direction == ParameterDirection.Output)
                            {
                                dr[dbParms.ParameterName] = dbParms.Value;
                            }
                        }
                        result.Rows.Add(dr);
                    }
                }
            }
            return result;
        }


        public void ExecuteSQL(string strSQL)
        {
            using (DbConnection dbConnection = dbProviderFactory.CreateConnection())
            {
                dbConnection.ConnectionString = GetConnectionString();
                dbConnection.Open();
                DbTransaction dbTransaction = dbConnection.BeginTransaction();
                try
                {
                    DbCommand dbCommand = dbProviderFactory.CreateCommand();
                    dbCommand.Connection = dbConnection;
                    dbCommand.CommandText = strSQL;
                    dbCommand.Transaction = dbTransaction;
                    dbCommand.ExecuteNonQuery();
                    dbTransaction.Commit();
                    //http://www.dotblogs.com.tw/jeff-yeh/archive/2009/12/04/12286.aspx
                    dbCommand.Cancel();
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    throw ex;
                }
            }
        }

        public void ExecuteSQL(string strSQL, List<DbParameter> parms)
        {
            using (DbConnection dbConnection = dbProviderFactory.CreateConnection())
            {
                dbConnection.ConnectionString = GetConnectionString();
                dbConnection.Open();
                DbTransaction dbTransaction = dbConnection.BeginTransaction();
                try
                {
                    DbCommand dbCommand = dbProviderFactory.CreateCommand();
                    dbCommand.Connection = dbConnection;
                    dbCommand.CommandText = strSQL;
                    dbCommand.Transaction = dbTransaction;
                    for (int i = 0; i < parms.Count; i++)
                    {
                        DbParameter dbParms = dbCommand.CreateParameter();
                        dbParms.Direction = parms[i].Direction;
                        dbParms.DbType = parms[i].DbType;
                        dbParms.ParameterName = parms[i].ParameterName;
                        dbParms.Value = parms[i].Value;
                        dbCommand.Parameters.Add(dbParms);
                    }
                    dbCommand.ExecuteNonQuery();
                    dbTransaction.Commit();
                    //http://www.dotblogs.com.tw/jeff-yeh/archive/2009/12/04/12286.aspx
                    dbCommand.Cancel();
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    throw ex;
                }
            }
        }
        #endregion


    }
}
