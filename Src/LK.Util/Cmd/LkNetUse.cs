using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace LK.Util
{
    public class LkNetUse
    {
        private string remoteHost = string.Empty;
        private string account = string.Empty;
        private string password = string.Empty;

        /// <summary>
        /// 網路磁碟機設定連線組態(需要可以執行cmd.exe)
        /// </summary>
        /// <param name="_remoteHost">要連線的主機(\\127.0.0.1)</param>
        /// <param name="_account">登入帳號</param>
        /// <param name="_password">登入密碼</param>
        public LkNetUse(string _remoteHost, string _account, string _password)
        {
            if (string.IsNullOrEmpty(_remoteHost))
            {
                throw new Exception("請設定要連線的主機");
            }
            if (string.IsNullOrEmpty(_account))
            {
                throw new Exception("請設定登入的帳號");
            }
            remoteHost = _remoteHost.Replace("/", "\\").TrimEnd('\\');
            account = _account;
            password = (string.IsNullOrEmpty(_password) ? "''" : _password);
        }

        /// <summary>
        /// 設定Net Use
        /// </summary>
        public bool Connect()
        {

            bool Flag = true;
            Process proc = new Process();
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.CreateNoWindow = true;

            try
            {

                proc.Start();
                string command = @"net  use  " + remoteHost + "  " + password + "  " + "  /user:" + account + ">NUL";
                proc.StandardInput.WriteLine(command);
                command = "exit";
                proc.StandardInput.WriteLine(command);

                while (proc.HasExited == false)
                {
                    proc.WaitForExit(1000);
                }

                string errormsg = proc.StandardError.ReadToEnd();
                if (errormsg != "")
                    Flag = false;

                proc.StandardError.Close();
            }
            catch (Exception ex)
            {
                Flag = false;
                throw ex;
            }
            finally
            {
                proc.Close();
                proc.Dispose();
            }
            return Flag;
        }

        /// <summary>
        /// 移除Net Use
        /// </summary>
        public bool DisConnect()
        {

            bool Flag = true;
            Process proc = new Process();
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.CreateNoWindow = true;

            try
            {

                proc.Start();
                string command = @"net  use  " + remoteHost + " /delete";
                proc.StandardInput.WriteLine(command);
                command = "exit";
                proc.StandardInput.WriteLine(command);

                while (proc.HasExited == false)
                {
                    proc.WaitForExit(1000);
                }

                string errormsg = proc.StandardError.ReadToEnd();
                if (errormsg != "")
                    Flag = false;

                proc.StandardError.Close();
            }
            catch (Exception ex)
            {
                Flag = false;
                throw ex;
            }
            finally
            {
                proc.Close();
                proc.Dispose();
            }
            return Flag;
        }


    }
}
