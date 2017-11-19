using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class Msg
    {
        public Msg(int msgType, string msgVal)
        {
            this.MsgType = msgType;
            this.MsgVal = msgVal;
        }

        /// <summary>
        /// 消息类型
        /// </summary>
        public int MsgType
        {
            get;
            set;
        }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string MsgVal
        {
            get;
            set;
        }
    }
}
