using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using System.IO;
//using MSScriptControl;

namespace LK.Util
{
    //------------------
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
    //-------------------




    public class LkCalculator
    {

        private Dictionary<string, List<LkParamInfo>> m_params = new Dictionary<string, List<LkParamInfo>>();

        private const double nanValue = double.NaN;

        /// <summary>
        /// 計算
        /// </summary>
        /// <param name="param">[Param:[{A,1},{B,2},{C,3},{D,4}],Other:[{X,90},{Y,80},{Z,70}]]</param>
        public LkCalculator(Dictionary<string, List<LkParamInfo>> param)
        {
            m_params = param;
        }


        /// <summary>
        /// 實際計算（if後面不能接^）
        /// <para>41.5 = (1*2+3^4)/2</para>
        /// <para>41.5 = ($param(a) * $param(b) + $param(c)^$param(d))/$param(b)</para>
        /// <para>40   = $param(a)^2 *  ($other(y)/$param(b))</para>
        /// <para>NaN  = if($param(b) == 3,2,nan)</para>
        /// <para>74   = if($param(b) == 2,2,nan) * $param(b) + $other(z)</para>
        /// <para>100  = if($param(b) == ($param(d)/$param(b)),if($other(x) == 10,90,10),-1) * 10</para>
        /// </summary>
        /// <param name="formula">公式</param>
        /// <returns></returns>
        public double Calculate(string formula)
        {
            double value = nanValue;
            try
            {
                string actualFormula;
                bool hasIf = false;
                if (ReplaceFormula(formula, out actualFormula)
                    && ParseFormula(actualFormula, out actualFormula, out hasIf))
                {
                    //var expression = string.Format("{0}",
                    //                   new Regex(@"([\+\-\*\/\^\(\)])").Replace(actualFormula, " ${1} "));



                    //Javascript
                    if (hasIf && !double.TryParse(new JEvalDynamicCode(actualFormula).Eval().ToString(), out value))
                    {
                        value = nanValue;
                    }
                    else if (!hasIf && !double.TryParse(JEval.Eval(actualFormula).ToString(), out value))
                    {
                        value = nanValue;
                    }

                    //MSScriptControl
                    //if(!double.TryParse( new ScriptControlClass() {Language="javascript" }.Eval(actualFormula).ToString(),out value))
                    //{
                    //    value = UtilParameters.DoubleNaN;
                    //}


                    //XPath
                    //var xsltExpression = string.Format("number({0})",
                    //                   new Regex(@"([\+\-\*])").Replace(actualFormula, " ${1} ")
                    //                   .Replace("/", " div ")
                    //                   .Replace("%", " mod "));
                    ////http://www.quackit.com/xml/tutorial/xpath_number_operators.cfm
                    ////http://www.w3school.com.cn/xpath/xpath_functions.asp
                    //value = (double)new XPathDocument(new StringReader("<r/>")).CreateNavigator().Evaluate(xsltExpression);
                }
                else
                {
                    value = nanValue;
                }
            }
            catch (Exception e)
            {
                value = nanValue;
                LkLog.WriteLog(LogLevel.Error, e);
            }
            return value;
        }

        private bool ParseFormula(string formula, out string actualFormula, out bool hasIf)
        {
            hasIf = false;
            bool result = false;
            actualFormula = formula;
            try
            {
                if (actualFormula.ToUpper().Contains("IF"))
                {
                    actualFormula = ParseIfExpress(actualFormula.ToUpper()).Replace("iif", string.Empty).Replace("_", ",");

                    hasIf = true;
                }
                actualFormula = actualFormula.ToUpper().Replace("NAN", "'NaN'").ToLower();
                if (actualFormula.Contains("^"))
                {
                    Regex pow = new Regex(@"[-]?\d*[.]?\d*\^[-]?\d*[.]?\d*");
                    MatchCollection matches = pow.Matches(actualFormula);
                    foreach (Match match in matches)
                    {
                        string powReplace = string.Format("Math.pow({0})", match.Value.Replace("^", ","));
                        actualFormula = actualFormula.Replace(match.Value, powReplace);
                    }
                }

                result = true;
            }
            catch (Exception e)
            {
                LkLog.WriteLog(LogLevel.Error, e);
            }
            return result;
        }

        private string ParseIfExpress(string actualFormula)
        {
            string result = actualFormula;


            List<string> ifExpressionArray = new List<string>();

            string tempIf = string.Empty;
            int bracketCount = 0;
            int executeCount = 0;
            while (actualFormula.Contains("IF"))
            {
                int ifIndex = actualFormula.IndexOf("IF");

                for (int i = ifIndex; i < actualFormula.Length; i++)
                {
                    executeCount++;
                    char c = actualFormula[i];
                    if (!tempIf.Contains("IF(") && c.Equals(','))
                    {
                        continue;
                    }
                    if (c.Equals('('))
                    {
                        bracketCount++;
                    }
                    else if (c.Equals(')'))
                    {
                        bracketCount--;
                    }
                    tempIf += c;

                    if (tempIf.Contains("IF(") && bracketCount == 0)
                    {
                        tempIf = tempIf.Trim().Trim(System.Environment.NewLine.ToCharArray());
                        ifExpressionArray.Add(tempIf);
                        actualFormula = actualFormula.Replace(tempIf, "".PadLeft(tempIf.Length, '_'));
                        tempIf = string.Empty;
                    }
                    if (executeCount == 5000)
                    {
                        return string.Empty;
                    }
                }
            }




            string expression = string.Empty;
            string tempFormula = string.Empty;
            foreach (string match in ifExpressionArray)
            {
                tempFormula = match;
                if (tempFormula.Contains("IF"))
                {
                    tempFormula = tempFormula.Replace(match, ParseIfExpress(tempFormula.Substring(2, match.Length - 2)));
                }

                string[] tokens = tempFormula.Split(',');
                if (tokens.Length == 3)
                {
                    tokens[0] = "iif" + tokens[0];
                    for (int i = 1; i <= 2; i++)
                    {
                        if (tokens[i].Contains("iif"))
                        {
                            continue;
                        }
                        else
                        {
                            tokens[i] = "eval(" + tokens[i].Trim(')') + ")" + (tokens[i].Contains(")") ? ")" : string.Empty);
                        }

                        //tokens[i] = tokens[i].Trim(')');
                        //expression = tokens[i];
                        //if (expression.ToUpper().Contains("IF"))
                        //{
                        //    result = result.Replace(tokens[i], ParseIfExpress(expression));
                        //}
                        //else
                        //{
                        //    result = result.Replace(tokens[i], "Eval(" + tokens[i] + ")");
                        //}

                    }
                    tempFormula = tokens[0] + "?" + tokens[1] + ":" + tokens[2];
                }
                result = result.Replace(match, tempFormula);
            }
            return result;
        }

        private bool ReplaceFormula(string formula, out string actualFormula)
        {
            bool result = false;
            actualFormula = formula;
            try
            {
                double value = nanValue;
                //http://ccckmit.wikidot.com/regularexpression
                Regex regex = new Regex(@"\$.+?\(.+?\)", RegexOptions.IgnoreCase);
                MatchCollection matches = regex.Matches(formula);
                string token = string.Empty;
                bool getDataResult = false;
                foreach (Match match in matches)
                {
                    string replaceToken = match.Value;
                    token = replaceToken.ToUpper();
                    getDataResult = false;

                    string[] replaceStrArray = token.Split(" ()\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (replaceStrArray.Count() == 2)
                    {
                        string key = replaceStrArray[0].ToUpper().Replace("$", string.Empty);
                        string id = replaceStrArray[1].ToUpper();
                        getDataResult = GetData(key, id, out value);
                    }
                    if (!getDataResult)
                    {
                        return result;
                    }
                    actualFormula = actualFormula.Replace(replaceToken, (double.IsNaN(value) ? "NaN" : value.ToString()));
                }
                result = true;
            }
            catch (Exception e)
            {
                LkLog.WriteLog(LogLevel.Error, e);
            }
            return result;
        }



        private bool GetData(string type, string key, out double value)
        {
            bool result = false;
            value = nanValue;
            try
            {
                var collect = m_params.Where(item => item.Key.ToUpper().Equals(type));
                if (collect.Any())
                {
                    var attr = collect.FirstOrDefault().Value.Where(item => item.Name.ToUpper().Equals(key)).FirstOrDefault();
                    if (attr != null)
                    {
                        if (attr.DoubleValue != null)
                        {
                            value = attr.DoubleValue.Value;
                            result = true;
                        }
                        else
                        {
                            result = double.TryParse(attr.StrValue, out value);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LkLog.WriteLog(LogLevel.Error, e);
            }
            return result;

        }
    }
}
