using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace LK.Util
{
    internal class ThreadFileLog : CommonFileLog
    {
        private Queue<Message> m_messageQueue = new Queue<Message>();
        public ThreadFileLog()
        {
            Thread commandThread = new Thread(DoCommand);
            commandThread.IsBackground = true;
            commandThread.Start();
        }

        protected override void DoWriteLog(LogLevel logLevel, string function, string msg)
        {
            //決定要用誰的 還不知道要怎麼寫
            base.DoWriteLog(logLevel, function, msg);
        }

        protected override void DoWriteLog(string fileName, string msg)
        {
            m_messageQueue.Enqueue(new Message() { FileName = fileName, Msg = msg });
        }

        private void DoCommand()
        {
            while (true)
            {
                Thread.Sleep(1);
                if (m_messageQueue.Count > 0)
                {
                    Message message = m_messageQueue.Dequeue();
                    base.DoWriteLog(message.FileName, message.Msg);
                }
            }
        }
    }
}
