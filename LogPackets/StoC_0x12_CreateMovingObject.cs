using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x12, -1, ePacketDirection.ServerToClient, "Create moving object")]
	public class StoC_0x12_CreateMovingObject : Packet, IObjectIdPacket
	{
		protected ushort objectOid;
		protected ushort unk1;
		protected ushort heading;
		protected ushort z;
		protected uint x;
		protected uint y;
		protected ushort model;
		protected byte level;
		protected byte flags;
		protected ushort emblem;
		protected ushort unk2;
		protected ushort unk3;
		protected ushort unk4;
		protected string name;
		protected byte unk5; // Trailing 0 ?

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { objectOid }; }
		}

		#region public access properties

		public ushort ObjectOid { get { return objectOid; } }
		public ushort Unk1 { get { return unk1; } }
		public ushort Heading { get { return heading; } }
		public ushort Z { get { return z; } }
		public uint X { get { return x; } }
		public uint Y { get { return y; } }
		public ushort Model { get { return model; } }
		public byte Flags { get { return flags; } }
		public byte Level { get { return level; } }
		public ushort Emblem { get { return emblem; } }
		public ushort Unk2 { get { return unk2; } }
		public ushort Unk3 { get { return unk3; } }
		public ushort Unk4 { get { return unk4; } }
		public string Name { get { return name; } }
		public byte Unk5 { get { return unk5; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			byte realm = 0;
			if ((flags & 0x10) == 0x10)
				realm = 1;
			else if ((flags & 0x20) == 0x20)
				realm = 2;
			else if ((flags & 0x40) == 0x40)
				realm = 3;
			text.Write(" oid:0x{0:X4} unk1:0x{1:X4} heading:0x{2:X4} x:{3,-6} y:{4,-6} z:{5,-5} model:0x{6:X4} level:{7,-3} realm:{8} flags:0x{9:X4} emblem:0x{10:X4} unk2-4:0x{11:X4}{12:X4}{13:X4} name:\"{14}\" unk5:{15}",
				objectOid, unk1, heading, x, y, z, model, level >> 1, realm, (((level & 1) << 8) + flags) & 0xFF8F /* (flags except realm bits)*/, emblem, unk2, unk3, unk4, name, unk5/*, (flags >> 10) & 7, (flags >> 4) & 7, flags & 0xE38F*/);
			if (flagsDescription)
			{
				string flag = "";
				if ((flags & 0x01) == 0x01)
					flag += ",Underwater";// not let drop on ground ?
				if ((flags & 0x02) == 0x02)
					flag += ",MovingObject";
				if ((flags & 0x04) == 0x04)
					flag += ",Loot";
				if ((flags & 0x08) == 0x08)
					flag += ",LongRangeVisible";// 4000, 5500, 8000
				if (flag != "")
				text.Write(" ({0})", flag);
				if (emblem != 0)
					text.Write(" logo:{0,-3} pattern:{1} primaryColor:{2,-2} secondaryColor:{3}", ((unk4 & 1) << 7) | (emblem >> 9), (emblem >> 7) & 2, (emblem >> 3) & 0x0F, emblem & 7);
			}
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			objectOid = ReadShort(); // 0x00
			unk1 = ReadShort();      // 0x02
			heading = ReadShort();   // 0x04
			z = ReadShort();         // 0x06
			x = ReadInt();           // 0x08
			y = ReadInt();           // 0x0C
			model = ReadShort();     // 0x10
			level = ReadByte();      // 0x12
			flags = ReadByte();      // 0x13
			emblem = ReadShort();    // 0x14
			unk2 = ReadShort();      // 0x16
			unk3 = ReadShort();      // 0x18
			unk4 = ReadShort();      // 0x1A
			name = ReadPascalString(); // 0x1C
			unk5 = ReadByte();       // ??
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x12_CreateMovingObject(int capacity) : base(capacity)
		{
		}
	}
}