using DragonFiesta.Providers.Maps;
using System;
using DragonFiesta.Database.SQL;

namespace DragonFiesta.Zone.Data.NPC
{
    public sealed class LinkTableInfo
    {
        /// <summary>
        ///  Need To Link From Npc Table RolArgement is this as id
        /// </summary>
        public ushort ID { get; private set; }

        /// <summary>
        /// The map to which the client gets ported.
        /// </summary>
        public MapInfo PortMap { get; private set; }

        /// <summary>
        /// The position where the client will spawn after port.
        /// </summary>
        public Position PortPosition { get; private set; }

        /// <summary>
        /// The min. and max. level required to pass this gate.
        /// </summary>
        public MinMax<byte> Level { get; private set; }

        /// <summary>
        /// True, if clients can pass this gate while they are in a party, false if not.
        /// </summary>
        public bool AllowParty { get; private set; }

        public LinkTableInfo(SQLResult pResult, int i)
        {
            Load(pResult, i);
        }

        private void Load(SQLResult pResult, int i)
        {
            ushort Mapid = pResult.Read<ushort>(i, "MapID");
            MapInfo mapInfo;
            if (!MapDataProvider.GetMapInfoByID(Mapid, out mapInfo))
            {
                throw new InvalidOperationException("Can't find MapInfo for LinkTable. Map ID: " + Mapid);
            }
            PortMap = mapInfo;
            int RotationInt = pResult.Read<int>(i, "Rotation");
            PortPosition = new Position(pResult.Read<uint>(i, "X"), pResult.Read<uint>(i, "Y"), (byte)(RotationInt < 0 ? ((360 + RotationInt) / 2) : (RotationInt / 2)));
            Level = new MinMax<byte>(pResult.Read<byte>(i, "MinLevel"), pResult.Read<byte>(i, "MaxLevel"));
            AllowParty = pResult.Read<bool>(i, "AllowParty");
        }
    }
}