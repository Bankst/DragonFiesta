using DragonFiesta.Game.Stats;
using DragonFiesta.Zone.Data.NPC;
using DragonFiesta.Zone.Game.Character;
using DragonFiesta.Zone.Game.Maps;
using DragonFiesta.Zone.Game.Mobs;
using DragonFiesta.Zone.Game.Stats;

namespace DragonFiesta.Zone.Game.NPC
{
    public abstract class NPCBase : Mob
    {
        public new NPCInfo Info { get; private set; }

        public sealed override StatsManager Stats => _Stats;

        public override MapObjectType Type => MapObjectType.NPC;

        private NPCStatsManager _Stats;

        public NPCBase(NPCInfo Info) : base(Info.MobInfo)
        {
            this.Info = Info;

            _Stats = new NPCStatsManager(this);
            _Stats.UpdateAll();

            LivingStats.Load((uint)_Stats.FullStats.MaxHP, (uint)_Stats.FullStats.MaxSP, (uint)_Stats.FullStats.MaxLP);

            if (Info.HasWayPoints)
            {
                WalkPosition = Info.WayPointInfo.WalkPosition;
            }

            Selection = new NPCObjectSelection(this);
        }

        protected override void DisposeInternal()
        {
            Info = null;
            _Stats.Dispose();
            _Stats = null;
            LivingStats.Dispose();
            LivingStats = null;
            Selection.DeselectObject();        
            base.DisposeInternal();
        }
        public override void WriteDisplay(FiestaPacket Packet)
        {
            Packet.Write<ushort>(MapObjectId);
            Packet.Write<byte>(2);//rank????
            Packet.Write<ushort>(Info.MobInfo.ID);
            Packet.Write<uint>(Position.X);
            Packet.Write<uint>(Position.Y);
            Packet.Write<byte>(Position.Rotation); // 14 bytes
            Packet.Write<bool>(Info.IsGate);//00 = buffArray

            switch (Info.Role)
            {
                case NPCRole.Gate:
                case NPCRole.IDGate:
                    Packet.WriteString(Info.LinkTable.PortMap.Index, 12);
                    Packet.Fill(127, 0x00);
                    break;
                case NPCRole.RandomGate:
                case NPCRole.ClientMenu:
                case NPCRole.Guard:
                case NPCRole.JobMaster:
                case NPCRole.Merchant:
                case NPCRole.None:
                case NPCRole.NPCMenu:
                case NPCRole.QuestNpc:
                case NPCRole.StoreManager:
                default:
                    Packet.Fill(139, 0x00);
                    break;
            }
        }

        internal virtual void HandleInteraction(ZoneCharacter Character)
        {
        }

        public abstract void OpenMenu(ZoneCharacter Character);

        public override void MoveToNextPoint()//hmm Use this is funny?
        {
            //(this.Map as LocalMap).BlockInfos.WalkPositions.Find
            /*
            if(WalkPosition.ContainsKey(NowMoveStep))
            {
                OnMove.Invoke(this, new LivingObjectMovementEventArgs(this,Position, WalkPosition[NowMoveStep], true, false));
            }

            NowMoveStep++;*/
            base.MoveToNextPoint();
        }

    }
}