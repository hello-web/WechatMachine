using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WechatBotManager.Paramter;

namespace WechatBotManager
{
    public class MemberListComparer : IEqualityComparer<MemberList>
    {
        public bool Equals(MemberList x, MemberList y)
        {
            if (Object.ReferenceEquals(x, y)) return true; 
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;
            return x.AttrStatus == y.AttrStatus;
        }

        public int GetHashCode(MemberList obj)
        {
            if (Object.ReferenceEquals(obj, null)) return 0;
             
            int hashProductName = obj.UserName == null ? 0 : obj.UserName.GetHashCode();
             
            int hashProductCode = obj.AttrStatus.GetHashCode(); 
            return hashProductName ^ hashProductCode;
        }
    }
}
