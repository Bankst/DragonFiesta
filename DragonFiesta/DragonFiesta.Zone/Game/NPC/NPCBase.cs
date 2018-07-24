using DragonFiesta.Game.Stats;
using DragonFiesta.Zone.Data.NPC;
using DragonFiesta.Zone.Game.Character;
using DragonFiesta.Zone.Game.Maps;
using DragonFiesta.Zone.Game.Mobs;
using DragonFiesta.Zone.Game.Stats;
using DragonFiesta.Zone.Network.Helpers;

namespace DragonFiesta.Zone.Game.NPC
{
    public abstract class NPCBase : Mob
    {
        public new NPCInfo Info { get; private set; }

        public sealed override StatsManager Stats => _stats;

        public override MapObjectType Type => MapObjectType.NPC;

        private NPCStatsManager _stats;

	    protected NPCBase(NPCInfo info) : base(info.MobInfo)
        {
			Info = info;

            _stats = new NPCStatsManager(this);
            _stats.UpdateAll();

            LivingStats.Load((uint)_stats.FullStats.MaxHP, (uint)_stats.FullStats.MaxSP, (uint)_stats.FullStats.MaxLP);

            if (info.HasWayPoints)
            {
                WalkPosition = info.WayPointInfo.WalkPosition;
            }

            Selection = new NPCObjectSelection(this);
        }

        protected override void DisposeInternal()
        {
            Info = null;
            _stats.Dispose();
            _stats = null;
            LivingStats.Dispose();
            LivingStats = null;
            Selection.DeselectObject();        
            base.DisposeInternal();
        }

        public override void WriteDisplay(FiestaPacket packet)
        {
            packet.Write<ushort>(MapObjectId);
            packet.Write<byte>(2); // Mode
            packet.Write<ushort>(Info.MobInfo.ID);
            packet.Write<uint>(Position.X);
            packet.Write<uint>(Position.Y);
            packet.Write<byte>(Position.Rotation);
            packet.Write<bool>(Info.IsGate);//00 = buffArray

            switch (Info.Role)
            {
                case NPCRole.Gate:
                case NPCRole.IDGate:
                    packet.WriteString(Info.LinkTable.PortMap.Index, 12);
                    packet.Fill(93, 0x00); // Unk
                    packet.Fill(32, 0x00); // Animation
                    packet.Write<byte>(0); // AnimationLevel
                    packet.Write<byte>(0); // KQTeamType
                    packet.Write<byte>(0); // RegenAni
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
                    packet.Fill(105, 0x00); // Abnormal_state_bit
                    packet.Fill(32, 0x00); // Animation
                    packet.Write<byte>(0); // AnimationLevel
                    packet.Write<byte>(0); // KQTeamType
                    packet.Write<byte>(0); // RegenAni
                    break;
            }
        }

        internal virtual void HandleInteraction(ZoneCharacter character)
        {
            using (var packet = SH08Helper.GetNPCInterActionPacket(this))
            {
                character.Session.SendPacket(packet);
            }
        }

        public abstract void OpenMenu(ZoneCharacter character);

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