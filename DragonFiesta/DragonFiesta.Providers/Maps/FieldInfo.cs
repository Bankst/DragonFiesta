using System;
using DragonFiesta.Database.SQL;

namespace DragonFiesta.Providers.Maps
{
    public sealed class FieldInfo
    {
        /// <summary>
        /// The map to which this field info belongs.
        /// </summary>
        public MapInfo MapInfo { get; set; }

        /// <summary>
        /// The time the player can't get hurt after spawning.
        /// </summary>
        public TimeSpan ImmortalTime { get; private set; }

        /// <summary>
        /// The map players will spawn on dead.
        /// </summary>
        public MapInfo RegenMap { get; set; }

        /// <summary>
        /// The position players will spawn on dead.
        /// </summary>
        public Position RegenPosition { get; private set; }

        /// <summary>
        /// True, if players are able to chat on this map, false if not.
        /// </summary>
        public bool CanChat { get; private set; }

        /// <summary>
        /// True, if players are able to shout on this map, false if not.
        /// </summary>
        public bool CanShout { get; private set; }

        /// <summary>
        /// True, if players are able to use HP/SP stones on this map, false if not.
        /// </summary>
        public bool CanUseStone { get; private set; }

        /// <summary>
        /// True, if players are able to go in their minihouse on this map, false if not.
        /// </summary>
        public bool CanMiniHouse { get; private set; }

        /// <summary>
        /// True, if players are able to trade money/items on this map, false if not.
        /// </summary>
        public bool CanTrade { get; private set; }

        /// <summary>
        /// True, if players are able to create/join parties on this map, false if not.
        /// </summary>
        public bool CanParty { get; private set; }

        /// <summary>
        /// True, if players are able to use items on this map, false if not.
        /// </summary>
        public bool CanUseItem { get; private set; }

        /// <summary>
        /// True, if players are able to use skills on this map, false if not.
        /// </summary>
        public bool CanUseSkill { get; private set; }

        /// <summary>
        /// True, if players are able to produce items on this map, false if not.
        /// </summary>
        public bool CanProduce { get; private set; }

        /// <summary>
        /// True, if players are able to use mounts on this map, false if not.
        /// </summary>
        public bool CanMount { get; private set; }

        /// <summary>
        /// The ID of the zone from which this map gets loaded (Column 'Piesta' in Field.txt).
        /// </summary>
        public byte ZoneID { get; set; } // Piesta

        public FieldInfo(SQLResult pResult, int i)
        {
            ImmortalTime = TimeSpan.FromMilliseconds(pResult.Read<uint>(i, "ImmortalTime"));
            RegenPosition = new Position()
            {
                X = pResult.Read<uint>(i, "RegenX"),
                Y = pResult.Read<uint>(i, "RegenY"),
            };
            CanChat = pResult.Read<bool>(i, "Chat");
            CanShout = pResult.Read<bool>(i, "Shout");
            CanUseStone = pResult.Read<bool>(i, "CanStone");
            CanMiniHouse = pResult.Read<bool>(i, "CanMiniHouse");
            ZoneID = pResult.Read<byte>(i, "ZoneID");
        }
    }
}