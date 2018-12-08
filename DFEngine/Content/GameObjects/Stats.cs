namespace DFEngine.Content.GameObjects
{
	public class Stats
	{
		private GameObject Object;

		public byte FreeSTR { get; set; }
		public byte FreeEND { get; set; }
		public byte FreeDEX { get; set; }
		public byte FreeINT { get; set; }
		public byte FreeSPR { get; set; }

		public int CurrentHP { get; set; }
		public int CurrentSP { get; set; }
		public int CurrentLP { get; set; }

		public int CurrentHPStones { get; set; }
		public int CurrentSPStones { get; set; }
		public int CurrentPwrStones { get; set; }
		public int CurrentGrdStones { get; set; }

		public int CurrentMaxHP => BaseMaxHP + BonusMaxHP;
		public int CurrentMaxSP => BaseMaxSP + BonusMaxSP;
		public int CurrentMaxLP => BaseMaxLP + BonusMaxLP;

		public int CurrentMaxHPStones => BaseMaxHPStones + BonusMaxHPStones;
		public int CurrentMaxSPStones => BaseMaxSPStones + BonusMaxSPStones;

		public int CurrentHPStoneHealth => BaseHPStoneHealth + BonusHPStoneHealth;
		public int CurrentSPStoneHealth => BaseSPStoneSpirit + BonusSPStoneSpirit;

		public int CurrentSTR => BaseSTR + BonusSTR;
		public int CurrentEND => BaseEND + BonusEND;
		public int CurrentDEX => BaseDEX + BonusDEX;
		public int CurrentINT => BaseINT + BonusINT;
		public int CurrentSPR => BaseSPR + BonusSPR;

		public int CurrentMinDmg => BaseMinDmg + BonusMinDmg;
		public int CurrentMaxDmg => BaseMaxDmg + BonusMaxDmg;
		public int CurrentMinMDmg => BaseMinMDmg + BonusMinMDmg;
		public int CurrentMaxMDmg => BaseMaxMDmg + BonusMaxMDmg;

		public int CurrentDef => BaseDef + BonusDef;
		public int CurrentMDef => BaseMDef + BonusMDef;
		public int CurrentAim => BaseAim + BonusAim;
		public int CurrentEvasion => BaseEvasion + BonusEvasion;

		public int CurrentIllnessResistance => BaseIllnessResistance + BonusIllnessResistance;
		public int CurrentDiseaseResistance => BaseDiseaseResistance + BonusDiseaseResistance;
		public int CurrentCurseResistance => BaseCurseResistance + BonusCurseResistance;
		public int CurrentStunResistance => BaseStunResistance + BonusStunResistance;

		// Base stats
		public int BaseMaxHP { get; set; }
		public int BaseMaxSP { get; set; }
		public int BaseMaxLP { get; set; }

		public int BaseMaxHPStones { get; set; }
		public int BaseMaxSPStones { get; set; }

		public int BaseHPStoneHealth { get; set; }
		public int BaseSPStoneSpirit { get; set; }

		public int BaseSTR { get; set; }
		public int BaseEND { get; set; }
		public int BaseDEX { get; set; }
		public int BaseINT { get; set; }
		public int BaseSPR { get; set; }

		public int BaseMinDmg { get; set; }
		public int BaseMaxDmg { get; set; }
		public int BaseMinMDmg { get; set; }
		public int BaseMaxMDmg { get; set; }
		public int BaseDef { get; set; }
		public int BaseMDef { get; set; }
		public int BaseAim { get; set; }
		public int BaseEvasion { get; set; }

		public int BaseIllnessResistance { get; set; }
		public int BaseDiseaseResistance { get; set; }
		public int BaseCurseResistance { get; set; }
		public int BaseStunResistance { get; set; }

		// Bonus Stats
		public int BonusMaxHP { get; set; }
		public int BonusMaxSP { get; set; } 
		public int BonusMaxLP { get; set; }

		public int BonusMaxHPStones { get; set; }
		public int BonusMaxSPStones { get; set; }

		public int BonusHPStoneHealth { get; set; }
		public int BonusSPStoneSpirit { get; set; }

		public int BonusSTR { get; set; }
		public int BonusEND { get; set; }
		public int BonusDEX { get; set; }
		public int BonusINT { get; set; }
		public int BonusSPR { get; set; }

		public int BonusMinDmg { get; set; }
		public int BonusMaxDmg { get; set; }
		public int BonusMinMDmg { get; set; }
		public int BonusMaxMDmg { get; set; }
		public int BonusDef { get; set; }
		public int BonusMDef { get; set; }
		public int BonusAim { get; set; }
		public int BonusEvasion { get; set; }

		public int BonusIllnessResistance { get; set; }
		public int BonusDiseaseResistance { get; set; }
		public int BonusCurseResistance { get; set; }
		public int BonusStunResistance { get; set; }

		public Stats(GameObject obj)
		{
			Object = obj;
		}

		public void Update()
		{
			if (Object is Character character)
			{
				BaseSTR = character.Parameters.STR;
				BaseEND = character.Parameters.END;
				BaseDEX = character.Parameters.DEX;
				BaseINT = character.Parameters.INT;
				BaseSPR = character.Parameters.SPR;

				BaseHPStoneHealth = character.Parameters.HPStoneHealth;
				BaseSPStoneSpirit = character.Parameters.SPStoneSpirit;

				BaseMaxHPStones = character.Parameters.MaxHPStones;
				BaseMaxSPStones = character.Parameters.MaxSPStones;

				BaseIllnessResistance = character.Parameters.IllnessResistance;
				BaseDiseaseResistance = character.Parameters.DiseaseResistance;
				BaseCurseResistance = character.Parameters.CurseResistance;
				BaseStunResistance = character.Parameters.StunResistance;

				BaseMaxHP = character.Parameters.MaxHP;
				BaseMaxSP = character.Parameters.MaxSP;
				BaseMaxLP = character.Parameters.MaxLP;


			}
		}
	}
}
