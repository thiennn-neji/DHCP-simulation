using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DHCPPacketNamespace;
using networkconfignamespace;

namespace DHCPServer
{
    public partial class Setting : Form
    {
        public Setting()
        {
            InitializeComponent();
        }       

        public bool isset { get; set; }
        public networkconfig network_config { get; set; }

        private void btnSetDefault_Click(object sender, EventArgs e)
        {
            network_config = new networkconfig();
            isset = true;
            this.Close();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            FileStream fs;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            try
            {
                fs = new FileStream(ofd.FileName, FileMode.Open);
                byte[] load = new byte[4096];
                int len = fs.Read(load, 0, 4096);
                fs.Close();
                byte[] data = new byte[len];
                for (int i = 0; i < len; i++)
                {
                    data[i] = load[i];
                }
                network_config = new networkconfig();
                network_config.fromByte(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }            
            if (!checkvalid())
            {
                network_config = new networkconfig();
                MessageBox.Show("Network config not valid");
                return;
            }
            isset = true;
            this.Close();
        }

        private void btnSetConfig_Click(object sender, EventArgs e)
        {
            try
            {
                network_config = new networkconfig();
                network_config.networkaddress = IPAddress.Parse(tb_NetworkAddress.Text.Trim());
                network_config.subnetmask = IPAddress.Parse(tb_SubnetMask.Text.Trim());
                network_config.dns = IPAddress.Parse(tb_DNS.Text.Trim());
                network_config.dhcpserver = IPAddress.Parse(tb_DHCPServerIP.Text.Trim());
                network_config.start = IPAddress.Parse(tb_IPStart.Text.Trim());
                network_config.end = IPAddress.Parse(tb_IPEnd.Text.Trim());
                network_config.leasetime = Int32.Parse(tb_LeaseTime.Text.Trim());
                for (int i = 0; i < lv_StaticIP.Items.Count; i++)
                {
                    staticip d = new staticip();
                    d.ip = IPAddress.Parse(lv_StaticIP.Items[i].SubItems[1].Text);
                    for (int j = 0; j < 6; j++)
                    {
                        d.mac[j] = (byte)Convert.ToInt32(lv_StaticIP.Items[i].SubItems[0].Text.Substring(j * 2, 2), 16);
                    }
                    network_config.static_ip.Add(d);
                }
            } catch (Exception z)
            {
                MessageBox.Show(z.Message);
               return;
            }
            
            if (!checkvalid())
            {
                network_config = new networkconfig();
                MessageBox.Show("Network config not valid");
                return;
            }
            isset = true;
            this.Close();
        }

        bool checkvalid()
        {

            return true;
        }

        private void btnAddStaticIP_Click(object sender, EventArgs e)
        {

        }
    }
}
