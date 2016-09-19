using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;

namespace LK.Util
{
    //---------example------------
    //object value;
    //object[] parameters =  new object[] { "lonka", "clare" };
    //LkReflectInstance instance;
    //if (LkReflector.GetDllClassInstance(AppDomain.CurrentDomain.BaseDirectory + "ClassLibrary1.dll", "ClassLibrary1.Class1", out instance))
    //{
    //    LkReflector.ExecuteMethod("Method", instance, parameters, out value);
    //}
    //---------------------------





    public class LkReflector
    {
        /// <summary>
        /// 取得Dll中某個Class的Instance
        /// </summary>
        /// <param name="dllPath">DLL 的路徑（絕對路徑）</param>
        /// <param name="className">Class name（ClassLibrary1.Class1）</param>
        /// <param name="output">輸出ReflectInstance，之後可以用該實體去呼叫Method</param>
        /// <returns></returns>
        public static bool GetDllClassInstance(string dllPath, string className, out LkReflectInstance output)
        {
            output = new LkReflectInstance();
            bool result = false;
            try
            {
                //output.AppDomainObj = AppDomain.CreateDomain("some");
                //Assembly assembly = null;
                //output.AppDomainObj.DoCallBack(() =>
                //{
                //    assembly = Assembly.LoadFrom(dllPath);
                //});
                //output.AssemblyDll = assembly;

                //TODO 不會被lock的方法
                //byte[] dllBytes = System.IO.File.ReadAllBytes(dllPath);
                //output.AssemblyDll = Assembly.Load(dllBytes);

                //TODO 省memory
                output.AssemblyDll = Assembly.LoadFile(dllPath);
                output.ClassType = output.AssemblyDll.GetType(className);
                output.ClassInstance = Activator.CreateInstance(output.ClassType);
                result = true;
            }
            catch (Exception e)
            {
                LkLog.WriteLog(LogLevel.Error, e);
            }
            return result;
        }

        /// <summary>
        /// 執行Method
        /// </summary>
        /// <param name="dllPath">DLL 的路徑（絕對路徑）></param>
        /// <param name="className">Class name</param>
        /// <param name="methodName">Method name</param>
        /// <param name="input">傳入的參數</param>
        /// <param name="output">回傳結果</param>
        /// <returns></returns>
        public static bool ExecuteMethod(string dllPath, string className, string methodName, object input, out object output)
        {
            bool result = false;
            output = null;
            try
            {
                Assembly assembly = Assembly.LoadFile(dllPath);
                Type type = assembly.GetType(className);

                object instance = Activator.CreateInstance(type);
                MethodInfo methodInfo = type.GetMethod(methodName);
                if (input == null)
                {
                    output = methodInfo.Invoke(instance, null);
                }
                else
                {
                    output = methodInfo.Invoke(instance, new object[] { input });
                }
                result = true;
            }
            catch (Exception e)
            {
                LkLog.WriteLog(LogLevel.Error, e);
            }
            return result;
        }

        /// <summary>
        /// 執行Method
        /// </summary>
        /// <param name="methodName">Method name</param>
        /// <param name="classInstance">要呼叫的Class Instance</param>
        /// <param name="output">回傳結果</param>
        /// <returns></returns>
        public static bool ExecuteMethod(string methodName, LkReflectInstance classInstance, out object output)
        {
            bool result = false;
            output = null;
            try
            {
                MethodInfo methodInfo = classInstance.ClassType.GetMethod(methodName);
                output = methodInfo.Invoke(classInstance.ClassInstance, null);
                result = true;
            }
            catch (Exception e)
            {
                LkLog.WriteLog(LogLevel.Error, e);
            }
            return result;
        }

        /// <summary>
        /// 執行Method
        /// </summary>
        /// <param name="methodName">Method name</param>
        /// <param name="classInstance">要呼叫的Class Instance</param>
        /// <param name="input">傳入的參數</param>
        /// <param name="output">回傳結果</param>
        /// <returns></returns>
        public static bool ExecuteMethod(string methodName, LkReflectInstance classInstance, object input, out object output)
        {
            bool result = false;
            output = null;
            try
            {
                MethodInfo methodInfo = classInstance.ClassType.GetMethod(methodName);
                output = methodInfo.Invoke(classInstance.ClassInstance, new object[] { input });
                result = true;
            }
            catch (Exception e)
            {
                LkLog.WriteLog(LogLevel.Error, "DLL:" + classInstance.AssemblyDll.ManifestModule.Name + " Class:" + classInstance.ClassType.Name + " Method:" + methodName, e);
            }
            return result;
        }

        /// <summary>
        /// 執行Method
        /// </summary>
        /// <param name="methodName">Method name</param>
        /// <param name="classInstance">要呼叫的Class Instance</param>
        /// <param name="input">傳入的參數</param>
        /// <param name="output">回傳結果</param>
        /// <returns></returns>
        public static bool ExecuteMethod(string methodName, LkReflectInstance classInstance, object[] input, out object output)
        {
            bool result = false;
            output = null;
            try
            {
                MethodInfo methodInfo = classInstance.ClassType.GetMethod(methodName);
                output = methodInfo.Invoke(classInstance.ClassInstance, input);
                result = true;
            }
            catch (Exception e)
            {
                LkLog.WriteLog(LogLevel.Error, e);
            }
            return result;
        }

    }
}
