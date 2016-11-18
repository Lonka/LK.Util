using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LK.Util;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Globalization;

using System.Xml.Linq;
using System.Xml;

namespace ForTest
{
    class Program
    {
        static void Main(string[] args)
        {
//            LkDao.DbType = LkDbType.CommonDb;
//            LkDao.CommonType = CommonType.SqlServer;
//            //LkDao.ConnectionString = "Provider=OraOLEDB.Oracle;Data Source=localhost:1521/orcl.delta.corp;User ID=c##mes_admin;Password=Admin123;";
//            LkDao.ConnectionString = "Data Source=127.0.0.1,1433;Initial Catalog=MES;User ID=sa;Password=Admin123;";

//            List<DbParameter> param = new List<DbParameter>();
//            //DateTime beginDate = (DateTime)sourceDt.AsEnumerable().Min(item => item["BEGIN_DATE"]);
//            DateTime beginDate = new DateTime(2016, 9, 23, 05, 50, 10);

//            DbParameter beginDateParam = LkDaoUtil.GetParameter(LkDao.CommonType);
//            beginDateParam.DbType = DbType.DateTime;
//            beginDateParam.Value = beginDate;
//            beginDateParam.ParameterName = "beginDate";
//            param.Add(beginDateParam);

//            //DateTime endDate = (DateTime)sourceDt.AsEnumerable().Max(item => item["END_DATE"]);
//            DateTime endDate = new DateTime(2016, 9, 23, 06, 10, 10);
//            DbParameter endDateParam = LkDaoUtil.GetParameter(LkDao.CommonType);
//            endDateParam.DbType = DbType.DateTime;
//            endDateParam.Value = endDate;
//            endDateParam.ParameterName = "endDate";
//            param.Add(endDateParam);

//            DataTable productHistory = LkDao.GetDataTable(
//                string.Format(" select * from product_pass_history where begin_date >= @beginDate and end_date <= @endDate ")
//                , param
//                );

//            int timeInterval = 180;
//            string strSql = string.Empty;
//            DateTime newTime = beginDate.GetStandardTime(timeInterval);
//            while (newTime <= endDate)
//            {
//                var coefficientGroup = productHistory.AsEnumerable().Where(item =>
//                {
//                    DateTime bDt;
//                    DateTime eDt;
//                    if (DateTime.TryParse(item["begin_date"].ToString(), out bDt) && DateTime.TryParse(item["end_date"].ToString(), out eDt))
//                    {
//                        return newTime > bDt && newTime <= eDt;
//                    }
//                    return false;
//                }).GroupBy(item => item["terminal_id"])
//                .Select(group => new
//                {
//                    key = group.Key,
//                    count = group.Count()
//                });

//                int i = 0;
//                foreach (var coefficient in coefficientGroup)
//                {
//                    if (i == 0)
//                    {
//                        strSql += string.Format("select * into #temp from (select '{0}' as terminal_id,to_date('{1}', 'YYYY/MM/DD HH24:MI:SS') as date_time,{2} as coefficient) as a;", coefficient.key, newTime.ToString("yyyy/MM/dd HH:mm:ss"), coefficient.count);
//                    }
//                    else
//                    {
//                        strSql += string.Format("insert into #temp valus ('{0}',to_date('{1}', 'YYYY/MM/DD HH24:MI:SS'),{2});", coefficient.key, newTime.ToString("yyyy/MM/dd HH:mm:ss"), coefficient.count);
//                    }
//                }
//            }

//            strSql += @"merge into product_pass_coefficient as a
//                            using #temp as b
//                            on (a.terminal_id = b.terminal_id and a.date_time = b.date_time)
//                            when matched
//                            then update set a.coefficient = b.coefficient
//                            when not matched by target
//                            then insert values (b.terminal_id,b.date_time,b.coefficient);";
//            LkDao.ExecuteSQL(strSql);

























            //DateTime.Now.GetStandardTime(180);

            #region LOG
            //LkLog.LogParams = new LkLogParams()
            //{
            //    LogRetentionDayCount = 30
            //};
            //LkLog.WriteLog(LogLevel.Error, "abc");

            //LkLogInstance log = new LkLogInstance(new LkLogParams() { 
            // LogPath=AppDomain.CurrentDomain.BaseDirectory+"abcde",
            //});
            //log.WriteLog(LogLevel.Critical, "111");
            //LK.Util.LkLog.WriteLog(LK.Util.LogLevel.Critical, "abb");
            #endregion

            #region 反射
            //object value;
            //object[] parameters = new object[] { "test" };
            //LkReflectInstance instance;
            //if (LkReflector.GetDllClassInstance(AppDomain.CurrentDomain.BaseDirectory + "Dll\\ClassLibrary1.dll", "ClassLibrary1.Class1", out instance))
            //{
            //    LkReflector.ExecuteMethod("WriteLog", instance, parameters, out value);
            //    parameters = new object[] { "lonka", "abc" };
            //    LkReflector.ExecuteMethod("WriteLog2", instance, parameters, out value);
            //}
            //string url = "http://172.16.22.112:916/test.asmx";
            string url = "http://210.22.80.17:50000/dir/wsdl?p=sa/a886e25e1f083e96ae1c381a3af3cd40";
            object value;

            LkReflectModel instance;
            if(LkReflector.GetWebServiceInstance(url, null, out instance))
            {
                
                var doc = new XDocument(
                    new XElement("DT_BJNY_Out_Req",
                        new XElement("IN_DATE",DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"))
                        )
                    );



                var ii = new DT_BJNY_Out_Req();
                ii.IN_DATE = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");



                var xmlDoc = new XmlDocument();
                var jio = xmlDoc.CreateElement("DT_BJNY_Out_Req");
                xmlDoc.AppendChild(jio);
                xmlDoc.CreateElement("DT_BJNY_Out_Req");
                XmlElement e = xmlDoc.CreateElement("IN_DATE");
                e.InnerText = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                jio.AppendChild(e);

                if (LkReflector.ExecuteMethod("SI_BJNY_Out_Syn", instance, new object[1] { xmlDoc }, out value))
                {

                }
            }

            if(LkReflector.ExecuteWebServiceMethod(url,null, "HelloWorld", null, out value))
            {

            }


            if (LkReflector.ExecuteWebServiceMethod(url, null, "GetData", new object[1] { DateTime.Now }, out value))
            {

            }

            #endregion

            #region 密碼
            //LkEncryptDecrypt.IVStr = "lonka0915";
            //LkEncryptDecrypt.KeyStr = "clare1102";
            //string result;
            //LkEncryptDecrypt.Encrypt("emsadmin", out result);
            //string result2;
            //LkEncryptDecrypt.Decrypt(result, out result2);

            #region Serialize
            //List<abc> ii = new List<abc>(){
            //    new abc(){ A=1,B=2},
            //    new abc(){ A=3,B=4}
            //};
            //LkDataSerializer.ObjectSerialize("iii", ii);
            //object obj;
            //LkDataSerializer.ObjectDeserialize("iii", out obj);
            #endregion
            #endregion

            #region calculate
            //Dictionary<string, List<LkParamInfo>> param = new Dictionary<string, List<LkParamInfo>>();
            //param["Param"] = new List<LkParamInfo>(){
            //    new LkParamInfo(){ Name = "A", DoubleValue=1},
            //    new LkParamInfo(){ Name = "B", DoubleValue=2},
            //    new LkParamInfo(){ Name = "C", DoubleValue=3},
            //    new LkParamInfo(){ Name = "D", DoubleValue=4}
            //};

            //param["Other"] = new List<LkParamInfo>(){
            //    new LkParamInfo(){ Name = "X", DoubleValue=90},
            //    new LkParamInfo(){ Name = "Y", DoubleValue=80},
            //    new LkParamInfo(){ Name = "Z", DoubleValue=70}
            //};
            //LkCalculator C = new LkCalculator(param);
            //double value = double.NaN;
            //value = C.Calculate("(1*2+3^4)/2");
            //value = C.Calculate("($param(a) * $param(b) + $param(c)^$param(d))/$param(b)");
            //value = C.Calculate("$param(a)^2 *  ($other(y)/$param(b))");
            //value = C.Calculate("if($param(b) == 3,2,nan)");
            //value = C.Calculate("if($param(b) == 2,2,nan) * $param(b) + $other(z)");
            //value = C.Calculate(@"if($param(b) == ($param(d)/$param(b)),if($other(x) == 10,90,10),-1) * 10");
            #endregion

            #region Mapping
            //DataTable a = new DataTable();
            //a.Columns.Add("A");
            //a.Columns.Add("B");
            //DataRow drX = a.NewRow();
            //drX["A"] = 1;
            //drX["B"] = 2;
            //abc item = new abc();
            //LkMapper.SetItemFromRow<abc>(item, drX);
            //abc xxx = LkMapper.CreateClassFromRow<abc>(drX);
            #endregion

            #region INI
            //LkIni.IniPath = AppDomain.CurrentDomain.BaseDirectory + "setting.ini";
            //string ii = LkIni.GetProfileString("test", "a", "AAA");
            //int oo = LkIni.GetProfileInt("test", "b", 0);
            //List<string> ojioj = LkIni.GetSections();
            //List<string> aerser = LkIni.GetKeys("test1");
            //Dictionary<string, string> aeersrrser = LkIni.GetKeyValues("test1");
            //Dictionary<string, string> allKeys = LkIni.GetKeyValues("Migrate Task ROUTE");
            #endregion

            #region SecurePass
            //string pass = string.Empty;
            //string value = "lonka0915";
            //pass = LkEncrypt.GetSecurePassword(LkEncryptType.MD5, value);
            //pass = LkEncrypt.GetSecurePassword(LkEncryptType.SHA1, value);
            //pass = LkEncrypt.GetSecurePassword(LkEncryptType.SHA256, value);
            //pass = LkEncrypt.GetSecurePassword(LkEncryptType.SHA384, value);
            //pass = LkEncrypt.GetSecurePassword(LkEncryptType.SHA512, value);
            #endregion

            #region DB


            //oracle

            //DateTime a = DateTime.Parse("09/20/2016 17:53:00");
            //string connOracle = "Data Source=127.0.0.1:1521/orcl.delta.corp;User ID=c##mes_admin;Password=Admin123;";
            //LkDaoInstance oracleDao = new LkDaoInstance(connOracle, CommonType.Oracle, null);


            //DbParameter ojaioer =  LkDaoUtil.GetParameter(CommonType.Oracle);
            //ojaioer.Value = a;
            //ojaioer.ParameterName = "end_date";
            //ojaioer.DbType = LkDaoUtil.GetDbType(typeof(DateTime));

            //List<DbParameter> oooose = new List<DbParameter>(){
            // ojaioer
            //};
            //DataTable i1231i = oracleDao.GetDataTable("select * from work_order where end_date > :end_date", oooose);


            //            oracleDao.ExecuteSQL("delete work_order");
            //            oracleDao.ExecuteSQL(@"insert all
            //            into WORK_ORDER (work_order_number,bom_id,route_id,mes_id,status,begin_date,end_date,qty) values ('23',1,1,1,'1',sysdate,sysdate,50)
            //            into WORK_ORDER (work_order_number,bom_id,route_id,mes_id,status,begin_date,end_date,qty) values ('23',1,1,1,'1',sysdate,sysdate,50)
            //            select * from dual");



            //            string connSql = "Data Source=127.0.0.1,1433;Initial Catalog=MES;User ID=sa;Password=Admin123;";
            //            LkDaoInstance sqlDao = new LkDaoInstance(connSql, CommonType.SqlServer, null);
            //            //DataTable o12313o = sqlDao.GetDataTable("select * from work_order");
            //            sqlDao.ExecuteSQL(@"insert  into WORK_ORDER (work_order_number,bom_id,route_id,mes_id,status,begin_date,end_date,qty) values ('26',1,1,1,'1',GETDATE(),GETDATE(),50);
            //
            //            insert  into WORK_ORDER (work_order_number,bom_id,route_id,mes_id,status,begin_date,end_date,qty) values ('28',1,1,1,'1',GETDATE(),GETDATE(),50);
            //
            //            ",false);



            #endregion

            #region Serializer

            //List<abc> ioser = new List<abc>();
            //ioser.Add(new abc() { A = 1, B = 2 });
            //ioser.Add(new abc() { A = 2, B = 3 });
            //LkDataSerializer.ObjectSerialize("test", ioser);
            //object oooieie;
            //LkDataSerializer.ObjectDeserialize("test", out oooieie);

            //DataTable iojio = new DataTable();
            //iojio.Columns.Add("A");
            //iojio.Columns.Add("B");
            //DataRow dr = iojio.NewRow();
            //dr["A"] = 1;
            //dr["B"] = 2;
            //iojio.Rows.Add(dr);
            //LkDataSerializer.DataTableSerialize(Path.Combine(Path.Combine("mes_1.ini", "MT1"), "abc"), iojio);
            ////LkDataSerializer.DataTableSerialize("123", iojio);
            //DataTable oiserjiso;
            //LkDataSerializer.DataTableDeserialize(Path.Combine(Path.Combine("mes_1.ini", "MT1"), "abc"), out oiserjiso);

            //string oojseirosj = @"D:\Project\Delta\prj-cn-csrgc\trunk\4-Development\40-Source\Window Service\DeltaBEMS System Service\DeltaBEMSSystemService\bin\Debug\RetryFiles\mes_1.ini\Migrate Task 1\20160921155819036";
            //LkDataSerializer.DataTableDeserialize(oojseirosj, out oiserjiso);

            #endregion

            //object obj1 = 3;
            //object obj2 = 2;
            //bool joisejrs= LkCompareUtil.Compare(LkCompareType.IsGreaterThen, typeof(int), obj1, obj2);

            //object oo1 = new DateTime(2016, 9, 20, 9, 9, 9);
            //object oo2 = new DateTime(2016, 9, 20, 9, 9, 9);
            //bool josejriso = LkCompareUtil.Compare(LkCompareType.Equal, typeof(DateTime), oo1, oo2);

            //string jojsioerjso= "abc".ParseDbField("2016/9/9", typeof(DateTime), CommonType.SqlServer);


            string joseijros = CheckExtansion("abc.dll");



        }

        private static string CheckExtansion(string file)
        {
            string result = string.Empty;
            if (file.ToUpper().LastIndexOf(".DLL") == 3)
            {
                result = file;
            }
            else if (file.ToUpper().LastIndexOf(".DLL") < 0)
            {
                result = file + ".dll";
            }
            return result;
        }

        class abc
        {
            public int A { get; set; }
            public int B { get; set; }
        }


    }

   public  class DT_BJNY_Out_Req
    {
        public string IN_DATE { get; set; }
    }
}
