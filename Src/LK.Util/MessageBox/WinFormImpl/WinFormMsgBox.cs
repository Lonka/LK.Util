using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace LK.Util
{
    class WinFormMsgBox : MsgBox
    {
        protected override IMsgBoxBtn CreateMsgBoxBtn(LkMessageBoxButtons lkMsgBoxBtns)
        {
            switch (lkMsgBoxBtns)
            {
                case LkMessageBoxButtons.OK:
                    return new WinFormMsgBoxBtnOk();
                case LkMessageBoxButtons.OKCancel:
                    return new WinFormMsgBoxBtnOkCancel();
                case LkMessageBoxButtons.AbortRetryIgnore:
                    return new WinFormMsgBoxBtnAbortRetryIgnore();
                case LkMessageBoxButtons.YesNoCancel:
                    return new WinFormMsgBoxBtnYesNoCancel();
                case LkMessageBoxButtons.YesNo:
                    return new WinFormMsgBoxBtnYesNo();
                case LkMessageBoxButtons.RetryCancel:
                    return new WinFormMsgBoxBtnRetryCancel();
            }
            return new WinFormMsgBoxBtnOk();
        }
    }
}
