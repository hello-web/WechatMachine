using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatBotManager.Paramter
{
    public class ContactResult
    {
        public BaseResponse BaseResponse { get; set; }
        public int MemberCount { get; set; }
        public IList<Member> MemberList { get; set; }
        public int Seq { get; set; }
    }
}
