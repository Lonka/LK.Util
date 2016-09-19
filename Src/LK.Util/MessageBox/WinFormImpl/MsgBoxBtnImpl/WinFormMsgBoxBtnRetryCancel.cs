using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
namespace LK.Util
{
    class WinFormMsgBoxBtnRetryCancel : IMsgBoxBtn
    {
        private string msgTitle = "Warning Message";
        private MessageBoxButtons messageBoxButtons = MessageBoxButtons.RetryCancel;
        public DialogResult ShowMessageBox(string msg, string title)
        {
            return MessageBox.Show(msg, title, messageBoxButtons, MessageBoxIcon.Warning);
        }

        public DialogResult ShowMessageBox(string msg)
        {
            return MessageBox.Show(msg, msgTitle, messageBoxButtons, MessageBoxIcon.Warning);
        }

        public DialogResult ShowMessageBoxAutoTurnOff(string msg, string title, TimeSpan ts)
        {
            LkProcess.WaitClose(null, title, ts);
            return MessageBox.Show(msg, title, messageBoxButtons, MessageBoxIcon.Warning);
        }


        public DialogResult ShowMessageBoxAutoTurnOff(string msg, TimeSpan ts)
        {
            LkProcess.WaitClose(null, msgTitle, ts);
            return MessageBox.Show(msg, msgTitle, messageBoxButtons, MessageBoxIcon.Warning);
        }
    }
}
