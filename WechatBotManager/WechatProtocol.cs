using CsharpHttpHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using WechatBotManager.Paramter;

namespace WechatBotManager
{
    public class WechatProtocol
    {   
        private string DeviceID = "e1615250492";
       
        /// <summary>
        /// 第一步：获取UUID
        /// </summary>
        /// <returns></returns>
        public string GetUUid()
        {
            string url = string.Format(WechatUrl.UUIDUrl, WechatCommon.GetTicks());
            string uuid = string.Empty;

            string html = HttpCommon.instance.GetHttp(url, ContentType.html);
            AppLog.WriteInfo(string.Format("协议：{0}，结果：{1}", "GetUUid", html));
            //HttpItem item = new HttpItem()
            //{
            //    URL = url,//URL     必需项    
            //    Method = "GET",//URL     可选项 默认为Get         
            //    UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0",//用户的浏览器类型，版本，操作系统     可选项有默认值    
            //    Accept = "text/html, application/xhtml+xml, */*",//    可选项有默认值    
            //    ContentType = "text/html",//返回类型    可选项有默认值     
            //};
            //HttpResult result = http.GetHtml(item);
            //string html = result.Html;

            try
            {
                if (!string.IsNullOrEmpty(html) && html != "操作超时")
                {
                    if (html.IndexOf("200") != -1)
                    {
                        Regex reg = new Regex("\"(.*?)\"");
                        Match m = reg.Match(html);
                        if (m.Success)
                        {
                            uuid = m.Value;
                            uuid = uuid.Substring(1, uuid.Length - 2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppLog.WriteError(ex.Message);
            }
            return uuid;
        }

        /// <summary>
        /// 第二步：根据UUID来获取二维码图片
        /// </summary>
        /// <param name="uuid"></param>
        public Image GetQrCode(string uuid)
        { 
            string url = string.Format(WechatUrl.QrCodeUrl, uuid); 
            Image img =HttpCommon.instance.GetImage(url); 
            return img;
        }

        /// <summary>
        /// 第三步：轮询检测是否扫描登录
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public string CheckLogin(string uuid)
        {
            string LoginUrl = "";
            string url = string.Format(WechatUrl.CheckLoginUrl, uuid, WechatCommon.GetTicks());

            string html = HttpCommon.instance.GetHttp(url, ContentType.html);
            AppLog.WriteInfo(string.Format("协议：{0}，结果：{1}", "CheckLogin", html));

            //408 登陆超时
            //201 扫描成功
            //200 确认登录
            try
            {
                if (!string.IsNullOrEmpty(html) && html != "操作超时")
                {
                    if (html.IndexOf("408") > 0)
                    {
                        LoginUrl = "408";
                    }
                    else if (html.IndexOf("200") == -1)
                    {
                        LoginUrl = "200";
                    }
                    else
                    {
                        Regex reg = new Regex("\"(.*?)\"");
                        Match m = reg.Match(html);
                        if (m.Success)
                        {
                            LoginUrl = m.Value;
                            LoginUrl = LoginUrl.Substring(1, LoginUrl.Length - 2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppLog.WriteError(ex.Message);
            }
            return LoginUrl;
        }

        /// <summary>
        /// 第四步：登录获取cookie
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public CookieResult GetCookie(string url)
        {
            if (url.StartsWith("?ticket"))
            {
                url = "https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxnewloginpage" + url;
            }

            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = url,//URL     必需项    
                Method = "GET",//URL     可选项 默认为Get         
                UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0",//用户的浏览器类型，版本，操作系统     可选项有默认值    
                Accept = "text/html, application/xhtml+xml, */*",//    可选项有默认值    
                ContentType = "text/html",//返回类型    可选项有默认值     
            }; 
            HttpResult result = http.GetHtml(item);
            CookieResult cookieResult = new CookieResult();
            WechatCookie cookie = new WechatCookie();
             
            string html = result.Html;
            AppLog.WriteInfo(string.Format("协议：{0}，结果：{1}", "GetCookie", html));

            if (!string.IsNullOrEmpty(html)&&html!="操作超时")
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(html);
                string json = JsonConvert.SerializeXmlNode(doc);
                ///改下代码，定义类
                dynamic d = JsonConvert.DeserializeObject<dynamic>(json);
                cookie.Skey = d.error.skey.Value;
                cookie.Pass_Ticket = d.error.pass_ticket.Value;
                cookie.Wxsid = d.error.wxsid.Value;
                cookie.Wxuin = Convert.ToInt32(d.error.wxuin.Value);
            }
            cookieResult.wechatCookie = cookie;

            #region 核心cookie处理
            string cookieStr = result.Cookie;
            cookieStr = cookieStr.Replace("Domain=wx.qq.com; Path=/;", "").Replace("Domain=.qq.com; Path=/;", "").Replace(" ", "");

            StringBuilder sb = new StringBuilder();
            string[] c = cookieStr.Split(';');
            sb.Append(c[0] + ";");
            for (int i = 1; i < 7; i++)
            {
                int firstIndex = c[i].LastIndexOf(',');
                string t = c[i].Substring(firstIndex + 1) + ";";
                sb.Append(t);
            }

            if (cookieStr.IndexOf("webwx_data_ticket=") >= 0)
            {
                string tmp = cookieStr.Substring(cookieStr.IndexOf("webwx_data_ticket="));
                int endIndex = tmp.IndexOf(";");
                cookieResult.SyncCookie = tmp.Substring(0, endIndex);
            }
            cookieResult.CookieStr = sb.ToString();
            #endregion
             
            return cookieResult;
        }

        /// <summary>
        /// 第五步：微信初始化
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public InitSyncResult Init(WechatCookie wechatCookie,string cookieStr)
        { 
            if (wechatCookie == null)
            {
                throw new Exception("cookie值为空");
            }

            string url = string.Format(WechatUrl.InitUrl, wechatCookie.Pass_Ticket, wechatCookie.Skey, WechatCommon.GetTicks());

            BaseRequest baseRequest = new BaseRequest();
            baseRequest.DeviceID = DeviceID;
            baseRequest.Sid = wechatCookie.Wxsid;
            baseRequest.Uin = wechatCookie.Wxuin;
            baseRequest.Skey = wechatCookie.Skey;
            InitParamter init = new InitParamter();
            init.BaseRequest = baseRequest;

            string postData = JsonConvert.SerializeObject(init);

            // string html =await HttpCommon.instance.PostHttp(url, postData, ContentType.json, cookieStr);
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = url,//URL     必需项    
                Method = "POST",//URL     可选项 默认为Get        
                Encoding = Encoding.UTF8,
                UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0",//用户的浏览器类型，版本，操作系统     可选项有默认值    
                Accept = "text/html, application/xhtml+xml, */*",//    可选项有默认值    
                ContentType = "application/json; charset=UTF-8",//返回类型    可选项有默认值   
                Cookie = cookieStr,//字符串Cookie     可选项  
                Postdata = postData,//Post数据     可选项GET时不需要写    
            };
            HttpResult result = http.GetHtml(item);
            string html = result.Html;
            AppLog.WriteInfo(string.Format("协议：{0}，结果：{1}", "Init", html));

            InitSyncResult syncKey = new InitSyncResult();
            try
            {
                if (!string.IsNullOrEmpty(html) && html != "操作超时")
                {
                    InitResult obj = JsonConvert.DeserializeObject<InitResult>(html);
                    syncKey.SynckeyStr = WechatCommon.SyncKeyFormat(obj.SyncKey);
                    syncKey.SyncKey = obj.SyncKey;
                    syncKey.UserName = obj.User.UserName;
                    syncKey.Uin = obj.User.Uin;
                    syncKey.Nickname = obj.User.NickName;
                }
            }
            catch (Exception ex)
            {
                AppLog.WriteError(ex.Message);
            }
                return syncKey;
        }

        /// <summary>
        /// 获取消息前同步（轮询同步有消息后执行Sync）
        /// </summary>
        /// <param name="host"></param>
        public SyncCheckResult SyncCheck(string host, WechatCookie wechatCookie, string cookieStr,string SynckeyStr)
        {
            string url = string.Format(WechatUrl.SnycCheckUrl, WechatCommon.GetTicks(), 
                HttpHelper.URLEncode(wechatCookie.Skey), wechatCookie.Wxsid, 
                wechatCookie.Wxuin, DeviceID, SynckeyStr, WechatCommon.GetTicks(), host);
            
            string html= HttpCommon.instance.Login(url, cookieStr);
            AppLog.WriteInfo(string.Format("协议：{0}，结果：{1}", "SyncCheck", html));

            //HttpItem item = new HttpItem()
            //{
            //    URL = url,//URL     必需项    
            //    Method = "GET",//URL     可选项 默认为Get         
            //    UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.79 Safari/537.36",//用户的浏览器类型，版本，操作系统     可选项有默认值    
            //    Accept = "application/json, text/plain, */*",//    可选项有默认值    
            //    ContentType = "application/json; charset=UTF-8",//返回类型    可选项有默认值   
            //                                                    // Encoding = Encoding.UTF8,
            //    Cookie = syncCookie,//字符串Cookie     可选项  
            //    Referer = "https://wx.qq.com/?&lang=zh_CN",

            //};
            //HttpResult result = http.GetHtml(item);
            //string html = result.Html;

            SyncCheckResult syncCheck = new SyncCheckResult();
            try
            {
                if (!string.IsNullOrEmpty(html) && html != "请求超时")
                {
                    if (html.IndexOf('{') != -1)
                    {
                        string sycheck = html.Substring(html.IndexOf('{'));
                        syncCheck = JsonConvert.DeserializeObject<SyncCheckResult>(sycheck);
                    }
                }
            }
            catch (Exception ex)
            {
                AppLog.WriteError(ex.Message);
            }
            return syncCheck;
        }

        /// <summary>
        /// 第六步：获取消息内容
        /// </summary>
        /// <returns></returns>
        public SyncResult Sync(WechatCookie wechatCookie,string cookieStr, SyncKey syncKey)
        {
            if (wechatCookie == null)
            {
                throw new Exception("cookie值为空");
            }
            string url = string.Format(WechatUrl.SnycUrl, wechatCookie.Wxsid, wechatCookie.Skey, wechatCookie.Pass_Ticket);
            SyncParamter param = new SyncParamter();
            param.SyncKey = syncKey;
            param.rr = WechatCommon.GetTicks();
            BaseRequest request = new BaseRequest();
            request.Uin = wechatCookie.Wxuin;
            request.DeviceID = DeviceID;
            request.Sid = wechatCookie.Wxsid;
            request.Skey = wechatCookie.Skey;
            param.BaseRequest = request;

            string postData = JsonConvert.SerializeObject(param);
            string html = HttpCommon.instance.PostHttp(url, postData, ContentType.json, cookieStr);
            AppLog.WriteInfo(string.Format("协议：{0}，结果：{1}", "Sync", html));

            //HttpItem item = new HttpItem()
            //{
            //    URL = url,//URL     必需项    
            //    Method = "POST",//URL     可选项 默认为Get        
            //    Encoding = Encoding.UTF8,
            //    UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0",//用户的浏览器类型，版本，操作系统     可选项有默认值    
            //    Accept = "text/html, application/xhtml+xml, */*",//    可选项有默认值    
            //    ContentType = "application/json; charset=UTF-8",//返回类型    可选项有默认值   
            //    Cookie = CookieStr,//字符串Cookie     可选项  
            //    Postdata = postData,//Post数据     可选项GET时不需要写    
            //};
            //HttpResult result = http.GetHtml(item);
            //string html = result.Html;
            SyncResult sync = new SyncResult();

            try
            {
                if (!string.IsNullOrEmpty(html) && html != "操作超时")
                {
                    sync = JsonConvert.DeserializeObject<SyncResult>(html);
                    //SyncKey = sync.SyncKey;
                    //SyncKeyFormat(); 
                    //msg = sync.AddMsgList;
                }
            }
            catch (Exception ex)
            {
                AppLog.WriteError(ex.Message);
            }

                return sync;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string StatusNotify(WechatCookie wechatCookie,string cookieStr,string username)
        {
            string url = string.Format(WechatUrl.NotifyUrl, wechatCookie.Pass_Ticket);
            StatusNotifyParamter param = new StatusNotifyParamter();
            param.Code = 3;
            param.ClientMsgId = WechatCommon.GetTicks();
            param.FromUserName = username;
            param.ToUserName = username;
            BaseRequest request = new BaseRequest();
            request.DeviceID = DeviceID;
            request.Sid = wechatCookie.Wxsid;
            request.Skey = wechatCookie.Skey;
            request.Uin = wechatCookie.Wxuin;
            param.BaseRequest = request;
            string postData = JsonConvert.SerializeObject(param);
            //HttpItem item = new HttpItem()
            //{
            //    URL = url,//URL     必需项    
            //    Method = "POST",//URL     可选项 默认为Get        
            //    Encoding = Encoding.UTF8,
            //    UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0",//用户的浏览器类型，版本，操作系统     可选项有默认值    
            //    Accept = "text/html, application/xhtml+xml, */*",//    可选项有默认值    
            //    ContentType = "application/json; charset=UTF-8",//返回类型    可选项有默认值   
            //    Cookie = CookieStr,//字符串Cookie     可选项  
            //    Postdata = postData,//Post数据     可选项GET时不需要写    
            //};
            //HttpResult result = http.GetHtml(item);
            //string html = result.Html;
            string html = string.Empty;
             
                html = HttpCommon.instance.PostHttp(url, postData, ContentType.json, cookieStr);
                AppLog.WriteInfo(string.Format("协议：{0}，结果：{1}", "StatusNotify", html));
        
            return html;
        }

        /// <summary>
        /// 获取联系人信息
        /// </summary>
        /// <returns></returns>
        public ContactResult GetContact(WechatCookie wechatCookie, string cookieStr)
        {
            string url = string.Format(WechatUrl.ContentUrl, wechatCookie.Pass_Ticket,WechatCommon.GetTicks(), wechatCookie.Skey);
      
            string html = HttpCommon.instance.PostHttp(url, "{}", ContentType.json, cookieStr);
            AppLog.WriteInfo(string.Format("协议：{0}，结果：{1}", "GetContact", html));

            ContactResult contact = new ContactResult();
            try
            {
                if (!string.IsNullOrEmpty(html) && html != "操作超时")
                {
                    contact = JsonConvert.DeserializeObject<ContactResult>(html);
                }
            }
            catch (Exception ex)
            {
                AppLog.WriteError(ex.Message);
            }
            return contact;
        }

        

        public void LogOut(WechatCookie wechatCookie, string cookieStr)
        {
            string url = string.Format(WechatUrl.LogoutUrl, wechatCookie.Skey);
            string postData = string.Format("sid={0}&uin={1}",wechatCookie.Wxsid,wechatCookie.Wxuin);
            HttpCommon.instance.PostHttp(url, postData, ContentType.json, cookieStr); 
        }

        public string SendMsg(string content, string toUserName,int msgType, WechatCookie wechatCookie,string cookieStr,string username)
        {
            string url = string.Format(WechatUrl.SendMsgUrl, wechatCookie.Pass_Ticket);
            MsgParamter param = new MsgParamter();
            BaseRequest request = new BaseRequest();
            request.Uin = wechatCookie.Wxuin;
            request.Sid = wechatCookie.Wxsid;
            request.Skey = wechatCookie.Skey;
            request.DeviceID = DeviceID;
            param.BaseRequest = request;
            Msg msg = new Msg();
            msg.Type = msgType;
            msg.Content = content;
            msg.FromUserName = username;
            msg.ToUserName = toUserName;
            msg.LocalID = WechatCommon.GetTicks();
            msg.ClientMsgId = WechatCommon.GetTicks();
            param.Msg = msg;

            string postData = JsonConvert.SerializeObject(param);
            string html = string.Empty;
       
                html = HttpUtil.PostDataToServer(url, postData);
                AppLog.WriteInfo(string.Format("协议：{0}，结果：{1}", "SendMsg", html));
           
            return html;
        }

        public BatchGetContactResult BatchGetContact(List<string> userNameList,WechatCookie wechatCookie, string cookieStr)
        {
            string url = string.Format(WechatUrl.BatchContentUrl, WechatCommon.GetTicks(), wechatCookie.Pass_Ticket);
            BatchGetContactParamter batch = new BatchGetContactParamter();
            BaseRequest request = new BaseRequest();
            request.DeviceID = DeviceID;
            request.Uin = wechatCookie.Wxuin;
            request.Sid = wechatCookie.Wxsid;
            request.Skey = wechatCookie.Skey;
            batch.BaseRequest = request;

            List<ChatRoomIdList> list_chatRoomId = new List<ChatRoomIdList>();
            foreach (var username in userNameList)
            {
                ChatRoomIdList chatRoomId = new ChatRoomIdList();
                chatRoomId.EncryChatRoomId = "";
                chatRoomId.UserName = username;
                list_chatRoomId.Add(chatRoomId);
            }
            batch.List = list_chatRoomId;
            batch.Count = list_chatRoomId.Count;

            string postData = JsonConvert.SerializeObject(batch);
            string html=  HttpCommon.instance.PostHttp(url, postData, ContentType.json, cookieStr);
            AppLog.WriteInfo(string.Format("协议：{0}，结果：{1}", "BatchGetContact", html));
            BatchGetContactResult result = new BatchGetContactResult();

            try
            {
                if (!string.IsNullOrEmpty(html) && html != "操作超时")
                {
                    result = JsonConvert.DeserializeObject<BatchGetContactResult>(html);
                }
            }
            catch (Exception ex)
            {
                AppLog.WriteError(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 验证用户信息(加好友)
        /// </summary>
        public BaseResponse VerifyUser(string userName, string ticket, WechatCookie wechatCookie, string cookieStr)
        {
            string url = string.Format(WechatUrl.VerifyUserUrl, WechatCommon.GetTicks(), wechatCookie.Pass_Ticket);
            VerifyUserParamter param = new VerifyUserParamter();
            BaseRequest request = new BaseRequest();
            request.DeviceID = DeviceID;
            request.Sid = wechatCookie.Wxsid;
            request.Skey = wechatCookie.Skey;
            request.Uin = wechatCookie.Wxuin;
            param.BaseRequest = request;
            param.Opcode = 3;
            param.SceneList = new int[] { 3 };
            param.SceneListCount = 1;
            param.skey = wechatCookie.Skey;
            param.VerifyContent = "";
            param.VerifyUserListSize = 1;
            IList<VerifyUserList> verifyUserList = new List<VerifyUserList>();
            VerifyUserList verifyUser = new VerifyUserList();
            verifyUser.Value = userName;
            verifyUser.VerifyUserTicket = ticket;
            verifyUserList.Add(verifyUser);
            param.VerifyUserList = verifyUserList;

            string postData = JsonConvert.SerializeObject(param);

            string html = HttpCommon.instance.PostHttp(url, postData, ContentType.json, cookieStr);
            AppLog.WriteInfo(string.Format("协议：{0}，结果：{1}", "VerifyUser", html));

            BaseResponse response = new BaseResponse();
            try
            {
                if (!string.IsNullOrEmpty(html) && html != "操作超时")
                {
                    response = JsonConvert.DeserializeObject<BaseResponse>(html);
                }
            }
            catch (Exception ex)
            {
                AppLog.WriteError(ex.Message);
            }
            return response;
        }

        /// <summary>
        /// 群中个人
        /// </summary>
        public Image GetIcon(string userName,string chatRoomId,string cookie)
        {
            string url = string.Format(WechatUrl.GetIconUrl, "0", userName, chatRoomId,"");
            Image image = HttpCommon.instance.GetImage(url, cookie);
            return image;
        }

        /// <summary>
        /// 群头像
        /// </summary>
        public Image GetHeadImage(string userName, string cookie)
        {
            string url = string.Format(WechatUrl.GetHeadImgUrl, "656901133", userName, "");
            Image image = HttpCommon.instance.GetImage(url,cookie);
            return image;
        }

        /// <summary>
        /// 好友
        /// </summary>
        public Image GetIconFriend(string userName,string skey, string cookie)
        {
            string url = string.Format(WechatUrl.GetIconFriendUrl, "656878457", userName, skey);
            Image image = HttpCommon.instance.GetImage(url, cookie);
            return image;
        }

    }
}
