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

namespace DHCPServer
{
    public partial class Server : Form
    {
        public Server()
        {
            InitializeComponent();
        }

        public class item
        {
            public string macaddress;
            public string ip;
            public Int64 time;
        }

        List<item> table;
        UdpClient udpclient;
        Thread t, t1;

        void listening()
        {
            udpclient = new UdpClient(68);
            while (true)
            {
                IPEndPoint IpEnd = new IPEndPoint(IPAddress.Any, 0);
                Byte[] recvBytes = udpclient.Receive(ref IpEnd);
                DHCPPacket d = new DHCPPacket();
                d.BytesToDHCPPacket(recvBytes);
                display(d);
                solve(d);
            }
        }

        void time()
        {
            while (true)
            {
                Int64 epoch = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
                for (int i = 0; i < table.Count(); i++)
                {
                    if (table[i].time < epoch)
                    {
                        table.RemoveAt(i);
                        i--;
                    }
                }
                ShowTime(epoch);
                Thread.Sleep(5000);
            }
        }

        void ShowTime(Int64 ti)
        {
            listView1.Items.Clear();
            for (int i = 0; i < table.Count(); i++)
            {
                ListViewItem lvItem = new ListViewItem(table[i].macaddress);
                lvItem.SubItems.Add(table[i].ip);
                lvItem.SubItems.Add((table[i].time - ti).ToString() + "s");
                listView1.Items.Add(lvItem);
            }
        }

        void solve(DHCPPacket d)
        {
            // Xu li goi tin dhcp
        }

        void sendack(DHCPPacket e, IPAddress z)
        {
            // send dhcp ack
            DHCPPacket d = new DHCPPacket();
            d.Init();
            d.op = 2;
            d.htype = 1;
            d.hlen = 6;
            d.hops = 0;
            for (int i = 0; i < e.xid.Length; i++)
            {
                d.xid[i] = e.xid[i];
            }

            d.yiaddr = z.GetAddressBytes();

            for (int i = 0; i < e.chaddr.Length; i++)
            {
                d.chaddr[i] = e.chaddr[i];
            }

            option f = new option();

            f.add(new byte[] { 99, 139, 83, 99 }); // add dhcp magic option
            f.add(new byte[] { 53, 1, 2 }); // add messeage type dhcp offer
            f.add(new byte[] { 54, 4, 192, 168, 1, 1 }); // add dhcp server identify
            f.add(new byte[] { 51, 4, 0, 0, 0, 120 }); // add ip lease time (120 s)
            f.add(new byte[] { 1, 4, 255, 255, 255, 0 }); // add subnetmask
            f.add(new byte[] { 3, 4, 192, 168, 1, 1 }); // add defualt gateway
            f.add(new byte[] { 6, 4, 192, 168, 1, 1 }); // add dns server
            f.add(new byte[] { 255 }); // add end

            d.options = new byte[f.size];
            for (int i = 0; i < f.size; i++)
            {
                d.options[i] = f.data[i];
            }
            //
            send(d);
        }

        void sendoffer(DHCPPacket e, IPAddress z) // take dhcp discover and allocated ip and send offer packet
        {
            // send dhcp offer
            DHCPPacket d = new DHCPPacket();
            d.Init();
            d.op = 2;
            d.htype = 1;
            d.hlen = 6;
            d.hops = 0;
            for (int i = 0; i < e.xid.Length; i++)
            {
                d.xid[i] = e.xid[i];
            }

            d.yiaddr = z.GetAddressBytes();

            for (int i = 0; i < e.chaddr.Length; i++)
            {
                d.chaddr[i] = e.chaddr[i];
            }

            option f = new option();

            f.add(new byte[] { 99, 139, 83, 99 }); // add dhcp magic option
            f.add(new byte[] { 53, 1, 2 }); // add messeage type dhcp offer
            f.add(new byte[] { 54, 4, 192, 168, 1, 1 }); // add dhcp server identify
            f.add(new byte[] { 51, 4, 0, 0, 0, 120 }); // add ip lease time (120 s)
            f.add(new byte[] { 1, 4, 255, 255, 255, 0 }); // add subnetmask
            f.add(new byte[] { 3, 4, 192, 168, 1, 1 }); // add defualt gateway
            f.add(new byte[] { 6, 4, 192, 168, 1, 1 }); // add dns server
            f.add(new byte[] { 255 }); // add end

            d.options = new byte[f.size];
            for (int i = 0; i < f.size; i++)
            {
                d.options[i] = f.data[i];
            }
            //
            send(d);
        }

        void display(DHCPPacket d)
        {
            // Hien thi goi tin dhcp vua nhan duoc len man hinh
            richTextBox1.Text += d.ToText() + "\r\n";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            table = new List<item>();
            CheckForIllegalCrossThreadCalls = false;
            t1 = new Thread(new ThreadStart(listening));
            t1.Start();
            t = new Thread(new ThreadStart(time));
            t.Start();
            button1.Enabled = false;
            t.IsBackground = true;
            t1.IsBackground = true;
        }

        void send(DHCPPacket d)
        {
            IPAddress ipadd = IPAddress.Parse("255.255.255.255");
            IPEndPoint ipend = new IPEndPoint(ipadd, 67);
            byte[] send = d.DHCPPacketToBytes();
            udpclient.Send(send, send.Length, ipend);
        }

    }
}
