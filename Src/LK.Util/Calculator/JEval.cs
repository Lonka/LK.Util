using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LK.Util
{
    public class JEval
    {
        static object m_objLock = new object();
        public static object Eval(string statement)
        {
            object objResult = null;
            lock (m_objLock)
            {
                try
                {
                    objResult = m_oEvaluatorType.InvokeMember("Eval", BindingFlags.InvokeMethod, null, m_oEvaluator,
                                                               new object[] { statement });
                }
                catch (Exception e)
                {
                    string strError = e.Message;
                }
            }
            return (objResult);
        }
        /// <summary>
        /// 
        /// </summary>
        static JEval()
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("JScript");

            CompilerParameters oParameters;
            oParameters = new CompilerParameters();
            oParameters.GenerateInMemory = true;

            CompilerResults oResults;
            oResults = provider.CompileAssemblyFromSource(oParameters, m_oJS);

            Assembly assembly = oResults.CompiledAssembly;
            m_oEvaluatorType = assembly.GetType("Evaluator");

            m_oEvaluator = Activator.CreateInstance(m_oEvaluatorType);
        }

        private static object m_oEvaluator = null;
        private static Type m_oEvaluatorType = null;
        /// <summary>
        /// JScript
        /// </summary>
        private static readonly string m_oJS =
                "class Evaluator " +
                "{ " +
                    "var a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z; " +
                    "public function Eval(expr : String) : String " +
                    "{ " +
                        "return eval(expr); " +
                    "} " +
                "}";
    }

    public class JEvalDynamicCode
    {
        static object m_objLock = new object();
        public object Eval()
        {
            object objResult = null;
            lock (m_objLock)
            {
                try
                {
                    objResult = m_oEvaluatorType.InvokeMember("Eval", BindingFlags.InvokeMethod, null, m_oEvaluator,
                                                               new object[] { string.Empty });
                }
                catch (Exception e)
                {
                    string strError = e.Message;
                }
            }
            return (objResult);
        }

        private void Initial(string code)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("JScript");

            CompilerParameters oParameters;
            oParameters = new CompilerParameters();
            oParameters.GenerateInMemory = true;

            CompilerResults oResults;
            oResults = provider.CompileAssemblyFromSource(oParameters, code);

            Assembly assembly = oResults.CompiledAssembly;
            m_oEvaluatorType = assembly.GetType("Evaluator");

            m_oEvaluator = Activator.CreateInstance(m_oEvaluatorType);
        }

        private object m_oEvaluator = null;
        private Type m_oEvaluatorType = null;
        /// <summary>
        /// JScript
        /// </summary>
        private string m_oJS =
                "class Evaluator " +
                "{ " +
                    "var a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z; " +
                    "public function Eval(expr : String) : String " +
                    "{ " +
                        "return eval(expr); " +
                    "} " +
                "}";

        public JEvalDynamicCode(string jScriptCode)
        {
            m_oJS = m_oJS.Replace("eval(expr)", jScriptCode);
            Initial(m_oJS);
        }

    }

}
