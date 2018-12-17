using System;
using System.Collections.Generic;
using System.Text;
using DFEngine.Content.Game;
using DFEngine.Content.GameObjects;
using DFEngine.Content.Other;
using DFEngine.Network;
using ZoneServer.Logic;

namespace ZoneServer.Handlers
{
	public static class ActionHandlers
	{
		internal static void NC_ACT_MOVEWALK_CMD(NetworkMessage message, NetworkConnection client)
		{
			var beginVector = new Vector2(message.ReadInt32(), message.ReadInt32());
			var endVector = new Vector2(message.ReadInt32(), message.ReadInt32());
			MoveLogic.Move(client.Character, beginVector, endVector, MoveState.MM_WALK);
		}

		internal static void NC_ACT_MOVERUN_CMD(NetworkMessage message, NetworkConnection client)
		{
			var beginVector = new Vector2(message.ReadInt32(), message.ReadInt32());
			var endVector = new Vector2(message.ReadInt32(), message.ReadInt32());
			MoveLogic.Move(client.Character, beginVector, endVector, MoveState.MM_RUN);
		}

		internal static void NC_ACT_STOP_REQ(NetworkMessage message, NetworkConnection client)
		{
			var endVector = new Vector2(message.ReadInt32(), message.ReadInt32());
			client.Character.AI?.StopMovement(endVector);
		}

		internal static void NC_ACT_CHAT_REQ(NetworkMessage message, NetworkConnection client)
		{
			var itemLinkDataCount = message.ReadByte();
			var length = message.ReadByte();
			var content = message.ReadString(length);
			var itemContent = itemLinkDataCount > (byte) 0 ? message.ReadBytes(message.RemainingBytes) : new byte[0];

		}

		internal static void NC_ACT_SHOUT_REQ(NetworkMessage message, NetworkConnection client)
		{
			var itemLinkDataCount = message.ReadByte();
			var length = message.ReadByte();
			var content = message.ReadString(length);
			var itemContent = itemLinkDataCount > (byte)0 ? message.ReadBytes(message.RemainingBytes) : new byte[0];

		}

		internal static void NC_ACT_NPCCLICK_CMD(NetworkMessage message, NetworkConnection client)
		{
			var handle = message.ReadUInt16();

		}

		internal static void NC_ACT_NPCMENUOPEN_ACK(NetworkMessage message, NetworkConnection client)
		{
			var num = (int) message.ReadByte();

		}

		internal static void NC_ACT_JUMP_CMD(NetworkMessage message, NetworkConnection client)
		{
			new PROTO_NC_ACT_SOMEEONEJUMP_CMD(client.Character).Broadcast(client.Character, client);
		}
	}
}
