using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using DHCPPacketNamespace;
using System.Threading;
using System.Net.NetworkInformation;

namespace DHCPClient
{
    public partial class Client : Form
    {
        public Client()
        {
            InitializeComponent();
            ip = IPAddress.Parse("0.0.0.0"); // Gan cac thong so mac dinh luc moi khoi dong
            defaultgateway = IPAddress.Parse("0.0.0.0");
            dns = IPAddress.Parse("0.0.0.0");
            subnetmask = IPAddress.Parse("255.255.255.255");
            
            var macAddr =
            (
                from nic in NetworkInterface.GetAllNetworkInterfaces()
                where nic.OperationalStatus == OperationalStatus.Up
                select nic.GetPhysicalAddress().ToString()
            ).FirstOrDefault(); // Lay dia chi mac
            try
            {
                MacAddr = StringToByteArray(macAddr);
            }
            catch (Exception)
            {
                MessageBox.Show("Không tìm thấy card mạng");
                MacAddr = new byte[] { 0, 0, 0, 0, 0, 0 };
            }  // Gan dia chi mac vao mang MacAddr
            
            CheckForIllegalCrossThreadCalls = false;

            udpclient = new UdpClient(6700); // Mo port
            //Thread dung de lang nghe 
            Listening_thread = new Thread(new ThreadStart(Listening));
            Listening_thread.IsBackground = true;
            Listening_thread.Start();

            //Thread dung de hien thi parameter cua client
            Display_Para_thread = new Thread(new ThreadStart(Display_Parameter));
            Display_Para_thread.IsBackground = true;
            Display_Para_thread.Start();

            //Thread dung de tu dong gia han ip khi het thoi gian 
            time = 0;
            AutoExtend_thread = new Thread(new ThreadStart(Auto_Extend));
            AutoExtend_thread.IsBackground = true;
            AutoExtend_thread.Start();          
            
            haveip = false; // Chua co ip

            autoextend = true;

            DHCPServer_IP = new byte[] { 0, 0, 0, 0 }; // Dia chi ip cua server dhcp
        }

        IPAddress ip; // Ip hien co cua client
        IPAddress defaultgateway; // default gateway cua client
        IPAddress subnetmask; // subneskmask hien co cua client
        IPAddress dns; // dns server
        UdpClient udpclient; // cong ip nhan va gui goi tin dhcp
        Int64 time; // thoi gian het han cua ip
        Int64 t1, t2; // thoi gian de gia han ip
        Thread Listening_thread, AutoExtend_thread, Display_Para_thread; // Luong t su dung de lang nghe doi dhcp, t1 su dung de cap nhat thong tin (display2), t2 su dung de kiem tra gia han
        bool haveip; // true neu dang co ip, false neu chua co ip
        bool autoextend;
        byte[] MacAddr; // chua mac address
        byte[] DHCPServer_IP; // chua dia chi ip cua server

        void Auto_Extend() // tu dong gia han ip
        {
            while (true)
            {
                if (autoextend) // Neu bat che do tu dong gia han
                {
                    if (haveip) // Neu dang co ip thi moi kiem tra
                    {
                        Int64 epoch = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds; // Lay thoi gian hien tai
                        if (time - epoch <= 0)
                        {
                            haveip = false;
                            Send_DHCPDiscover();
                        }
                        else if (t2 - epoch <= 0)
                        {
                            Send_DHCPRequest_Renew(ip); // gui goi tin gia han
                        }
                        else if (t1 - epoch <= 0)
                        {
                            Send_DHCPRequest_Renew(ip);
                        }
                    } 
                    else
                    {
                        Send_DHCPDiscover(); // gui doi tin DHCP Discover
                    }
                    Thread.Sleep(1000); // ngu 1s
                }
            }
        }

        void Listening() // Lang nghe goi dhcp
        {            
            while (true)
            {
                IPEndPoint IpEnd = new IPEndPoint(IPAddress.Any, 0);
                Byte[] recvBytes = udpclient.Receive(ref IpEnd);
                DHCPPacket packet = new DHCPPacket();
                packet.BytesToDHCPPacket(recvBytes); // chuyen goi tin tu dang byte sang dang DHCPPacket
                Display_Message(packet); // Hien thi goi len mang hinh
                HandlePacket(packet); // xu li goi vua nhan
            }
        }

        private void btnRenew_Click(object sender, EventArgs e)
        {
            if (haveip) // Neu ma co ip roi thi phai gui release roi moi gui discover
            {
                Send_DHCPRelease();
            }            
            Send_DHCPDiscover(); // gui goi tin dhcp discover
        }

        void HandlePacket(DHCPPacket packet)
        {
            // Xu li khi nhan goi DHCP tu server
            List<byte[]> Option = packet.optionsplit();
            for (int i = 0; i < Option.Count(); i++)
            {
                if (Option[i][0] == 53) // dhcp messeage type
                {
                    if (Option[i][2] == 2) // xac dinh dhcp offer
                    {
                        if (!DHCPServer_IP.SequenceEqual(new byte[] { 0, 0, 0, 0 }))
                        {
                            return; // if we already have choose dhcp server, so not doing anything
                        }                           
                        for (int j = 0; j < Option.Count(); j++)
                        {
                            if (Option[j][0] == 54) // server dhcp minh da chon
                            {
                                for (int z = 0; z < 4; z++)
                                {
                                    DHCPServer_IP[z] = Option[j][z + 2];
                                }
                                break;
                            }
                        }
                        Send_DHCPRequest(packet, DHCPServer_IP); // gui goi tin request gom goi dhcp offer va dhcp server da chon
                    }
                    else if (Option[i][2] == 5) // Xac dinh day la goi dhcp ack
                    {
                        ip = new IPAddress(packet.yiaddr);
                        for (int j = 0; j < Option.Count(); j++)
                        {
                            if (Option[j][0] == 1) 
                            {
                                subnetmask = new IPAddress(new byte[] { Option[j][2], Option[j][3], Option[j][4], Option[j][5] });
                            }
                            if (Option[j][0] == 3)
                            {
                                defaultgateway = new IPAddress(new byte[] { Option[j][2], Option[j][3], Option[j][4], Option[j][5] });
                            }
                            if (Option[j][0] == 6)
                            {
                                dns = new IPAddress(new byte[] { Option[j][2], Option[j][3], Option[j][4], Option[j][5] });
                            }
                            if (Option[j][0] == 51)
                            {
                                Int64 epoch = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
                                Int64 leasetime = BitConverter.ToInt32(new byte[] { Option[j][2], Option[j][3], Option[j][4], Option[j][5] }, 0);
                                time = epoch + leasetime;
                                t1 = epoch + (int)(leasetime * 0.5);
                                t2 = epoch + (int)(leasetime * 0.875);
                            }
                        }
                        haveip = true;
                    }
                    else // Goi tin DHCP Nak
                    {
                        for (int j = 0; j < Option.Count(); j++)
                        {
                            if (Option[j][0] == 54) // server dhcp minh da chon
                            {
                                for (int z = 0; z < 4; z++)
                                {
                                    if (DHCPServer_IP[z] != Option[j][z + 2]) // Neu khong phai server dhcp da chon
                                    {
                                        return;
                                    }                                    
                                }
                                Send_DHCPDiscover(); // Chuyen ve trang thai requesting
                                return;
                            }
                        }
                    }
                    return;
                }
            }
        }

        void Display_Message(DHCPPacket packet)
        {
            // Display dhcp messeage
            rtbMess.Text += packet.ToText() + "\r\n"; // d.ToText() la ham tra ve mot string tu DHCPPacket
        }

        void Display_Parameter() // Ham cap nhat thong tin su dung luon t1
        {
            while (true)
            {
                Int64 epoch = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds; // Lay thong tin thoi gian thuc
                rtbPara.Text = "Mac address: " + ByteArrayToString(MacAddr) + "\r\n";
                rtbPara.Text += "Ip address: " + ip.ToString() + "\r\n";
                rtbPara.Text += "Default Gateway: " + defaultgateway.ToString() + "\r\n";
                rtbPara.Text += "Subnet Mask: " + subnetmask.ToString() + "\r\n";
                rtbPara.Text += "Dns server: " + dns.ToString() + "\r\n";
                rtbPara.Text += "Time remaining: " + Math.Max(time - epoch, 0).ToString() + "s\r\n";
                rtbPara.Text += "Refresh after 5s\r\n";
                Thread.Sleep(1000);
            }
        }
        private void btnRelease_Click(object sender, EventArgs e)
        { // nut release
            if (haveip)
            {
                Send_DHCPRelease();
            }            
        }
        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        } // chuyen doi tu string sang byte

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        } // Chuyen tu byte array sang string

        void Send_DHCPDiscover()
        {
            // send dhcp discover
            DHCPPacket packet = new DHCPPacket();
            packet.Init();
            packet.op = 1;
            packet.htype = 1;
            packet.hlen = 6;
            packet.hops = 0;
            Random _random = new Random();
            packet.xid[0] = (byte)_random.Next(0, 255);
            packet.xid[1] = (byte)_random.Next(0, 255);
            packet.xid[2] = (byte)_random.Next(0, 255);
            packet.xid[3] = (byte)_random.Next(0, 255);

            for (int i = 0; i < MacAddr.Length; i++)
            {
                packet.chaddr[i] = MacAddr[i];
            }

            option op_field = new option();

            op_field.add(new byte[] { 99, 139, 83, 99 }); // add dhcp magic option
            op_field.add(new byte[] { 53, 1, 1 }); // add messeage type dhcp discover
            op_field.add(new byte[] { 55, 3, 1, 3, 6 }); // add parament request list
            op_field.add(new byte[] { 255 }); // add end

            packet.options = new byte[op_field.size];
            for (int i = 0; i < op_field.size; i++)
            {
                packet.options[i] = op_field.data[i];
            }
            //
            SendPacket(packet);
        }

        void Send_DHCPRequest(DHCPPacket packet, byte[] dhcpserver)
        {
            // send dhcp request
            DHCPPacket n_packet = new DHCPPacket();
            n_packet.Init();
            n_packet.op = 1;
            n_packet.htype = 1;
            n_packet.hlen = 6;
            n_packet.hops = 0;
            for (int i = 0; i < packet.xid.Length; i++)
            {
                n_packet.xid[i] = packet.xid[i];
            }
                        
            for (int i = 0; i < MacAddr.Length; i++)
            {
                n_packet.chaddr[i] = MacAddr[i];
            }

            option op_field = new option();

            op_field.add(new byte[] { 99, 139, 83, 99 }); // add dhcp magic option
            op_field.add(new byte[] { 53, 1, 3 }); // add messeage type dhcp request
            op_field.add(new byte[] { 50, 4 }); // add request ip address
            op_field.add(packet.yiaddr); // ip address
            op_field.add(new byte[] { 54, 4 }); // add dhcp server identify
            op_field.add(dhcpserver); // add dhcp server identify            
            op_field.add(new byte[] { 55, 3, 1, 3, 6 }); // add parament request list
            op_field.add(new byte[] { 255 }); // add end

            n_packet.options = new byte[op_field.size];
            for (int i = 0; i < op_field.size; i++)
            {
                n_packet.options[i] = op_field.data[i];
            }
            //
            SendPacket(n_packet);
        }

        void Send_DHCPRequest_Renew(IPAddress ip)
        {
            // send dhcp request
            DHCPPacket packet = new DHCPPacket();
            packet.Init();
            packet.op = 1;
            packet.htype = 1;
            packet.hlen = 6;
            packet.hops = 0;

            Random _random = new Random();
            packet.xid[0] = (byte)_random.Next(0, 255);
            packet.xid[1] = (byte)_random.Next(0, 255);
            packet.xid[2] = (byte)_random.Next(0, 255);
            packet.xid[3] = (byte)_random.Next(0, 255);

            packet.ciaddr = ip.GetAddressBytes();

            for (int i = 0; i < MacAddr.Length; i++)
            {
                packet.chaddr[i] = MacAddr[i];
            }

            option op_field = new option();

            op_field.add(new byte[] { 99, 139, 83, 99 }); // add dhcp magic option
            op_field.add(new byte[] { 53, 1, 3 }); // add messeage type dhcp request                   
            op_field.add(new byte[] { 55, 3, 1, 3, 6 }); // add parament request list
            op_field.add(new byte[] { 255 }); // add end

            packet.options = new byte[op_field.size];
            for (int i = 0; i < op_field.size; i++)
            {
                packet.options[i] = op_field.data[i];
            }
            //
            SendPacket(packet);
        }

        private void btnClearLog_Click(object sender, EventArgs e) // Clear log button
        {
            rtbMess.Text = ""; 
        }

        private void btnExtendIP_Click(object sender, EventArgs e) // Extend button
        {
            if (haveip)
            {
                Send_DHCPRequest_Renew(ip);
            }
            else
            {

            }
        }

        void Send_DHCPRelease()
        {
            // send dhcp release        
            DHCPPacket packet = new DHCPPacket();
            packet.Init();
            packet.op = 1;
            packet.htype = 1;
            packet.hlen = 6;
            packet.hops = 0;
            Random _random = new Random();
            packet.xid[0] = (byte)_random.Next(0, 255);
            packet.xid[1] = (byte)_random.Next(0, 255);
            packet.xid[2] = (byte)_random.Next(0, 255);
            packet.xid[3] = (byte)_random.Next(0, 255);

            packet.ciaddr = ip.GetAddressBytes();

            for (int i = 0; i < MacAddr.Length; i++)
            {
                packet.chaddr[i] = MacAddr[i];
            }
            option op_field = new option();

            op_field.add(new byte[] { 99, 139, 83, 99 }); // add dhcp magic option
            op_field.add(new byte[] { 53, 1, 7 }); // add messeage type dhcp request
            op_field.add(new byte[] { 54, 4 }); // add dhcp server identify
            op_field.add(DHCPServer_IP);
            op_field.add(new byte[] { 255 });
            packet.options = new byte[op_field.size];
            for (int i = 0; i < op_field.size; i++)
            {
                packet.options[i] = op_field.data[i];
            }
            //
            SendPacket(packet);

            haveip = false;
            ip = IPAddress.Parse("0.0.0.0");
            defaultgateway = IPAddress.Parse("0.0.0.0");
            subnetmask = IPAddress.Parse("255.255.255.255");
            dns = IPAddress.Parse("0.0.0.0");
            time = 0;
            DHCPServer_IP = new byte[] { 0, 0, 0, 0 };
        }
        void SendPacket(DHCPPacket packet) // Nhan mot goi DHCP packet chuyen thanh dang byte va gui di
        {
            IPAddress ipadd = IPAddress.Parse("255.255.255.255");
            IPEndPoint ipend = new IPEndPoint(ipadd, 6800);
            byte[] send = packet.DHCPPacketToBytes();
            udpclient.Send(send, send.Length, ipend);
            //MessageBox.Show(ByteArrayToString(send));
        }

    }
}
