using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WechatBotManager.Models;
using WechatBotManager.Paramter;

namespace WechatBotManager.Service
{
    public class GroupService
    {
        /// <summary>
        /// 同步群信息
        /// </summary>
        /// <param name="groupList"></param>
        public void SyncGroup(IList<ContactList> groupList)
        {
            //新增
            //删除
            //修改
           List<wx_group> listGroup= tobotDB.GetInstance().Fetch<wx_group>("select * from wx_group where RobotId=@0 ",Global.robot.Id);
            List<string> groupIdList = listGroup.Select(n => n.GroupId).ToList(); //数据库中
            List<string> aliasList = groupList.Select(n => n.Alias).ToList(); //当前读取

            var delList= groupIdList.Except(aliasList).ToList();
            var insertList = aliasList.Except(groupIdList).ToList();
            var updateList= groupIdList.Intersect(aliasList).ToList();

            string groupId = string.Join(",", delList.ToArray());
            if (!string.IsNullOrEmpty(groupId))
            {
                string delSQL = string.Format(@" update wx_group set Isdelete=1 where GroupId in({0}) ", groupId);
                int result = tobotDB.GetInstance().Execute(delSQL);
            }

            foreach (string item in insertList)
            {
                ContactList contact= groupList.Where(n => n.Alias == item).FirstOrDefault();

                wx_group group = new wx_group();
                group.GroupId = contact.Alias;
                group.GroupName = contact.NickName;
                group.Isdelete = false;
                group.Createtime = DateTime.Now;
                group.Updatetime = DateTime.Now;
                group.HeadImgUrl = contact.HeadImgUrl;
                group.MemberCount = contact.MemberCount;
                group.OwnerUin = contact.OwnerUin;
                group.PYQuanPin = contact.PYQuanPin;
                group.RobotId = Global.robot.Id;
                group.UserName = contact.UserName;
                group.RobotAlias = Global.robot.Alias;
                group.Statues = contact.Statues;
                tobotDB.GetInstance().Insert(group);
            }

            foreach (string item in updateList)
            {
                wx_group group = listGroup.Where(n => n.GroupId == item).FirstOrDefault();
                tobotDB.GetInstance().Update(group);
            } 
        }

        /// <summary>
        /// 同步群人员信息
        /// </summary>
        public void SyncGroupMember(IList<MemberList> memberList,string groupId,string groupUserName)
        {
            string sql = " select * from wx_group_member where GroupId=@groupId and RobotId=@robotId ";
            List<wx_group_member> dbMemberList = tobotDB.GetInstance().Fetch<wx_group_member>(sql,
                new { groupId = groupId, robotId = Global.robot.Id });

            List<int> dbMemberAttrStatus = dbMemberList.Select(n => n.AttrStatus).ToList(); //数据库
            List<int> memberAttrStatus = memberList.Select(n => n.AttrStatus).ToList(); //最新
             
            var delList = dbMemberAttrStatus.Except(memberAttrStatus).ToList();
            var insertList = memberAttrStatus.Except(dbMemberAttrStatus).ToList();
            var updateList = dbMemberAttrStatus.Intersect(memberAttrStatus).ToList();
             
            string attrStatus = string.Join(",", delList.ToArray());
            if (!string.IsNullOrEmpty(attrStatus))
            {
                string delSQL = string.Format(@" update wx_group_member set IsDelete=1 where AttrStatus in({0}) ", attrStatus);
                int result = tobotDB.GetInstance().Execute(delSQL);
            }

            foreach (int item in insertList)
            {
                MemberList member = memberList.Where(n => n.AttrStatus == item).FirstOrDefault();
                wx_group_member m = new wx_group_member();
                try
                {
                    
                    m.Nickname = member.NickName;
                    m.Uin = member.Uin.ToString();
                    m.Createtime = DateTime.Now;
                    m.GroupId = groupId;
                    m.GroupUserName = groupUserName;
                    m.RobotId = Global.robot.Id;
                    m.UserName = member.UserName;
                    m.AttrStatus = member.AttrStatus;
                    m.IsDelete = false;
                    tobotDB.GetInstance().Insert(m);
                }
                catch (Exception ex)
                {

                }
            }

            foreach (int item in updateList)
            {
                wx_group_member member = dbMemberList.Where(n => n.AttrStatus == item).FirstOrDefault();
                tobotDB.GetInstance().Update(member);
            }

        }

        public void SyncGroup(ContactList contact)
        {
            string sql = @" select top 1 * from wx_group where RobotId=@robotId and IsDelete=0 and GroupId=@groupId ";
            string groupId = WechatCommon.GetTrueId(contact.UserName);

            wx_group group = tobotDB.GetInstance().Query<wx_group>(sql, new { robotId = Global.robot.Id, groupId = groupId }).FirstOrDefault();
            if (group == null)
            {
                group = new wx_group();
                group.GroupId = contact.Alias;
                group.Isdelete = false;
                group.GroupName = contact.NickName;
                group.Createtime = DateTime.Now;
                group.Updatetime = DateTime.Now;
                //group.HeadImgUrl = contact.HeadImgUrl;
                group.MemberCount = contact.MemberCount;
                group.OwnerUin = contact.OwnerUin;
                group.PYQuanPin = contact.PYQuanPin;
                group.RobotId = Global.robot.Id;
                group.UserName = contact.UserName;
                group.RobotAlias = Global.robot.Alias;
                group.Statues = contact.Statues;

                tobotDB.GetInstance().Insert(group);
            }
            else
            { 
                group.GroupName = contact.NickName;  
                group.Updatetime = DateTime.Now;
                //group.HeadImgUrl = contact.HeadImgUrl;
                group.MemberCount = contact.MemberCount;
                group.OwnerUin = contact.OwnerUin;
                group.PYQuanPin = contact.PYQuanPin;
                group.RobotId = Global.robot.Id;
                group.UserName = contact.UserName;
                group.RobotAlias = Global.robot.Alias;
                group.Statues = contact.Statues;

                tobotDB.GetInstance().Update(group);
            }

            SyncGroupMember(contact.MemberList, groupId, contact.UserName);
        }

    }
}
