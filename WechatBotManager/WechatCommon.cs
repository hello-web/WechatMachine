using CsharpHttpHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WechatBotManager.Paramter;

namespace WechatBotManager
{
    public class WechatCommon
    {
        /// <summary>
        /// 时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetTicks()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }

        public static string SyncKeyFormat(SyncKey syncKey)
        {
            StringBuilder result = new StringBuilder();
            foreach (var key in syncKey.List)
            {
                result.AppendFormat("{0}_{1}|", key.Key, key.Val);
            }
             
            string str = result.ToString().TrimEnd('|');
            str= HttpHelper.URLEncode(str);
            return str;
        }

        /// <summary>
        /// 根据UserName获取永久Id
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static string GetTrueId(string userName)
        {
           string id= Global.robot.WxUserList.Where(n => n.UserName == userName).Select(n => n.Id).FirstOrDefault();
           return id;
        }

        /// <summary>
        /// 从群中找对应人的昵称
        /// </summary>
        /// <param name="groupUserName"></param>
        /// <returns></returns>
        public static string GetNickNameFromGroup(string groupUserName,string userName)
        {
            string nickName = string.Empty;
            IList<MemberList> memberList = Global.robot.GroupInfo.Where(n => n.UserName == groupUserName).Select(n => n.MemberList).FirstOrDefault();
            if (memberList != null && memberList.Count > 0)
            {
                nickName = memberList.Where(n => n.UserName == userName).Select(n => n.NickName).FirstOrDefault();
            }
            return nickName;
        }

        public static int GetAttrStatusFromGroup(string groupUserName, string userName)
        {
            int attrStatus = 0;
            IList<MemberList> memberList = Global.robot.GroupInfo.Where(n => n.UserName == groupUserName).Select(n => n.MemberList).FirstOrDefault();
            if (memberList != null && memberList.Count > 0)
            {
                attrStatus = memberList.Where(n => n.UserName == userName).Select(n => n.AttrStatus).FirstOrDefault();
            }
            return attrStatus;
        }

    }
}
