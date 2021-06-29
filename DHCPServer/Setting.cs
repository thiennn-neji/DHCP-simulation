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
        public Setting(networkconfig n)
        {
            InitializeComponent();

            tb_NetworkAddress.Text = n.networkaddress.ToString();
            tb_SubnetMask.Text = n.subnetmask.ToString();
            tb_DNS.Text = n.dns.ToString();
            tb_DefaultGateway.Text = n.defaultgateway.ToString();
            tb_DHCPServerIP.Text = n.dhcpserver.ToString();
            tb_IPStart.Text = n.start.ToString();
            tb_IPEnd.Text = n.end.ToString();
            tb_LeaseTime.Text = n.leasetime.ToString();

            staticips = new List<staticip>();

            for (int i = 0; i < n.static_ip.Count; i++)
            {
                staticip s = new staticip();
                s.ip = n.static_ip[i].ip;
                for (int j = 0; j < 6; j++)
                {
                    s.mac[j] = n.static_ip[i].mac[j];
                }
                ListViewItem o = new ListViewItem(displaymac(s.mac));
                o.SubItems.Add(s.ip.ToString());
                lv_StaticIP.Items.Add(o);
            }
        }       

        public bool isset { get; set; }
        public networkconfig network_config { get; set; }

        List<staticip> staticips;

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
            if (!checkvalidnetwork())
            {
                network_config = new networkconfig();
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
                network_config.defaultgateway = IPAddress.Parse(tb_DefaultGateway.Text.Trim());
                network_config.dhcpserver = IPAddress.Parse(tb_DHCPServerIP.Text.Trim());
                network_config.start = IPAddress.Parse(tb_IPStart.Text.Trim());
                network_config.end = IPAddress.Parse(tb_IPEnd.Text.Trim());
                network_config.leasetime = Int32.Parse(tb_LeaseTime.Text.Trim());
                for (int i = 0; i < staticips.Count; i++)
                {
                    network_config.static_ip.Add(staticips[i]);
                }
            } catch (Exception z)
            {
                MessageBox.Show(z.Message);
               return;
            }
            
            if (!checkvalidnetwork())
            {
                network_config = new networkconfig();
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
            return ((convert(ip) & convert(subnet)) == convert(network));
        }

        bool checkvalidIP(IPAddress network, IPAddress subnet, IPAddress ip)
        {
            if (!checksamesubnet(network, subnet, ip)) // check same subnet
            {
                return false;
            }
            if ((convert(ip) == convert(network)) || (convert(ip) == (convert(network) | convert(IPAddress.Parse("255.255.255.255"))))) // check if network address or broadcast address
            {
                return false;
            }
            return true;
        }

        bool checkvalidnetwork()
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
                    for (int j = 7; j >= 0; j--)
                    {
                        if (((b & (1 << j)) >> j) == 1) // lay bit thu j tu trai sang
                        {
                            if (!flag)
                            {
                                MessageBox.Show("Subnetmask not valid");
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
                    MessageBox.Show("Network address not fix with subnetmask");
                    return false;
                }
                // check is private IP and not loopback
                byte[] networkaddress = network_config.networkaddress.GetAddressBytes();
                if (networkaddress[0] == 127) // loopback
                {
                    MessageBox.Show("Network address is loopback");
                    return false;
                }
            }
            // Check default gateway
            if (!checkvalidIP(network_config.networkaddress, network_config.subnetmask, network_config.defaultgateway))
            {
                MessageBox.Show("Default gateway must be a valid IP in network address subnet");
                return false;
            }
            // Check IP start
            if (!checkvalidIP(network_config.networkaddress, network_config.subnetmask, network_config.start))
            {
                MessageBox.Show("IP Start must be a valid IP in network address subnet");
                return false;
            }
            // Check IP end
            if (!checkvalidIP(network_config.networkaddress, network_config.subnetmask, network_config.end))
            {
                MessageBox.Show("IP End must be a valid IP in network address subnet");
                return false;
            }
            if (convert(network_config.end) < convert(network_config.start))
            {
                MessageBox.Show("IP Range not valid");
                return false;
            }
            // Check DHCP Server IP
            if (!checkvalidIP(network_config.networkaddress, network_config.subnetmask, network_config.dhcpserver))
            {
                MessageBox.Show("DHCP Server IP must be a valid IP in network address subnet");
                return false;
            }
            for (int i = 0; i < network_config.static_ip.Count; i++)
            {
                if (!checkvalidIP(network_config.networkaddress, network_config.subnetmask, network_config.static_ip[i].ip))
                {
                    MessageBox.Show("Static IP must be a valid IP in network address subnet");
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
                for (int i = 0; i < staticips.Count; i++)
                {
                    if (g.ToString() == staticips[i].ip.ToString())
                    {
                        MessageBox.Show("Static IP already used");
                        return;
                    }
                    if (d[0] == staticips[i].mac[0])
                    {
                        bool flag = true;
                        for (int j = 0; j < 6; j++)
                        {                            
                            if (d[j] != staticips[i].mac[j])
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
                staticips.Add(s);
                ListViewItem o = new ListViewItem(displaymac(s.mac));
                o.SubItems.Add(s.ip.ToString());
                lv_StaticIP.Items.Add(o);
            }            
        }

        string displaymac(byte[] b)
        {
            string z = "";
            for (int i = 0; i < 6; i++)
            {
                z += b[i].ToString("x2");
                if (i < 6)
                {
                    z += ":";
                }
            }
            return z;
        }

        private void btnClearStaticIP_Click(object sender, EventArgs e)
        {
            lv_StaticIP.Items.Clear();
            staticips.Clear();
        }
    }
}
