using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x24, 188, ePacketDirection.ServerToClient, "Unknown")]
	public class StoC_0x24_UnknownPacket: Packet
	{
		protected uint id;

		#region public access properties

		public uint Id { get { return id; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("unk:0x{0:X8}", id);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			id = ReadIntLowEndian();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x24_UnknownPacket(int capacity) : base(capacity)
		{
		}
	}
}