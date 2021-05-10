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

            t = new Thread(new ThreadStart(listening));
            t.IsBackground = true;
            t.Start();
            t2 = new Thread(new ThreadStart(display2));
            t2.IsBackground = true;
            t2.Start();
            time = 0;
            t1 = new Thread(new ThreadStart(auto_extend));
            t1.IsBackground = true;
            t1.Start();            
            haveip = false; // Chua co ip            

            dhcpserver = new byte[] { 0, 0, 0, 0 }; // Dia chi ip cua server dhcp
        }

        IPAddress ip; // Ip hien co cua client
        IPAddress defaultgateway; // default gateway cua client
        IPAddress subnetmask; // subneskmask hien co cua client
        IPAddress dns; // dns server
        UdpClient udpclient; // cong ip nhan va gui goi tin dhcp
        Int64 time; // thoi gian het han cua ip
        Thread t, t1, t2; // Luong t su dung de lang nghe doi dhcp, t1 su dung de cap nhat thong tin (display2), t2 su dung de kiem tra gia han
        bool haveip; // true neu dang co ip, false neu chua co ip
        byte[] MacAddr; // chua mac address
        byte[] dhcpserver; // chua dia chi ip chua server

        void auto_extend() // tu dong gia han ip
        {
            while (true)
            {
                if (haveip) // Neu dang co ip thi moi kiem tra
                {
                    Int64 epoch = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds; // Lay thoi gian hien tai
                    if (time - epoch <= 60) // neu thoi gian het han - thoi gian hien tai <= 30s
                    {
                        // viet ham
                        sendrequest_Renew(); // gui goi tin gia han
                    }
                }
                Thread.Sleep(15000); // ngu 15s
            }
        }

        void listening() // Lang nghe goi dhcp
        {            
            while (true)
            {
                IPEndPoint IpEnd = new IPEndPoint(IPAddress.Any, 0);
                Byte[] recvBytes = udpclient.Receive(ref IpEnd);
                DHCPPacket d = new DHCPPacket();
                d.BytesToDHCPPacket(recvBytes); // chuyen goi tin tu dang byte sang dang DHCPPacket
                display1(d); // Hien thi goi len mang hinh
                solve(d); // xu li goi vua nhan
            }
        }

        private void btnRenew_Click(object sender, EventArgs e)
        {
            if (haveip) // Neu ma co ip roi thi phai gui release roi moi gui discover
            {
                sendrelease();
            }            
            SendDiscover(); // gui goi tin dhcp discover
        }

        void solve(DHCPPacket d)
        {
            // Xu li khi nhan goi DHCP tu server
            List<byte[]> o = d.optionsplit();
            for (int i = 0; i < o.Count(); i++)
            {
                if (o[i][0] == 53) // dhcp messeage type
                {
                    if (o[i][2] == 2) // xac dinh dhcp offer
                    {
                        if (!dhcpserver.SequenceEqual(new byte[] { 0, 0, 0, 0 }))
                        {
                            return; // if we already have dhcp server, so not doing anything
                        }                           
                        for (int j = 0; j < o.Count(); j++)
                        {
                            if (o[j][0] == 54) // server dhcp minh da chon
                            {
                                for (int z = 0; z < 4; z++)
                                {
                                    dhcpserver[z] = o[j][z + 2];
                                }
                                break;
                            }
                        }
                        sendrequest(d, dhcpserver); // gui goi tin request gom goi dhcp offer va dhcp server da chon
                    } 
                    else // Xac dinh day la goi dhcp ack (nak coming soon)
                    {
                        ip = new IPAddress(d.yiaddr);
                        for (int j = 0; j < o.Count(); j++)
                        {
                            if (o[j][0] == 1) 
                            {
                                subnetmask = new IPAddress(new byte[] { o[j][2], o[j][3], o[j][4], o[j][5] });
                            }
                            if (o[j][0] == 3)
                            {
                                defaultgateway = new IPAddress(new byte[] { o[j][2], o[j][3], o[j][4], o[j][5] });
                            }
                            if (o[j][0] == 6)
                            {
                                dns = new IPAddress(new byte[] { o[j][2], o[j][3], o[j][4], o[j][5] });
                            }
                            if (o[j][0] == 51)
                            {
                                Int64 epoch = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
                                time = epoch + BitConverter.ToInt32(new byte[] { o[j][2], o[j][3], o[j][4], o[j][5] }, 0);
                            }
                        }
                        haveip = true;
                    }
                    break;
                }
            }
        }

        void display1(DHCPPacket d)
        {
            // Display dhcp messeage
            rtbMess.Text += d.ToText() + "\r\n"; // d.ToText() la ham tra ve mot string tu DHCPPacket
        }

        void display2() // Ham cap nhat thong tin su dung luon t1
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
                Thread.Sleep(5000);
            }
        }
        private void btnRelease_Click(object sender, EventArgs e)
        { // nut release
            if (haveip)
            {
                sendrelease();
            }            
        }
        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        } // chuyen doi tu string sang byte stack over flow

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        } // Chuyen tu byte array sang string stack over flow

        void SendDiscover()
        {
            // send dhcp discover
            DHCPPacket d = new DHCPPacket();
            d.Init();
            d.op = 1;
            d.htype = 1;
            d.hlen = 6;
            d.hops = 0;
            Random _random = new Random();
            d.xid[0] = (byte)_random.Next(0, 255);
            d.xid[1] = (byte)_random.Next(0, 255);
            d.xid[2] = (byte)_random.Next(0, 255);
            d.xid[3] = (byte)_random.Next(0, 255);

            for (int i = 0; i < MacAddr.Length; i++)
            {
                d.chaddr[i] = MacAddr[i];
            }

            option f = new option();

            f.add(new byte[] { 99, 139, 83, 99 }); // add dhcp magic option
            f.add(new byte[] { 53, 1, 1 }); // add messeage type dhcp discover
            f.add(new byte[] { 55, 3, 1, 3, 6 }); // add parament request list
            f.add(new byte[] { 255 }); // add end

            d.options = new byte[f.size];
            for (int i = 0; i < f.size; i++)
            {
                d.options[i] = f.data[i];
            }
            //
            send(d);
        }

        void sendrequest(DHCPPacket e, byte[] dhcpserver)
        {
            // send dhcp request
            DHCPPacket d = new DHCPPacket();
            d.Init();
            d.op = 1;
            d.htype = 1;
            d.hlen = 6;
            d.hops = 0;
            for (int i = 0; i < e.xid.Length; i++)
            {
                d.xid[i] = e.xid[i];
            }
                        
            for (int i = 0; i < MacAddr.Length; i++)
            {
                d.chaddr[i] = MacAddr[i];
            }

            option f = new option();

            f.add(new byte[] { 99, 139, 83, 99 }); // add dhcp magic option
            f.add(new byte[] { 53, 1, 3 }); // add messeage type dhcp request
            f.add(new byte[] { 50, 4 }); // add request ip address
            f.add(e.yiaddr); // ip address
            f.add(new byte[] { 54, 4 }); // add dhcp server identify
            f.add(dhcpserver); // add dhcp server identify            
            f.add(new byte[] { 55, 3, 1, 3, 6 }); // add parament request list
            f.add(new byte[] { 255 }); // add end

            d.options = new byte[f.size];
            for (int i = 0; i < f.size; i++)
            {
                d.options[i] = f.data[i];
            }
            //
            send(d);
        }

        void sendrequest_Renew()
        {
            // send dhcp request
            DHCPPacket d = new DHCPPacket();
            d.Init();
            d.op = 1;
            d.htype = 1;
            d.hlen = 6;
            d.hops = 0;

            Random _random = new Random();
            d.xid[0] = (byte)_random.Next(0, 255);
            d.xid[1] = (byte)_random.Next(0, 255);
            d.xid[2] = (byte)_random.Next(0, 255);
            d.xid[3] = (byte)_random.Next(0, 255);

            d.ciaddr = ip.GetAddressBytes();

            for (int i = 0; i < MacAddr.Length; i++)
            {
                d.chaddr[i] = MacAddr[i];
            }

            option f = new option();

            f.add(new byte[] { 99, 139, 83, 99 }); // add dhcp magic option
            f.add(new byte[] { 53, 1, 3 }); // add messeage type dhcp request                   
            f.add(new byte[] { 55, 3, 1, 3, 6 }); // add parament request list
            f.add(new byte[] { 255 }); // add end

            d.options = new byte[f.size];
            for (int i = 0; i < f.size; i++)
            {
                d.options[i] = f.data[i];
            }
            //
            send(d);
        }

        private void button1_Click(object sender, EventArgs e) // Clear log button
        {
            rtbMess.Text = ""; 
        }

        private void button2_Click(object sender, EventArgs e) // Extend button
        {
            if (haveip)
            {
                sendrequest_Renew();
            }
        }

        void sendrelease()
        {
            // send dhcp release          

            DHCPPacket d = new DHCPPacket();
            d.Init();
            d.op = 1;
            d.htype = 1;
            d.hlen = 6;
            d.hops = 0;
            Random _random = new Random();
            d.xid[0] = (byte)_random.Next(0, 255);
            d.xid[1] = (byte)_random.Next(0, 255);
            d.xid[2] = (byte)_random.Next(0, 255);
            d.xid[3] = (byte)_random.Next(0, 255);

            d.ciaddr = ip.GetAddressBytes();

            for (int i = 0; i < MacAddr.Length; i++)
            {
                d.chaddr[i] = MacAddr[i];
            }
            option f = new option();

            f.add(new byte[] { 99, 139, 83, 99 }); // add dhcp magic option
            f.add(new byte[] { 53, 1, 7 }); // add messeage type dhcp request
            f.add(new byte[] { 54, 4 }); // add dhcp server identify
            f.add(dhcpserver);
            f.add(new byte[] { 255 });
            d.options = new byte[f.size];
            for (int i = 0; i < f.size; i++)
            {
                d.options[i] = f.data[i];
            }
            //
            send(d);

            haveip = false;
            ip = IPAddress.Parse("0.0.0.0");
            defaultgateway = IPAddress.Parse("0.0.0.0");
            subnetmask = IPAddress.Parse("255.255.255.255");
            dns = IPAddress.Parse("0.0.0.0");
            time = 0;
            dhcpserver = new byte[] { 0, 0, 0, 0 };
        }
        void send(DHCPPacket d) // Nhan mot goi DHCP packet chuyen thanh dang byte va gui di
        {
            IPAddress ipadd = IPAddress.Parse("255.255.255.255");
            IPEndPoint ipend = new IPEndPoint(ipadd, 6800);
            byte[] send = d.DHCPPacketToBytes();
            udpclient.Send(send, send.Length, ipend);
            //MessageBox.Show(ByteArrayToString(send));
        }

    }
}
