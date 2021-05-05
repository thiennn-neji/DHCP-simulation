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

        void sendack()
        {
            // send dhcp ack
        }

        void sendoffer()
        {
            // send dhcp offer
        }

        void display(DHCPPacket d)
        {
            // Hien thi goi tin dhcp vua nhan duoc len man hinh
        }

        private void button1_Click(object sender, EventArgs e)
        {
            table = new List<item>();
            t1 = new Thread(new ThreadStart(listening));
            t1.Start();
            t = new Thread(new ThreadStart(time));
            t.Start();
            button1.Enabled = false;
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
