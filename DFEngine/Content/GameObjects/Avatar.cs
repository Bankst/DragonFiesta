﻿using System;

using DFEngine.Content.Game;
using DFEngine.Content.Items;
using DFEngine.Content.Tutorial;
using DFEngine.Network;
using DFEngine.Server;

namespace DFEngine.Content.GameObjects
{
	/// <summary>
	/// Class that contains avatar information. This class is not used
	/// in-game, but just stands to display a <see cref="Character"/> instance.
	/// </summary>
	public class Avatar
	{
		public NetworkConnection Connection { get; set; }
		public Zone Zone { get; set; }

		/// <summary>
		/// The character's unique ID.
		/// </summary>
		public int CharNo { get; set; }
		/// <summary>
		/// The time that the character was deleted.
		/// </summary>
		public DateTime DeleteTime { get; set; }
		/// <summary>
		/// Represents the items that the character current has equipped.
		/// </summary>
		public Equipment Equipment { get; set; }
		/// <summary>
		/// Returns true if the character was deleted.
		/// </summary>
		public bool IsDeleted { get; set; }
		/// <summary>
		/// The character's KQ start date.
		/// </summary>
		public DateTime KQDate { get; set; }
		/// <summary>
		/// The handle of the character's current KQ instance.
		/// </summary>
		public int KQHandle { get; set; }
		/// <summary>
		/// The character's current KQ map index name.
		/// </summary>
		public string KQMapIndx { get; set; }
		/// <summary>
		/// The character's position in their KQ instance.
		/// </summary>
		public Vector2 KQPosition { get; set; }
		/// <summary>
		/// The character's level.
		/// </summary>
		public byte Level { get; set; }
		/// <summary>
		/// The index name of the character's map.
		/// </summary>
		public string MapIndx { get; set; }
		/// <summary>
		/// The character's name.
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// The shape of the character.
		/// </summary>
		public CharacterShape Shape { get; set; }
		/// <summary>
		/// The character's slot.
		/// </summary>
		public byte Slot { get; set; }
		/// <summary>
		/// The state of the character's tutorial instance.
		/// </summary>
		public TutorialState TutorialState { get; set; }
		/// <summary>
		/// The step of the character's tutorial instance.
		/// </summary>
		public byte TutorialStep { get; set; }

		public byte[] WindowPosData { get; set; }
		public byte[] ShortcutData { get; set; }
		public byte[] ShortcutSizeData { get; set; }
		public byte[] GameOptionData { get; set; }
		public byte[] KeyMapData { get; set; }

		public bool HasLoggedIn { get; set; }

		/// <summary>
		/// Creates a new instance of the <see cref="Avatar"/> class.
		/// </summary>
		public Avatar()
		{
			Shape = new CharacterShape();
			Equipment = new Equipment();
		}

		public void Login(NetworkConnection connection, Zone zone)
		{
			if (zone == null)
			{
				new PROTO_NC_CHAR_LOGINFAIL_ACK((ushort) CharLoginError.MAP_UNDER_MAINT).Send(connection);
				return;
			}

			Connection = connection;
			Zone = zone;

			new PROTO_NC_CHAR_LOGIN_ACK(Zone).Send(connection);
			new PROTO_NC_CHAR_OPTION_IMPROVE_GET_SHORTCUTDATA_CMD(ShortcutSizeData).Send(connection);
			new PROTO_NC_CHAR_OPTION_IMPROVE_GET_KEYMAP_CMD(KeyMapData).Send(connection);
			new PROTO_NC_CHAR_OPTION_IMPROVE_GET_GAMEOPTION_CMD(GameOptionData).Send(connection);
		}

		public void FinalizeLogin(string mapIndx, Zone zone)
		{
			MapIndx = mapIndx;
			Zone = zone;

			if (!HasLoggedIn)
			{
				HasLoggedIn = true;

				new PROTO_NC_MAP_LINKEND_CLIENT_CMD().Send(Connection);
			}
		}
	}
}
