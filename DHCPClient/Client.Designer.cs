
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
            this.rtbMess = new System.Windows.Forms.RichTextBox();
            this.rtbPara = new System.Windows.Forms.RichTextBox();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.btnExtendIP = new System.Windows.Forms.Button();
            this.btn_Extend = new DHCPClient.RJToggleButton();
            this.label1 = new System.Windows.Forms.Label();
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
            // rtbMess
            // 
            this.rtbMess.Location = new System.Drawing.Point(24, 137);
            this.rtbMess.Name = "rtbMess";
            this.rtbMess.Size = new System.Drawing.Size(774, 407);
            this.rtbMess.TabIndex = 2;
            this.rtbMess.Text = "";
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
            // btn_Extend
            // 
            this.btn_Extend.AutoSize = true;
            this.btn_Extend.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Extend.Location = new System.Drawing.Point(900, 32);
            this.btn_Extend.MinimumSize = new System.Drawing.Size(70, 35);
            this.btn_Extend.Name = "btn_Extend";
            this.btn_Extend.OffBackColor = System.Drawing.Color.Gray;
            this.btn_Extend.OffToggleColor = System.Drawing.Color.Gainsboro;
            this.btn_Extend.OnBackColor = System.Drawing.Color.MediumSlateBlue;
            this.btn_Extend.OnToggleColor = System.Drawing.Color.WhiteSmoke;
            this.btn_Extend.Size = new System.Drawing.Size(70, 35);
            this.btn_Extend.TabIndex = 6;
            this.btn_Extend.UseVisualStyleBackColor = true;
            this.btn_Extend.CheckedChanged += new System.EventHandler(this.btn_Extend_CheckedChanged);
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
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1262, 556);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_Extend);
            this.Controls.Add(this.btnExtendIP);
            this.Controls.Add(this.btnClearLog);
            this.Controls.Add(this.rtbPara);
            this.Controls.Add(this.rtbMess);
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
        private System.Windows.Forms.RichTextBox rtbMess;
        private System.Windows.Forms.RichTextBox rtbPara;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.Button btnExtendIP;
        private RJToggleButton btn_Extend;
        private System.Windows.Forms.Label label1;
    }
}