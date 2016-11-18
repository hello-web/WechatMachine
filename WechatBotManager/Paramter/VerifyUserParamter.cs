using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatBotManager.Paramter
{
    public class VerifyUserParamter
    {
        public BaseRequest BaseRequest { get; set; }
        public int Opcode { get; set; }
        public int VerifyUserListSize { get; set; }
        public IList<VerifyUserList> VerifyUserList { get; set; }
        public string VerifyContent { get; set; }
        public int SceneListCount { get; set; }
        public IList<int> SceneList { get; set; }
        public string skey { get; set; }
    }

    public class VerifyUserList
    {
        public string Value { get; set; }
        public string VerifyUserTicket { get; set; }
    }
}
