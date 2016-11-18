using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WechatBotManager.Models;

namespace WechatBotManager.Service
{
    /// <summary>
    /// 联系人管理操作
    /// </summary>
    public class ContactService
    {
        //保存联系人（登录后同步操作）

        //新增联系人
        public void AddContent(wx_friend friend)
        {
            tobotDB.GetInstance().Insert(friend);
        }


        //删除联系人

    }
}
