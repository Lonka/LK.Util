using System;
using System.Collections.Generic;
using System.Text;

namespace LK.Util
{
    class CloseThread
    {
        private string lpClassName = string.Empty;
        private string lpWindowName = string.Empty;
        private TimeSpan ts;
        public CloseThread(string _lpClassName, string _lpWindowName,TimeSpan _ts)
        {
            lpClassName = _lpClassName;
            lpWindowName = _lpWindowName;
            ts = _ts;
        }
        public void Close()
        {
            bool exitFlag = false;
            int totleSeconds = (int)ts.TotalSeconds;
            while (exitFlag == false)
            {
                while (totleSeconds > 0)
                {
                    System.Threading.Thread.Sleep(1000);
                    totleSeconds--;
                    System.Windows.Forms.Application.DoEvents();
                }
                exitFlag = true;
            }
            LkProcess.Close(lpClassName, lpWindowName);
        }
    }
}
