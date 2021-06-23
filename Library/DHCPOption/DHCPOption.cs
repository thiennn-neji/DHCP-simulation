using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LibraryDHCP
{
    // Abstract class đại diện cho tất cả DHCPOption
    public abstract class DHCPOption : IDHCPOption
    {

        #region DHCPOpion protected elements

        // Loại option của đối tượng này
        protected EDHCPOption _optionType;

        // Phương thức khởi tạo
        protected DHCPOption(EDHCPOption optionType)
        {
            this._optionType = optionType;
        }

        #endregion

        #region IDHCPOtion elements
        // Dùng để xác định kiểu string, dành cho các DHCPOtion như Message, FileName, ServerName 
        public bool NullTerminatedStrings { get; set; }
        // Get loại option của đối tượng này
        public EDHCPOption OptionType
        {
            get { return _optionType; }
        }

		// Tạo đối tượng DHCPOption từ MemStream
		public abstract IDHCPOption FromStream(Stream stream);

		// Ghi đối tượng DHCPOption vào MemStream, chuẩn bị chuyển sang ByteArray
		public abstract void ToStream(Stream stream);

        #endregion


    }

    public class DHCPOptionFixedLength : DHCPOption
    {
        #region IDHCPOption elements

        public override IDHCPOption FromStream(Stream s) { return this; }
        public override void ToStream(Stream s) { }
        #endregion

        public DHCPOptionFixedLength(EDHCPOption optionType) : base(optionType) { }

        public override string ToString()
        {
            return string.Format("DHCPOption(type={0})", _optionType);
        }
    }
}
