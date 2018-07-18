using DragonFiesta.Zone.Data.NPC;
using DragonFiesta.Zone.Game.Character;
using DragonFiesta.Zone.Game.NPC;
using DragonFiesta.Zone.Network.FiestaHandler.Server;
using DragonFiesta.Zone.Game.Maps.Types;
using DragonFiesta.Zone.Game.Maps.Event;
using System;
using System.Collections.Generic;
using System.Threading;
using DragonFiesta.Utils.Logging;

namespace DragonFiesta.Zone.Game.Maps
{
    public sealed class NPCManager : IUpdateAbleServer
    {
        private LocalMap Map;

        private List<NPCBase> NPCList;

        TimeSpan IUpdateAbleServer.UpdateInterval => TimeSpan.FromMilliseconds((int)ServerTaskTimes.MAP_NPC_UPDATE_INTERVAL);

        public GameTime LastUpdate { get; private set; }

        public bool IsDisposed { get { return (IsDisposedInt > 0); } }
        private int IsDisposedInt;

        public NPCManager(LocalMap Map)
        {
            LastUpdate = GameTime.Now();
            this.Map = Map;
            NPCList = new List<NPCBase>();
            this.Map.OnObjectAdded += Map_OnObjectAdded;
        }

        private void Map_OnObjectAdded(object sender, MapObjectEventArgs e)
        {
            if (e.MapObject.Type == MapObjectType.Character && e.MapObject is ZoneCharacter Character)
            {
                SH07Handler.SpawnMultiObject(NPCList.ToArray(), false, (packet) =>
                {
                    Character.Session.SendPacket(packet);
                }, 255);
            }
        }

        public bool AddNPC(NPCInfo Inf, Position Pos, bool Sending = false)
        {
            if (!CreateNPC(Inf, out NPCBase NPC))
            {
                GameLog.Write(GameLogLevel.Warning, $"Failed to Create NPC Please Check Role of NPC {Inf.MobInfo}");
                return false;
            }

            NPC.Position = Pos;

            if (!Map.AddObject(NPC))
            {
                return false;
            }

            NPCList.Add(NPC);

            if (Sending)
            {
                using (var packet = SH07Handler.SpawnSingleObject(NPC))
                {
                    NPC.Broadcast(packet, false);
                }
            }
            return true;
        }

        public bool CreateNPC(NPCInfo info, out NPCBase npc)
        {
            npc = null;

            switch (info.Role)
            {
                case NPCRole.Merchant:
					switch (info.RoleArgument)
                    {
                        case NPCArgument.Weapon:
                            npc = new WeaponNPC(info);
                            break;

                        case NPCArgument.SoulStone:
                            npc = new SoulStoneNPC(info);
                            break;

                        case NPCArgument.WeaponTitle:
                            npc = new WeaponTitle(info);
                            break;

                        case NPCArgument.Quest:
                            npc = new QuestNPC(info);
                            break;

                        case NPCArgument.Skill:
                            npc = new SkillNPC(info);
                            break;

                        case NPCArgument.Item:
                            npc = new StuffNPC(info);
                            break;

                        case NPCArgument.Guild:
                            npc = new GuildNPC(info);
                            break;

                    }
                    break;

                case NPCRole.ClientMenu:
                    npc = new ClientMenu(info);
                    break;

                case NPCRole.QuestNpc:
                    switch (info.RoleArgument)
                    {
                        case NPCArgument.Quest:
                            npc = new QuestNPC(info);
                            break;

                        case NPCArgument.GBDice:
                            npc = new GBDice(info);
                            break;
                    }

                    break;

                case NPCRole.StoreManager:
                    switch (info.RoleArgument)
                    {
                        case NPCArgument.None:
                            npc = new StoreManager(info);
                            break;

                        case NPCArgument.Quest:
                            npc = new QuestNPC(info);
                            break;
                    }

                    break;

                case NPCRole.NPCMenu:
                    switch (info.RoleArgument)
                    {
                        case NPCArgument.Guild:
                            npc = new GuildNPC(info);
                            break;

                        case NPCArgument.ExchangeCoin:
                            npc = new ExchangeCoin(info);
                            break;
                    }

                    break;

                //Todo Add GateNPC

                case NPCRole.JobMaster:
                    npc = new JobNPC(info);
                    break;

                case NPCRole.Guard:
                    npc = new QuestNPC(info);
                    break;

                case NPCRole.Gate:
                    npc = new GateNPC(info);
                    break;

                case NPCRole.None:
                default:
                    npc = new DefaultNPC(info);
                    break;
            }

            return npc != null;
        }

        public bool Start()
        {
            if (NPCDataProvider.GetNPCListByMapId(Map.MapId, out List<NPCInfo> NpcList))
            {
                foreach (var npcinfo in NpcList)
                {
                    if (!AddNPC(npcinfo, npcinfo.Position, false))
                        continue;
                }
            }
            return true;
        }

        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref IsDisposedInt, 1, 0) == 0)
            {
                Map = null;
                NPCList.Clear();
                NPCList = null;
            }
        }

        public bool Update(GameTime gameTime)
        {
            if (IsDisposed) return false;

            foreach (var npc in NPCList)
            {
                if (npc.WalkPosition.Count > 0)
                {
                    npc.MoveToNextPoint();
                }
            }

            LastUpdate = GameTime.Now();
            return true;
        }
    }
}