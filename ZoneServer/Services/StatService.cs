using System;
using System.Collections.Generic;
using System.Linq;
using DFEngine.Content.GameObjects;
using DFEngine.Content.GameObjects.Characters;
using DFEngine.Content.Items;
using DFEngine.Content.Other;
using DFEngine.Network;

namespace ZoneServer.Services
{
	internal static class StatService
	{
		internal static void Calculate(Character character)
		{
			character.Stats.BonusMinDmg = 0;
			character.Stats.BonusMaxDmg = 0;
			character.Stats.CriticalMinDmg = 0;
			character.Stats.CriticalMaxDmg = 0;
			character.Stats.BonusDef = 0;
			character.Stats.BonusEvasion = 0;
			character.Stats.BonusAim = 0;
			character.Stats.BonusMinMDmg = 0;
			character.Stats.BonusMaxMDmg = 0;
			character.Stats.CriticalMinMDmg = 0;
			character.Stats.CriticalMaxMDmg = 0;
			character.Stats.BonusMDef = 0;
			character.Stats.BonusSTR = 0;
			character.Stats.BonusEND = 0;
			character.Stats.BonusDEX = 0;
			character.Stats.BonusINT = 0;
			character.Stats.BonusSPR = 0;
			character.Stats.BonusMaxHP = 0;
			character.Stats.BonusMaxSP = 0;
			character.Stats.BonusMaxLP = 0;
			character.Stats.BlockRate = decimal.Zero;
			character.Stats.CriticalRate = decimal.Zero;
			character.Stats.BonusIllnessResistance = 0;
			character.Stats.BonusDiseaseResistance = 0;
			character.Stats.BonusCurseResistance = 0;
			character.Stats.BonusStunResistance = 0;
			character.Stats.BonusWalkSpeed = 0;
			character.Stats.BonusRunSpeed = 0;
			character.Stats.CurrentMinHP = 0;
			character.Stats.BonusDmgRate = 0.0;
			character.Stats.BonusMDmgRate = 0.0;
			character.Stats.BonusDefRate = 0.0;
			character.Stats.BonusMDefRate = 0.0;
			character.Stats.BaseMinDmg = character.Parameters.STR + (int)Math.Ceiling(character.Stats.FreeSTR * 1.2);
			character.Stats.BaseMaxDmg = character.Parameters.STR + (int)Math.Ceiling(character.Stats.FreeSTR * 1.2);
			character.Stats.BaseDef = character.Parameters.END + (int)Math.Ceiling(character.Stats.FreeEND * 0.5);
			character.Stats.BaseEvasion = character.Parameters.DEX;
			character.Stats.BaseAim = character.Parameters.DEX;
			character.Stats.BaseMinMDmg = character.Parameters.INT + (int)Math.Ceiling(character.Stats.FreeINT * 1.2);
			character.Stats.BaseMaxMDmg = character.Parameters.INT + (int)Math.Ceiling(character.Stats.FreeINT * 1.2);
			character.Stats.BaseMDef = character.Parameters.SPR + (int)Math.Ceiling(character.Stats.FreeSPR * 0.5);
			character.Stats.BonusMinDmg += character.Stats.BonusSTR;
			character.Stats.BonusMaxDmg += character.Stats.BonusSTR;
			character.Stats.BonusDef += character.Stats.BonusEND;
			character.Stats.BonusEvasion += character.Stats.BonusDEX;
			character.Stats.BonusAim += character.Stats.BonusDEX;
			character.Stats.BonusMinMDmg += character.Stats.BonusINT;
			character.Stats.BonusMaxMDmg += character.Stats.BonusINT;
			character.Stats.BonusMDef += character.Stats.BonusSPR;
			if (character.Stats.FreeEND <= 50)
			{
				var num = (decimal)(character.Stats.FreeEND * 0.001);
				character.Stats.BlockRate += num * character.Stats.BlockRate;
			}
			else if (character.Stats.FreeEND > 50)
			{
				var num = (decimal)(0.05 + (character.Stats.FreeEND - 50) * 0.0005);
				character.Stats.BlockRate += num * character.Stats.BlockRate;
			}
			if (character.Stats.FreeDEX <= 50)
			{
				var num = character.Stats.FreeDEX * 0.002;
				character.Stats.BaseEvasion += (int)Math.Ceiling(num * character.Stats.BaseEvasion);
			}
			else if (character.Stats.FreeDEX > 50)
			{
				var num = 0.1 + (character.Stats.FreeDEX - 50) * 0.001;
				character.Stats.BaseEvasion += (int)Math.Ceiling(num * character.Stats.BaseEvasion);
			}
			if (character.Stats.FreeDEX <= 33)
			{
				var num = character.Stats.FreeDEX * 0.003;
				character.Stats.BaseAim += (int)Math.Ceiling(num * character.Stats.BaseAim);
			}
			else if (character.Stats.FreeDEX <= 67)
			{
				var num = 0.099 + (character.Stats.FreeDEX - 33) * 0.002;
				character.Stats.BaseAim += (int)Math.Ceiling(num * character.Stats.BaseAim);
			}
			else if (character.Stats.FreeDEX > 67)
			{
				var num = 0.233 + (character.Stats.FreeDEX - 67) * 0.001;
				character.Stats.BaseAim += (int)Math.Ceiling(num * character.Stats.BaseAim);
			}
			if (character.Stats.FreeSPR <= 25)
			{
				var num = (decimal)(character.Stats.FreeSPR * 0.002);
				character.Stats.CriticalRate += num * character.Stats.CriticalRate;
			}
			else if (character.Stats.FreeSPR <= 61)
			{
				var num = (decimal)(0.05 + (character.Stats.FreeSPR - 25) * 0.001);
				character.Stats.CriticalRate += num * character.Stats.CriticalRate;
			}
			else if (character.Stats.FreeSPR > 61)
			{
				var num = (decimal)(0.111 + (character.Stats.FreeSPR - 61) * 0.0005);
				character.Stats.CriticalRate += num * character.Stats.CriticalRate;
			}
			// TODO: Abstates!
			/*
			for (int upperBound = character.AbStates.GetUpperBound(); upperBound >= 0; --upperBound)
				AbStateService.Calculate(character.AbStates[upperBound]);
			for (int upperBound = character.ChargedBuffs.GetUpperBound(); upperBound >= 0; --upperBound)
				AbStateService.Calculate(character.ChargedBuffs[upperBound]);
			*/
		}
		
		// TODO: UpgradeGroup, MobInstance, ActiveSkillInstance
		/*
		internal static void Calculate(ItemInstance Item)
		{
			Item.Stats.BonusMinDmg = 0;
			Item.Stats.BonusMaxDmg = 0;
			Item.Stats.BonusDef = 0;
			Item.Stats.BonusMinMDmg = 0;
			Item.Stats.BonusMaxMDmg = 0;
			Item.Stats.BonusMDef = 0;
			if (Item.Upgrades > 0)
			{
				short num = Item.Item.UpgradeGroup.Values[Item.Upgrades];
				switch (Item.Item.UpgradeGroup.UpFactor)
				{
					case UpFactor.DEF:
						Item.Stats.BonusDef += num;
						break;
					case UpFactor.DMG:
						Item.Stats.BonusMinDmg += num;
						Item.Stats.BonusMaxDmg += num;
						break;
					case UpFactor.MDMG:
						Item.Stats.BonusMinMDmg += num;
						Item.Stats.BonusMaxMDmg += num;
						break;
					case UpFactor.MDEF:
						Item.Stats.BonusMDef += num;
						break;
					case UpFactor.ALLDMG:
						Item.Stats.BonusMinDmg += num;
						Item.Stats.BonusMaxDmg += num;
						Item.Stats.BonusMinMDmg += num;
						Item.Stats.BonusMaxMDmg += num;
						break;
				}
			}
		}

		internal static void Calculate(MobInstance Mob)
		{
			Mob.Stats.BonusMinDmg = 0;
			Mob.Stats.BonusMaxDmg = 0;
			Mob.Stats.BonusDef = 0;
			Mob.Stats.BonusEvasion = 0;
			Mob.Stats.BonusAim = 0;
			Mob.Stats.BonusMinMDmg = 0;
			Mob.Stats.BonusMaxMDmg = 0;
			Mob.Stats.BonusMDef = 0;
			Mob.Stats.BonusMaxHP = 0;
			Mob.Stats.BonusMaxSP = 0;
			Mob.Stats.BonusSTR = 0;
			Mob.Stats.BonusEND = 0;
			Mob.Stats.BonusDEX = 0;
			Mob.Stats.BonusINT = 0;
			Mob.Stats.BonusSPR = 0;
			Mob.Stats.CriticalRate = decimal.Zero;
			Mob.Stats.BonusWalkSpeed = 0;
			Mob.Stats.BonusRunSpeed = 0;
			Mob.Stats.CurrentMaxHP = Mob.Mob.Stats.BaseMaxHP + Mob.Stats.BonusMaxHP;
			Mob.Stats.CurrentMaxSP = Mob.Mob.Stats.BaseMaxSP + Mob.Stats.BonusMaxSP;
			Mob.Stats.CurrentWalkSpeed = Mob.Mob.Stats.BaseWalkSpeed + Mob.Stats.BonusWalkSpeed;
			Mob.Stats.CurrentRunSpeed = Mob.Mob.Stats.BaseRunSpeed + Mob.Stats.BonusRunSpeed;
			Mob.Stats.BaseMinDmg = Mob.Mob.Stats.BaseSTR;
			Mob.Stats.BaseMaxDmg = Mob.Mob.Stats.BaseSTR;
			Mob.Stats.BaseDef = Mob.Mob.Stats.BaseEND;
			Mob.Stats.BaseEvasion = Mob.Mob.Stats.BaseDEX;
			Mob.Stats.BaseAim = Mob.Mob.Stats.BaseDEX;
			Mob.Stats.BaseMinMDmg = Mob.Mob.Stats.BaseINT;
			Mob.Stats.BaseMaxMDmg = Mob.Mob.Stats.BaseINT;
			Mob.Stats.BaseMDef = Mob.Mob.Stats.BaseSPR;
			Mob.Stats.BonusMinDmg += Mob.Stats.BonusSTR;
			Mob.Stats.BonusMaxDmg += Mob.Stats.BonusSTR;
			Mob.Stats.BonusDef += Mob.Stats.BonusEND;
			Mob.Stats.BonusEvasion += Mob.Stats.BonusDEX;
			Mob.Stats.BonusAim += Mob.Stats.BonusDEX;
			Mob.Stats.BonusMinMDmg += Mob.Stats.BonusINT;
			Mob.Stats.BonusMaxMDmg += Mob.Stats.BonusINT;
			Mob.Stats.BonusMDef += Mob.Stats.BonusSPR;
			Mob.Stats.CurrentMinDmg = Mob.Stats.BaseMinDmg + Mob.Stats.BonusMinDmg;
			Mob.Stats.CurrentMaxDmg = Mob.Stats.BaseMaxDmg + Mob.Stats.BonusMaxDmg;
			Mob.Stats.CurrentDef = Mob.Stats.BaseDef + Mob.Stats.BonusDef;
			Mob.Stats.CurrentEvasion = Mob.Stats.BaseEvasion + Mob.Stats.BonusEvasion;
			Mob.Stats.CurrentAim = Mob.Stats.BaseAim + Mob.Stats.BonusAim;
			Mob.Stats.CurrentMinMDmg = Mob.Stats.BaseMinMDmg + Mob.Stats.BonusMinMDmg;
			Mob.Stats.CurrentMaxMDmg = Mob.Stats.BaseMaxMDmg + Mob.Stats.BonusMaxMDmg;
			Mob.Stats.CurrentMDef = Mob.Stats.BaseMDef + Mob.Stats.BonusMDef;
			for (int upperBound = Mob.AbStates.GetUpperBound(); upperBound >= 0; --upperBound)
				AbStateService.Calculate(Mob.AbStates[upperBound]);
		}

		internal static void Calculate(ActiveSkillInstance Skill)
		{
			Skill.Stats.BonusMinDmg = 0;
			Skill.Stats.BonusMaxDmg = 0;
			Skill.Stats.BonusDef = 0;
			Skill.Stats.BonusEvasion = 0;
			Skill.Stats.BonusAim = 0;
			Skill.Stats.BonusMinMDmg = 0;
			Skill.Stats.BonusMaxMDmg = 0;
			Skill.Stats.BonusMDef = 0;
			if (Skill.ActiveSkill == null)
			{
				Skill.Stats.CurrentMinDmg = Skill.Stats.BaseMinDmg + Skill.Stats.BonusMinDmg;
				Skill.Stats.CurrentMaxDmg = Skill.Stats.BaseMaxDmg + Skill.Stats.BonusMaxDmg;
				Skill.Stats.CurrentDef = Skill.Stats.BaseDef + Skill.Stats.BonusDef;
				Skill.Stats.CurrentEvasion = Skill.Stats.BaseEvasion + Skill.Stats.BonusEvasion;
				Skill.Stats.CurrentAim = Skill.Stats.BaseAim + Skill.Stats.BonusAim;
				Skill.Stats.CurrentMinMDmg = Skill.Stats.BaseMinMDmg + Skill.Stats.BonusMinMDmg;
				Skill.Stats.CurrentMaxMDmg = Skill.Stats.BaseMaxMDmg + Skill.Stats.BonusMaxMDmg;
				Skill.Stats.CurrentMDef = Skill.Stats.BaseMDef + Skill.Stats.BonusMDef;
			}
			else
			{
				Skill.Stats.CurrentMinDmg = Skill.ActiveSkill.Stats.BaseMinDmg + Skill.Stats.BonusMinDmg;
				Skill.Stats.CurrentMaxDmg = Skill.ActiveSkill.Stats.BaseMaxDmg + Skill.Stats.BonusMaxDmg;
				Skill.Stats.CurrentDef = Skill.ActiveSkill.Stats.BaseDef + Skill.Stats.BonusDef;
				Skill.Stats.CurrentEvasion = Skill.ActiveSkill.Stats.BaseEvasion + Skill.Stats.BonusEvasion;
				Skill.Stats.CurrentAim = Skill.ActiveSkill.Stats.BaseAim + Skill.Stats.BonusAim;
				Skill.Stats.CurrentMinMDmg = Skill.ActiveSkill.Stats.BaseMinMDmg + Skill.Stats.BonusMinMDmg;
				Skill.Stats.CurrentMaxMDmg = Skill.ActiveSkill.Stats.BaseMaxMDmg + Skill.Stats.BonusMaxMDmg;
				Skill.Stats.CurrentMDef = Skill.ActiveSkill.Stats.BaseMDef + Skill.Stats.BonusMDef;
			}
		}
		*/

		internal static void SendParameterUpdate(Character character, ItemInstance changedItem = null, byte stat = 99, params StatType[] stats)
		{
			var changes = new List<ParameterChange>
			{
			new ParameterChange(StatType.PLUSDMG, GetStat(character, StatType.PLUSDMG)),
			new ParameterChange(StatType.PLUSDEF, GetStat(character, StatType.PLUSDEF)),
			new ParameterChange(StatType.PLUSMDMG, GetStat(character, StatType.PLUSMDMG)),
			new ParameterChange(StatType.PLUSMDEF, GetStat(character, StatType.PLUSMDEF)),
			new ParameterChange(StatType.PLUSAIM, GetStat(character, StatType.PLUSAIM)),
			new ParameterChange(StatType.PLUSEVASION, GetStat(character, StatType.PLUSEVASION)),
			new ParameterChange(StatType.CRITRATE, GetStat(character, StatType.CRITRATE)),
			new ParameterChange(StatType.BLOCKRATE, GetStat(character, StatType.BLOCKRATE)),
			new ParameterChange(StatType.PLUSMAXHP, GetStat(character, StatType.PLUSMAXHP)),
			new ParameterChange(StatType.PLUSMAXSP, GetStat(character, StatType.PLUSMAXSP))
  };
			if (stats != null)
			{
				changes.AddRange(stats.Select(s => new ParameterChange(s, GetStat(character, s))));
			}
			switch (stat)
			{
				case 0:
					changes.Add(new ParameterChange(StatType.MINDMG, character.Stats.CurrentMinDmg));
					changes.Add(new ParameterChange(StatType.MAXDMG, character.Stats.CurrentMaxDmg));
					break;
				case 1:
					changes.Add(new ParameterChange(StatType.DEF, character.Stats.CurrentDef));
					changes.Add(new ParameterChange(StatType.MAXHP, character.Stats.CurrentMaxHP));
					break;
				case 2:
					changes.Add(new ParameterChange(StatType.EVASION, character.Stats.CurrentEvasion));
					changes.Add(new ParameterChange(StatType.AIM, character.Stats.CurrentAim));
					break;
				case 3:
					changes.Add(new ParameterChange(StatType.MINMDMG, character.Stats.CurrentMinMDmg));
					changes.Add(new ParameterChange(StatType.MAXMDMG, character.Stats.CurrentMaxMDmg));
					break;
				case 4:
					changes.Add(new ParameterChange(StatType.MDEF, character.Stats.CurrentMDef));
					changes.Add(new ParameterChange(StatType.MAXSP, character.Stats.CurrentMaxSP));
					break;
			}
			if (changedItem != null)
			{
				if (changedItem.Stats.BonusSTR > 0 || changedItem.Item.Stats.BaseSTR > 0)
					changes.Add(new ParameterChange(StatType.STR, character.Stats.CurrentSTR));
				if (changedItem.Stats.BonusEND > 0 || changedItem.Item.Stats.BaseEND > 0)
					changes.Add(new ParameterChange(StatType.END, character.Stats.CurrentEND));
				if (changedItem.Stats.BonusDEX > 0 || changedItem.Item.Stats.BaseDEX > 0)
					changes.Add(new ParameterChange(StatType.DEX, character.Stats.CurrentDEX));
				if (changedItem.Stats.BonusINT > 0 || changedItem.Item.Stats.BaseINT > 0)
					changes.Add(new ParameterChange(StatType.INT, character.Stats.CurrentINT));
				if (changedItem.Stats.BonusSPR > 0 || changedItem.Item.Stats.BaseSPR > 0)
					changes.Add(new ParameterChange(StatType.SPR, character.Stats.CurrentSPR));
				if (changedItem.Stats.BonusSTR > 0 || changedItem.Item.Stats.BaseSTR > 0 || (changedItem.Stats.CurrentMinDmg > 0 || changedItem.Item.Stats.CurrentMinDmg > 0) || (changedItem.Stats.CurrentMaxDmg > 0 || changedItem.Item.Stats.CurrentMaxDmg > 0))
				{
					changes.Add(new ParameterChange(StatType.MINDMG, character.Stats.CurrentMinDmg));
					changes.Add(new ParameterChange(StatType.MAXDMG, character.Stats.CurrentMaxDmg));
				}
				if (changedItem.Stats.BonusEND > 0 || changedItem.Item.Stats.BaseEND > 0 || (changedItem.Stats.CurrentDef > 0 || changedItem.Item.Stats.CurrentDef > 0))
					changes.Add(new ParameterChange(StatType.DEF, character.Stats.CurrentDef));
				if (changedItem.Stats.BonusDEX > 0 || changedItem.Item.Stats.BaseDEX > 0 || (changedItem.Stats.CurrentEvasion > 0 || changedItem.Item.Stats.CurrentEvasion > 0) || (changedItem.Stats.CurrentAim > 0 || changedItem.Item.Stats.CurrentAim > 0))
				{
					changes.Add(new ParameterChange(StatType.EVASION, character.Stats.CurrentEvasion));
					changes.Add(new ParameterChange(StatType.AIM, character.Stats.CurrentAim));
				}
				if (changedItem.Stats.BonusINT > 0 || changedItem.Item.Stats.BaseINT > 0 || (changedItem.Stats.CurrentMinMDmg > 0 || changedItem.Item.Stats.CurrentMinMDmg > 0) || (changedItem.Stats.CurrentMaxMDmg > 0 || changedItem.Item.Stats.CurrentMaxMDmg > 0))
				{
					changes.Add(new ParameterChange(StatType.MINMDMG, character.Stats.CurrentMinMDmg));
					changes.Add(new ParameterChange(StatType.MAXMDMG, character.Stats.CurrentMaxMDmg));
				}
				if (changedItem.Stats.BonusSPR > 0 || changedItem.Item.Stats.BaseSPR > 0 || (changedItem.Stats.CurrentMDef > 0 || changedItem.Item.Stats.CurrentMDef > 0))
					changes.Add(new ParameterChange(StatType.MDEF, character.Stats.CurrentMDef));
				if (changedItem.Stats.BonusEND > 0 || changedItem.Item.Stats.BaseEND > 0 || (changedItem.Stats.CurrentMaxHP > 0 || changedItem.Item.Stats.CurrentMaxHP > 0))
					changes.Add(new ParameterChange(StatType.MAXHP, character.Stats.CurrentMaxHP));
				if (changedItem.Stats.BonusSPR > 0 || changedItem.Item.Stats.BaseSPR > 0 || (changedItem.Stats.CurrentMaxSP > 0 || changedItem.Item.Stats.CurrentMaxSP > 0))
					changes.Add(new ParameterChange(StatType.MAXSP, character.Stats.CurrentMaxSP));
			}
			if (character.Stats.CurrentHP > character.Stats.CurrentMaxHP)
				CharacterService.ChangeHP(character, character.Stats.CurrentMaxHP);
			if (character.Stats.CurrentSP > character.Stats.CurrentMaxSP)
				CharacterService.ChangeSP(character, character.Stats.CurrentMaxSP);
			if (character.Stats.CurrentLP > character.Stats.CurrentMaxLP)
				CharacterService.ChangeLP(character, character.Stats.CurrentMaxLP);
			character.ToSelectedBy(c => new PROTO_NC_BAT_TARGETINFO_CMD(0, character).Send((Character)c), true);
			new PROTO_NC_CHAR_CHANGEPARAMCHANGE_CMD(changes).Send(character);
		}

		internal static int GetStat(Character character, StatType type)
		{
			switch (type)
			{
				case StatType.STR:
					return character.Stats.CurrentSTR;
				case StatType.END:
					return character.Stats.CurrentEND;
				case StatType.DEX:
					return character.Stats.CurrentDEX;
				case StatType.INT:
					return character.Stats.CurrentINT;
				case StatType.SPR:
					return character.Stats.CurrentSPR;
				case StatType.MINDMG:
					return character.Stats.CurrentMinDmg;
				case StatType.MAXDMG:
					return character.Stats.CurrentMaxDmg;
				case StatType.DEF:
					return character.Stats.CurrentDef;
				case StatType.AIM:
					return character.Stats.CurrentAim;
				case StatType.EVASION:
					return character.Stats.CurrentEvasion;
				case StatType.MINMDMG:
					return character.Stats.CurrentMinMDmg;
				case StatType.MAXMDMG:
					return character.Stats.CurrentMaxMDmg;
				case StatType.MDEF:
					return character.Stats.CurrentMDef;
				case StatType.MAXHP:
					return character.Stats.CurrentMaxHP;
				case StatType.MAXSP:
					return character.Stats.CurrentMaxSP;
				case StatType.PLUSDMG:
					return (int)Math.Ceiling(character.Stats.FreeSTR * 1.2);
				case StatType.PLUSDEF:
					return (int)Math.Ceiling(character.Stats.FreeEND * 0.5);
				case StatType.PLUSMDMG:
					return (int)Math.Ceiling(character.Stats.FreeINT * 1.2);
				case StatType.PLUSMDEF:
					return (int)Math.Ceiling(character.Stats.FreeSPR * 0.5);
				case StatType.PLUSAIM:
					if (character.Stats.FreeDEX <= 33)
						return character.Stats.FreeDEX * 3;
					if (character.Stats.FreeDEX <= 67)
						return 99 + (character.Stats.FreeDEX - 33) * 2;
					if (character.Stats.FreeDEX > 67)
						return 233 + (character.Stats.FreeDEX - 67);
					return 0;
				case StatType.PLUSEVASION:
					if (character.Stats.FreeDEX <= 50)
						return character.Stats.FreeDEX * 2;
					if (character.Stats.FreeDEX > 50)
						return 100 + (character.Stats.FreeDEX - 50);
					return 0;
				case StatType.CRITRATE:
					if (character.Stats.FreeSPR <= 25)
						return character.Stats.FreeSPR * 2;
					if (character.Stats.FreeSPR <= 61)
						return 50 + (character.Stats.FreeSPR - 25);
					if (character.Stats.FreeSPR > 61)
						return (int)(111.0 + (character.Stats.FreeSPR - 61) * 0.5);
					return 0;
				case StatType.BLOCKRATE:
					if (character.Stats.FreeEND <= 50)
						return character.Stats.FreeEND;
					if (character.Stats.FreeEND > 50)
						return (int)(50.0 + (character.Stats.FreeDEX - 50) * 0.5);
					return 0;
				case StatType.PLUSMAXHP:
					return character.Stats.FreeEND * 5;
				case StatType.PLUSMAXSP:
					return character.Stats.FreeSPR * 5;
				case StatType.MAXLP:
					return character.Stats.CurrentMaxLP;
				default:
					return 0;
			}
		}
	}
}
