using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace LK.Util
{
    internal interface IMsgBoxBtn
    {
        DialogResult ShowMessageBox(string msg, string title);
        DialogResult ShowMessageBox(string msg);
        DialogResult ShowMessageBoxAutoTurnOff(string msg, string title, TimeSpan ts);
        DialogResult ShowMessageBoxAutoTurnOff(string msg, TimeSpan ts);
    }
}
