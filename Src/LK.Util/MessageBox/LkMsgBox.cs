using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace LK.Util
{
    /// <summary>
    /// Messagebox operation(Static)
    /// </summary>
    public class LkMsgBox
    {
        private static MessageModes _messageMode = MessageModes.None;
        /// <summary>
        /// 設定Message模式 (預設為WinForm)
        /// </summary>
        public static MessageModes MessageMode
        {
            get
            {
                if (_messageMode == Util.MessageModes.None)
                {
                    _messageMode = Util.MessageModes.WinForm;
                }
                return _messageMode;
            }
            set
            {
                _messageMode = value;
            }
        }

        private static MsgBox _msgBox = null;
        internal static MsgBox msgBox
        {
            get
            {
                if (_msgBox == null)
                {
                    switch (MessageMode)
                    {
                        case MessageModes.WinForm:
                            _msgBox = new WinFormMsgBox();
                            break;
                        case MessageModes.Web:
                            //TODO Web Implement
                            _msgBox = new WebMsgBox();
                            break;
                    }
                }
                return _msgBox;
            }
        }


        /// <summary>
        /// 提示訊息
        /// </summary>
        /// <param name="msg">訊息內容</param>
        /// <param name="title">標題</param>
        /// <param name="lkMsgBoxBtns">Button Type</param>
        /// <returns></returns>
        public static LkDialogResult ShowMessage(string msg, string title, LkMessageBoxButtons lkMsgBoxBtns)
        {
            return msgBox.ShowMessage(msg, title, lkMsgBoxBtns);
        }

        /// <summary>
        /// 提示訊息
        /// </summary>
        /// <param name="msg">訊息內容</param>
        /// <param name="lkMsgBoxBtns">Button Type</param>
        /// <returns></returns>
        public static LkDialogResult ShowMessage(string msg, LkMessageBoxButtons lkMsgBoxBtns)
        {
            return msgBox.ShowMessage(msg, lkMsgBoxBtns);
        }

        /// <summary>
        /// 自動關閉的提示訊息 (YesNo和AbortRetryIgnore 無法自動關閉)
        /// </summary>
        /// <param name="msg">訊息內容</param>
        /// <param name="title">標題</param>
        /// <param name="lkMsgBoxBtns">Button Type</param>
        /// <param name="ts">時間</param>
        /// <returns></returns>
        public static LkDialogResult ShowMessageAutoTurnOff(string msg, string title, LkMessageBoxButtons lkMsgBoxBtns,TimeSpan ts)
        {
            return msgBox.ShowMessageAutoTurnOff(msg, title, lkMsgBoxBtns, ts);
        }

        /// <summary>
        /// 自動關閉的提示訊息 (YesNo和AbortRetryIgnore 無法自動關閉)
        /// </summary>
        /// <param name="msg">訊息內容</param>
        /// <param name="title">標題</param>
        /// <param name="lkMsgBoxBtns">Button Type</param>
        /// <param name="second">秒數</param>
        /// <returns></returns>
        public static LkDialogResult ShowMessageAutoTurnOff(string msg, string title, LkMessageBoxButtons lkMsgBoxBtns, int second)
        {
            TimeSpan ts = new TimeSpan(0, 0, second);
            return msgBox.ShowMessageAutoTurnOff(msg, title, lkMsgBoxBtns, ts);
        }

        /// <summary>
        /// 自動關閉的提示訊息 (YesNo和AbortRetryIgnore 無法自動關閉)
        /// </summary>
        /// <param name="msg">訊息內容</param>
        /// <param name="title">標題</param>
        /// <param name="lkMsgBoxBtns">Button Type</param>
        /// <param name="ts">時間</param>
        /// <returns></returns>
        public static LkDialogResult ShowMessageAutoTurnOff(string msg, LkMessageBoxButtons lkMsgBoxBtns, TimeSpan ts)
        {
            return msgBox.ShowMessageAutoTurnOff(msg, lkMsgBoxBtns, ts);
        }

        /// <summary>
        /// 自動關閉的提示訊息 (YesNo和AbortRetryIgnore 無法自動關閉)
        /// </summary>
        /// <param name="msg">訊息內容</param>
        /// <param name="title">標題</param>
        /// <param name="lkMsgBoxBtns">Button Type</param>
        /// <param name="second">秒數</param>
        /// <returns></returns>
        public static LkDialogResult ShowMessageAutoTurnOff(string msg, LkMessageBoxButtons lkMsgBoxBtns, int second)
        {
            TimeSpan ts = new TimeSpan(0, 0, second);
            return msgBox.ShowMessageAutoTurnOff(msg, lkMsgBoxBtns, ts);
        }



    }
}
