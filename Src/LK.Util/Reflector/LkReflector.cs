using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Web.Services.Description;
using System.Xml.Serialization;

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
        /// <param name="dllPath">DLL 的路徑</param>
        /// <param name="className">Class name（ClassLibrary1.Class1）</param>
        /// <param name="output">輸出ReflectInstance，之後可以用該實體去呼叫Method</param>
        /// <returns></returns>
        public static bool GetDllClassInstance(string dllPath, string className, out LkReflectModel output)
        {
            output = new LkReflectModel();
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
                output.Assembly = Assembly.LoadFile(LkCommonUtil.GetFilePath(dllPath.CheckExtansion("dll")));
                output.Class = output.Assembly.GetType(className);
                output.ClassInstance = Activator.CreateInstance(output.Class);
                result = true;
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        public static bool GetWebServiceInstance(string url, string[] assemblyReferences, out LkReflectModel output)
        {
            //http://www.codeproject.com/Articles/18950/Dynamic-Discovery-and-Invocation-of-Web-Services
            //https://dotblogs.com.tw/jimmyyu/archive/2009/04/22/8139.aspx

            output = new LkReflectModel();
            if (assemblyReferences == null)
            {
                assemblyReferences = new string[3] { "System.Web.Services.dll", "System.Xml.dll", "System.Data.dll" };
            }
            bool result = false;
            try
            {
                WebRequest webRequest = WebRequest.Create(url + "?WSDL");
                
                //有要權限的話要加下面
                //webRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;
                //webRequest.PreAuthenticate = true;
                Stream requestStream = webRequest.GetResponse().GetResponseStream();
                // Get a WSDL file describing a service
                //ServiceDescription类提供一种方法，以创建和格式化用于描述 XML Web services 的有效的 Web 服务描述语言 (WSDL) 文档文件，该文件是完整的，具有适当的命名空间、元素和特性。 无法继承此类。
                //ServiceDescription.Read 方法 (Stream) 通过直接从 Stream实例加载 XML 来初始化ServiceDescription类的实例。
                ServiceDescription sd = ServiceDescription.Read(requestStream);
                string sdName = sd.Services[0].Name;

                // Initialize a service description servImport
                //ServiceDescriptionImporter 类 公开一种为 XML Web services 生成客户端代理类的方法。
                //ServiceDescriptionImporter.AddServiceDescription 方法 将指定的ServiceDescription添加到要导入的ServiceDescriptions值的集合中。
                ServiceDescriptionImporter servImport = new ServiceDescriptionImporter();
                servImport.AddServiceDescription(sd, String.Empty, String.Empty);
                servImport.ProtocolName = "Soap";
                servImport.CodeGenerationOptions = CodeGenerationOptions.GenerateProperties;

                //CodeNamespace表示命名空间声明。
                CodeNamespace nameSpace = new CodeNamespace();
                //CodeCompileUnit会提供一个CodeDOM程式圆形的容器，CodeCompileUnit含有一个集合，可以储存含有CodeDOM原始程式码原形，专案参考的组件集合以及专案组件属性集合的CodeNamespace物件。
                CodeCompileUnit codeCompileUnit = new CodeCompileUnit();
                codeCompileUnit.Namespaces.Add(nameSpace);
                // Set Warnings
                ServiceDescriptionImportWarnings warnings = servImport.Import(nameSpace, codeCompileUnit);

                if (warnings == 0)
                {
                    StringWriter stringWriter = new StringWriter(System.Globalization.CultureInfo.CurrentCulture);
                    //CSharpCodeProvider类提供存取C#程式码产生器和程式码编译器的执行个体。
                    Microsoft.CSharp.CSharpCodeProvider prov = new Microsoft.CSharp.CSharpCodeProvider();
                    // 取得C#程式码编译器的执行个体
                    prov.GenerateCodeFromNamespace(nameSpace, stringWriter, new CodeGeneratorOptions());

                    // Compile the assembly with the appropriate references
                    // 创建编译器的参数实例
                    CompilerParameters param = new CompilerParameters(assemblyReferences);
                    param.GenerateExecutable = false;
                    param.GenerateInMemory = true;
                    param.TreatWarningsAsErrors = false;
                    param.WarningLevel = 4;

                    // CompilerResults表示从编译器返回的编译结果。使用指定的编译器设定，根据CodeCompileUnit物件之指定阵列所包含的System.CodeDom树状结构，编译一个组件。
                    CompilerResults results = new CompilerResults(new TempFileCollection());
                    // Compile
                    results = prov.CompileAssemblyFromDom(param, codeCompileUnit);

                    if (true == results.Errors.HasErrors)
                    {
                        System.Text.StringBuilder tStr = new System.Text.StringBuilder();
                        foreach (System.CodeDom.Compiler.CompilerError tComError in results.Errors)
                        {
                            tStr.Append(tComError.ToString());
                            tStr.Append(System.Environment.NewLine);
                        }
                        throw new Exception(tStr.ToString());
                    }

                    output.Assembly = results.CompiledAssembly;
                    output.Class = output.Assembly.GetType(sdName);
                    output.ClassInstance = Activator.CreateInstance(output.Class);
                    result = true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        public static bool ExecuteWebServiceMethod(string url, string[] assemblyReferences, string methodName, object input, out object output)
        {
            bool result = false;
            output = null;
            try
            {
                LkReflectModel instance;
                if (GetWebServiceInstance(url, assemblyReferences, out instance))
                {
                    result = ExecuteMethod(methodName, instance, input, out output);
                }
            }
            catch (Exception e)
            {
                throw (e);
            }
            return result;
        }

        public static bool ExecuteWebServiceMethod(string url, string[] assemblyReferences, string methodName, object[] input, out object output)
        {
            bool result = false;
            output = null;
            try
            {
                LkReflectModel instance;
                if (GetWebServiceInstance(url, assemblyReferences, out instance))
                {
                    result = ExecuteMethod(methodName, instance, input, out output);
                }
            }
            catch (Exception e)
            {
                throw (e);
            }
            return result;
        }

        /// <summary>
        /// 執行Method
        /// </summary>
        /// <param name="dllPath">DLL 的路徑></param>
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
                Assembly assembly = Assembly.LoadFile(LkCommonUtil.GetFilePath(dllPath.CheckExtansion("dll")));
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
                throw e;
            }
            return result;
        }

        /// <summary>
        /// 執行Method
        /// </summary>
        /// <param name="dllPath">DLL 的路徑></param>
        /// <param name="className">Class name</param>
        /// <param name="methodName">Method name</param>
        /// <param name="input">傳入的參數</param>
        /// <param name="output">回傳結果</param>
        /// <returns></returns>
        public static bool ExecuteMethod(string dllPath, string className, string methodName, object[] input, out object output)
        {
            bool result = false;
            output = null;
            try
            {
                Assembly assembly = Assembly.LoadFile(LkCommonUtil.GetFilePath(dllPath.CheckExtansion("dll")));
                Type type = assembly.GetType(className);
                object instance = Activator.CreateInstance(type);
                MethodInfo methodInfo = type.GetMethod(methodName);
                output = methodInfo.Invoke(instance, input);
                result = true;
            }
            catch (Exception e)
            {
                throw e;
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
        public static bool ExecuteMethod(string methodName, LkReflectModel classInstance, out object output)
        {
            bool result = false;
            output = null;
            try
            {
                MethodInfo methodInfo = classInstance.Class.GetMethod(methodName);
                output = methodInfo.Invoke(classInstance.ClassInstance, null);
                result = true;
            }
            catch (Exception e)
            {
                throw e;
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
        public static bool ExecuteMethod(string methodName, LkReflectModel classInstance, object input, out object output)
        {
            bool result = false;
            output = null;
            try
            {
                MethodInfo methodInfo = classInstance.Class.GetMethod(methodName);
                output = methodInfo.Invoke(classInstance.ClassInstance, new object[] { input });
                result = true;
            }
            catch (Exception e)
            {
                //LkLog.WriteLog(LogLevel.Error, "DLL:" + classInstance.AssemblyDll.ManifestModule.Name + " Class:" + classInstance.ClassType.Name + " Method:" + methodName, e);
                throw e;
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
        public static bool ExecuteMethod(string methodName, LkReflectModel classInstance, object[] input, out object output)
        {
            bool result = false;
            output = null;
            try
            {
                MethodInfo methodInfo = classInstance.Class.GetMethod(methodName);
                output = methodInfo.Invoke(classInstance.ClassInstance, input);
                result = true;
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }




    }
}
