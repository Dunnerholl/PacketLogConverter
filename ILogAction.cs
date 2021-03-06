namespace PacketLogConverter
{
	/// <summary>
	/// An action on selected in the log packet.
	/// </summary>
	public interface ILogAction
	{
		/// <summary>
		/// Determines whether the action is enabled.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="selectedPacket">The selected packet.</param>
		/// <returns>
		/// 	<c>true</c> if the action is enabled; otherwise, <c>false</c>.
		/// </returns>
		bool IsEnabled(IExecutionContext context, PacketLocation selectedPacket);

		/// <summary>
		/// Activates a log action.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="selectedPacket">The selected packet.</param>
		/// <returns><c>true</c> if log data tab should be updated.</returns>
		bool Activate(IExecutionContext context, PacketLocation selectedPacket);
	}
}
