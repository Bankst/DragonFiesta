using DragonFiesta.Providers.Maps;
using DragonFiesta.Zone.Data.Mob;
using DragonFiesta.Zone.Data.WayPoints;
using System;
using System.Collections.Generic;
using DragonFiesta.Database.SQL;

namespace DragonFiesta.Zone.Data.NPC
{
    public sealed class NPCInfo
    {
        /// <summary>
        /// Walk Position of NPC
        /// </summary>
        ///
        public bool HasWayPoints => WayPointInfo != null;

        public WayPointInfo WayPointInfo { get; private set; }

        public MobInfo MobInfo { get; private set; }

        /// <summary>
        /// The Map on which this NPC stands.
        /// </summary>
        public MapInfo MapInfo { get; private set; }

        /// <summary>
        /// The position on which this NPC stands.
        /// </summary>
        public Position Position { get; private set; }

        /// <summary>
        /// True, if this NPC displays a menu at the client, false if not.
        /// </summary>
        public bool HasNPCMenu { get; private set; }

        /// <summary>
        /// The role of this NPC.
        /// </summary>
        public NPCRole Role { get; private set; }

        /// <summary>
        /// The argument of the NPCRole.
        /// </summary>
        public NPCArgument RoleArgument { get; private set; }

        /// <summary>
        /// The LinkTableInfo if this is a gate.
        /// </summary>
        ///
        public bool IsGate => LinkTable != null && (Role == NPCRole.Gate || Role == NPCRole.IDGate);

        public LinkTableInfo LinkTable { get; set; }
        public List<NPCItem> Items { get; private set; }

        public NPCInfo(SQLResult pResult, int i)
        {
            Load(pResult, i);
            Items = new List<NPCItem>();
        }

        private void Load(
			SQLResult pResult, int i)
        {
            ushort MobID = pResult.Read<ushort>(i, "MobID");
            ushort MapID = pResult.Read<ushort>(i, "MapID");

            if (!MobDataProvider.GetMobInfoByID(MobID, out MobInfo mobInfo))
            {
                throw new InvalidOperationException("Can't find MobInfo for NPC. Mob ID: " + MobID);
            }

            MobInfo = mobInfo;

            if (!MapDataProvider.GetMapInfoByID(MapID, out MapInfo mapInfo))
            {
                throw new InvalidOperationException("Can't find MapInfo for NPC. Map ID: " + MapID);
            }

            int WayPointId = pResult.Read<int>(i, "WayPoint");
            WayPointInfo WayPoint = null;

            if (WayPointId != 0 && !MobDataProvider.GetWayPointById(WayPointId, out WayPoint))
            {
                throw new InvalidOperationException($"Can't find Waypoint {WayPointId} for NPC MobId {MobID}");
            }

            if (WayPoint != null)
            {
                WayPointInfo = WayPoint;
            }

            MapInfo = mapInfo;
            uint X = pResult.Read<uint>(i, "X");
            uint Y = pResult.Read<uint>(i, "Y");
            int RotationInt = pResult.Read<int>(i, "Rotation");
            Position = new Position(X, Y, (byte)(RotationInt < 0 ? ((360 + RotationInt) / 2) : (RotationInt / 2)));
            HasNPCMenu = pResult.Read<bool>(i, "HasMenu");
            Role = (NPCRole)pResult.Read<byte>(i, "Role");
            RoleArgument = (NPCArgument)pResult.Read<ushort>(i, "RoleArgument");
        }
    }
}