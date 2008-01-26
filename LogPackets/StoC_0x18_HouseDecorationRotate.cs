using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x18, -1, ePacketDirection.ServerToClient, "House decoration rotate")]
	public class StoC_0x18_HouseDecorationRotate: Packet, IHouseIdPacket
	{
		protected ushort houseOid;
		protected byte index;
		protected byte place;

		#region public access properties

		public ushort HouseId { get { return houseOid; } }
		public byte Index { get { return index; } }
		public byte Place { get { return place; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("houseOid:0x{0:X4} index:{1} place:{2}({3})", houseOid, index, place, (StoC_0x09_HouseDecorationUpdate.ePlaceType)place);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			houseOid = ReadShort();
			index = ReadByte();
			place = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x18_HouseDecorationRotate(int capacity) : base(capacity)
		{
		}
	}
}