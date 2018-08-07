using DragonFiesta.Zone.Data.Mob;
using System;
using System.Linq;

namespace DragonFiesta.Zone.Game.Mobs
{
    public class MobChat : IDisposable
    {
        private Mob Mob { get; set; }

        public MobChat(Mob Mob)
        {
            this.Mob = Mob;
        }

        public void Damage() => CastByType(MobChatType.Damged);

        public void Peace() => CastByType(MobChatType.Piece);

        public void Attack() => CastByType(MobChatType.Attack);

        public void Die()
        {
            if (Mob.Info.BroadcastAtDead)
                CastByType(MobChatType.Dead);
        }

        public void Say(string Text)
        {
            Mob.InRange.CharacterAction((character) =>
            {
                //TODO
            });
        }

        private void CastByType(MobChatType Type)
        {
            if (MobDataProvider.GetMobChatInfo(Mob.Info.ID, Type, out MobChatInfo Info))
            {
                //TODO Korrect Calclualtion
                var n = Info.MobChatList.First();
                if (n != null)
                {
                    Say(n.ChatText);
                }
            }
        }

        public void Dispose()
        {
            Mob = null;
        }
    }
}