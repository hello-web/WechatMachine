using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatBotManager
{
    public enum MsgTypeEnum
    {
        初始化消息=0,
        自己消息 =1,
        群消息=3,
        联系人消息=4,
        公众号消息=5,
        特殊账号消息=6,
        加好友 =37,
        系统消息=51,
        群通知= 10000
    }
}
