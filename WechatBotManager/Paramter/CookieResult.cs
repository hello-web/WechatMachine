using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WechatBotManager.Paramter
{
    public class CookieResult
    {
        public WechatCookie wechatCookie { get; set; }

        public string CookieStr { get; set; }
        /// <summary>
        /// 同步专用cookie
        /// </summary>
        public string SyncCookie { get; set; }
    }
}
