using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WechatBotManager.Paramter
{
    public class InitSyncResult
    {
        public SyncKey SyncKey { get; set; }

        public string SynckeyStr { get; set; }
        public string UserName { get; set; }
        public int Uin { get; set; }
        public string Nickname { get; set; }

    }
}
