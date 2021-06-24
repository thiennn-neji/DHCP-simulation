using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DHCPPacketNamespace;
using networkconfignamespace;

namespace DHCPServer
{
    public partial class Server : Form
    {
        public Server()
        {
            InitializeComponent();
        }

        public class Item// item hien thi ra man hinh
        {
            public string macaddress { get; set; }
            public string ip { get; set; }
            public Int64 time { get; set; }
        }       

        List<Item> table; // danh sach bang ip va mac de hien thi
        UdpClient udpclient; // udp mo port de nhan dhcp
        Thread Time_thread, Listen_thread; // luong t su dung cho viec nhan goi dhcp, t1 su dung cho viec hien thi

        networkconfig network_config = new networkconfig();

        void Listening()
        {
            while (true)
            {
                IPEndPoint IpEnd = new IPEndPoint(IPAddress.Any, 0);
                Byte[] recvBytes = udpclient.Receive(ref IpEnd);
                DHCPPacket packet = new DHCPPacket();
                packet.BytesToDHCPPacket(recvBytes);
                DisplayPacket(packet);
                HandlePacket(packet);
            }
        }

        void Time()
        {
            while (true)
            {
                Int64 epoch = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
                for (int i = 0; i < table.Count(); i++) // Duyet bang ip
                {
                    if (table[i].time < epoch) // neu ip nao qua thoi han thi xoa khoi bang
                    {
                        table.RemoveAt(i);
                        i--;
                    }
                }
                ShowTime(epoch);
                Thread.Sleep(1000);
            }
        }

        void ShowTime(Int64 ti) // Hien thi ra man hinh
        {
            lv_IPTable.Items.Clear();
            for (int i = 0; i < table.Count(); i++)
            {
                ListViewItem lvItem = new ListViewItem(table[i].macaddress);
                lvItem.SubItems.Add(table[i].ip);
                lvItem.SubItems.Add((table[i].time - ti).ToString() + "s");
                lv_IPTable.Items.Add(lvItem);
            }
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        void HandlePacket(DHCPPacket packet)
        {
            // Xu li goi tin dhcp
            List<byte[]> Option = packet.optionsplit();
            for (int i = 0; i < Option.Count(); i++) // dhcp server identify khi goi request_new hoac release
            {
                if (Option[i][0] == 54)
                {
                    if (Option[i][2] != 192 || Option[i][3] != 168 || Option[i][4] != 1 || Option[i][5] != 1)
                    {
                        return;
                    }
                    break;
                }
            }
            for (int i = 0; i < Option.Count(); i++)
            {
                if (Option[i][0] == 53)
                {
                    if (Option[i][2] == 1) // xac dinh dhcp discover
                    {
                        byte[] b_MacAddress = new byte[packet.hlen];
                        for (int j = 0; j < packet.hlen; j++)
                        {
                            b_MacAddress[j] = packet.chaddr[j];
                        }
                        string s_MacAddress = ByteArrayToString(b_MacAddress);
                        for (int z = 0; z < table.Count(); z++)
                        {                            
                            if (s_MacAddress == table[z].macaddress)
                            {
                                Send_DHCPOffer(packet, IPAddress.Parse(table[z].ip));
                                return;
                            }
                        }

                        int loop = 20;
                        while (loop >= 0)
                        {
                            Random _random = new Random();
                            int last = (byte)_random.Next(2, 254);
                            loop--;
                            IPAddress IP = new IPAddress(new byte[] { 192, 168, 1, (byte)last });
                            bool flag = true;
                            for (int z = 0; z < table.Count(); z++)
                            {
                                if (IP.ToString() == table[z].ip)
                                {
                                    flag = false;
                                    break;
                                }
                            }
                            if (flag)
                            {
                                Send_DHCPOffer(packet, IP);
                                return;
                            }
                        } 
                        
                        for (int j = 2; j <= 254; j++) // Kiem tra tung dia chi ip
                        {
                            IPAddress IP = new IPAddress(new byte[] { 192, 168, 1, (byte)j });
                            bool flag = true;
                            for (int z = 0; z < table.Count(); z++)
                            {
                                if (IP.ToString() == table[z].ip)
                                {
                                    flag = false;
                                    break;
                                }
                            }
                            if (flag)
                            {
                                Send_DHCPOffer(packet, IP);
                                return;
                            }
                        }
                    }
                    else if (Option[i][2] == 3) // Xac dinh dhcp request
                    {
                        IPAddress IP;
                        IP = new IPAddress(packet.ciaddr); // request o dang rebound, ip -> ciaddr
                        if (packet.ciaddr.SequenceEqual(new byte[] { 0, 0, 0, 0 })) // chua co ip -> dhcp dang o dang new
                        {
                            for (int j = 0; j < Option.Count(); j++)
                            {
                                if (Option[j][0] == 50) // request IP address
                                {
                                    IP = new IPAddress(new byte[] { Option[j][2], Option[j][3], Option[j][4], Option[j][5] });
                                    break;
                                }
                            }
                        }

                        byte[] b_MacAddress = new byte[packet.hlen];
                        for (int j = 0; j < packet.hlen; j++)
                        {
                            b_MacAddress[j] = packet.chaddr[j];
                        }
                        string s_MacAddress = ByteArrayToString(b_MacAddress);                          
                        
                        Item item = new Item();
                        item.macaddress = s_MacAddress;
                        item.ip = IP.ToString();
                        Int64 epoch = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds; // setup item
                        item.time = epoch + network_config.leasetime;

                        for (int j = 0; j < table.Count(); j++)
                        {
                            if (table[j].ip == item.ip)
                            {
                                if (table[j].macaddress == item.macaddress) // goi tin dang xin gia han
                                {
                                    table.RemoveAt(j); // xoa goi cu
                                } else
                                {
                                    Send_DHCPNak(packet);
                                    return;
                                }
                            }
                        }
                        table.Add(item); // add goi moi vo
                        Send_DHCPAck(packet, IP); // send goi ack
                    }
                    else // goi dhcp release
                    {
                        IPAddress IP = new IPAddress(packet.ciaddr);
                        byte[] b_MacAddress = new byte[packet.hlen];
                        for (int j = 0; j < packet.hlen; j++)
                        {
                            b_MacAddress[j] = packet.chaddr[j];
                        }
                        string s_mac_address = ByteArrayToString(b_MacAddress);
                        for (int j = 0; j < table.Count(); j++) // Duyet ban va xoa di ip
                        {
                            if (table[j].ip == IP.ToString() || table[j].macaddress == s_mac_address)
                            {
                                table.RemoveAt(j);
                                j--;
                            }
                        }
                    }                    
                    break;
                }                
            }
        }

        void Send_DHCPOffer(DHCPPacket packet, IPAddress IP) // take dhcp discover and allocated ip and send offer packet
        {
            // send dhcp offer
            DHCPPacket n_packet = new DHCPPacket();
            n_packet.Init();
            n_packet.op = 2;
            n_packet.htype = 1;
            n_packet.hlen = 6;
            n_packet.hops = 0;
            for (int i = 0; i < packet.xid.Length; i++)
            {
                n_packet.xid[i] = packet.xid[i];
            }

            n_packet.yiaddr = IP.GetAddressBytes();

            for (int i = 0; i < packet.flags.Length; i++)
            {
                n_packet.flags[i] = packet.flags[i];
            }

            for (int i = 0; i < packet.giaddr.Length; i++)
            {
                n_packet.giaddr[i] = packet.giaddr[i];
            }

            for (int i = 0; i < packet.chaddr.Length; i++)
            {
                n_packet.chaddr[i] = packet.chaddr[i];
            }           

            option op_field = new option();

            op_field.add(new byte[] { 99, 139, 83, 99 }); // add dhcp magic option
            op_field.add(new byte[] { 53, 1, 2 }); // add messeage type dhcp offer
            op_field.add(new byte[] { 54, 4 }); // add dhcp server identify
            op_field.add(network_config.dhcpserver.GetAddressBytes());
            op_field.add(new byte[] { 51, 4, (byte)(network_config.leasetime), (byte)(network_config.leasetime >> 8), (byte)(network_config.leasetime >> 16), (byte)(network_config.leasetime >> 24) }); // add ip lease time (120 s)
            op_field.add(new byte[] { 1, 4 }); // add subnetmask
            op_field.add(network_config.subnetmask.GetAddressBytes());
            op_field.add(new byte[] { 3, 4 }); // add defualt gateway
            op_field.add(network_config.defaultgateway.GetAddressBytes());
            op_field.add(new byte[] { 6, 4 }); // add dns server
            op_field.add(network_config.dns.GetAddressBytes());
            op_field.add(new byte[] { 255 }); // add end

            n_packet.options = new byte[op_field.size];
            for (int i = 0; i < op_field.size; i++)
            {
                n_packet.options[i] = op_field.data[i];
            }
            //
            SendPacket(n_packet);
        }

        void Send_DHCPAck(DHCPPacket packet, IPAddress IP) // take dhcp discover and allocated ip and send offer packet
        {
            // send dhcp offer
            DHCPPacket n_packet = new DHCPPacket();
            n_packet.Init();
            n_packet.op = 2;
            n_packet.htype = 1;
            n_packet.hlen = 6;
            n_packet.hops = 0;
            for (int i = 0; i < packet.xid.Length; i++)
            {
                n_packet.xid[i] = packet.xid[i];
            }

            n_packet.yiaddr = IP.GetAddressBytes();

            for (int i = 0; i < packet.flags.Length; i++)
            {
                n_packet.flags[i] = packet.flags[i];
            }

            for (int i = 0; i < packet.giaddr.Length; i++)
            {
                n_packet.giaddr[i] = packet.giaddr[i];
            }

            for (int i = 0; i < packet.chaddr.Length; i++)
            {
                n_packet.chaddr[i] = packet.chaddr[i];
            }

            option op_field = new option();

            op_field.add(new byte[] { 99, 139, 83, 99 }); // add dhcp magic option
            op_field.add(new byte[] { 53, 1, 5 }); // add messeage type dhcp ack
            op_field.add(new byte[] { 54, 4 }); // add dhcp server identify
            op_field.add(network_config.dhcpserver.GetAddressBytes());
            op_field.add(new byte[] { 51, 4, (byte)(network_config.leasetime), (byte)(network_config.leasetime >> 8), (byte)(network_config.leasetime >> 16), (byte)(network_config.leasetime >> 24) }); // add ip lease time (120 s)
            op_field.add(new byte[] { 1, 4 }); // add subnetmask
            op_field.add(network_config.subnetmask.GetAddressBytes());
            op_field.add(new byte[] { 3, 4 }); // add defualt gateway
            op_field.add(network_config.defaultgateway.GetAddressBytes());
            op_field.add(new byte[] { 6, 4 }); // add dns server
            op_field.add(network_config.dns.GetAddressBytes());
            op_field.add(new byte[] { 255 }); // add end

            n_packet.options = new byte[op_field.size];
            for (int i = 0; i < op_field.size; i++)
            {
                n_packet.options[i] = op_field.data[i];
            }
            //
            SendPacket(n_packet);
        }

        void Send_DHCPNak(DHCPPacket packet) // take dhcp discover and allocated ip and send offer packet
        {
            // send dhcp NACK
            DHCPPacket n_packet = new DHCPPacket();
            n_packet.Init();
            n_packet.op = 2;
            n_packet.htype = 1;
            n_packet.hlen = 6;
            n_packet.hops = 0;
            for (int i = 0; i < packet.xid.Length; i++)
            {
                n_packet.xid[i] = packet.xid[i];
            }

            for (int i = 0; i < packet.flags.Length; i++)
            {
                n_packet.flags[i] = packet.flags[i];
            }

            for (int i = 0; i < packet.giaddr.Length; i++)
            {
                n_packet.giaddr[i] = packet.giaddr[i];
            }

            for (int i = 0; i < packet.chaddr.Length; i++)
            {
                n_packet.chaddr[i] = packet.chaddr[i];
            }

            option op_field = new option();

            op_field.add(new byte[] { 99, 139, 83, 99 }); // add dhcp magic option
            op_field.add(new byte[] { 53, 1, 6 }); // add messeage type dhcp nack
            op_field.add(new byte[] { 54, 4 }); // add dhcp server identify
            op_field.add(network_config.dhcpserver.GetAddressBytes());
            op_field.add(new byte[] { 255 }); // add end

            n_packet.options = new byte[op_field.size];
            for (int i = 0; i < op_field.size; i++)
            {
                n_packet.options[i] = op_field.data[i];
            }
            //
            SendPacket(n_packet);
        }

        void DisplayPacket(DHCPPacket packet) // hien thi goi tin vua nhan ra man hinh
        {
            // Hien thi goi tin dhcp vua nhan duoc len man hinh
            rtb_DHCPMessage.Text += packet.ToText() + "\r\n";
        }

        private void btnStart_Click(object sender, EventArgs e) // start dhcp server button
        {
            table = new List<Item>();
            udpclient = new UdpClient(6800);
            CheckForIllegalCrossThreadCalls = false;

            Listen_thread = new Thread(new ThreadStart(Listening));
            Time_thread = new Thread(new ThreadStart(Time));
            Listen_thread.IsBackground = true;
            Time_thread.IsBackground = true;            

            //Start thread
            Listen_thread.Start();            
            Time_thread.Start();
            btnStart.Enabled = false;                       
        }

        private void btnClearLog_Click(object sender, EventArgs e) // clear log button
        {
            rtb_DHCPMessage.Text = "";
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            Setting f = new Setting();
            f.ShowDialog();
            if (f.isset)
            {
                network_config = f.network_config;
            }
        }

        void SendPacket(DHCPPacket d) // send
        {
            IPAddress ipadd = IPAddress.Parse("255.255.255.255");
            IPEndPoint ipend = new IPEndPoint(ipadd, 6700);
            byte[] send = d.DHCPPacketToBytes();
            udpclient.Send(send, send.Length, ipend);
        }

    }
}
