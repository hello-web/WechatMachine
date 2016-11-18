using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatBotManager.Paramter
{
    public class BatchGetContactParamter
    {
        public BaseRequest BaseRequest { get; set; }

        public int Count { get; set; }

        public IList<ChatRoomIdList> List { get; set; }

    }

    public class ChatRoomIdList
    {
        public string UserName { get; set; }
        public string EncryChatRoomId { get; set; }
    }
}
