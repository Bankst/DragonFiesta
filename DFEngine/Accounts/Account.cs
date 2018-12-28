using System.Collections.Generic;
using DFEngine.Content.GameObjects;

namespace DFEngine.Accounts
{
	/// <summary>
	/// A class that represents players' account data.
	/// </summary>
	public class Account
	{
		/// <summary>
		/// The account's avatars.
		/// </summary>
		public List<Avatar> Avatars { get; set; }

		/// <summary>
		/// The name of the account.
		/// </summary>
		public string Username { get; set; }

		/// <summary>
		/// The ID of the account.
		/// </summary>
		public int UserNo { get; set; }

		/// <summary>
		/// Creates a new instance of the <see cref="Account"/> class.
		/// </summary>
		public Account(int userNo, string username)
		{
			Avatars = new List<Avatar>();
			UserNo = userNo;
			Username = username;
		}
	}
}
