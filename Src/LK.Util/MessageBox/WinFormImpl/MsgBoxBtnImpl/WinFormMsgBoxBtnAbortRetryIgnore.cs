using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace LK.Util
{
    class WinFormMsgBoxBtnAbortRetryIgnore : IMsgBoxBtn
    {
        private string msgTitle = "Error Message";
        private MessageBoxButtons messageBoxButtons = MessageBoxButtons.AbortRetryIgnore;
        public DialogResult ShowMessageBox(string msg, string title)
        {
            return MessageBox.Show(msg, title, messageBoxButtons, MessageBoxIcon.Error);
        }

        public DialogResult ShowMessageBox(string msg)
        {
            return MessageBox.Show(msg, msgTitle, messageBoxButtons, MessageBoxIcon.Error);
        }

        public DialogResult ShowMessageBoxAutoTurnOff(string msg, string title, TimeSpan ts)
        {
            return MessageBox.Show(msg, title, messageBoxButtons, MessageBoxIcon.Error);
        }


        public DialogResult ShowMessageBoxAutoTurnOff(string msg, TimeSpan ts)
        {
            return MessageBox.Show(msg, msgTitle, messageBoxButtons, MessageBoxIcon.Error);
        }
    }
}
