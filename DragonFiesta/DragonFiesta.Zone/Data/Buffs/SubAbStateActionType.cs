﻿namespace DragonFiesta.Zone.Data.Buffs
{
    public enum SubAbStateActionType : uint
    {
        SAA_None = 0,
        SAA_STRRATE = 1,   // 5% bonus increase for 1 additional party member
        SAA_STRPLUS = 2,
        SAA_WCPLUS = 3,    // Increased Physical Damage and Physical Defense
        SAA_WCRATE = 4,    // 5% bonus increase for 1 additional party member
        SAA_ACPLUS = 5,    // Decreased Defense
        SAA_ACRATE = 6,    // Decreased Defense
        SAA_DEXPLUS = 7,   // Increased DEX
        SAA_TBPLUS = 8,    // Decreased Evasion
        SAA_TBRATE = 9,
        SAA_THPLUS = 10,   // Increased Aim (10% bonus increase for 1 additional party member)
        SAA_THRATE = 11,
        SAA_INTPLUS = 12,
        SAA_MAPLUS = 13,   // Increased Magic Damage and Magical Defense
        SAA_MENTALPLUS = 14,
        SAA_MRPLUS = 15,   // Decreased Magical Defense
        SAA_MRRATE = 16,   // Christmas blessing [Increases magic defense]
        SAA_SHIELDAMOUNT = 17,   // Damage Absorption
        SAA_SHIELDACRATE = 18,   // Increased Block Rate
        SAA_NOMOVE = 19,   // Stunned
        SAA_SPEEDRATE = 20,   // Increased Travel Speed by 5%
        SAA_ATTACKSPEEDRATE = 21,   // Decreased Attack Rate
        SAA_MAXHPRATE = 22,   // Christmas blessing [Increases MaxHP]
        SAA_MAXSPRATE = 23,   // Christmas blessing [Increases MaxSP]
        SAA_DEADHPSPRECOVRATE = 24,   // Recover party's HP and SP upon death
        SAA_NOATTACK= 25,
        SAA_TICK = 26,   // ActionArg = timer interval in ms
        SAA_DOTDAMAGE = 27,   // damages hp ...
        SAA_CONHEAL = 28,
        SAA_CASTINGTIMEPLUS = 29,   // Increased Casting Time
        SAA_HEALAMOUNT = 30,
        SAA_POISONRESISTRATE = 31,   // Increased Poison and Illness Resistiance
        SAA_DISEASERESISTRATE = 32,   // Increased Disease Resistance (Tier 1)
        SAA_CURSERESISTRATE = 33,   // Increased Curse Resistance (Tier 1)
        SAA_CRITICALRATE = 34,   // Critical 100%
        SAA_MAXHPPLUS = 35,   // Increased Maximum HP
        SAA_MAXSPPLUS = 36,   // Increased Maximum SP
        SAA_INTRATE = 37,   // Increased Intelligence
        SAA_FEAR = 38,   // Fear
        SAA_ALLSTATEPLUS = 39,   // Power of love (Increases all stats by  5%)
        SAA_REVIVEHEALRATE = 40,   // Automatically revive upon death
        SAA_COUNT = 41,   // Blocks a long ranged attack 2 times
        SAA_SILIENCE= 42,
        SAA_DEADLYBLESSING = 43,
        SAA_DAMAGERATE = 44,   // Damage received from selected monster is reduced
        SAA_TARGETENEMY = 45,   // Damage received from selected monster is reduced
        SAA_MARATE = 46,
        SAA_HEALRATE = 47,
        SAA_DOTRATE = 48,
        SAA_AWAY = 49,   // KnockBack lolz
        SAA_TOTALDAMAGERATE = 50,   // Raged to increase attack power.
        SAA_DISPELSPEEDRATE = 51,   // Time attack
        SAA_SETABSTATEME= 52,
        SAA_SETABSTATEFRIEND = 53,   // Bomb is installed to explode in a moment.
        SAA_SETABSTATE = 54,
        SAA_AREA = 55,   // Decreases all healing effects, decrease health by 10% every 5 seconds.
        SAA_GTIRESISTRATE = 56,
        SAA_MAXHPRATEDAMAGE = 57,    // Constant damage to oneself and surrounding enemies.
        SAA_METAABILITY = 58,
        SAA_METASKIN = 59,
        SAA_MISSRATE = 60,
        SAA_REFLECTDAMAGE = 61,
        SAA_RELESEACTION = 62,
        SAA_SCANENEMYUSER = 63,
        SAA_TARGETALL = 64,
        SAA_HIDEENEMY = 65,
        SAA_TARGETNOTME = 66,
        SAA_DOTDIEDAMAGE = 67,
        SAA_ADDALLDOTDMG = 68,
        SAA_ADDBLOODINGDMG = 69,
        SAA_ADDPOISONDMG = 70,
        SAA_EVASIONAMOUNT = 71,
        SAA_USESPRATE = 72,
        SAA_ACMINUS = 73,
        SAA_ACDOWNRATE = 74,
        SAA_SUBTRACTALLDOTDMG = 75,
        SAA_SUBTRACTBLOODINGDMG = 76,
        SAA_SUBTRACTPOISONDMG = 77,
        SAA_ATKSPEEDDOWNRATE = 78,
        SAA_AWAYBACK = 79,
        SAA_CRITICALDOWNRATE = 80,
        SAA_DEXMINUS = 81,
        SAA_HEALAMOUNTMINUS = 82,
        SAA_MAMINUS = 83,
        SAA_MADOWNRATE = 84,
        SAA_MAXHPDOWNRATE = 85,
        SAA_MRMINUS = 86,
        SAA_MRDOWNRATE = 87,
        SAA_SPEEDDOWNRATE = 88,
        SAA_STRMINUS = 89,
        SAA_TBMINUS = 90,
        SAA_TBDOWNRATE = 91,
        SAA_THMINUS = 92,
        SAA_THDOWNRATE = 93,
        SAA_WCMINUS = 94,
        SAA_WCDOWNRATE = 95,
        SAA_DOTWCRATE = 96,
        SAA_TARGETNUMVER = 97,
        SAA_DOTMARATE = 98,
        SAA_MENDOWNRATE = 99,
        SAA_USESPDOWN = 100,
        SAA_CRIUPRATE = 101,
        SAA_MRSHIELDRATE = 102,
        SAA_ACSHIELDRATE = 103,
        SAA_MONSTERSTICK = 104,
        SAA_SETACTIVESKILL = 105,
        SAA_HPRATEDAMAGE = 106,
        SAA_EXPRATE = 107,
        SAA_DROPRATE = 108,
        SAA_AWAYBACKSPOT = 109,
        SAA_STOPANI = 110,
        SAA_SPEEDRESISTRATE = 111,
        SAA_DOTDMGDOWNRATE = 112,
        SAA_SHIELDRATE = 113,
        SAA_LPAMOUNT = 114,
        SAA_MINHP = 115,
        SAA_DMGDOWNRATE = 116,
        SAA_MELEE = 117,
        SAA_RANGE = 118,
        SAA_ALLSTATPLUS = 119,
        SAA_RANGEOVER = 120
    }
}