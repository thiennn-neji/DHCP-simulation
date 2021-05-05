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

        void SendDiscover()
        {
            // send dhcp discover
        }

        void sendrequest(IPAddress ip)
        {
            // send dhcp request
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
