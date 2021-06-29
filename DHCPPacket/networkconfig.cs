using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace networkconfignamespace
{
    public class networkconfig
    {
        public IPAddress networkaddress;
        public IPAddress subnetmask;
        public IPAddress dns;
        public IPAddress defaultgateway;
        public IPAddress dhcpserver;
        public IPAddress start;
        public IPAddress end;
        public Int32 leasetime;
        
        public List<staticip> static_ip;

        public networkconfig()
        {
            networkaddress = IPAddress.Parse("192.168.1.0");
            subnetmask = IPAddress.Parse("255.255.255.0");
            dns = IPAddress.Parse("192.168.1.1");
            defaultgateway = IPAddress.Parse("192.168.1.1");
            dhcpserver = IPAddress.Parse("192.168.1.1");
            start = IPAddress.Parse("192.168.1.2");
            end = IPAddress.Parse("192.168.1.254");
            static_ip = new List<staticip>();
            leasetime = 60;
        }

        public byte[] toBytes()
        {
            byte[] d = new byte[32 + 10 * static_ip.Count];
            int index = 0;
            index = add(d, index, networkaddress.GetAddressBytes());
            index = add(d, index, subnetmask.GetAddressBytes());
            index = add(d, index, dns.GetAddressBytes());
            index = add(d, index, defaultgateway.GetAddressBytes());
            index = add(d, index, dhcpserver.GetAddressBytes());
            index = add(d, index, start.GetAddressBytes());
            index = add(d, index, end.GetAddressBytes());
            for (int i = 0; i < static_ip.Count; i++)
            {
                index = add(d, index, static_ip[i].mac);
                index = add(d, index, static_ip[i].ip.GetAddressBytes());
            }
            index = add(d, index, new byte[] { (byte)(leasetime), (byte)(leasetime >> 8), (byte)(leasetime >> 16), (byte)(leasetime >> 24) });
            return d;
        }

        int add(byte[] a, int index, byte[] b)
        {
            for (int i = 0; i < b.Length; i++)
            {
                a[index++] = b[i];
            }
            return index;
        }

        public void fromByte(byte[] data)
        {
            System.IO.MemoryStream stm = new System.IO.MemoryStream(data, 0, data.Length);
            System.IO.BinaryReader rdr = new System.IO.BinaryReader(stm);
            networkaddress =  new IPAddress(rdr.ReadBytes(4));
            subnetmask = new IPAddress(rdr.ReadBytes(4));
            dns = new IPAddress(rdr.ReadBytes(4));
            defaultgateway = new IPAddress(rdr.ReadBytes(4));
            dhcpserver = new IPAddress(rdr.ReadBytes(4));
            start = new IPAddress(rdr.ReadBytes(4));
            end = new IPAddress(rdr.ReadBytes(4));
            static_ip = new List<staticip>();
            for (int i = 7 * 4; i + 10 < data.Length; i += 10)
            {
                staticip d = new staticip();
                d.mac = rdr.ReadBytes(6);
                d.ip = new IPAddress(rdr.ReadBytes(4));
                static_ip.Add(d);
            }
            leasetime = BitConverter.ToInt32(rdr.ReadBytes(4), 0);
        }
    }
    public class staticip
    {
        public byte[] mac;
        public IPAddress ip;
        public staticip()
        {
            mac = new byte[6];
        }
    }
}
