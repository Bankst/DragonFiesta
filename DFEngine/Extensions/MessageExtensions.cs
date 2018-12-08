using System.Collections.Generic;
using System.Linq;

using DFEngine.Content.Game;
using DFEngine.Content.GameObjects;
using DFEngine.Content.Items;
using DFEngine.Network;
using DFEngine.Server;

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

		public static void WriteParameters(this NetworkMessage message, Character character)
		{
			message.Write(Character.Global.GetNextEXP(character.Level - 1));
			message.Write(Character.Global.GetNextEXP(character.Level));
			message.Write(character.Stats.BaseSTR);
			message.Write(character.Stats.CurrentSTR);
			message.Write(character.Stats.BaseEND);
			message.Write(character.Stats.CurrentEND);
			message.Write(character.Stats.BaseDEX);
			message.Write(character.Stats.CurrentDEX);
			message.Write(character.Stats.BaseINT);
			message.Write(character.Stats.CurrentINT);
			message.Write(0); // BaseWIZ
			message.Write(0); // BonusWIZ
			message.Write(character.Stats.BaseSPR);
			message.Write(character.Stats.CurrentSPR);
			message.Write(character.Stats.BaseMinDmg);
			message.Write(character.Stats.CurrentMinDmg);
			message.Write(character.Stats.BaseMaxDmg);
			message.Write(character.Stats.CurrentMaxDmg);
			message.Write(character.Stats.BaseDef);
			message.Write(character.Stats.CurrentDef);
			message.Write(character.Stats.BaseAim);
			message.Write(character.Stats.CurrentAim);
			message.Write(character.Stats.BaseEvasion);
			message.Write(character.Stats.CurrentEvasion);
			message.Write(character.Stats.BaseMinMDmg);
			message.Write(character.Stats.CurrentMinMDmg);
			message.Write(character.Stats.BaseMaxMDmg);
			message.Write(character.Stats.CurrentMaxMDmg);
			message.Write(character.Stats.BaseMDef);
			message.Write(character.Stats.CurrentMDef);
			message.Write(0); // BaseMH
			message.Write(0); // BonusMH
			message.Write(0); // BaseMB
			message.Write(0); // BonusMB
			message.Write(character.Stats.CurrentMaxHP);
			message.Write(character.Stats.CurrentMaxSP);
			message.Write(character.Shape.BaseClass == CharacterClass.CC_CRUSADER ? character.Stats.CurrentMaxLP : 0);
			message.Write(0);
			message.Write(character.Stats.CurrentMaxHPStones);
			message.Write(character.Stats.CurrentMaxSPStones);
			message.Fill(16, 0); //PwrStone
			message.Fill(16, 0); //GrdStone
			message.Write(character.Stats.BaseIllnessResistance);
			message.Write(character.Stats.CurrentIllnessResistance);
			message.Write(character.Stats.BaseDiseaseResistance);
			message.Write(character.Stats.CurrentDiseaseResistance);
			message.Write(character.Stats.BaseCurseResistance);
			message.Write(character.Stats.CurrentCurseResistance);
			message.Write(character.Stats.BaseStunResistance);
			message.Write(character.Stats.CurrentStunResistance);
			message.Write((int) character.Position.X);
			message.Write((int) character.Position.Y);
		}
	}
}
