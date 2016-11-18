namespace WechatBotManager
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonStart = new System.Windows.Forms.Button();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonInit = new System.Windows.Forms.Button();
            this.textBoxUserName = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.buttonContact = new System.Windows.Forms.Button();
            this.buttonSync = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBoxQrCode = new System.Windows.Forms.PictureBox();
            this.labelStatus = new System.Windows.Forms.Label();
            this.backgroundWorkerSync = new System.ComponentModel.BackgroundWorker();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBoxMsgType = new System.Windows.Forms.ComboBox();
            this.labelMsgType = new System.Windows.Forms.Label();
            this.buttonSendMsg = new System.Windows.Forms.Button();
            this.textBoxContent = new System.Windows.Forms.TextBox();
            this.textBoxToUserName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.backgroundWorkerContact = new System.ComponentModel.BackgroundWorker();
            this.comboBoxContact = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxHeadUserName = new System.Windows.Forms.TextBox();
            this.buttonGetAction = new System.Windows.Forms.Button();
            this.buttonGetHeadimg = new System.Windows.Forms.Button();
            this.pictureBoxIcon = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxQrCode)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(28, 23);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 0;
            this.buttonStart.Text = "启动";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // textBoxLog
            // 
            this.textBoxLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBoxLog.Location = new System.Drawing.Point(0, 464);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLog.Size = new System.Drawing.Size(986, 375);
            this.textBoxLog.TabIndex = 1;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pictureBoxIcon);
            this.groupBox1.Controls.Add(this.buttonGetHeadimg);
            this.groupBox1.Controls.Add(this.buttonGetAction);
            this.groupBox1.Controls.Add(this.textBoxHeadUserName);
            this.groupBox1.Controls.Add(this.buttonInit);
            this.groupBox1.Controls.Add(this.textBoxUserName);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.buttonContact);
            this.groupBox1.Controls.Add(this.buttonSync);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.buttonStart);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(463, 211);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "基本功能";
            // 
            // buttonInit
            // 
            this.buttonInit.Location = new System.Drawing.Point(28, 60);
            this.buttonInit.Name = "buttonInit";
            this.buttonInit.Size = new System.Drawing.Size(75, 23);
            this.buttonInit.TabIndex = 7;
            this.buttonInit.Text = "初始化";
            this.buttonInit.UseVisualStyleBackColor = true;
            this.buttonInit.Click += new System.EventHandler(this.buttonInit_Click);
            // 
            // textBoxUserName
            // 
            this.textBoxUserName.Location = new System.Drawing.Point(126, 60);
            this.textBoxUserName.Name = "textBoxUserName";
            this.textBoxUserName.Size = new System.Drawing.Size(201, 21);
            this.textBoxUserName.TabIndex = 6;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(333, 60);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "批量获取";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // buttonContact
            // 
            this.buttonContact.Location = new System.Drawing.Point(333, 23);
            this.buttonContact.Name = "buttonContact";
            this.buttonContact.Size = new System.Drawing.Size(75, 23);
            this.buttonContact.TabIndex = 4;
            this.buttonContact.Text = "同步联系人";
            this.buttonContact.UseVisualStyleBackColor = true;
            this.buttonContact.Click += new System.EventHandler(this.buttonContact_Click);
            // 
            // buttonSync
            // 
            this.buttonSync.Location = new System.Drawing.Point(233, 23);
            this.buttonSync.Name = "buttonSync";
            this.buttonSync.Size = new System.Drawing.Size(75, 23);
            this.buttonSync.TabIndex = 3;
            this.buttonSync.Text = "重新同步";
            this.buttonSync.UseVisualStyleBackColor = true;
            this.buttonSync.Click += new System.EventHandler(this.buttonSync_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(126, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "退出";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBoxQrCode
            // 
            this.pictureBoxQrCode.Location = new System.Drawing.Point(674, 12);
            this.pictureBoxQrCode.Name = "pictureBoxQrCode";
            this.pictureBoxQrCode.Size = new System.Drawing.Size(300, 300);
            this.pictureBoxQrCode.TabIndex = 3;
            this.pictureBoxQrCode.TabStop = false;
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(811, 315);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(29, 12);
            this.labelStatus.TabIndex = 4;
            this.labelStatus.Text = "状态";
            // 
            // backgroundWorkerSync
            // 
            this.backgroundWorkerSync.WorkerReportsProgress = true;
            this.backgroundWorkerSync.WorkerSupportsCancellation = true;
            this.backgroundWorkerSync.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerSync_DoWork);
            this.backgroundWorkerSync.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerSync_ProgressChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBoxMsgType);
            this.groupBox2.Controls.Add(this.labelMsgType);
            this.groupBox2.Controls.Add(this.buttonSendMsg);
            this.groupBox2.Controls.Add(this.textBoxContent);
            this.groupBox2.Controls.Add(this.textBoxToUserName);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(12, 229);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(463, 217);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "发送消息";
            // 
            // comboBoxMsgType
            // 
            this.comboBoxMsgType.FormattingEnabled = true;
            this.comboBoxMsgType.Items.AddRange(new object[] {
            "文本"});
            this.comboBoxMsgType.Location = new System.Drawing.Point(108, 60);
            this.comboBoxMsgType.Name = "comboBoxMsgType";
            this.comboBoxMsgType.Size = new System.Drawing.Size(102, 20);
            this.comboBoxMsgType.TabIndex = 7;
            // 
            // labelMsgType
            // 
            this.labelMsgType.AutoSize = true;
            this.labelMsgType.Location = new System.Drawing.Point(23, 63);
            this.labelMsgType.Name = "labelMsgType";
            this.labelMsgType.Size = new System.Drawing.Size(65, 12);
            this.labelMsgType.TabIndex = 6;
            this.labelMsgType.Text = "消息类型：";
            // 
            // buttonSendMsg
            // 
            this.buttonSendMsg.Location = new System.Drawing.Point(372, 184);
            this.buttonSendMsg.Name = "buttonSendMsg";
            this.buttonSendMsg.Size = new System.Drawing.Size(75, 23);
            this.buttonSendMsg.TabIndex = 4;
            this.buttonSendMsg.Text = "发送消息";
            this.buttonSendMsg.UseVisualStyleBackColor = true;
            this.buttonSendMsg.Click += new System.EventHandler(this.buttonSendMsg_Click);
            // 
            // textBoxContent
            // 
            this.textBoxContent.Location = new System.Drawing.Point(108, 100);
            this.textBoxContent.Multiline = true;
            this.textBoxContent.Name = "textBoxContent";
            this.textBoxContent.Size = new System.Drawing.Size(339, 78);
            this.textBoxContent.TabIndex = 3;
            // 
            // textBoxToUserName
            // 
            this.textBoxToUserName.Location = new System.Drawing.Point(108, 20);
            this.textBoxToUserName.Name = "textBoxToUserName";
            this.textBoxToUserName.Size = new System.Drawing.Size(339, 21);
            this.textBoxToUserName.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 129);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "消息：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "接收方Id：";
            // 
            // backgroundWorkerContact
            // 
            this.backgroundWorkerContact.WorkerReportsProgress = true;
            this.backgroundWorkerContact.WorkerSupportsCancellation = true;
            this.backgroundWorkerContact.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerContact_DoWork);
            this.backgroundWorkerContact.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerContact_RunWorkerCompleted);
            // 
            // comboBoxContact
            // 
            this.comboBoxContact.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.comboBoxContact.FormattingEnabled = true;
            this.comboBoxContact.Location = new System.Drawing.Point(481, 38);
            this.comboBoxContact.Name = "comboBoxContact";
            this.comboBoxContact.Size = new System.Drawing.Size(187, 306);
            this.comboBoxContact.TabIndex = 6;
            this.comboBoxContact.SelectedIndexChanged += new System.EventHandler(this.comboBoxContact_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(481, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "联系人列表";
            // 
            // textBoxHeadUserName
            // 
            this.textBoxHeadUserName.Location = new System.Drawing.Point(6, 98);
            this.textBoxHeadUserName.Name = "textBoxHeadUserName";
            this.textBoxHeadUserName.Size = new System.Drawing.Size(240, 21);
            this.textBoxHeadUserName.TabIndex = 8;
            // 
            // buttonGetAction
            // 
            this.buttonGetAction.Location = new System.Drawing.Point(253, 98);
            this.buttonGetAction.Name = "buttonGetAction";
            this.buttonGetAction.Size = new System.Drawing.Size(75, 23);
            this.buttonGetAction.TabIndex = 9;
            this.buttonGetAction.Text = "个人";
            this.buttonGetAction.UseVisualStyleBackColor = true;
            this.buttonGetAction.Click += new System.EventHandler(this.buttonGetAction_Click);
            // 
            // buttonGetHeadimg
            // 
            this.buttonGetHeadimg.Location = new System.Drawing.Point(344, 98);
            this.buttonGetHeadimg.Name = "buttonGetHeadimg";
            this.buttonGetHeadimg.Size = new System.Drawing.Size(75, 23);
            this.buttonGetHeadimg.TabIndex = 10;
            this.buttonGetHeadimg.Text = "群";
            this.buttonGetHeadimg.UseVisualStyleBackColor = true;
            this.buttonGetHeadimg.Click += new System.EventHandler(this.buttonGetHeadimg_Click);
            // 
            // pictureBoxIcon
            // 
            this.pictureBoxIcon.Location = new System.Drawing.Point(355, 127);
            this.pictureBoxIcon.Name = "pictureBoxIcon";
            this.pictureBoxIcon.Size = new System.Drawing.Size(92, 78);
            this.pictureBoxIcon.TabIndex = 11;
            this.pictureBoxIcon.TabStop = false;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(986, 839);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBoxContact);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.pictureBoxQrCode);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBoxLog);
            this.Name = "FormMain";
            this.Text = "微信机器人";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxQrCode)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBoxQrCode;
        private System.Windows.Forms.Label labelStatus;
        private System.ComponentModel.BackgroundWorker backgroundWorkerSync;
        private System.Windows.Forms.Button buttonSync;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboBoxMsgType;
        private System.Windows.Forms.Label labelMsgType;
        private System.Windows.Forms.Button buttonSendMsg;
        private System.Windows.Forms.TextBox textBoxContent;
        private System.Windows.Forms.TextBox textBoxToUserName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonContact;
        private System.ComponentModel.BackgroundWorker backgroundWorkerContact;
        private System.Windows.Forms.ComboBox comboBoxContact;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBoxUserName;
        private System.Windows.Forms.Button buttonInit;
        private System.Windows.Forms.Button buttonGetHeadimg;
        private System.Windows.Forms.Button buttonGetAction;
        private System.Windows.Forms.TextBox textBoxHeadUserName;
        private System.Windows.Forms.PictureBox pictureBoxIcon;
    }
}

