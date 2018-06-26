using System;

namespace DragonFiesta.Providers.Characters
{
    [Serializable]
    public enum ClassId : byte
    {
        None,
        Fighter,
        CleverFighter,
        Warrior,
        Gladiator,
        Knight,
        Cleric,
        HighCleric,
        Paladin,
        HolyKnight,
        Guardian,
        Archer,
        HawkArcher,
        Scout,
        SharpShooter,
        Ranger,
        Mage,
        WizMage,
        Enchanter,
        Warlock,
        Wizard,
        Joker,
        Chaser,
        Cruel,
        Closer,
        Assassin,
        Sentinel,
        Savior,
    }

    public class CharacterClass
    {
        public static bool IsValidClass(byte ClassID) => Enum.IsDefined(typeof(ClassId), ClassID);
        
        public static bool ClassUsedLP(ClassId Class) => Class == ClassId.Sentinel || Class == ClassId.Savior;
                
    }
}