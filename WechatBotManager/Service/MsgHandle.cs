using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WechatBotManager.Models;
using WechatBotManager.Paramter;

namespace WechatBotManager.Service
{
    public class MsgHandle
    {
        
        /// <summary>
        /// 初始化微信用户关系映射
        /// </summary>
        public void InitWxUser(AddMsgList msg)
        { 
            string content = msg.Content;
            string statusNotifyUserName = msg.StatusNotifyUserName;

            if (!string.IsNullOrEmpty(content))
            {
                Regex reg = new Regex("<br/>&lt;op id='\\d'&gt;<br/>");

                Match m = reg.Match(content);
                if (m.Value == "<br/>&lt;op id='4'&gt;<br/>")
                {
                    List<WxUser> wxUserList = new List<WxUser>();
                    List<string> IdList = GetId(content);
                    List<string> userNameList = GetUserName(statusNotifyUserName);

                    #region 初始化微信用户关系
                    if (IdList.Count == userNameList.Count)
                    { 
                        for (int i = 0; i < IdList.Count; i++)
                        {
                            WxUser user = new WxUser();
                            user.Id = IdList[i];
                            user.UserName = userNameList[i];
                            wxUserList.Add(user); 
                        }
                        Global.robot.WxUserList = wxUserList; 
                    }
                    else
                    {
                        throw new Exception("数据不完整");
                    }
                    #endregion

                    WechatProtocol protocol = new WechatProtocol(); 
                    List<string> chatRoomIds = userNameList.Where(n => n.StartsWith("@@")).ToList();
                    var groupInfo = protocol.BatchGetContact(chatRoomIds, Global.robot.Cookie, Global.robot.CookieStr).ContactList;
                    Global.robot.GroupInfo = groupInfo;

                    /*
                   WechatProtocol protocol = new WechatProtocol();

                   #region 同步机器人信息
                   RobotService robotService = new RobotService();

                   wx_robot robot = robotService.GetRobot(Global.robot.Id); 
                   robot.Alias = WechatCommon.GetTrueId(Global.robot.UserName);
                   Global.robot.Alias = robot.Alias;
                   List<string> contact = new List<string>();
                   contact.Add(Global.robot.UserName);
                   var robotInfo = protocol.BatchGetContact(contact, Global.robot.Cookie, Global.robot.CookieStr);
                   var c = robotInfo.ContactList.FirstOrDefault();
                   if (c != null)
                   {
                       robot.Nickname = c.NickName;
                       robot.Sex = c.Sex;
                       robot.City = c.City;
                       robot.Province = c.Province;
                       robot.Headimgurl = c.HeadImgUrl;
                       robot.Updatetime = DateTime.Now;
                       robot.MemberCount = c.MemberCount;
                       robot.Signature = c.Signature;
                       robot.AttrStatus = c.AttrStatus;
                       robot.Status = c.Statues;
                   }


                   robotService.UpdateRobot(robot);

                   #endregion


                   #region　用户群同步
                   List<string> chatRoomIds = userNameList.Where(n => n.StartsWith("@@")).ToList();  
                   var groupInfo = protocol.BatchGetContact(chatRoomIds, Global.robot.Cookie, Global.robot.CookieStr).ContactList;
                   foreach (var group in groupInfo)
                   {
                       group.Alias = WechatCommon.GetTrueId(group.UserName);
                   }

                   Global.robot.GroupInfo = groupInfo;
                   //同步数据到数据库
                   GroupService service = new GroupService();
                   service.SyncGroup(Global.robot.GroupInfo);

                   #endregion

                   #region 群里面用户信息同步

                   //foreach (var group in Global.robot.GroupInfo)
                   //{
                   //    string groupId = WechatCommon.GetTrueId(group.UserName);
                   //    service.SyncGroupMember(group.MemberList, groupId, group.UserName);


                   //}



                   #endregion  
                   */
                }
            } 
        }

        /// <summary>
        /// 消息类型是51的
        /// </summary>
        private List<string> GetId(string content)
        {
            List<string> chatRoomId = new List<string>(); 
            int startIndex = content.IndexOf("&lt;username&gt;") + ("&lt;username&gt;").Length;
            int endIndex = content.IndexOf("&lt;/username&gt;<br/>");
            string chatRoomIdList= content.Substring(startIndex, endIndex- startIndex);
            chatRoomId = chatRoomIdList.Split(',').ToList(); 
            return chatRoomId;
        }

        private List<string> GetUserName(string StatusNotifyUserName)
        {
            List<string> listUserName =   StatusNotifyUserName.Split(',').ToList();  
            return listUserName;
        }

    }
}
