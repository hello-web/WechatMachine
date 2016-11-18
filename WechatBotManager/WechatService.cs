using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WechatBotManager.Paramter;

namespace WechatBotManager
{
    public class WechatService
    {
        WechatProtocol protocol = new WechatProtocol();

        public QrCode Start()
        {
            QrCode qrcode = new QrCode();
            WechatRobot robot = new WechatRobot(); 
            string uuid =protocol.GetUUid(); 
            robot.UUID = uuid; 
            Image image =protocol.GetQrCode(uuid);
            qrcode.UUID = uuid;
            qrcode.IMAGE = image;
            //Global.robotList.Add(robot);
            Global.robot = robot;
            return qrcode;
        }

        public QrCode NewQrCode(string uuid)
        {
            QrCode qrcode = new QrCode();
            Image image = protocol.GetQrCode(uuid);
            qrcode.UUID = uuid;
            qrcode.IMAGE = image;
     
            return qrcode;
        }

      
       
    }
}
