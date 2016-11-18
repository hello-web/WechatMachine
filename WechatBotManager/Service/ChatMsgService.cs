using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WechatBotManager.Models;

namespace WechatBotManager.Service
{
    public class ChatMsgService
    {
        public void InsertMsg(wx_group_chat chat)
        {
            tobotDB.GetInstance().Insert(chat);
             
        }
    }
}
