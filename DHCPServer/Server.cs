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

        public class item// item hien thi ra man hinh
        {
            public string macaddress { get; set; }
            public string ip { get; set; }
            public Int64 time { get; set; }
        }

        List<item> table; // danh sach bang ip va mac de hien thi
        UdpClient udpclient; // udp mo port de nhan dhcp
        Thread t, t1; // luong t su dung cho viec nhan goi dhcp, t1 su dung cho viec hien thi

        void listening()
        {
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
                for (int i = 0; i < table.Count(); i++) // Duyet bang ip
                {
                    if (table[i].time < epoch) // neu ip nao qua thoi han thi xoa khoi bang
                    {
                        table.RemoveAt(i);
                        i--;
                    }
                }
                ShowTime(epoch);
                Thread.Sleep(5000);
            }
        }

        void ShowTime(Int64 ti) // Hien thi ra man hinh
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

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        void solve(DHCPPacket d)
        {
            // Xu li goi tin dhcp
            List<byte[]> o = d.optionsplit();
            for (int i = 0; i < o.Count(); i++) // dhcp server identify
            {
                if (o[i][0] == 54)
                {
                    if (o[i][2] != 192 || o[i][3] != 168 || o[i][4] != 1 || o[i][5] != 1)
                    {
                        return;
                    }
                    break;
                }
            }
            for (int i = 0; i < o.Count(); i++)
            {
                if (o[i][0] == 53)
                {
                    if (o[i][2] == 1) // xac dinh dhcp discover
                    {
                        int loop = 20;
                        while (loop >= 0)
                        {
                            Random _random = new Random();
                            int j = (byte)_random.Next(2, 254);
                            loop--;
                            IPAddress g = new IPAddress(new byte[] { 192, 168, 1, (byte)j });
                            bool flag = true;
                            for (int z = 0; z < table.Count(); z++)
                            {
                                if (g.ToString() == table[z].ip)
                                {
                                    flag = false;
                                    break;
                                }
                            }
                            if (flag)
                            {
                                sendoffer(d, g);
                                return;
                            }
                        }    
                        for (int j = 2; j <= 254; j++) // Kiem tra tung dia chi ip
                        {
                            IPAddress g = new IPAddress(new byte[] { 192, 168, 1, (byte)j });
                            bool flag = true;
                            for (int z = 0; z < table.Count(); z++)
                            {
                                if (g.ToString() == table[z].ip)
                                {
                                    flag = false;
                                    break;
                                }
                            }
                            if (flag)
                            {
                                sendoffer(d, g);
                                return;
                            }
                        }
                    }
                    else if (o[i][2] == 3) // Xac dinh dhcp request
                    {
                        IPAddress g;
                        g = new IPAddress(d.ciaddr); // request o dang rebound, ip -> ciaddr
                        if (d.ciaddr.SequenceEqual(new byte[] { 0, 0, 0, 0 })) // chua co ip -> dhcp dang o dang new
                        {
                            for (int j = 0; j < o.Count(); j++)
                            {
                                if (o[j][0] == 50) // request IP address
                                {
                                    g = new IPAddress(new byte[] { o[j][2], o[j][3], o[j][4], o[j][5] });
                                    break;
                                }
                            }
                        }
                        byte[] Mc = new byte[d.hlen];
                        for (int j = 0; j < d.hlen; j++)
                        {
                            Mc[j] = d.chaddr[j];
                        }
                        string mc = ByteArrayToString(Mc);                          
                        
                        item q = new item();
                        q.macaddress = mc;
                        q.ip = g.ToString();
                        Int64 epoch = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds; // setup item
                        q.time = epoch + 120;
                        for (int j = 0; j < table.Count(); j++)
                        {
                            if (table[j].ip == q.ip)
                            {
                                if (table[j].macaddress == q.macaddress) // goi tin dang xin gia han
                                {
                                    table.RemoveAt(j); // xoa goi cu
                                } else
                                {
                                    // send NAK T^T coming soon
                                    MessageBox.Show("NAK needed");
                                }
                            }
                        }
                        table.Add(q); // add goi moi vo
                        sendack(d, g); // send goi ack
                    }
                    else // goi dhcp release
                    {
                        IPAddress f = new IPAddress(d.ciaddr);
                        byte[] Mc = new byte[d.hlen];
                        for (int j = 0; j < d.hlen; j++)
                        {
                            Mc[j] = d.chaddr[j];
                        }
                        string mc = ByteArrayToString(Mc);
                        for (int j = 0; j < table.Count(); j++) // Duyet ban va xoa di ip
                        {
                            if (table[j].ip == f.ToString() || table[j].macaddress == mc)
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

        void sendack(DHCPPacket e, IPAddress z) // take dhcp discover and allocated ip and send offer packet
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

            for (int i = 0; i < e.flags.Length; i++)
            {
                d.flags[i] = e.flags[i];
            }

            for (int i = 0; i < e.giaddr.Length; i++)
            {
                d.giaddr[i] = e.giaddr[i];
            }

            for (int i = 0; i < e.chaddr.Length; i++)
            {
                d.chaddr[i] = e.chaddr[i];
            }

            option f = new option();

            f.add(new byte[] { 99, 139, 83, 99 }); // add dhcp magic option
            f.add(new byte[] { 53, 1, 5 }); // add messeage type dhcp ack
            f.add(new byte[] { 54, 4, 192, 168, 1, 1 }); // add dhcp server identify
            f.add(new byte[] { 51, 4, 120, 0, 0, 0 }); // add ip lease time (120 s)
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

            for (int i = 0; i < e.flags.Length; i++)
            {
                d.flags[i] = e.flags[i];
            }

            for (int i = 0; i < e.giaddr.Length; i++)
            {
                d.giaddr[i] = e.giaddr[i];
            }

            for (int i = 0; i < e.chaddr.Length; i++)
            {
                d.chaddr[i] = e.chaddr[i];
            }           

            option f = new option();

            f.add(new byte[] { 99, 139, 83, 99 }); // add dhcp magic option
            f.add(new byte[] { 53, 1, 2 }); // add messeage type dhcp offer
            f.add(new byte[] { 54, 4, 192, 168, 1, 1 }); // add dhcp server identify
            f.add(new byte[] { 51, 4, 120, 0, 0, 0 }); // add ip lease time (120 s)
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

        void display(DHCPPacket d) // hien thi goi tin vua nhan ra man hinh
        {
            // Hien thi goi tin dhcp vua nhan duoc len man hinh
            richTextBox1.Text += d.ToText() + "\r\n";
        }

        private void button1_Click(object sender, EventArgs e) // start dhcp server button
        {
            table = new List<item>();
            udpclient = new UdpClient(6800);
            CheckForIllegalCrossThreadCalls = false;
            t1 = new Thread(new ThreadStart(listening));
            t = new Thread(new ThreadStart(time));
            t.IsBackground = true;
            t1.IsBackground = true;
            t1.Start();            
            t.Start();
            button1.Enabled = false;                       
        }

        private void button2_Click(object sender, EventArgs e) // clear log button
        {
            richTextBox1.Text = "";
        }

        void send(DHCPPacket d) // send
        {
            IPAddress ipadd = IPAddress.Parse("255.255.255.255");
            IPEndPoint ipend = new IPEndPoint(ipadd, 6700);
            byte[] send = d.DHCPPacketToBytes();
            udpclient.Send(send, send.Length, ipend);
        }

    }
}
