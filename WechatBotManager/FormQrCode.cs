using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WechatBotManager.Paramter;

namespace WechatBotManager
{
    public partial class FormQrCode : Form
    {
        private IList<QrCode> qrCodeList { get; set; }
        WechatService service = new WechatService();
        WechatProtocol protocol = new WechatProtocol();
        public FormQrCode()
        {
            InitializeComponent();
        }

        public FormQrCode(IList<QrCode> qrCodeList)
        {
            InitializeComponent();
            this.qrCodeList = qrCodeList;
        }

        private void FormQrCode_Load(object sender, EventArgs e)
        {
            foreach (var qrCode in qrCodeList)
            { 
                PictureBox pic = new PictureBox();
                Size size = new Size(300, 300);
                pic.Size = size;
                pic.Image = qrCode.IMAGE;
                pic.Name = qrCode.UUID;
                flowLayoutPanel1.Controls.Add(pic); 
            }
            backgroundWorker1.RunWorkerAsync(); 
        }

        private void TimeRun(BackgroundWorker worker)
        {
            for (int r = 0; r < Global.robotList.Count; r++)
            {
                var robot = Global.robotList[r]; 
                if (robot.State == 0)
                {
                    string loginUrl =protocol.CheckLogin(robot.UUID);

                    if (loginUrl == "408")
                    {
                       
                     //   timerQrCode.Enabled = false;
                    }
                    else if (loginUrl == "200")
                    {
                        // Thread.Sleep(2000);
                    }
                    else {
                        CookieResult cookieResult = protocol.GetCookie(loginUrl);
                        robot.CookieStr = cookieResult.CookieStr;
                        robot.Cookie = cookieResult.wechatCookie;
                        robot.SyncCookie = cookieResult.SyncCookie;

                        InitSyncResult initSyncResult =protocol.Init(robot.Cookie, robot.CookieStr);
                        robot.SyncKey = initSyncResult.SyncKey;
                        robot.SynckeyStr = initSyncResult.SynckeyStr;
                        robot.UserName = initSyncResult.UserName;

                       protocol.StatusNotify(robot.Cookie, robot.CookieStr, robot.UserName);
                        robot.State = 1;
                        Global.robotList[r] = robot;

                        if (worker.CancellationPending)
                            return;
                        worker.ReportProgress(1, robot.UUID);
                    }
                }
            }
               
        }

        private void timerQrCode_Tick(object sender, EventArgs e)
        {
            timerQrCode.Stop();
           // TimeRun();
            timerQrCode.Start();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                TimeRun(backgroundWorker1);
                Thread.Sleep(3000);
            } 
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string uuid = e.UserState as string;
            for (int i = 0; i < flowLayoutPanel1.Controls.Count; i++)
            {
                if (flowLayoutPanel1.Controls[i].Name ==uuid)
                {
                    var control= flowLayoutPanel1.Controls.Find(uuid, false).First();
                    flowLayoutPanel1.Controls.Remove(control);
                    break;
                }
            }
        }
    }
}
