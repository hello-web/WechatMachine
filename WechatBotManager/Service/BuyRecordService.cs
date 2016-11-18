using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WechatBotManager.Models;

namespace WechatBotManager.Service
{
    /// <summary>
    /// 购买记录
    /// </summary>
    public class BuyRecordService
    {
        public int VaildCode(int robotId,string nickName, string code)
        {
            string sql = @" select Id from wx_BuyRecord 
                                where IsUsed=0 and RobotId=@robotId and NickName=@nickname 
                                and VerificationCode=@code and VerificationCodeTime>= getdate() ";

            int id = tobotDB.GetInstance().ExecuteScalar<int?>(sql, new { robotId=robotId,nickname=nickName,code=code })??0;
            return id; 
        }

        public int UseCode(int id,string alias)
        {
            string sql = @" update wx_BuyRecord set IsUsed=1,UseTime=getdate() where Alias=@alias and Id=@id and IsUsed=0 ";
            return  tobotDB.GetInstance().Execute(sql, new { alias=alias,id=id });

        }

        /// <summary>
        /// 查询是否能加好友
        /// </summary>
        /// <param name="robotId"></param>
        /// <param name="nickName"></param>
        /// <returns></returns>
        public bool HaveUser(int robotId, string nickName)
        {
            string sql = @" select count(1) from wx_BuyRecord where RobotId=@robotId 
                                and IsUsed=0 and VerificationCodeTime>= getdate() and NickName=@nickName ";
            int count= tobotDB.GetInstance().ExecuteScalar<int?>(sql, new { robotId=robotId,nickName=nickName })??0;
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;   
            }
        }

        /// <summary>
        /// 判断群是否已经开通过了
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public bool IsBind(string groupId)
        {
            string sql = @" select count(1) from wx_BuyRecord where GroupId=@0 and IsUsed=2  ";
            int result= tobotDB.GetInstance().ExecuteScalar<int?>(sql, groupId) ?? 0;
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

       /// <summary>
       /// 查询空闲名额id
       /// </summary>
       /// <param name="robotId"></param>
       /// <param name="alias"></param>
       /// <returns></returns>
        public int GetBuyRecordId(int robotId, string alias)
        {
            string sql = @" select top 1 Id from wx_BuyRecord where robotId=@robotId and Alias=@alias and IsUsed=1 ";
            int id = tobotDB.GetInstance().ExecuteScalar<int?>(sql, new { robotId = robotId, alias = alias }) ?? 0;
            return id;
        }

        /// <summary>
        /// 绑定群
        /// </summary>
        /// <param name="id"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public int BindGroup(int id, string groupId)
        {
            string sql = @" update wx_BuyRecord set BindTime=getdate(),GroupId=@groupId,IsUsed=2 
                               where Id=@id  and IsUsed=1";
           return tobotDB.GetInstance().Execute(sql, new { groupId = groupId, id = id });

        }

    }
}
