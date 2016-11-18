using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WechatBotManager.Models;

namespace WechatBotManager.Service
{
    public class RobotService
    {
        public int IsRobot(int uin)
        {
            string sql = @" select top 1 Id from wx_robot where Uin=@0 ";
            int r = tobotDB.GetInstance().ExecuteScalar<int?>(sql, uin) ?? 0;
            return r;
        }

        public int AddRobot(wx_robot robot)
        {
            int id = (int)tobotDB.GetInstance().Insert(robot); 
            return id;
        }

        /// <summary>
        /// 更新机器人信息
        /// </summary>
        /// <param name="robot"></param>
        public void UpdateRobot(wx_robot robot)
        {
            tobotDB.GetInstance().Update(robot);
        }

        public wx_robot GetRobot(int id)
        {
            string sql = @" select * from wx_robot where Id=@0 ";
           return  tobotDB.GetInstance().Query<wx_robot>(sql, id).FirstOrDefault();
        }

    }
}
