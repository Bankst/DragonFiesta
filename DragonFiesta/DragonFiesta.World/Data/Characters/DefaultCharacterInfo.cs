using DragonFiesta.Providers.Maps;
using DragonFiesta.World.Data.Characters;
using System.Collections.Concurrent;

public class DefaultCharacterInfo
{
    /// <summary>
    /// The map where characters of this class will start.
    /// </summary>
    public MapInfo StartMap { get; private set; }

    /// <summary>
    /// The position where characters of this class will start.
    /// </summary>
    public Position StartPosition { get; private set; }
    /// <summary>
    /// The Ammount of LP With CharacterSTarts
    /// </summary>
    public uint LP { get; private set; }
    /// <summary>
    /// The amount of HP with which characters of this class will start.
    /// </summary>
    public uint HP { get; private set; }

    /// <summary>
    /// The amount of SP with which characters of this class will start.
    /// </summary>
    public uint SP { get; private set; }

    /// <summary>
    /// The amount of HP Stones with which characters of this class will start.
    /// </summary>
    public ushort HPStones { get; private set; }

    /// <summary>
    /// The amount of SP Stones with which characters of this class will start.
    /// </summary>
    public ushort SPStones { get; private set; }

    /// <summary>
    /// The amount of money with which characters of this class will start.
    /// </summary>
    ///
    public ulong Money { get; private set; }

    /// <summary>
    /// A list with all default character items from DefaultCharacterData.txt for this class.
    /// </summary>
    public ConcurrentDictionary<ushort, DefaultCharacterItem> DefaultCharacterItems { get; private set; }

    /// <summary>
    /// A list with all default character skills from DefaultCharacterData.txt for this class.
    /// </summary>
    public SecureCollection<DefaultCharacterActiveSkillInfo> DefaultCharacterActiveSkills { get; private set; }

    public SecureCollection<DefaultCharacterPassiveSkillInfo> DefaultCharacterPassiveSkills { get; private set; }

    /// <summary>
    /// StartLevel for The Class
    /// </summary>
    public byte StartLevel { get; private set; }

    public DefaultCharacterInfo(MapInfo StartMap, SQLResult pRes, int i)
    {
        DefaultCharacterItems = new ConcurrentDictionary<ushort, DefaultCharacterItem>();
        DefaultCharacterActiveSkills = new SecureCollection<DefaultCharacterActiveSkillInfo>();
        this.StartMap = StartMap;
        StartPosition = new Position(pRes.Read<uint>(i, "X"), pRes.Read<uint>(i, "Y"), pRes.Read<byte>(i, "Rotation"));
        HP = pRes.Read<uint>(i, "HP");
        SP = pRes.Read<uint>(i, "SP");
        LP = pRes.Read<uint>(i, "LP");
        HPStones = pRes.Read<ushort>(i, "HPStones");
        SPStones = pRes.Read<ushort>(i, "SPStones");
        Money = pRes.Read<uint>(i, "Money");
        StartLevel = pRes.Read<byte>(i, "StartLevel");
    }

    /*
    public void AddDefaultSkillsToCharacter(WorldCharacter Character)
    {
    }

    public void AddDefaultPassoveSkillsToCharacter(WorldCharacter Character)
    {
    }

    public void AddDefaultItemsToCharacter(WorldCharacter Character)
    {
        foreach (var DefaultItem in DefaultCharacterItems.Values)
        {
            if (DefaultItem.pFlags != InventoryType.Equipped)
            {
                if (!Character.Inventar.IsActiveInventory(DefaultItem.pFlags))
                {
                    if (!Character.Inventar.ActivateInventory(DefaultItem.pFlags))
                        continue;//unkown type
                }

                CharacterItem pInvItem = new CharacterItem
                {
                    OwnerID = Character.CharacterID,
                    Owner = Character,
                    Amount = DefaultItem.Amount,
                    ItemID = DefaultItem.ItemInfo.ID,
                    Info = DefaultItem.ItemInfo,
                    Flags = DefaultItem.pFlags,
                    Upgrades = DefaultItem.Upgrades,
                };

                pInvItem.ItemOptions = new CharacterItemOptions(pInvItem, DefaultItem.Options);

                //add to db
                pInvItem.ItemOptions.Save();
                pInvItem.AddItemToDatabase();

                //Add To Another Inventory??
            }
            else
            {
                Item_Equip EqippedItem = new Item_Equip
                {
                    OwnerID = Character.CharacterID,
                    Owner = Character,
                    Amount = DefaultItem.Amount,
                    ItemID = DefaultItem.ItemInfo.ID,
                    Info = DefaultItem.ItemInfo,
                    Flags = DefaultItem.pFlags,
                    Slot = (byte)DefaultItem.ItemInfo.EquipSlot,
                    Upgrades = DefaultItem.Upgrades,
                };

                EqippedItem.ItemOptions = new CharacterItemOptions(EqippedItem, DefaultItem.Options);

                //save to db
                EqippedItem.AddItemToDatabase();
                EqippedItem.ItemOptions.Save();

                Character.Inventar.EquiptInventory.AddItem(EqippedItem);
            }
        }
    }*/
}