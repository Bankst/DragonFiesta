using DFEngine.Server;
using DFEngine.Utils;

namespace DFEngine.Network
{
	/// <summary>
	/// Class that holds information about a server transfer.
	/// </summary>
	public class NetworkTransfer : Object
	{
		/// <summary>
		/// The account of the transferring client.
		/// </summary>
		public Account Account { get; set; }
		/// <summary>
		/// Timestamp representing the time when the transfer was initialized.
		/// </summary>
		public long CreateTime { get; set; }
		/// <summary>
		/// The connection that is being transferred.
		/// </summary>
		public NetworkConnection Connection { get; set; }
		/// <summary>
		/// The connection's unique ID.
		/// </summary>
		public string Guid { get; set; }
		/// <summary>
		/// The world that the connection is transferring to.
		/// </summary>
		public World World { get; set; }

		/// <summary>
		/// Creates a new instance of the <see cref="NetworkTransfer"/> class.
		/// </summary>
		/// <param name="account">The transfer's account.</param>
		/// <param name="guid">The transfer's unique ID.</param>
		public NetworkTransfer(Account account, string guid)
		{
			Account = account;
			CreateTime = Time.Milliseconds;
			Guid = guid;
		}

		/// <summary>
		/// Returns the Guid of the transfer.
		/// </summary>
		public override string ToString()
		{
			return Guid;
		}
	}
}
