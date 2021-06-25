
namespace DHCPClient
{
    partial class Client
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnRenew = new System.Windows.Forms.Button();
            this.btnRelease = new System.Windows.Forms.Button();
            this.lbMess = new System.Windows.Forms.Label();
            this.lbInfo = new System.Windows.Forms.Label();
            this.rtbPara = new System.Windows.Forms.RichTextBox();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.btnExtendIP = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lv_Message = new System.Windows.Forms.ListView();
            this.coltype = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.coltime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnExtend = new DHCPClient.RJToggleButton();
            this.SuspendLayout();
            // 
            // btnRenew
            // 
            this.btnRenew.Font = new System.Drawing.Font("Segoe Script", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRenew.Location = new System.Drawing.Point(18, 12);
            this.btnRenew.Name = "btnRenew";
            this.btnRenew.Size = new System.Drawing.Size(157, 73);
            this.btnRenew.TabIndex = 0;
            this.btnRenew.Text = "New ";
            this.btnRenew.UseVisualStyleBackColor = true;
            this.btnRenew.Click += new System.EventHandler(this.btnRenew_Click);
            // 
            // btnRelease
            // 
            this.btnRelease.Font = new System.Drawing.Font("Segoe Script", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRelease.Location = new System.Drawing.Point(437, 12);
            this.btnRelease.Name = "btnRelease";
            this.btnRelease.Size = new System.Drawing.Size(148, 73);
            this.btnRelease.TabIndex = 0;
            this.btnRelease.Text = "Release";
            this.btnRelease.UseVisualStyleBackColor = true;
            this.btnRelease.Click += new System.EventHandler(this.btnRelease_Click);
            // 
            // lbMess
            // 
            this.lbMess.AutoSize = true;
            this.lbMess.Font = new System.Drawing.Font("Segoe Script", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMess.Location = new System.Drawing.Point(13, 102);
            this.lbMess.Name = "lbMess";
            this.lbMess.Size = new System.Drawing.Size(95, 33);
            this.lbMess.TabIndex = 1;
            this.lbMess.Text = "Message";
            // 
            // lbInfo
            // 
            this.lbInfo.AutoSize = true;
            this.lbInfo.Font = new System.Drawing.Font("Segoe Script", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbInfo.Location = new System.Drawing.Point(835, 102);
            this.lbInfo.Name = "lbInfo";
            this.lbInfo.Size = new System.Drawing.Size(172, 33);
            this.lbInfo.TabIndex = 1;
            this.lbInfo.Text = "Parameter Info";
            // 
            // rtbPara
            // 
            this.rtbPara.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbPara.Location = new System.Drawing.Point(818, 137);
            this.rtbPara.Name = "rtbPara";
            this.rtbPara.Size = new System.Drawing.Size(432, 407);
            this.rtbPara.TabIndex = 3;
            this.rtbPara.Text = "";
            // 
            // btnClearLog
            // 
            this.btnClearLog.Font = new System.Drawing.Font("Segoe Script", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearLog.Location = new System.Drawing.Point(641, 12);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(157, 73);
            this.btnClearLog.TabIndex = 4;
            this.btnClearLog.Text = "Clear log";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // btnExtendIP
            // 
            this.btnExtendIP.Font = new System.Drawing.Font("Segoe Script", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExtendIP.Location = new System.Drawing.Point(221, 12);
            this.btnExtendIP.Name = "btnExtendIP";
            this.btnExtendIP.Size = new System.Drawing.Size(157, 73);
            this.btnExtendIP.TabIndex = 5;
            this.btnExtendIP.Text = "Extend";
            this.btnExtendIP.UseVisualStyleBackColor = true;
            this.btnExtendIP.Click += new System.EventHandler(this.btnExtendIP_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe Script", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(976, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 33);
            this.label1.TabIndex = 7;
            this.label1.Text = "Auto Extend";
            // 
            // lv_Message
            // 
            this.lv_Message.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.coltype,
            this.coltime});
            this.lv_Message.Font = new System.Drawing.Font("Segoe Script", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lv_Message.HideSelection = false;
            this.lv_Message.Location = new System.Drawing.Point(13, 139);
            this.lv_Message.Name = "lv_Message";
            this.lv_Message.Size = new System.Drawing.Size(785, 405);
            this.lv_Message.TabIndex = 8;
            this.lv_Message.UseCompatibleStateImageBehavior = false;
            this.lv_Message.View = System.Windows.Forms.View.Details;
            this.lv_Message.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lv_Message_DoubleClick);
            // 
            // coltype
            // 
            this.coltype.Text = "DHCP Type";
            this.coltype.Width = 296;
            // 
            // coltime
            // 
            this.coltime.Text = "Time";
            this.coltime.Width = 300;
            // 
            // btnExtend
            // 
            this.btnExtend.AutoSize = true;
            this.btnExtend.Location = new System.Drawing.Point(881, 30);
            this.btnExtend.MinimumSize = new System.Drawing.Size(75, 35);
            this.btnExtend.Name = "btnExtend";
            this.btnExtend.OffBackColor = System.Drawing.Color.Gray;
            this.btnExtend.OffToggleColor = System.Drawing.Color.Gainsboro;
            this.btnExtend.OnBackColor = System.Drawing.Color.MediumSlateBlue;
            this.btnExtend.OnToggleColor = System.Drawing.Color.WhiteSmoke;
            this.btnExtend.Size = new System.Drawing.Size(75, 35);
            this.btnExtend.TabIndex = 9;
            this.btnExtend.UseVisualStyleBackColor = true;
            this.btnExtend.CheckedChanged += new System.EventHandler(this.btnExtend_CheckedChanged);
            // 
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1262, 556);
            this.Controls.Add(this.btnExtend);
            this.Controls.Add(this.lv_Message);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnExtendIP);
            this.Controls.Add(this.btnClearLog);
            this.Controls.Add(this.rtbPara);
            this.Controls.Add(this.lbInfo);
            this.Controls.Add(this.lbMess);
            this.Controls.Add(this.btnRelease);
            this.Controls.Add(this.btnRenew);
            this.Name = "Client";
            this.Text = "DHCP_Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRenew;
        private System.Windows.Forms.Button btnRelease;
        private System.Windows.Forms.Label lbMess;
        private System.Windows.Forms.Label lbInfo;
        private System.Windows.Forms.RichTextBox rtbPara;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.Button btnExtendIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView lv_Message;
        private System.Windows.Forms.ColumnHeader coltype;
        private System.Windows.Forms.ColumnHeader coltime;
        private RJToggleButton btnExtend;
    }
}