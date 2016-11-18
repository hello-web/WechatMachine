using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatBotManager.Paramter
{
    public class StatusNotifyParamter
    {
        public BaseRequest BaseRequest { get; set; }
        public int Code { get; set; }

        public string FromUserName { get; set; }

        public string ToUserName { get; set; }
        public long ClientMsgId { get; set; }
    }
}
