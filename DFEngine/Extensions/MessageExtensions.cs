using System.Collections.Generic;
using System.Linq;
using DFEngine.Content.GameObjects;
using DFEngine.Content.Items;
using DFEngine.Network;
using DFEngine.Server;
using DFEngine.Utils;

namespace DFEngine
{
	/// <summary>
	/// Class that extends the <see cref="NetworkMessage"/> class,
	/// allowing us to declare additional methods to help us.
	/// </summary>
	public static class MessageExtensions
	{
		/// <summary>
		/// Writes the world's information to the message.
		/// </summary>
		public static void Write(this NetworkMessage message, World world)
		{
			message.Write(world.Number);
			message.Write(world.Name, 16);
			message.Write(world.Status);
		}

		/// <summary>
		/// Writes a list of worlds to the message.
		/// </summary>
		public static void Write(this NetworkMessage message, List<World> worlds)
		{
			foreach (var world in worlds.OrderBy(world => world.Number))
			{
				message.Write(world);
			}
		}

		/// <summary>
		/// Writes the character shape to the message.
		/// </summary>
		public static void Write(this NetworkMessage message, CharacterShape shape)
		{
			message.Write((byte)(shape.Race | (byte)shape.Class << 2 | (byte)shape.Gender << 7));
			message.Write(shape.Hair);
			message.Write(shape.HairColor);
			message.Write(shape.Face);
		}

		/// <summary>
		/// Writes an avatar's information to the message.
		/// </summary>
		public static void Write(this NetworkMessage message, Avatar avatar)
		{
			message.Write(avatar.CharNo);
			message.Write(avatar.Name, 20);
			message.Write((ushort)avatar.Level);
			message.Write(avatar.Slot);
			message.Write(avatar.MapIndx, 12);
			message.Write((byte)avatar.DeleteTime.Year);
			message.Write((byte)avatar.DeleteTime.Month);
			message.Write((byte)avatar.DeleteTime.Day);
			message.Write((byte)avatar.DeleteTime.Hour);
			message.Write((byte)avatar.DeleteTime.Minute);
			message.Write(avatar.Shape);
			message.Write(avatar.Equipment);
			message.Write(avatar.KQHandle);
			message.Write(avatar.KQMapIndx, 12);
			message.Write(avatar.KQPosition);
			message.Write(avatar.KQDate.ToInt32());
			message.Fill(6, 0); // name change data, not used.
			message.Write((int)avatar.TutorialState);
			message.Write(avatar.TutorialStep);
		}

		/// <summary>
		/// Writes a list of avatars to the message.
		/// </summary>
		public static void Write(this NetworkMessage message, List<Avatar> avatars)
		{
			avatars.For(message.Write);
		}

		/// <summary>
		/// Writes a character's equipment to a message.
		/// </summary>
		public static void Write(this NetworkMessage message, Equipment equipment)
		{
			message.Write(equipment[ItemEquip.ITEMEQUIP_HAT]?.Info.ID ?? ushort.MaxValue);
			message.Write(equipment[ItemEquip.ITEMEQUIP_MOUTH]?.Info.ID ?? ushort.MaxValue);
			message.Write(equipment[ItemEquip.ITEMEQUIP_RIGHTHAND]?.Info.ID ?? ushort.MaxValue);
			message.Write(equipment[ItemEquip.ITEMEQUIP_BODY]?.Info.ID ?? ushort.MaxValue);
			message.Write(equipment[ItemEquip.ITEMEQUIP_LEFTHAND]?.Info.ID ?? ushort.MaxValue);
			message.Write(equipment[ItemEquip.ITEMEQUIP_LEG]?.Info.ID ?? ushort.MaxValue);
			message.Write(equipment[ItemEquip.ITEMEQUIP_SHOES]?.Info.ID ?? ushort.MaxValue);
			message.Write(equipment[ItemEquip.ITEMEQUIP_SHOESACC]?.Info.ID ?? ushort.MaxValue);
			message.Write(equipment[ItemEquip.ITEMEQUIP_LEGACC]?.Info.ID ?? ushort.MaxValue);
			message.Write(equipment[ItemEquip.ITEMEQUIP_BODYACC]?.Info.ID ?? ushort.MaxValue);
			message.Write(equipment[ItemEquip.ITEMEQUIP_HATACC]?.Info.ID ?? ushort.MaxValue);
			message.Write(equipment[ItemEquip.ITEMEQUIP_SHOULDER_B]?.Info.ID ?? ushort.MaxValue);
			message.Write(equipment[ItemEquip.ITEMEQUIP_EYE]?.Info.ID ?? ushort.MaxValue);
			message.Write(equipment[ItemEquip.ITEMEQUIP_LEFTHANDACC]?.Info.ID ?? ushort.MaxValue);
			message.Write(equipment[ItemEquip.ITEMEQUIP_RIGHTHANDACC]?.Info.ID ?? ushort.MaxValue);
			message.Write(equipment[ItemEquip.ITEMEQUIP_BACK]?.Info.ID ?? ushort.MaxValue);
			message.Write(equipment[ItemEquip.ITEMEQUIP_BELT]?.Info.ID ?? ushort.MaxValue);
			message.Write(equipment[ItemEquip.ITEMEQUIP_TAIL]?.Info.ID ?? ushort.MaxValue);
			message.Write(equipment[ItemEquip.ITEMEQUIP_MINIMON]?.Info.ID ?? ushort.MaxValue);
			message.Write(equipment[ItemEquip.ITEMEQUIP_SHIELDACC]?.Info.ID ?? ushort.MaxValue);
			message.Write((byte)(((equipment[ItemEquip.ITEMEQUIP_RIGHTHAND]?.Upgrades ?? 0) << 4) | (equipment[ItemEquip.ITEMEQUIP_LEFTHAND]?.Upgrades ?? 0)));
			message.Fill(2, 0);
		}

		/// <summary>
		/// Writes a vector to the message.
		/// </summary>
		public static void Write(this NetworkMessage message, Vector2 vector)
		{
			message.Write((int)(vector?.X ?? 0));
			message.Write((int)(vector?.Y ?? 0));
		}
	}
}
