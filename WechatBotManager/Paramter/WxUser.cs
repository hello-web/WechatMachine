using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatBotManager.Paramter
{
     public class WxUser
    {
        /// <summary>
        /// 临时唯一
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 永久唯一
        /// </summary>
        public string Id { get; set; }
    }
}
