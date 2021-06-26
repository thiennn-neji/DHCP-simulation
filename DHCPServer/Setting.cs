using System;
using System.Collections;
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
                        d.mac[j] = (byte)Convert.ToInt32(lv_StaticIP.Items[i].SubItems[0].Text.Substring(j * 3, 2), 16);
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

        uint convert(IPAddress i)
        {
            byte[] d = i.GetAddressBytes().Reverse().ToArray();
            return BitConverter.ToUInt32(d, 0);
        }

        bool checksamesubnet(IPAddress network, IPAddress subnet, IPAddress ip)
        {
            return true;
        }

        bool checkvalid()
        {
            // check subnetmask (cac bit 1 tu trai sang phai)
            {
                byte[] subnetmask = network_config.subnetmask.GetAddressBytes();
                bool flag = true;
                for (int i = 0; i < 4; i++)
                {
                    if (subnetmask[i] == 255 && flag)
                    {
                        continue;
                    }
                    byte b = subnetmask[i];
                    for (int j = 0; j < 8; j++)
                    {
                        if (((b & (1 << j)) >> j) == 1) // lay bit thu j tu trai sang
                        {
                            if (!flag)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            flag = false;
                        }
                    }
                }
            }
            // check networkaddress
            {
                // check valid with subnetmask
                if ((convert(network_config.networkaddress) & convert(network_config.subnetmask)) != convert(network_config.networkaddress))
                {
                    return false;
                }
                // check is private IP and not loopback
                byte[] networkaddress = network_config.networkaddress.GetAddressBytes();
                if (networkaddress[0] == 127) // loopback
                {
                    return false;
                }
            }


            return true;
        }

        private void btnAddStaticIP_Click(object sender, EventArgs e)
        {
            AddStaticIP f = new AddStaticIP();
            f.ShowDialog();
            if (f.ok)
            {
                IPAddress g = f.ip;
                byte[] d = f.mac;
                for (int i = 0; i < network_config.static_ip.Count; i++)
                {
                    if (g == network_config.static_ip[i].ip)
                    {
                        MessageBox.Show("Static IP already used");
                        return;
                    }
                    if (d[0] == network_config.static_ip[i].mac[0])
                    {
                        bool flag = true;
                        for (int j = 1; j < 6; j++)
                        {                            
                            if (d[j] != network_config.static_ip[i].mac[j])
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag)
                        {
                            MessageBox.Show("Mac address already have ip");
                            return;
                        }
                    }                    
                }
                staticip s = new staticip();
                s.ip = g;
                for (int i = 0; i < 6; i++)
                {
                    s.mac[i] = d[i];
                }
                network_config.static_ip.Add(s);
            }            
        }
    }
}
