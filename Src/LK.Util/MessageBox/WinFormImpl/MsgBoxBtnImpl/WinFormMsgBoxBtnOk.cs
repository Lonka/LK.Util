using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
namespace LK.Util
{
    class WinFormMsgBoxBtnOk : IMsgBoxBtn
    {
        private string msgTitle = "Window Message";
        private MessageBoxButtons messageBoxButtons = MessageBoxButtons.OK;
        public DialogResult ShowMessageBox(string msg, string title)
        {
            return MessageBox.Show(msg, title, messageBoxButtons, MessageBoxIcon.Information);
        }

        public DialogResult ShowMessageBox(string msg)
        {
            return MessageBox.Show(msg, msgTitle, messageBoxButtons, MessageBoxIcon.Information);
        }


        public DialogResult ShowMessageBoxAutoTurnOff(string msg, string title, TimeSpan ts)
        {
            LkProcess.WaitClose(null, title, ts);
            return MessageBox.Show(msg, title, messageBoxButtons, MessageBoxIcon.Information);
        }


        public DialogResult ShowMessageBoxAutoTurnOff(string msg, TimeSpan ts)
        {
            LkProcess.WaitClose(null, msgTitle, ts);
            return MessageBox.Show(msg, msgTitle, messageBoxButtons, MessageBoxIcon.Information);
        }
    }
}
