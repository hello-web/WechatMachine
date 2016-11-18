using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatBotManager.Paramter
{ 
    public class List
    {
        public int Key { get; set; }
        public int Val { get; set; }
    }

    public class SyncKey
    {
        public int Count { get; set; }
        public IList<List> List { get; set; }
    }
     
}
