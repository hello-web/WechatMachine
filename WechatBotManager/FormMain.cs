using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WechatBotManager.Models;
using WechatBotManager.Paramter;
using WechatBotManager.Service;

namespace WechatBotManager
{
    public partial class FormMain : Form
    {
        WechatService service = new WechatService();
        WechatProtocol protocol = new WechatProtocol();
        ChatMsgService msgService = new ChatMsgService();
        BuyRecordService buyService = new BuyRecordService();
        bool workerRun = true;
        bool syncRun = true; 

        public FormMain()
        {
            InitializeComponent();
            buttonStart_Click(this,null);
        }

        /// <summary>
        /// 启动一个机器人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                LogOut("二维码已生成");
                return; 
            }  
            LogOut("机器人开始启动");
            QrCode qrCode = service.Start();
            pictureBoxQrCode.Image = qrCode.IMAGE;
            labelStatus.Text = "等待扫描";
            //监控扫描状态
            workerRun = true;
            backgroundWorker1.RunWorkerAsync(); 
           
        }
          
        private void LogOut(string log)
        {
            textBoxLog.Text += log + "\r\n";
            //让文本框获取焦点 
            this.textBoxLog.Focus();
            //设置光标的位置到文本尾 
            this.textBoxLog.Select(this.textBoxLog.TextLength, 0);
            //滚动到控件光标处 
            this.textBoxLog.ScrollToCaret();
        }

        private void LogClear()
        {
            textBoxLog.Text = "";
        }

        private void timerQrCode_Tick(object sender, EventArgs e)
        { 
        }

        private void FormMain_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 选择同步域名
        /// </summary>
        /// <returns></returns>
        private string SyncCheck()
        { 
            var robot = Global.robot;

            List<string> hostList = new List<string>();
            hostList.Add("webpush.wx.qq.com");
            hostList.Add("webpush.weixin.qq.com");
            hostList.Add("webpush2.weixin.qq.com");
            hostList.Add("webpush.wechat.com");
            hostList.Add("webpush1.wechat.com");
            hostList.Add("webpush2.wechat.com");
            hostList.Add("webpush1.wechatapp.com");
            string host = string.Empty;
            foreach (var hostName in hostList)
            { 
               SyncCheckResult checkResult = protocol.SyncCheck(hostName, robot.Cookie, robot.CookieStr, robot.SynckeyStr);
                if (checkResult.retcode == "0")
                {
                    host = hostName;
                    break;
                }
            }
            return host;
        }

        public void Sync(BackgroundWorker work,string host)
        {
            if (work.CancellationPending)
            {
                return;
            }

            var robot = Global.robot;
            if (robot.State == 1)
            { 
                var result = protocol.SyncCheck(host, robot.Cookie, robot.CookieStr, robot.SynckeyStr);
                /*
                retcode:
                    0 正常
                    1100 失败/登出微信
                selector:
                    0 正常
                    2 新的消息
                    7 进入/离开聊天界面
                */
                if (result.retcode == "0")
                {
                    //switch (result.selector)
                    //{
                    //    case "0":
                    //        work.ReportProgress(1, "正常");
                    //        break;
                    //    default:
                            SyncResult syncResult = protocol.Sync(robot.Cookie, robot.CookieStr, robot.SyncKey);
                            robot.SyncKey = syncResult.SyncKey;
                            robot.SynckeyStr = WechatCommon.SyncKeyFormat(syncResult.SyncKey);
                            List<AddMsgList> msgList = syncResult.AddMsgList;

                            if (syncResult.ModContactCount > 0)
                            {
                                //群人员变化了
                                //判断是否已经存在的群，不存在的重新初始化
                                bool change = false;
                                foreach (var contact in syncResult.ModContactList)
                                {
                                    if (contact.UserName.StartsWith("@@"))
                                    {
                                        WxUser user = Global.robot.WxUserList.Where(n => n.UserName == contact.UserName).FirstOrDefault();
                                        if (user == null)
                                        {
                                            change = true;
                                            break;
                                        }
                                    }
                                }
                                if (change)
                                { 
                                    protocol.StatusNotify(Global.robot.Cookie, Global.robot.CookieStr, Global.robot.UserName);
                                }
                              
                                //欢迎通知逻辑;对比两个群人员列表
                                foreach (var contact in syncResult.ModContactList)
                                {
                                    string fromAlias = WechatCommon.GetTrueId(contact.UserName);
                                    bool isBind = buyService.IsBind(fromAlias);
                                    if (isBind)
                                    {
                                        var newMember = contact.MemberList;
                                        var haveMember= Global.robot.GroupInfo.Where(n => n.UserName == contact.UserName).FirstOrDefault();
                                if (haveMember != null)
                                {
                                    MemberListComparer compar = new MemberListComparer();
                                    var welcomeUser = newMember.Except(haveMember.MemberList, compar); //差集新增
                                    Task.Run(() =>
                                    {
                                        foreach (var u in welcomeUser)
                                        {
                                            protocol.SendMsg("欢迎" + u.NickName + "加入群！", contact.UserName, 1, Global.robot.Cookie, Global.robot.CookieStr, Global.robot.UserName);
                                            Thread.Sleep(2000);
                                        }
                                    }); 
                                            haveMember.MemberList = newMember;
                                            haveMember.MemberCount = newMember.Count;
                                        } 

                                    }
                                }
                                
                            }
                            foreach (var msg in msgList)
                            {
                                work.ReportProgress(1, msg);
                            } 
                          //  break;  
                  //  }
                }
                else
                {
                    work.ReportProgress(1, result.retcode);
                }
            }
            else
            {
                //还未登录
                work.ReportProgress(1, "0");
            } 
        }

        private void TimeRun(BackgroundWorker worker)
        {
            if (worker.CancellationPending)
            {
                return;
            }
            var robot = Global.robot;
            if (robot == null)
            {
                worker.ReportProgress(1, "noRobot");
            }
            if (robot.State == 0)
            {
                worker.ReportProgress(2, "processing");
                string loginUrl = protocol.CheckLogin(robot.UUID);
                worker.ReportProgress(3, "processed");
                if (loginUrl == "408")
                {
                    //超时需要重新生成二维码  
                }
                else if (loginUrl == "200")
                {
                    //已扫描待确认 
                }
                else {
                    CookieResult cookieResult = protocol.GetCookie(loginUrl);
                    robot.CookieStr = cookieResult.CookieStr;
                    robot.Cookie = cookieResult.wechatCookie;
                    robot.SyncCookie = cookieResult.SyncCookie;

                    InitSyncResult initSyncResult = protocol.Init(robot.Cookie, robot.CookieStr);
                    robot.Uin = initSyncResult.Uin; 

                    RobotService service = new RobotService();
                    int robotId = service.IsRobot(robot.Uin);
                    if (robotId > 0)
                    {
                        robot.Id = robotId;
                        robot.SyncKey = initSyncResult.SyncKey;
                        robot.SynckeyStr = initSyncResult.SynckeyStr;
                        robot.UserName = initSyncResult.UserName; 
                        robot.NickName = initSyncResult.Nickname;
                        protocol.StatusNotify(robot.Cookie, robot.CookieStr, robot.UserName);
                        robot.State = 1;
                        robot.Contact = protocol.GetContact(robot.Cookie, robot.CookieStr).MemberList.ToList();
                        Global.robot = robot; 
                    }
                    else
                    {
                        loginUrl = "noRobot";
                    } 
                }
                worker.ReportProgress(1, loginUrl);
            }
            else
            {
                worker.ReportProgress(1, "1");
            }
         
        }
         
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (workerRun)
            {
                TimeRun(backgroundWorker1); 
                Thread.Sleep(3000);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string state = e.UserState as string;
            switch (state.Trim())
            {
                case "processing":
                    LogOut("二维码扫描请求开始...");
                    break;
                case "processed":
                    LogOut("二维码扫描请求结束");
                    break;
                case "1":
                    labelStatus.Text = "已登录";
                    workerRun = false;
                    backgroundWorker1.CancelAsync();
                    break;
                case "408":
                    LogOut("请求超时，需要重新生成二维码"); 
                    QrCode qrCode= service.NewQrCode(Global.robot.UUID);
                    pictureBoxQrCode.Image = qrCode.IMAGE;
                    labelStatus.Text = "等待扫描";
                    break;
                case "200":
                    labelStatus.Text = "已扫描待确认";
                    break;
                case "noRobot":
                    LogOut("非系统机器人，无法登陆");
                    Global.robot = null;
                    workerRun = false; 
                    backgroundWorker1.CancelAsync();
                    break;
                default:
                    workerRun = false;
                    syncRun = true;
                    labelStatus.Text = "扫描成功";
                    LogClear();
                    LogOut("微信号已登录");
                   
                    backgroundWorkerSync.RunWorkerAsync(); 
                    break;
            } 
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 同步消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorkerSync_DoWork(object sender, DoWorkEventArgs e)
        {
            string host = SyncCheck();
            if (!string.IsNullOrEmpty(host))
            {
                while (syncRun)
                {
                    Sync(backgroundWorkerSync, host);
                    Thread.Sleep(5000);
                }
            }
            else
            {
                backgroundWorkerSync.ReportProgress(100, "1100");
            }
        }

        private void backgroundWorkerSync_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //MsgHandle handle = new MsgHandle(); 
            string state = e.UserState as string;
            if (string.IsNullOrEmpty(state))
            {
                AddMsgList msg = e.UserState as AddMsgList;
                if (msg != null)
                {
                    LogOut(string.Format("消息类型：{0},FromUserName：{1},ToUserName：{2}，消息内容：{3}，\r\n NotifyUserName：{4}，时间：{5}",
                        msg.MsgType, msg.FromUserName, msg.ToUserName, msg.Content, msg.StatusNotifyUserName, DateTime.Now));
                 
                    MsgHandle handle = new MsgHandle();
                    int msgType = msg.MsgType;
                    switch (msgType)
                    {
                        case (int)MsgTypeEnum.系统消息:
                            handle.InitWxUser(msg);
                            break;
                        case (int)MsgTypeEnum.加好友:
                            if (buyService.HaveUser(Global.robot.Id, msg.RecommendInfo.NickName))
                            {
                                var result = protocol.VerifyUser(msg.RecommendInfo.UserName, msg.RecommendInfo.Ticket, Global.robot.Cookie, Global.robot.CookieStr);
                                LogOut("加好友" + result.ErrMsg);
                                //加完好友之后增加到机器人的好友表中
                                //wx_friend friend = new wx_friend();
                                //friend.UserName = msg.RecommendInfo.UserName;
                                //friend.NickName = msg.RecommendInfo.NickName;
                                //friend.AttrStatus = msg.RecommendInfo.AttrStatus;
                                //ContactService service = new ContactService();
                                //service.AddContent(friend); 
                                Global.robot.Contact = protocol.GetContact(Global.robot.Cookie, Global.robot.CookieStr).MemberList.ToList();
                                Task.Run(() =>
                                {
                                    protocol.StatusNotify(Global.robot.Cookie, Global.robot.CookieStr, Global.robot.UserName);
                                });

                                //发送消息
                                protocol.SendMsg("请输入验证码，如未购买，请访问", msg.RecommendInfo.UserName, 1, Global.robot.Cookie, Global.robot.CookieStr, Global.robot.UserName);

                            }
                            break;
                        case (int)MsgTypeEnum.群通知:
                            //好友已添加，被拉进群
                            if (msg.FromUserName.StartsWith("@@"))
                            {
                                //拉进群
                                //初始化
                                //拉别人进群也会有此通知
                                //判断群id是否存在，不存在初始化
                                var user = Global.robot.WxUserList.Where(u => u.UserName == msg.FromUserName).FirstOrDefault();
                                if (user == null)
                                {
                                    protocol.StatusNotify(Global.robot.Cookie, Global.robot.CookieStr, Global.robot.UserName);
                                }
                                //protocol.SendMsg("群绑定成功，机器人开始为您服务", msg.FromUserName, 1, Global.robot.Cookie, Global.robot.CookieStr, Global.robot.UserName);
                            }
                            else
                            {
                                //好友
                            }
                            break;
                        default:
                            if (msg.FromUserName.StartsWith("@@") && msg.ToUserName.StartsWith("@"))
                            {
                                //群消息对个人
                                string fromAlias = WechatCommon.GetTrueId(msg.FromUserName);
                                bool isBind = buyService.IsBind(fromAlias);
                                string content = msg.Content;
                            
                                if (isBind)
                                {
                                    //聊天记录
                                    wx_group_chat chat = new wx_group_chat();
                                    chat.Createtime = DateTime.Now;
                                    chat.MsgType = msg.MsgType;
                                    chat.GroupUserName = msg.FromUserName;
                                    chat.GroupId = WechatCommon.GetTrueId(msg.FromUserName);
                                 
                                    if (content.StartsWith("@"))
                                    {
                                        int startIndex = content.IndexOf(':');
                                        string fromUserName = content.Substring(0, startIndex); 
                                        chat.SendAlias = WechatCommon.GetTrueId(fromUserName); 
                                        chat.SendUserName = fromUserName;
                                        chat.Content = content.Substring(startIndex + 6); 
                                        chat.SendNickName = WechatCommon.GetNickNameFromGroup(msg.FromUserName, fromUserName);
                                        chat.SendAttrStatus = WechatCommon.GetAttrStatusFromGroup(msg.FromUserName, fromUserName);
                                    }
                                    else
                                    {
                                        chat.Content = content;
                                    }
                                    msgService.InsertMsg(chat);
                                }
                                //判断是否是开通群空间
                                if (content.StartsWith("@"))
                                {
                                    int startIndex = content.IndexOf(':');
                                    string fromUserName = content.Substring(0, startIndex);
                                   string sendAlias = WechatCommon.GetTrueId(fromUserName); 
                                   string message = content.Substring(startIndex + 6);
                                      
                                    string[] c= message.Split(' ');
                                    if (c != null && c.Length == 2)
                                    {
                                        if (c[0] == "@" + Global.robot.NickName && c[1] == "开通群空间")
                                        {
                                            //判断已经开通的群无法再次开通
                                            if (!isBind)
                                            {
                                                int recordId = buyService.GetBuyRecordId(Global.robot.Id, sendAlias);
                                                if (recordId > 0)
                                                {
                                                    int result = buyService.BindGroup(recordId, fromAlias);
                                                    if (result > 0)
                                                    {
                                                        protocol.SendMsg("群绑定成功，机器人开始为您服务", msg.FromUserName, 1, Global.robot.Cookie, Global.robot.CookieStr, Global.robot.UserName);
                                                        //群空间开通功能，捞取人员信息存入数据库等操作

                                                        var groupInfo = Global.robot.GroupInfo.Where(n => n.UserName == msg.FromUserName).FirstOrDefault();
                                                        if (groupInfo == null)
                                                        {
                                                            BatchGetContactResult contactResult = protocol.BatchGetContact(new List<string>() { msg.FromUserName }, Global.robot.Cookie, Global.robot.CookieStr);
                                                            if (contactResult != null)
                                                            {
                                                                groupInfo = contactResult.ContactList[0];
                                                            }
                                                        }
                                                        //存数据库
                                                        Task.Run(() => {
                                                            GroupService groupService = new GroupService();
                                                            groupService.SyncGroup(groupInfo);
                                                        }); 
                                                    }
                                                    else
                                                    {
                                                        protocol.SendMsg("群绑定失败", msg.FromUserName, 1, Global.robot.Cookie, Global.robot.CookieStr, Global.robot.UserName);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                protocol.SendMsg("群已经被绑定", msg.FromUserName, 1, Global.robot.Cookie, Global.robot.CookieStr, Global.robot.UserName);
                                            }
                                        }
                                    } 
                                } 
                            }
                            else if (msg.FromUserName.StartsWith("@") && msg.ToUserName.StartsWith("@@"))
                            {
                                //个人发群消息
                            }
                            else if (msg.FromUserName.StartsWith("@") && msg.ToUserName.StartsWith("@"))
                            {
                                //个人对个人
                                if (msg.ToUserName == Global.robot.UserName)
                                {
                                    //判断内容是否跟验证码符合
                                    string nickName = Global.robot.Contact.Where(n => n.UserName == msg.FromUserName).Select(n => n.NickName).FirstOrDefault();
                                    int recordId = buyService.VaildCode(Global.robot.Id, nickName, msg.Content);
                                    if (recordId > 0)
                                    {
                                        //绑定成功
                                        string alias = WechatCommon.GetTrueId(msg.FromUserName);
                                        if (buyService.UseCode(recordId, alias) > 0)
                                        {
                                            protocol.SendMsg("验证成功，请添加到群中", msg.FromUserName, 1, Global.robot.Cookie, Global.robot.CookieStr, Global.robot.UserName);

                                        }
                                        else
                                        {
                                            protocol.SendMsg("验证失败，请重新输入", msg.FromUserName, 1, Global.robot.Cookie, Global.robot.CookieStr, Global.robot.UserName);

                                        }
                                    }
                                    else
                                    {
                                        protocol.SendMsg("验证码错误，如未购买，请访问", msg.FromUserName, 1, Global.robot.Cookie, Global.robot.CookieStr, Global.robot.UserName);

                                    }
                                }
                            }
                            break;
                    }

                    //if (msg.MsgType == (int)MsgTypeEnum.加好友)
                    //{
                    //    //加好友
                    //    var result = protocol.VerifyUser(msg.RecommendInfo.UserName, msg.RecommendInfo.Ticket, Global.robot.Cookie, Global.robot.CookieStr);
                    //    LogOut("加好友" + result.ErrMsg);
                    //}
                    //else if (msg.MsgType == (int)MsgTypeEnum.系统消息&&msg.ToUserName.StartsWith("@@"))
                    //{
                    //    //群
                    //    //protocol.SendMsg("欢迎加入群聊天", msg.ToUserName, 1, Global.robot.Cookie, Global.robot.CookieStr,Global.robot.UserName);
                    //    //string html = protocol.BatchGetContact(msg.ToUserName, Global.robot.Cookie, Global.robot.CookieStr);
                    //}
                }
            }
            else if (state == "1100")
            {
                LogOut("拉取消息请求超时，请重新登录" + DateTime.Now);
                syncRun = false;
            }
            else if (state == "正常")
            {
                LogOut("0正常捞取" + DateTime.Now);
            }
            else
            {
                LogOut(state + "未知异常，请重新登录" + DateTime.Now);
                syncRun = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        { 
            if (Global.robot.State == 1)
            {
                Task.Run(() => {
                    protocol.LogOut(Global.robot.Cookie, Global.robot.CookieStr);
                }); 
            }
            Global.robot.State = 0;
            workerRun = false;
            syncRun = false;
            backgroundWorker1.CancelAsync();
            backgroundWorkerSync.CancelAsync();
            LogClear();
            pictureBoxQrCode.Image = null;
            comboBoxContact.Text = "";
           //comboBoxContact.Items.Clear();
        }

        private void buttonSync_Click(object sender, EventArgs e)
        {
            WechatRobot robot = Global.robot;

            if (robot.State == 1)
            {
                if (!backgroundWorkerSync.IsBusy)
                {
                    syncRun = true;
                    backgroundWorkerSync.RunWorkerAsync();

                }
                else
                {
                    if (backgroundWorkerSync.CancellationPending)
                    {
                        LogOut("取消中，请稍后再试！");
                    }
                    else
                    {
                        LogOut("后台进程仍在运行，请稍后再试");
                    }
                }
            }
            else
            {
                LogOut("微信号未上线");
            }
        }

        private void buttonSendMsg_Click(object sender, EventArgs e)
        {
            WechatRobot robot = Global.robot;
            if (robot.State == 1)
            {
                string username = textBoxToUserName.Text.Trim();
                string content = textBoxContent.Text.Trim();
                if (string.IsNullOrEmpty(username))
                {
                    LogOut("接收方Id未填");
                    return;
                }
                else if (string.IsNullOrEmpty(content))
                {
                    LogOut("内容未填");
                    return;
                }

                protocol.SendMsg(content, username, 1, robot.Cookie, robot.CookieStr, robot.UserName);
            }
            else
            {
                LogOut("微信号未上线");
            }
        }

        private void buttonContact_Click(object sender, EventArgs e)
        {
            WechatRobot robot = Global.robot;
            if (robot.State == 1)
            {
                backgroundWorkerContact.RunWorkerAsync();
            }
            else
            {
                LogOut("微信号未上线");
            }
        }

        private void backgroundWorkerContact_DoWork(object sender, DoWorkEventArgs e)
        {
            WechatRobot robot = Global.robot;
            ContactResult contact = protocol.GetContact(robot.Cookie, robot.CookieStr);
             
            e.Result = contact;
        }

        private void backgroundWorkerContact_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ContactResult contact = e.Result as ContactResult;
            InitContact(contact.MemberList);
            Global.robot.Contact = contact.MemberList.ToList();
            LogOut("联系人获取完成");
        }

        private void InitContact(IList<Member> MemberList)
        {
            var items= MemberList.Select(n => new ComboBoxItem { NickName = n.NickName, UserName = n.UserName }).ToList();

            comboBoxContact.DataSource = items;
            comboBoxContact.ValueMember = "UserName";
            comboBoxContact.DisplayMember = "NickName"; 
        }

        private void comboBoxContact_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxToUserName.Text = comboBoxContact.SelectedValue.ToString();
        }

        private void button2_Click_1(object sender, EventArgs e)
        { 
            WechatRobot robot = Global.robot;
            string username = textBoxUserName.Text;
            List<string> list_userName = new List<string>();
            list_userName.Add(username);
            BatchGetContactResult result= protocol.BatchGetContact(list_userName,robot.Cookie, robot.CookieStr);

        }

        private void buttonInit_Click(object sender, EventArgs e)
        {
          //  InitSyncResult initSyncResult = protocol.Init(Global.robot.Cookie, Global.robot.CookieStr);
            protocol.StatusNotify(Global.robot.Cookie, Global.robot.CookieStr, Global.robot.UserName);
        }

        private void buttonGetAction_Click(object sender, EventArgs e)
        {
           // protocol.GetIcon
        }

        private void buttonGetHeadimg_Click(object sender, EventArgs e)
        {
            string userName = textBoxHeadUserName.Text;
              Image img=  protocol.GetHeadImage(userName,Global.robot.CookieStr);
            pictureBoxIcon.Image = img;
        }
    }
}
