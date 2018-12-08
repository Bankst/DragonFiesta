using DFEngine.Content.GameObjects;

namespace DFEngine.Network.Protocols
{
	public sealed class PROTO_NC_CHAR_CLIENT_BASE_CMD : NetworkMessage
	{
		public PROTO_NC_CHAR_CLIENT_BASE_CMD(Character character) : base(NetworkCommand.NC_CHAR_CLIENT_BASE_CMD)
		{
			Write(character.CharNo);
			Write(character.Name, 20);
			Write(character.Slot);
			Write(character.Level);
			Write(character.EXP);
			Write((ushort) character.Stats.CurrentPwrStones);
			Write((ushort) character.Stats.CurrentGrdStones);
			Write((ushort) character.Stats.CurrentHPStones);
			Write((ushort) character.Stats.CurrentSPStones);
			Write(character.Stats.CurrentHP);
			Write(character.Stats.CurrentSP);
			Write(character.Shape.BaseClass == CharacterClass.CC_CRUSADER ? character.Stats.CurrentLP : 0);
			Write((byte) 1); // unk
			Write(character.Fame);
			Write(character.Cen);
			Write(character.Position.Map.Info.MapName, 12);
			Write((int) character.Position.X);
			Write((int) character.Position.Y);
			Write(character.Position.DByte);
			Write(character.Stats.FreeSTR);
			Write(character.Stats.FreeEND);
			Write(character.Stats.FreeDEX);
			Write(character.Stats.FreeINT);
			Write(character.Stats.FreeSPR);
			Write(character.PKYellowTime);
			Write(character.KillPoints);
			Write(character.PrisonMinutes);
			Write(character.AdminLevel);
			Write(character.Flags);
			Write((byte) 0);
		}
	}
}
