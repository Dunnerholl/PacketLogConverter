using System;
using System.Collections;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogReaders
{
	/// <summary>
	/// Parse DOL server v1.7 logs
	/// </summary>
	[LogReader("DOL Server v1.7 logs", "*.log", Priority=8000)]
	public class DolServerV17LogReader : ILogReader
	{
		public ICollection ReadLog(Stream stream, ProgressCallback callback)
		{
			ArrayList packets = new ArrayList();

			int index = 0;
			int currentVersion = -1;
			int counter = 0;
			string line = null;
			Packet pak = null;
			PacketLog log = new PacketLog();
			ArrayList badLines = new ArrayList();
			byte[] data;
			
			using(StreamReader s = new StreamReader(stream, Encoding.ASCII))
			{
				while ((line = s.ReadLine()) != null)
				{
					try
					{
						if (callback != null && (counter++ & 0x1FFF) == 0) // update progress every 8k lines
							callback((int)(stream.Position>>10),
								(int)(stream.Length>>10));

						if (line.Length < 1) continue;
						
//						if (line.IndexOf("DOL.GS.PacketHandler.GSUDPPacketOut") >= 0)
						if (line.IndexOf("GSUDPPacketOut") >= 0)
						{
							data = ReadPacketData(s);
							if (data.Length < 5)
								continue;
							pak = new Packet(data.Length - 5);
							pak.Protocol = ePacketProtocol.UDP;
							pak.Code = data[4];
							pak.Direction = ePacketDirection.ServerToClient;
							pak.Write(data, 5, data.Length - 5);
						}
//						else if (line.IndexOf("DOL.GS.PacketHandler.GSTCPPacketOut") >= 0)
						else if (line.IndexOf("GSTCPPacketOut") >= 0)
						{
							data = ReadPacketData(s);
							if (data.Length < 3)
								continue;
							pak = new Packet(data.Length - 3);
							pak.Protocol = ePacketProtocol.TCP;
							pak.Code = data[2];
							pak.Direction = ePacketDirection.ServerToClient;
							pak.Write(data, 3, data.Length - 3);
						}
//						else if (line.IndexOf("DOL.GS.PacketHandler.GSPacketIn") >= 0)
						else if (line.IndexOf("GSPacketIn:") >= 0)
						{
							int code = line.IndexOf(" ID=0x");
							if (code >= 0 && Util.ParseHexFast(line, code+6, 2, out code))
							{
								data = ReadPacketData(s);
								pak = new Packet(data.Length);
								pak.Protocol = ePacketProtocol.TCP; // can't detect protocol
								pak.Direction = ePacketDirection.ClientToServer;
								pak.Code = code;
								pak.Write(data, 0, data.Length);
							}
							else
							{
								badLines.Add("not GSPacketIn?  : " + line);
								continue;
							}
						}
						else if ((index = line.IndexOf("Client crash (Version")) >= 0)
						{
							int len = "Client crash (Version".Length;
							if (!Util.ParseDecFast(line, index + len, 3, out currentVersion))
								currentVersion = -1;
							continue;
						}
						else
						{
							continue;
						}
						
						pak = PacketManager.ChangePacketClass(pak, currentVersion);
						pak.AllowClassChange = false;
						packets.Add(pak);
					}
					catch (Exception e)
					{
//						MessageBox.Show(e.ToString());
						badLines.Add(e.GetType().FullName + ": " + line);
					}
				}
			}
			
			if (badLines.Count > 0)
			{
				StringBuilder str = new StringBuilder("error parsing following lines (" + badLines.Count + "):\n\n");
				int i = 0;
				foreach (string s in badLines)
				{
					str.Append(s).Append('\n');
					if (++i > 15)
					{
						str.Append("...\n").Append(badLines.Count - i).Append(" lines more.\n");
						break;
					}
				}
				Log.Info(str.ToString());
			}
			
			return packets;
		}

		private byte[] ReadPacketData(StreamReader s)
		{
			MemoryStream buf = new MemoryStream();
			string line;
			
			while ((line = s.ReadLine()) != null)
			{
				int dataOffset = 6;
				int byteInLine = 0;
				do
				{
					int i;
					if (!Util.ParseHexFast(line, dataOffset, 2, out i))
					{
						buf.Capacity = (int) buf.Length;
						return buf.GetBuffer();
					}
					buf.WriteByte((byte) i);
					byteInLine++;
					dataOffset += 3;
				} while (byteInLine < 16);
			}
			
			buf.Capacity = (int) buf.Length;
			return buf.GetBuffer();
		}
	}
}