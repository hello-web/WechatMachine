using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WechatBotManager.Paramter;

namespace WechatBotManager
{
    /// <summary>
    /// 微信机器人
    /// </summary>
    public class WechatRobot
    {
        #region 机器人配置信息
        public WechatRobot()
        {
            _createTime = DateTime.Now;
           
            _guid = Guid.NewGuid().ToString();
        }

        private string _guid;
        public string GUID
        {
            get {
                return _guid;
            }
        }

        private DateTime _createTime;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return _createTime;
            }
        }
 
        /// <summary>
        /// 状态；1上线，0下线
        /// </summary>
        public int State
        {
            get;set;
        }
        #endregion

        public string UUID { get; set; }
        /// <summary>
        /// 微信用户名（加密字段）
        /// </summary>
        public string UserName { get; set; }

        public WechatCookie Cookie = new WechatCookie(); 
        public string CookieStr { get; set; }
        /// <summary>
        /// 同步专用cookie
        /// </summary>
        public string SyncCookie { get; set; }

        public SyncKey SyncKey = new SyncKey();
        public string SynckeyStr { get; set; }

        /// <summary>
        /// 好友
        /// </summary>
        public List<Member> Contact = new List<Member>();

        /// <summary>
        /// 账号上群和好友Id的映射关系
        /// </summary>
        public List<WxUser> WxUserList = new List<WxUser>();

        /// <summary>
        /// 群组
        /// </summary>
        public IList<ContactList> GroupInfo = new List<ContactList>();

        public string Alias { get; set; }

        /// <summary>
        /// 唯一键
        /// </summary>
        public int Uin { get; set; }

        /// <summary>
        /// 数据库主键
        /// </summary>
        public int Id { get; set; }

        public string NickName { get; set; }

    }
}
