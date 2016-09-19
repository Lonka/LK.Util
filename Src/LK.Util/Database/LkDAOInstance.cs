using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
namespace LK.Util
{
    /// <summary>
    /// Data Access Object(Instance)
    /// </summary>
    public class LkDAOInstance
    {
        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="connectionString">連線字串</param>
        /// <param name="dataBaseType">連線資料庫種類(預設為Oracle)</param>
        /// <param name="useType">使用種類(預設為Self)</param>
        public LkDAOInstance(string connectionString, CommonType? dataBaseType, LkDbType? useType)
        {
            _connectionString = connectionString;
            if (dataBaseType != null)
            {
                _commonType = dataBaseType.Value;
            }
            if (useType != null)
            {
                _useType = useType.Value;
            }
        }

        private string _connectionString;

        /// <summary>
        /// 連線字串
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
            set
            {
                _connectionString = value;
            }
        }

        private CommonType _commonType = CommonType.Oracle;
        /// <summary>
        /// 連線DB的類型
        /// 可以不輸入 預設為Oracle
        /// </summary>
        public CommonType CommonType
        {
            get
            {
                return _commonType;
            }
            set
            {
                _commonType = value;
            }
        }

        private LkDbType _useType = LkDbType.CommonDb;
        /// <summary>
        /// 使用那個DAO
        /// </summary>
        public LkDbType UseType
        {
            get
            {
                return _useType;
            }
            set
            {
                _useType = value;
            }
        }

        private IDatabase _dataBase;
        private IDatabase DataBase
        {
            get
            {
                if (_dataBase == null)
                {
                    switch (UseType)
                    {
                        case LkDbType.CommonDb:
                            _dataBase = new CommonDb(ConnectionString, CommonType);
                            break;
                    }
                }
                return _dataBase;
            }
        }

        /// <summary>
        /// 取得資料(查詢句超過一個欄位時也只會回傳第一個欄位)
        /// </summary>
        /// <param name="strSQL">Select Statement</param>
        /// <returns></returns>
        public string GetValue(string strSQL)
        {
            return DataBase.GetValue(strSQL);
        }

        /// <summary>
        /// 取得資料(查詢句超過一個欄位時也只會回傳第一個欄位)
        /// </summary>
        /// <param name="strSQL">Select Statement</param>
        /// <param name="parms">Parameter</param>
        /// <returns></returns>
        public string GetValue(string strSQL, List<DbParameter> parms)
        {
            return DataBase.GetValue(strSQL, parms);
        }

        /// <summary>
        /// 取得資料表
        /// </summary>
        /// <param name="strSQL">Select Statement</param>
        /// <returns></returns>
        public DataTable GetDataTable(string strSQL)
        {
            return DataBase.GetDataTable(strSQL);
        }

        /// <summary>
        /// 取得資料表
        /// </summary>
        /// <param name="strSQL">Select Statement</param>
        /// <param name="parms">Parameter</param>
        /// <returns></returns>
        public DataTable GetDataTable(string strSQL, List<DbParameter> parms)
        {
            return DataBase.GetDataTable(strSQL, parms);
        }

        /// <summary>
        /// 執行Stored Procedure (如有要Output時請用Datatable接回回傳值)
        /// </summary>
        /// <param name="spName">Stored Procedure Name</param>
        /// <param name="parms">Parameter</param>
        /// <returns></returns>
        public DataTable GetStoredProcedure(string spName, List<DbParameter> parms)
        {
            return DataBase.GetStoredProcedure(spName, parms);
        }

        /// <summary>
        /// 執行SQL Statement
        /// </summary>
        /// <param name="strSQL">SQL Statement</param>
        public void ExecuteSQL(string strSQL)
        {
            DataBase.ExecuteSQL(strSQL);
        }

        /// <summary>
        /// 執行SQL Statement
        /// </summary>
        /// <param name="strSQL">SQL Statement</param>
        /// <param name="parms">Parameter</param>
        public void ExecuteSQL(string strSQL, List<DbParameter> parms)
        {
            DataBase.ExecuteSQL(strSQL, parms);
        }
    }
}
