using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PacketLogConverter
{
	/// <summary>
	/// Holds all the log data and all information about it
	/// </summary>
	public class PacketLog : IEnumerable<Packet>, IIndexedContainer<Packet>
	{
		private bool m_isDirty = true;

		public bool IsDirty
		{
			get { return m_isDirty; }
			set { m_isDirty = value; }
		}

		private string m_streamName = string.Empty;

		public string StreamName
		{
			get { return m_streamName; }
			set { m_streamName = value; }
		}

		private List<Packet> m_packets = new List<Packet>();

		public Packet this[int index]
		{
			get { return m_packets[index]; }
		}

		public void AddPacket(Packet pak)
		{
			if (pak == null)
				throw new ArgumentNullException("pak");
			m_packets.Add(pak);
		}

		public void AddRange(ICollection<Packet> collection)
		{
			m_packets.AddRange(collection);
		}

		public List<Packet> GetRange(int startIndex, int endIndex)
		{
			List<Packet> ret = m_packets.GetRange(startIndex, endIndex - startIndex);
			return ret;
		}

		public int Count
		{
			get { return m_packets.Count; }
		}

		private int m_unknownPacketsCount;

		public int UnknownPacketsCount
		{
			get { return m_unknownPacketsCount; }
		}

		private float m_version;
		private bool m_reinitRequired;
		private bool m_ignoreVersionChanges;

		/// <summary>
		/// Gets or sets the version.
		/// </summary>
		/// <value>The version.</value>
		public float Version
		{
			get { return m_version; }
			set
			{
				if (IgnoreVersionChanges)
					return;
				if (m_version != value)
					m_reinitRequired = true;

				IsDirty = m_version != value;
				m_version = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether version changes should be ignored.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if version changes ignored; otherwise, <c>false</c>.
		/// </value>
		public bool IgnoreVersionChanges
		{
			get { return m_ignoreVersionChanges; }
			set
			{
				IsDirty = m_ignoreVersionChanges != value;
				m_ignoreVersionChanges = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether [reinit required].
		/// </summary>
		/// <value><c>true</c> if [reinit required]; otherwise, <c>false</c>.</value>
		public bool ReinitRequired
		{
			get { return m_reinitRequired; }
			set { m_reinitRequired = value; }
		}

		private bool m_subversionReinit = false;
		public bool SubversionReinit
		{
			get { return m_subversionReinit; }
			set { m_subversionReinit = value; }
		}

		/// <summary>
		/// Loads the packet parsers based on current log version.
		/// </summary>
		/// <param name="manager">The log manager initializing logs.</param>
		/// <param name="maxRepeat">The max repeat.</param>
		/// <param name="callback">The callback for UI updates.</param>
		public void Init(LogManager manager, int maxRepeat, ProgressCallback callback)
		{
			if (maxRepeat < 0)
			{
				Log.Info("Log info keep changing, giving up version auto detection.");
				return;
			}

			ReinitRequired = false;

			int workTotal = m_packets.Count;
			int workDone = 0;

			for (int i = 0; i < m_packets.Count; i++)
			{
				if (callback != null && (workDone++ & 0xFFF) == 0)
					callback(workDone, workTotal);

				Packet packet = (Packet)m_packets[i];
//				LogWriters.Logger.Say("maxRepeat:{0} IsDirty:{1} ver:{2} reinit:{3} [{4}]:{5}", maxRepeat, IsDirty, Version, ReinitRequired, i, packet.GetType().ToString());
				if (packet.AllowClassChange)
					packet = PacketManager.ChangePacketClass(packet, Version);
				packet.InitLog(this);
				m_packets[i] = packet;

				if (ReinitRequired)
				{
					Init(manager, maxRepeat - 1, callback);
					return; // version changed by the packets, start again...
				}
			}

			m_unknownPacketsCount = 0;
			foreach (Packet packet in m_packets)
			{
//				if (callback != null && (workDone++ & 0xFFF) == 0)
//					callback(workDone, workTotal);

				try
				{
					packet.InitException = null;
					packet.Initialized = false;
					packet.Position = 0;
					packet.Init();
					packet.Initialized = true;
				}
				catch (OutOfMemoryException e)
				{
					packet.InitException = e;
					packet.Initialized = false;
					Log.Info(string.Format("{0}: {1}", e.Message, packet.GetType().ToString()));
				}
				catch (Exception e)
				{
					packet.InitException = e;
					packet.Initialized = false;
				}
				packet.PositionAfterInit = packet.Position;
				if (packet.GetType() == typeof(Packet))
					++m_unknownPacketsCount;
			}
		}

		public int GetPacketIndexByTextIndex(int textIndex)
		{
			int pakIndex = m_packets.Count;
			while(--pakIndex >= 0)
			{
				Packet pak = (Packet)m_packets[pakIndex];
				if (pak.LogTextIndex < 0) continue;
				if (pak.LogTextIndex > textIndex) continue;

				return pakIndex;
			}
			return -1;
		}

		#region IEnumerable Members

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<Packet> GetEnumerator()
		{
			return m_packets.GetEnumerator();
		}

		///<summary>
		///Returns an enumerator that iterates through a collection.
		///</summary>
		///
		///<returns>
		///An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
		///</returns>
		///<filterpriority>2</filterpriority>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return m_packets.GetEnumerator();
		}

		#endregion

	}
}
