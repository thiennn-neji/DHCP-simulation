using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDHCP
{
    public class DHCPOptionParameterRequestList55 : DHCPOption
    {
        private List<EDHCPOption> _ParameterRequestList = new List<EDHCPOption>();
        public List<EDHCPOption> ParameterRequestList
        {
            get { return _ParameterRequestList; }
        }

        #region IDHCP elements
        public override IDHCPOption FromStream(Stream stream)
        {
            // Length of this option is n octets

            var obj = new DHCPOptionParameterRequestList55();
            int c = stream.ReadByte();
            while (c >= 0)
            {
                obj._ParameterRequestList.Add((EDHCPOption)c);
                c = stream.ReadByte();
            }
            return obj;
        }

        public override void ToStream(Stream stream)
        {
            foreach (var item in _ParameterRequestList)
            {
                stream.WriteByte((byte)item);
            }
        }
        #endregion

        public DHCPOptionParameterRequestList55() : base(EDHCPOption.Parameter_Request_List) { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in _ParameterRequestList)
            {
                sb.AppendFormat("{0},", item.ToString());
            }
            if (_ParameterRequestList.Count > 0) sb.Remove(sb.Length - 1, 1); // Xóa dấu , cuối cùng
            return String.Format("DHCPOption(type={0},value=[{1}])", _optionType, sb.ToString());
        }
    }
}
