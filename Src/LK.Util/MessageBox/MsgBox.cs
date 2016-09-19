using System;
using System.Collections.Generic;
using System.Text;

namespace LK.Util
{
    abstract class MsgBox
    {
        public LkDialogResult ShowMessage(string msg, string title, LkMessageBoxButtons lkMsgBoxBtns)
        {
            IMsgBoxBtn msgBoxBtn = CreateMsgBoxBtn(lkMsgBoxBtns);
            return (LkDialogResult)Enum.Parse(typeof(LkDialogResult), msgBoxBtn.ShowMessageBox(msg, title).ToString());
        }
        public LkDialogResult ShowMessage(string msg, LkMessageBoxButtons lkMsgBoxBtns)
        {
            IMsgBoxBtn msgBoxBtn = CreateMsgBoxBtn(lkMsgBoxBtns);
            return (LkDialogResult)Enum.Parse(typeof(LkDialogResult), msgBoxBtn.ShowMessageBox(msg).ToString());
        }
        public LkDialogResult ShowMessageAutoTurnOff(string msg, string title, LkMessageBoxButtons lkMsgBoxBtns, TimeSpan ts)
        {
            IMsgBoxBtn msgBoxBtn = CreateMsgBoxBtn(lkMsgBoxBtns);
            return (LkDialogResult)Enum.Parse(typeof(LkDialogResult), msgBoxBtn.ShowMessageBoxAutoTurnOff(msg, title, ts).ToString());
        }
        public LkDialogResult ShowMessageAutoTurnOff(string msg, LkMessageBoxButtons lkMsgBoxBtns, TimeSpan ts)
        {
            IMsgBoxBtn msgBoxBtn = CreateMsgBoxBtn(lkMsgBoxBtns);
            return (LkDialogResult)Enum.Parse(typeof(LkDialogResult), msgBoxBtn.ShowMessageBoxAutoTurnOff(msg, ts).ToString());

        }
        protected abstract IMsgBoxBtn CreateMsgBoxBtn(LkMessageBoxButtons lkMsgBoxBtns);
    }
}
