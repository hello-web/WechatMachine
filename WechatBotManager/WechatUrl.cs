using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WechatBotManager
{
    public struct WechatUrl
    {
        public static string UUIDUrl = "https://login.weixin.qq.com/jslogin?appid=wx782c26e4c19acffb&redirect_uri=https%3A%2F%2Fwx.qq.com%2Fcgi-bin%2Fmmwebwx-bin%2Fwebwxnewloginpage&fun=new&lang=zh_CN&_={0}";
        public static string QrCodeUrl = "https://login.weixin.qq.com/qrcode/{0}?t=webwx";
        public static string CheckLoginUrl = "https://login.weixin.qq.com/cgi-bin/mmwebwx-bin/login?uuid={0}&tip=1&_={1}";
        public static string InitUrl = "https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxinit?pass_ticket={0}&skey={1}&r={2}";
        public static string NotifyUrl = "https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxstatusnotify?pass_ticket={0}";
        public static string SnycUrl = "https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxsync?sid={0}&skey={1}&lang=zh_CN&pass_ticket={2}";
        public static string SnycCheckUrl = "https://{7}/cgi-bin/mmwebwx-bin/synccheck?r={0}&skey={1}&sid={2}&uin={3}&deviceid={4}&synckey={5}&_={6}";

        public static string ContentUrl = "https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxgetcontact?lang=zh_CN&pass_ticket={0}&r={1}&seq=0&skey={2}"; //联系人
        public static string BatchContentUrl = "https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxbatchgetcontact?type=ex&r={0}&lang=zh_CN&pass_ticket={1}"; //批量
        public static string LogoutUrl = "https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxlogout?redirect=1&type=0&skey={0}";
        public static string SendMsgUrl = "https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxsendmsg?lang=zh_CN&pass_ticket={0}";
        public static string VerifyUserUrl = "https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxverifyuser?r={0}&pass_ticket={1}";
        public static string GetIconUrl = "https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxgeticon?seq={0}&username={1}&chatroomid={2}&skey={3}";
        public static string GetHeadImgUrl = "https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxgetheadimg?seq={0}&username={1}&skey={2}";
        public static string GetIconFriendUrl = "https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxgeticon?seq={0}&username={1}&skey={2}";
    }
}
