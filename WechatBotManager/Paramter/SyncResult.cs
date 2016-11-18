using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatBotManager.Paramter
{
    public class SyncResult
    {
        public int AddMsgCount { get; set; }

        public List<AddMsgList> AddMsgList { get; set; }

        public BaseResponse BaseResponse { get; set; }

        public int ContinueFlag { get; set; }

        public int DelContactCount { get; set; }

        public IList<object> DelContactList { get; set; }

        public int ModChatRoomMemberCount { get; set; }

        public IList<object> ModChatRoomMemberList { get; set; }

        public int ModContactCount { get; set; }

        public IList<ModContactList> ModContactList { get; set; }

        public Profile Profile { get; set; }

        public string SKey { get; set; }

        public SyncKey SyncKey { get; set; }

    }

    public class RecommendInfo
    {
        public string UserName { get; set; }
        public string NickName { get; set; }
        public int QQNum { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Content { get; set; }
        public string Signature { get; set; }
        public string Alias { get; set; }
        public int Scene { get; set; }
        public int VerifyFlag { get; set; }
        public int AttrStatus { get; set; }
        public int Sex { get; set; }
        public string Ticket { get; set; }
        public int OpCode { get; set; }
    }

    public class AppInfo
    {
        public string AppID { get; set; }
        public int Type { get; set; }
    }

    public class AddMsgList
    {
        public string MsgId { get; set; }
        public string FromUserName { get; set; }
        public string ToUserName { get; set; }
        public int MsgType { get; set; }
        public string Content { get; set; }
        public int Status { get; set; }
        public int ImgStatus { get; set; }
        public int CreateTime { get; set; }
        public int VoiceLength { get; set; }
        public int PlayLength { get; set; }
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string MediaId { get; set; }
        public string Url { get; set; }
        public int AppMsgType { get; set; }
        public int StatusNotifyCode { get; set; }
        public string StatusNotifyUserName { get; set; }
        public RecommendInfo RecommendInfo { get; set; }
        public int ForwardFlag { get; set; }
        public AppInfo AppInfo { get; set; }
        public int HasProductId { get; set; }
        public string Ticket { get; set; }
        public int ImgHeight { get; set; }
        public int ImgWidth { get; set; }
        public int SubMsgType { get; set; }
        public long NewMsgId { get; set; }
    }

    public class ModContactList
    {
        public string UserName { get; set; }
        public string NickName { get; set; }
        public int Sex { get; set; }
        public int HeadImgUpdateFlag { get; set; }
        public int ContactType { get; set; }
        public string Alias { get; set; }
        public string ChatRoomOwner { get; set; }
        public string HeadImgUrl { get; set; }
        public int ContactFlag { get; set; }
        public int MemberCount { get; set; }
        public IList<MemberList> MemberList { get; set; }
        public int HideInputBarFlag { get; set; }
        public string Signature { get; set; }
        public int VerifyFlag { get; set; }
        public string RemarkName { get; set; }
        public int Statues { get; set; }
        public int AttrStatus { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public int SnsFlag { get; set; }
        public string KeyWord { get; set; }
    }


    public class MemberList
    {
        public object Uin { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public int AttrStatus { get; set; }
        public string PYInitial { get; set; }
        public string PYQuanPin { get; set; }
        public string RemarkPYInitial { get; set; }
        public string RemarkPYQuanPin { get; set; }
        public int MemberStatus { get; set; }
        public string DisplayName { get; set; }
        public string KeyWord { get; set; }
    }

    public class UserName
    {
        public string Buff { get; set; }
    }

    public class NickName
    {
        public string Buff { get; set; }
    }

    public class BindEmail
    {
        public string Buff { get; set; }
    }

    public class BindMobile
    {
        public string Buff { get; set; }
    }

    public class Profile
    {
        public int BitFlag { get; set; }
        public UserName UserName { get; set; }
        public NickName NickName { get; set; }
        public int BindUin { get; set; }
        public BindEmail BindEmail { get; set; }
        public BindMobile BindMobile { get; set; }
        public int Status { get; set; }
        public int Sex { get; set; }
        public int PersonalCard { get; set; }
        public string Alias { get; set; }
        public int HeadImgUpdateFlag { get; set; }
        public string HeadImgUrl { get; set; }
        public string Signature { get; set; }
    }


}
