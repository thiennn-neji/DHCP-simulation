
namespace DHCPServer
{
    partial class Setting
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
            this.btnSetConfig = new System.Windows.Forms.Button();
            this.btnSetDefault = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.tb_NetworkAddress = new System.Windows.Forms.TextBox();
            this.tb_SubnetMask = new System.Windows.Forms.TextBox();
            this.tb_DNS = new System.Windows.Forms.TextBox();
            this.tb_DefaultGateway = new System.Windows.Forms.TextBox();
            this.tb_DHCPServerIP = new System.Windows.Forms.TextBox();
            this.tb_IPStart = new System.Windows.Forms.TextBox();
            this.tb_IPEnd = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lb_DNS = new System.Windows.Forms.Label();
            this.lb_DefaultGateway = new System.Windows.Forms.Label();
            this.lb_DHCPServerIP = new System.Windows.Forms.Label();
            this.lb_IPStart = new System.Windows.Forms.Label();
            this.lb_IPEnd = new System.Windows.Forms.Label();
            this.lv_StaticIP = new System.Windows.Forms.ListView();
            this.MacAddr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.IP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label8 = new System.Windows.Forms.Label();
            this.btnAddStaticIP = new System.Windows.Forms.Button();
            this.tb_LeaseTime = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnClearStaticIP = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSetConfig
            // 
            this.btnSetConfig.Font = new System.Drawing.Font("Segoe Script", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetConfig.Location = new System.Drawing.Point(109, 397);
            this.btnSetConfig.Name = "btnSetConfig";
            this.btnSetConfig.Size = new System.Drawing.Size(168, 41);
            this.btnSetConfig.TabIndex = 0;
            this.btnSetConfig.Text = "OK";
            this.btnSetConfig.UseVisualStyleBackColor = true;
            this.btnSetConfig.Click += new System.EventHandler(this.btnSetConfig_Click);
            // 
            // btnSetDefault
            // 
            this.btnSetDefault.Font = new System.Drawing.Font("Segoe Script", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetDefault.Location = new System.Drawing.Point(326, 396);
            this.btnSetDefault.Name = "btnSetDefault";
            this.btnSetDefault.Size = new System.Drawing.Size(168, 41);
            this.btnSetDefault.TabIndex = 1;
            this.btnSetDefault.Text = "Use default";
            this.btnSetDefault.UseVisualStyleBackColor = true;
            this.btnSetDefault.Click += new System.EventHandler(this.btnSetDefault_Click);
            // 
            // btnImport
            // 
            this.btnImport.Font = new System.Drawing.Font("Segoe Script", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImport.Location = new System.Drawing.Point(534, 396);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(168, 41);
            this.btnImport.TabIndex = 2;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // tb_NetworkAddress
            // 
            this.tb_NetworkAddress.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_NetworkAddress.Location = new System.Drawing.Point(12, 58);
            this.tb_NetworkAddress.Name = "tb_NetworkAddress";
            this.tb_NetworkAddress.Size = new System.Drawing.Size(253, 30);
            this.tb_NetworkAddress.TabIndex = 3;
            // 
            // tb_SubnetMask
            // 
            this.tb_SubnetMask.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_SubnetMask.Location = new System.Drawing.Point(12, 94);
            this.tb_SubnetMask.Name = "tb_SubnetMask";
            this.tb_SubnetMask.Size = new System.Drawing.Size(253, 30);
            this.tb_SubnetMask.TabIndex = 4;
            // 
            // tb_DNS
            // 
            this.tb_DNS.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_DNS.Location = new System.Drawing.Point(12, 130);
            this.tb_DNS.Name = "tb_DNS";
            this.tb_DNS.Size = new System.Drawing.Size(253, 30);
            this.tb_DNS.TabIndex = 5;
            // 
            // tb_DefaultGateway
            // 
            this.tb_DefaultGateway.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_DefaultGateway.Location = new System.Drawing.Point(12, 166);
            this.tb_DefaultGateway.Name = "tb_DefaultGateway";
            this.tb_DefaultGateway.Size = new System.Drawing.Size(253, 30);
            this.tb_DefaultGateway.TabIndex = 6;
            // 
            // tb_DHCPServerIP
            // 
            this.tb_DHCPServerIP.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_DHCPServerIP.Location = new System.Drawing.Point(12, 202);
            this.tb_DHCPServerIP.Name = "tb_DHCPServerIP";
            this.tb_DHCPServerIP.Size = new System.Drawing.Size(253, 30);
            this.tb_DHCPServerIP.TabIndex = 7;
            // 
            // tb_IPStart
            // 
            this.tb_IPStart.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_IPStart.Location = new System.Drawing.Point(12, 238);
            this.tb_IPStart.Name = "tb_IPStart";
            this.tb_IPStart.Size = new System.Drawing.Size(253, 30);
            this.tb_IPStart.TabIndex = 8;
            // 
            // tb_IPEnd
            // 
            this.tb_IPEnd.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_IPEnd.Location = new System.Drawing.Point(12, 274);
            this.tb_IPEnd.Name = "tb_IPEnd";
            this.tb_IPEnd.Size = new System.Drawing.Size(253, 30);
            this.tb_IPEnd.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe Script", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(271, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(194, 33);
            this.label1.TabIndex = 10;
            this.label1.Text = "Network Address";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe Script", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(271, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(154, 33);
            this.label2.TabIndex = 11;
            this.label2.Text = "Subnet Mask";
            // 
            // lb_DNS
            // 
            this.lb_DNS.AutoSize = true;
            this.lb_DNS.Font = new System.Drawing.Font("Segoe Script", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_DNS.Location = new System.Drawing.Point(271, 130);
            this.lb_DNS.Name = "lb_DNS";
            this.lb_DNS.Size = new System.Drawing.Size(62, 33);
            this.lb_DNS.TabIndex = 12;
            this.lb_DNS.Text = "DNS";
            // 
            // lb_DefaultGateway
            // 
            this.lb_DefaultGateway.AutoSize = true;
            this.lb_DefaultGateway.Font = new System.Drawing.Font("Segoe Script", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_DefaultGateway.Location = new System.Drawing.Point(271, 166);
            this.lb_DefaultGateway.Name = "lb_DefaultGateway";
            this.lb_DefaultGateway.Size = new System.Drawing.Size(192, 33);
            this.lb_DefaultGateway.TabIndex = 13;
            this.lb_DefaultGateway.Text = "Default Gateway";
            // 
            // lb_DHCPServerIP
            // 
            this.lb_DHCPServerIP.AutoSize = true;
            this.lb_DHCPServerIP.Font = new System.Drawing.Font("Segoe Script", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_DHCPServerIP.Location = new System.Drawing.Point(271, 202);
            this.lb_DHCPServerIP.Name = "lb_DHCPServerIP";
            this.lb_DHCPServerIP.Size = new System.Drawing.Size(183, 33);
            this.lb_DHCPServerIP.TabIndex = 14;
            this.lb_DHCPServerIP.Text = "DHCP Server IP";
            // 
            // lb_IPStart
            // 
            this.lb_IPStart.AutoSize = true;
            this.lb_IPStart.Font = new System.Drawing.Font("Segoe Script", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_IPStart.Location = new System.Drawing.Point(271, 238);
            this.lb_IPStart.Name = "lb_IPStart";
            this.lb_IPStart.Size = new System.Drawing.Size(97, 33);
            this.lb_IPStart.TabIndex = 15;
            this.lb_IPStart.Text = "IP Start";
            // 
            // lb_IPEnd
            // 
            this.lb_IPEnd.AutoSize = true;
            this.lb_IPEnd.Font = new System.Drawing.Font("Segoe Script", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_IPEnd.Location = new System.Drawing.Point(271, 274);
            this.lb_IPEnd.Name = "lb_IPEnd";
            this.lb_IPEnd.Size = new System.Drawing.Size(89, 33);
            this.lb_IPEnd.TabIndex = 16;
            this.lb_IPEnd.Text = "IP End";
            // 
            // lv_StaticIP
            // 
            this.lv_StaticIP.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.MacAddr,
            this.IP});
            this.lv_StaticIP.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lv_StaticIP.HideSelection = false;
            this.lv_StaticIP.Location = new System.Drawing.Point(484, 58);
            this.lv_StaticIP.Name = "lv_StaticIP";
            this.lv_StaticIP.Size = new System.Drawing.Size(309, 239);
            this.lv_StaticIP.TabIndex = 17;
            this.lv_StaticIP.UseCompatibleStateImageBehavior = false;
            this.lv_StaticIP.View = System.Windows.Forms.View.Details;
            // 
            // MacAddr
            // 
            this.MacAddr.Text = "MacAddr";
            this.MacAddr.Width = 134;
            // 
            // IP
            // 
            this.IP.Text = "IP";
            this.IP.Width = 118;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe Script", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(594, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(108, 33);
            this.label8.TabIndex = 18;
            this.label8.Text = "Static IP";
            // 
            // btnAddStaticIP
            // 
            this.btnAddStaticIP.Font = new System.Drawing.Font("Segoe Script", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddStaticIP.Location = new System.Drawing.Point(484, 310);
            this.btnAddStaticIP.Name = "btnAddStaticIP";
            this.btnAddStaticIP.Size = new System.Drawing.Size(146, 41);
            this.btnAddStaticIP.TabIndex = 19;
            this.btnAddStaticIP.Text = "Add";
            this.btnAddStaticIP.UseVisualStyleBackColor = true;
            this.btnAddStaticIP.Click += new System.EventHandler(this.btnAddStaticIP_Click);
            // 
            // tb_LeaseTime
            // 
            this.tb_LeaseTime.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_LeaseTime.Location = new System.Drawing.Point(12, 313);
            this.tb_LeaseTime.Name = "tb_LeaseTime";
            this.tb_LeaseTime.Size = new System.Drawing.Size(253, 30);
            this.tb_LeaseTime.TabIndex = 20;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe Script", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(271, 313);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 33);
            this.label3.TabIndex = 21;
            this.label3.Text = "Lease Time";
            // 
            // btnClearStaticIP
            // 
            this.btnClearStaticIP.Font = new System.Drawing.Font("Segoe Script", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearStaticIP.Location = new System.Drawing.Point(647, 310);
            this.btnClearStaticIP.Name = "btnClearStaticIP";
            this.btnClearStaticIP.Size = new System.Drawing.Size(146, 41);
            this.btnClearStaticIP.TabIndex = 22;
            this.btnClearStaticIP.Text = "Clear";
            this.btnClearStaticIP.UseVisualStyleBackColor = true;
            this.btnClearStaticIP.Click += new System.EventHandler(this.btnClearStaticIP_Click);
            // 
            // Setting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnClearStaticIP);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_LeaseTime);
            this.Controls.Add(this.btnAddStaticIP);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lv_StaticIP);
            this.Controls.Add(this.lb_IPEnd);
            this.Controls.Add(this.lb_IPStart);
            this.Controls.Add(this.lb_DHCPServerIP);
            this.Controls.Add(this.lb_DefaultGateway);
            this.Controls.Add(this.lb_DNS);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_IPEnd);
            this.Controls.Add(this.tb_IPStart);
            this.Controls.Add(this.tb_DHCPServerIP);
            this.Controls.Add(this.tb_DefaultGateway);
            this.Controls.Add(this.tb_DNS);
            this.Controls.Add(this.tb_SubnetMask);
            this.Controls.Add(this.tb_NetworkAddress);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnSetDefault);
            this.Controls.Add(this.btnSetConfig);
            this.Name = "Setting";
            this.Text = "Setting";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSetConfig;
        private System.Windows.Forms.Button btnSetDefault;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.TextBox tb_NetworkAddress;
        private System.Windows.Forms.TextBox tb_SubnetMask;
        private System.Windows.Forms.TextBox tb_DNS;
        private System.Windows.Forms.TextBox tb_DefaultGateway;
        private System.Windows.Forms.TextBox tb_DHCPServerIP;
        private System.Windows.Forms.TextBox tb_IPStart;
        private System.Windows.Forms.TextBox tb_IPEnd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lb_DNS;
        private System.Windows.Forms.Label lb_DefaultGateway;
        private System.Windows.Forms.Label lb_DHCPServerIP;
        private System.Windows.Forms.Label lb_IPStart;
        private System.Windows.Forms.Label lb_IPEnd;
        private System.Windows.Forms.ListView lv_StaticIP;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnAddStaticIP;
        private System.Windows.Forms.ColumnHeader MacAddr;
        private System.Windows.Forms.ColumnHeader IP;
        private System.Windows.Forms.TextBox tb_LeaseTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnClearStaticIP;
    }
}