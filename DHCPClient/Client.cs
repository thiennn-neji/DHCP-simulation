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
            ip = IPAddress.Parse("0.0.0.0");
            defaultgateway = IPAddress.Parse("0.0.0.0");
            subnetmask = IPAddress.Parse("255.255.255.255");
            CheckForIllegalCrossThreadCalls = false;
            t = new Thread(new ThreadStart(listening));
            t.Start();
            t2 = new Thread(new ThreadStart(display2));
            t2.Start();
            time = 0;
            t1 = new Thread(new ThreadStart(auto_extend));
            t1.Start();
            t.IsBackground = true;
            t1.IsBackground = true;
            t2.IsBackground = true;
            haveip = false;
        }

        IPAddress ip; // Ip hien co cua client
        IPAddress defaultgateway; // default gateway cua client
        IPAddress subnetmask; // subneskmask hien co cua client
        UdpClient udpclient;
        Int64 time;
        Thread t, t1, t2;
        bool haveip;

        void auto_extend() // tu dong gia han ip
        {
            while (true)
            {
                if (haveip)
                {
                    Int64 epoch = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
                    if (time - epoch <= 30000)
                    {
                        // viet ham
                    }
                }
                Thread.Sleep(30000);
            }
        }

        void listening()
        {
            udpclient = new UdpClient(67);
            while (true)
            {
                IPEndPoint IpEnd = new IPEndPoint(IPAddress.Any, 0);
                Byte[] recvBytes = udpclient.Receive(ref IpEnd);
                DHCPPacket d = new DHCPPacket();
                d.BytesToDHCPPacket(recvBytes);
                display1(d);
                solve(d);
            }
        }

        private void btnRenew_Click(object sender, EventArgs e)
        {
            sendrelease();
            SendDiscover();
        }

        void solve(DHCPPacket d)
        {
            // Xu li khi nhan goi DHCP tu server
        }

        void display1(DHCPPacket d)
        {
            // Display dhcp messeage
            rtbMess.Text += d.ToText() + "\r\n";
        }

        void display2()
        {
            Int64 epoch = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
            rtbPara.Text = "Ip address: " + ip.ToString() + "\r\n";
            rtbPara.Text += "Default Gateway: " + defaultgateway.ToString() + "\r\n";
            rtbPara.Text += "Subnet Mask: " + subnetmask.ToString() + "\r\n";
            rtbPara.Text += "Time remaining: " + Math.Max(time - epoch, 0).ToString() + "s\r\n";
            rtbPara.Text += "Refresh after 5s\r\n";
            Thread.Sleep(5000);
        }
        private void btnRelease_Click(object sender, EventArgs e)
        {
            sendrelease();
        }
        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

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

            d.flags[0] = 1;           

            var macAddr =
            (
                from nic in NetworkInterface.GetAllNetworkInterfaces()
                where nic.Name == "Wi-Fi"
                select nic.GetPhysicalAddress().ToString()
            ).FirstOrDefault();

            byte[] MacAddr = StringToByteArray(macAddr);

            for (int i = 0; i < MacAddr.Length; i++)
            {
                d.chaddr[i] = MacAddr[i];
            }

            option f = new option();

            f.add(new byte[] { 99, 139, 83, 99 }); // add dhcp magic option
            f.add(new byte[] { 53, 1, 1 }); // add messeage type dhcp discover
            f.add(new byte[] { 61, 7, 1 }); // add client identify
            f.add(MacAddr); // mac address
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

        void sendrequest(DHCPPacket e)
        {
            // send dhcp request
            DHCPPacket d = new DHCPPacket();
            d.Init();
            d.op = 1;
            d.htype = 1;
            d.hlen = 6;
            d.hops = 0;
            d.xid = e.xid;
            d.flags[0] = 1;
            d.options = new byte[] { 99, 130, 83, 99, 53, 1, 3, 61, 7, 1, 0, 0, 0, 0, 0, 0, 50, 4, 0, 0, 0, 0, 54, 4, 0, 0, 0, 0, 55, 4, 1, 3, 6 };
            //
            send(d);
        }
        void sendrelease()
        {
            // send dhcp release
            haveip = false;

        }
        void send(DHCPPacket d)
        {
            IPAddress ipadd = IPAddress.Parse("255.255.255.255");
            IPEndPoint ipend = new IPEndPoint(ipadd, 68);
            byte[] send = d.DHCPPacketToBytes();
            udpclient.Send(send, send.Length, ipend);
        }

    }
}
