using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;

namespace LK.Util
{
    /// <summary>
    /// Data Access Object(Static),請先給好ConnectionString(Chroma可以不給),DataBaseType(Oracle),UseType(Chroma)
    /// </summary>
    public class LkDAO
    {
        private static string _connectionString;

        /// <summary>
        /// 連線字串
        /// </summary>
        public static string ConnectionString
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

        private static CommonType _commonType = CommonType.Oracle;
        /// <summary>
        /// 連線DB的類型
        /// 可以不輸入 預設為Oracle
        /// </summary>
        public static CommonType CommonType
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

        private static LkDbType _useType = LkDbType.CommonDb;
        /// <summary>
        /// 使用那個DAO
        /// </summary>
        public static LkDbType UseType
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

        private static IDatabase _dataBase;
        private static IDatabase DataBase
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
        public static string GetValue(string strSQL)
        {
            return DataBase.GetValue(strSQL);
        }

        /// <summary>
        /// 取得資料(查詢句超過一個欄位時也只會回傳第一個欄位)
        /// </summary>
        /// <param name="strSQL">Select Statement</param>
        /// <param name="parms">Parameter</param>
        /// <returns></returns>
        public static string GetValue(string strSQL, List<DbParameter> parms)
        {
            return DataBase.GetValue(strSQL, parms);
        }

        /// <summary>
        /// 取得資料表
        /// </summary>
        /// <param name="strSQL">Select Statement</param>
        /// <returns></returns>
        public static DataTable GetDataTable(string strSQL)
        {
            return DataBase.GetDataTable(strSQL);
        }

        /// <summary>
        /// 取得資料表
        /// </summary>
        /// <param name="strSQL">Select Statement</param>
        /// <param name="parms">Parameter</param>
        /// <returns></returns>
        public static DataTable GetDataTable(string strSQL, List<DbParameter> parms)
        {
            return DataBase.GetDataTable(strSQL, parms);
        }

        /// <summary>
        /// 執行Stored Procedure (如有要Output時請用Datatable接回回傳值)
        /// </summary>
        /// <param name="spName">Stored Procedure Name</param>
        /// <param name="parms">Parameter</param>
        /// <returns></returns>
        public static DataTable GetStoredProcedure(string spName, List<DbParameter> parms)
        {
            return DataBase.GetStoredProcedure(spName, parms);
        }

        /// <summary>
        /// 執行SQL Statement
        /// </summary>
        /// <param name="strSQL">SQL Statement</param>
        public static void ExecuteSQL(string strSQL)
        {
            DataBase.ExecuteSQL(strSQL);
        }

        /// <summary>
        /// 執行SQL Statement
        /// </summary>
        /// <param name="strSQL">SQL Statement</param>
        /// <param name="parms">Parameter</param>
        public static void ExecuteSQL(string strSQL, List<DbParameter> parms)
        {
            DataBase.ExecuteSQL(strSQL, parms);
        }

    }
}
