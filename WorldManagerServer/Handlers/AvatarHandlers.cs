using System.Linq;

using DFEngine.Content.GameObjects;
using DFEngine.Network;
using DFEngine.Network.Protocols;

using WorldManagerServer.Services;

namespace WorldManagerServer.Handlers
{
	internal static class AvatarHandlers
	{
		internal static void NC_AVATAR_CREATE_REQ(NetworkMessage message, NetworkConnection connection)
		{
			var slot = message.ReadByte();
			var name = message.ReadString(20);
			var classGenderRace = message.ReadByte();
			var hair = message.ReadByte();
			var hairColor = message.ReadByte();
			var face = message.ReadByte();

			var race = (byte)(classGenderRace & 3);
			var @class = (CharacterClass)(byte)((classGenderRace >> 2) & 0x1F);
			var gender = (Gender)(byte)((classGenderRace >> 7) & 0x01);

			if (CharacterShape.GetBaseClass(@class) == CharacterClass.CC_CRUSADER && !connection.Account.Avatars.Exists(x => x.Level >= WorldData.SingleData["ChrLevel_CanCreateSen"]?.SingleDataValue))
			{
				new PROTO_NC_AVATAR_CREATEFAIL_ACK(0x86).Send(connection);
				return;
			}

			if (connection.Account.Avatars.Count >= 6)
			{
				new PROTO_NC_AVATAR_CREATEFAIL_ACK(0x85).Send(connection);
				return;
			}

			if (AvatarService.IsNameInUse(name))
			{
				new PROTO_NC_AVATAR_CREATEFAIL_ACK(0x84).Send(connection);
				return;
			}

			if (WorldData.HairInfo[hair]?.Grade != 0 || WorldData.HairColorInfo[hairColor]?.Grade != 0 || WorldData.FaceInfo[face]?.Grade != 0)
			{
				new PROTO_NC_AVATAR_CREATEFAIL_ACK(0x82).Send(connection);
				return;
			}

			if (CharacterShape.GetBaseClass(@class) == CharacterClass.CC_NONE)
			{
				new PROTO_NC_AVATAR_CREATEFAIL_ACK(0x83).Send(connection);
				return;
			}

			if (!AvatarService.Create(connection, slot, name, race, @class, gender, hair, hairColor, face, out var newAvatar))
			{
				new PROTO_NC_AVATAR_CREATEFAIL_ACK(0x82).Send(connection);
				return;
			}

			connection.Account.Avatars.Add(newAvatar);

			new PROTO_NC_AVATAR_CREATESUCC_ACK((byte)connection.Account.Avatars.Count, newAvatar).Send(connection);
		}

		internal static void NC_AVATAR_ERASE_REQ(NetworkMessage message, NetworkConnection connection)
		{
			var slot = message.ReadByte();

			var avatar = connection.Account.Avatars.First(x => x.Slot == slot);

			if (avatar == null)
			{
				return;
			}

			if (!AvatarService.Delete(connection, avatar))
			{
				return;
			}

			connection.Account.Avatars.Remove(avatar);

			new PROTO_NC_AVATAR_ERASESUCC_ACK(slot).Send(connection);

			// Leave party, remove friends, remove apprentices & remove master.
		}
	}
}
