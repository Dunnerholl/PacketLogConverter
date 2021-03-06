using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xFB, 175, ePacketDirection.ServerToClient, "Char Stats Update 175")]
	public class StoC_0xFB_CharStatsUpdate_175 : Packet
	{
		#region protected fields

		protected ushort _str;
		protected ushort _dex;
		protected ushort _con;
		protected ushort _qui;
		protected ushort _int;
		protected ushort _pie;
		protected ushort _emp;
		protected ushort _chr;
		protected ushort unk1;
		protected short b_str;
		protected short b_dex;
		protected short b_con;
		protected short b_qui;
		protected short b_int;
		protected short b_pie;
		protected short b_emp;
		protected short b_chr;
		protected short b_unk1;
		protected ushort i_str;
		protected ushort i_dex;
		protected ushort i_con;
		protected ushort i_qui;
		protected ushort i_int;
		protected ushort i_pie;
		protected ushort i_emp;
		protected ushort i_chr;
		protected ushort i_unk1;
		protected byte c_str;
		protected byte c_dex;
		protected byte c_con;
		protected byte c_qui;
		protected byte c_int;
		protected byte c_pie;
		protected byte c_emp;
		protected byte c_chr;
		protected byte c_unk1;
		protected byte r_str;
		protected byte r_dex;
		protected byte r_con;
		protected byte r_qui;
		protected byte r_int;
		protected byte r_pie;
		protected byte r_emp;
		protected byte r_chr;
		protected byte r_unk1;
		protected byte flag;
		protected byte conLost;
		protected ushort maxHealth;
		protected ushort unk2;

		#endregion

		#region public access properties

		public ushort str { get { return _str; } }
		public ushort dex { get { return _dex; } }
		public ushort con { get { return _con; } }
		public ushort qui { get { return _qui; } }
		public ushort @int { get { return _int; } }
		public ushort pie { get { return _pie; } }
		public ushort emp { get { return _emp; } }
		public ushort chr { get { return _chr; } }
		public ushort Unk1 { get { return unk1; } }
		public short B_str { get { return b_str; } }
		public short B_dex { get { return b_dex; } }
		public short B_con { get { return b_con; } }
		public short B_qui { get { return b_qui; } }
		public short B_int { get { return b_int; } }
		public short B_pie { get { return b_pie; } }
		public short B_emp { get { return b_emp; } }
		public short B_chr { get { return b_chr; } }
		public short B_unk1 { get { return b_unk1; } }
		public ushort I_str { get { return i_str; } }
		public ushort I_dex { get { return i_dex; } }
		public ushort I_con { get { return i_con; } }
		public ushort I_qui { get { return i_qui; } }
		public ushort I_int { get { return i_int; } }
		public ushort I_pie { get { return i_pie; } }
		public ushort I_emp { get { return i_emp; } }
		public ushort I_chr { get { return i_chr; } }
		public ushort I_unk1 { get { return i_unk1; } }
		public byte C_str { get { return c_str; } }
		public byte C_dex { get { return c_dex; } }
		public byte C_con { get { return c_con; } }
		public byte C_qui { get { return c_qui; } }
		public byte C_int { get { return c_int; } }
		public byte C_pie { get { return c_pie; } }
		public byte C_emp { get { return c_emp; } }
		public byte C_chr { get { return c_chr; } }
		public byte C_unk1 { get { return c_unk1; } }
		public byte R_str { get { return r_str; } }
		public byte R_dex { get { return r_dex; } }
		public byte R_con { get { return r_con; } }
		public byte R_qui { get { return r_qui; } }
		public byte R_int { get { return r_int; } }
		public byte R_pie { get { return r_pie; } }
		public byte R_emp { get { return r_emp; } }
		public byte R_chr { get { return r_chr; } }
		public byte R_unk1 { get { return r_unk1; } }
		public byte Flag { get { return flag; } }
		public byte ConLost { get { return conLost; } }
		public ushort MaxHealth { get { return maxHealth; } }
		public ushort Unk2 { get { return unk2; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			if (flag==0xFF)	text.Write("\n\t    resist |cru|sla|thr|hea|col|mat|bod|spi|ene");
			else text.Write("\n\t      stat |str|dex|con|qui|int|pie|emp|chr|");

			text.Write("\n\tbase       |{0,-3}|{1,-3}|{2,-3}|{3,-3}|{4,-3}|{5,-3}|{6,-3}|{7,-3}|{8,-3}",
				_str, _dex, _con, _qui, _int, _pie, _emp, _chr, unk1);

			text.Write("\n\tbuf        |{0,-3}|{1,-3}|{2,-3}|{3,-3}|{4,-3}|{5,-3}|{6,-3}|{7,-3}|{8,-3}",
				b_str, b_dex, b_con, b_qui, b_int, b_pie, b_emp, b_chr, b_unk1);

			text.Write("\n\titem bonus |{0,-3}|{1,-3}|{2,-3}|{3,-3}|{4,-3}|{5,-3}|{6,-3}|{7,-3}|{8,-3}",
				i_str, i_dex, i_con, i_qui, i_int, i_pie, i_emp, i_chr, i_unk1);

			text.Write("\n\titem cap   |{0,-3}|{1,-3}|{2,-3}|{3,-3}|{4,-3}|{5,-3}|{6,-3}|{7,-3}|{8,-3}",
				c_str, c_dex, c_con, c_qui, c_int, c_pie, c_emp, c_chr, c_unk1);

			text.Write("\n\tra bonus   |{0,-3}|{1,-3}|{2,-3}|{3,-3}|{4,-3}|{5,-3}|{6,-3}|{7,-3}|{8,-3}",
				r_str, r_dex, r_con, r_qui, r_int, r_pie, r_emp, r_chr, r_unk1);

			if (flag != 0xFF) text.Write("\n\t{4}:{0} conLost:{1,-2} maxHealth:{2,-4} unk2:0x{3:X4}", flag, conLost, maxHealth, unk2, flag == 0 ? "subCode" : "vampBonus");
			else text.Write("\n\tsubCode:{0} unk1:0x{1:X4} unk2:0x{2:X4} unk3:0x{3:X4}",  flag, conLost, maxHealth, unk2);

		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			_str = ReadShort();         // 0x00
			_dex = ReadShort();         // 0x02
			_con = ReadShort();         // 0x04
			_qui = ReadShort();         // 0x06
			_int = ReadShort();         // 0x08
			_pie = ReadShort();         // 0x0A
			_emp = ReadShort();         // 0x0C
			_chr = ReadShort();         // 0x0E
			unk1 = ReadShort();         // 0x10
			b_str = (short)ReadShort(); // 0x12
			b_dex = (short)ReadShort(); // 0x14
			b_con = (short)ReadShort(); // 0x16
			b_qui = (short)ReadShort(); // 0x18
			b_int = (short)ReadShort(); // 0x1A
			b_pie = (short)ReadShort(); // 0x1C
			b_emp = (short)ReadShort(); // 0x1E
			b_chr = (short)ReadShort(); // 0x20
			b_unk1 = (short)ReadShort();// 0x22
			i_str = ReadShort();        // 0x24
			i_dex = ReadShort();        // 0x26
			i_con = ReadShort();        // 0x28
			i_qui = ReadShort();        // 0x2A
			i_int = ReadShort();        // 0x2C
			i_pie = ReadShort();        // 0x2E
			i_emp = ReadShort();        // 0x30
			i_chr = ReadShort();        // 0x32
			i_unk1 = ReadShort();       // 0x34
			c_str = ReadByte();         // 0x36
			c_dex = ReadByte();         // 0x37
			c_con = ReadByte();         // 0x38
			c_qui = ReadByte();         // 0x39
			c_int = ReadByte();         // 0x3A
			c_pie = ReadByte();         // 0x3B
			c_emp = ReadByte();         // 0x3C
			c_chr = ReadByte();         // 0x3D
			c_unk1 = ReadByte();        // 0x3E
			r_str = ReadByte();         // 0x3F
			r_dex = ReadByte();         // 0x40
			r_con = ReadByte();         // 0x41
			r_qui = ReadByte();         // 0x42
			r_int = ReadByte();         // 0x43
			r_pie = ReadByte();         // 0x44
			r_emp = ReadByte();         // 0x45
			r_chr = ReadByte();         // 0x46
			r_unk1 = ReadByte();        // 0x47
			flag = ReadByte();          // 0x48
			conLost = ReadByte();       // 0x49

			maxHealth = ReadShort();    // 0x4A
			unk2 = ReadShort();         // 0x4C
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xFB_CharStatsUpdate_175(int capacity) : base(capacity)
		{
		}
	}
}