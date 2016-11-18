using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatBotManager.Paramter
{
    public class InitResult
    {
        public BaseResponse BaseResponse { get; set; }
        public int Count { get; set; }
        public IList<object> ContactList { get; set; }
        public SyncKey SyncKey { get; set; }
        public User User { get; set; }
        public string ChatSet { get; set; }
        public string SKey { get; set; }
        public int ClientVersion { get; set; }
        public int SystemTime { get; set; }
        public int GrayScale { get; set; }
        public int InviteStartCount { get; set; }
        public int MPSubscribeMsgCount { get; set; }
        public IList<object> MPSubscribeMsgList { get; set; }
        public int ClickReportInterval { get; set; }
    }
}
