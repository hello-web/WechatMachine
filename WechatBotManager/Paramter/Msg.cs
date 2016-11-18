using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatBotManager.Paramter
{
    public class Msg
    {
        public int Type { get; set; }
        public string Content { get; set; }
        public string FromUserName { get; set; }
        public string ToUserName { get; set; }
        public long LocalID { get; set; }
        public long ClientMsgId { get; set; }

    }
}
