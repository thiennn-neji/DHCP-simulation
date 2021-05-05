using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DHCPStimulate
{
    public partial class DHCPClient : Form
    {
        private Button btnRenew;
        private Button btnRelease;
        private RichTextBox rtbMessage;
        private RichTextBox rtbIPInfo;
        private Label lbMess;
        private Label label1;

        public DHCPClient()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.btnRenew = new System.Windows.Forms.Button();
            this.btnRelease = new System.Windows.Forms.Button();
            this.rtbMessage = new System.Windows.Forms.RichTextBox();
            this.rtbIPInfo = new System.Windows.Forms.RichTextBox();
            this.lbMess = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnRenew
            // 
            this.btnRenew.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRenew.Location = new System.Drawing.Point(601, 21);
            this.btnRenew.Name = "btnRenew";
            this.btnRenew.Size = new System.Drawing.Size(195, 59);
            this.btnRenew.TabIndex = 0;
            this.btnRenew.Text = "New / Renew";
            this.btnRenew.UseVisualStyleBackColor = true;
            this.btnRenew.Click += new System.EventHandler(this.btnRenew_Click);
            // 
            // btnRelease
            // 
            this.btnRelease.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRelease.Location = new System.Drawing.Point(837, 21);
            this.btnRelease.Name = "btnRelease";
            this.btnRelease.Size = new System.Drawing.Size(195, 59);
            this.btnRelease.TabIndex = 0;
            this.btnRelease.Text = "Release";
            this.btnRelease.UseVisualStyleBackColor = true;
            this.btnRelease.Click += new System.EventHandler(this.btnRelease_Click);
            // 
            // rtbMessage
            // 
            this.rtbMessage.Location = new System.Drawing.Point(13, 75);
            this.rtbMessage.Name = "rtbMessage";
            this.rtbMessage.Size = new System.Drawing.Size(551, 420);
            this.rtbMessage.TabIndex = 1;
            this.rtbMessage.Text = "";
            // 
            // rtbIPInfo
            // 
            this.rtbIPInfo.Location = new System.Drawing.Point(601, 146);
            this.rtbIPInfo.Name = "rtbIPInfo";
            this.rtbIPInfo.Size = new System.Drawing.Size(420, 349);
            this.rtbIPInfo.TabIndex = 2;
            this.rtbIPInfo.Text = "";
            // 
            // lbMess
            // 
            this.lbMess.AutoSize = true;
            this.lbMess.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMess.Location = new System.Drawing.Point(12, 21);
            this.lbMess.Name = "lbMess";
            this.lbMess.Size = new System.Drawing.Size(88, 28);
            this.lbMess.TabIndex = 3;
            this.lbMess.Text = "Message";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(596, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 28);
            this.label1.TabIndex = 3;
            this.label1.Text = "Parameter Info";
            // 
            // DHCPClient
            // 
            this.ClientSize = new System.Drawing.Size(1057, 537);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbMess);
            this.Controls.Add(this.rtbIPInfo);
            this.Controls.Add(this.rtbMessage);
            this.Controls.Add(this.btnRelease);
            this.Controls.Add(this.btnRenew);
            this.Name = "DHCPClient";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void btnRenew_Click(object sender, EventArgs e)
        {

        }

        private void btnRelease_Click(object sender, EventArgs e)
        {

        }
    }
}
