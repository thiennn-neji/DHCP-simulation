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
            dns = IPAddress.Parse("0.0.0.0");
            subnetmask = IPAddress.Parse("255.255.255.255");
            CheckForIllegalCrossThreadCalls = false;

            udpclient = new UdpClient(6700);

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
            haveip = false;
            var macAddr =
            (
                from nic in NetworkInterface.GetAllNetworkInterfaces()
                where nic.Name == "Wi-Fi"
                select nic.GetPhysicalAddress().ToString()
            ).FirstOrDefault();

            MacAddr = StringToByteArray(macAddr);

            dhcpserver = new byte[] { 0, 0, 0, 0 };
        }

        IPAddress ip; // Ip hien co cua client
        IPAddress defaultgateway; // default gateway cua client
        IPAddress subnetmask; // subneskmask hien co cua client
        IPAddress dns; // dns server
        UdpClient udpclient;
        Int64 time;
        Thread t, t1, t2;
        bool haveip;
        byte[] MacAddr;
        byte[] dhcpserver;

        void auto_extend() // tu dong gia han ip
        {
            while (true)
            {
                if (haveip)
                {
                    Int64 epoch = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
                    if (time - epoch <= 45)
                    {
                        MessageBox.Show((time - epoch).ToString());
                        // viet ham
                        sendrequest_Renew(dhcpserver);
                    }
                }
                Thread.Sleep(5000);
            }
        }

        void listening()
        {            
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
            if (haveip)
            {
                sendrelease();
            }            
            SendDiscover();
        }

        void solve(DHCPPacket d)
        {
            // Xu li khi nhan goi DHCP tu server
            List<byte[]> o = d.optionsplit();
            for (int i = 0; i < o.Count(); i++)
            {
                if (o[i][0] == 53)
                {
                    if (o[i][2] == 2)
                    {
                        if (!dhcpserver.SequenceEqual(new byte[] { 0, 0, 0, 0 }))
                        {
                            return; // if we already have dhcp server, so not doing anything
                        }                           
                        for (int j = 0; j < o.Count(); j++)
                        {
                            if (o[j][0] == 54) // server identify
                            {
                                for (int z = 0; z < 4; z++)
                                {
                                    dhcpserver[z] = o[j][z + 2];
                                }
                                break;
                            }
                        }
                        sendrequest(d, dhcpserver);
                        //MessageBox.Show("DHCPOffer");
                    } 
                    else
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
                                time = epoch + BitConverter.ToInt32(new byte[] { o[j][5], o[j][4], o[j][3], o[j][2] }, 0);
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
            rtbMess.Text += d.ToText() + "\r\n";
        }

        void display2()
        {
            while (true)
            {
                Int64 epoch = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
                rtbPara.Text = "Ip address: " + ip.ToString() + "\r\n";
                rtbPara.Text += "Default Gateway: " + defaultgateway.ToString() + "\r\n";
                rtbPara.Text += "Subnet Mask: " + subnetmask.ToString() + "\r\n";
                rtbPara.Text += "Dns server: " + dns.ToString() + "\r\n";
                rtbPara.Text += "Time remaining: " + Math.Max(time - epoch, 0).ToString() + "s\r\n";
                rtbPara.Text += "Refresh after 5s\r\n";
                Thread.Sleep(5000);
            }
        }
        private void btnRelease_Click(object sender, EventArgs e)
        {
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
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
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

        void sendrequest_Renew(byte[] dhcpserver)
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

        private void button1_Click(object sender, EventArgs e)
        {
            rtbMess.Text = "";
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
            //
            send(d);

            haveip = false;
            ip = IPAddress.Parse("0.0.0.0");
            defaultgateway = IPAddress.Parse("0.0.0.0");
            subnetmask = IPAddress.Parse("255.255.255.255");
            time = 0;
            dhcpserver = new byte[] { 0, 0, 0, 0 };
        }
        void send(DHCPPacket d)
        {
            IPAddress ipadd = IPAddress.Parse("255.255.255.255");
            IPEndPoint ipend = new IPEndPoint(ipadd, 6800);
            byte[] send = d.DHCPPacketToBytes();
            udpclient.Send(send, send.Length, ipend);
            //MessageBox.Show(ByteArrayToString(send));
        }

    }
}
