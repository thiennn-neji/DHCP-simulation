
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
            this.SuspendLayout();
            // 
            // btnRenew
            // 
            this.btnRenew.Location = new System.Drawing.Point(521, 12);
            this.btnRenew.Name = "btnRenew";
            this.btnRenew.Size = new System.Drawing.Size(157, 73);
            this.btnRenew.TabIndex = 0;
            this.btnRenew.Text = "New ";
            this.btnRenew.UseVisualStyleBackColor = true;
            this.btnRenew.Click += new System.EventHandler(this.btnRenew_Click);
            // 
            // btnRelease
            // 
            this.btnRelease.Location = new System.Drawing.Point(701, 12);
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
            this.lbMess.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMess.Location = new System.Drawing.Point(13, 30);
            this.lbMess.Name = "lbMess";
            this.lbMess.Size = new System.Drawing.Size(88, 28);
            this.lbMess.TabIndex = 1;
            this.lbMess.Text = "Message";
            // 
            // lbInfo
            // 
            this.lbInfo.AutoSize = true;
            this.lbInfo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbInfo.Location = new System.Drawing.Point(516, 102);
            this.lbInfo.Name = "lbInfo";
            this.lbInfo.Size = new System.Drawing.Size(139, 28);
            this.lbInfo.TabIndex = 1;
            this.lbInfo.Text = "Parameter Info";
            // 
            // rtbMess
            // 
            this.rtbMess.Location = new System.Drawing.Point(18, 87);
            this.rtbMess.Name = "rtbMess";
            this.rtbMess.Size = new System.Drawing.Size(466, 329);
            this.rtbMess.TabIndex = 2;
            this.rtbMess.Text = "";
            // 
            // rtbPara
            // 
            this.rtbPara.Location = new System.Drawing.Point(521, 147);
            this.rtbPara.Name = "rtbPara";
            this.rtbPara.Size = new System.Drawing.Size(318, 269);
            this.rtbPara.TabIndex = 3;
            this.rtbPara.Text = "";
            // 
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(851, 450);
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
    }
}