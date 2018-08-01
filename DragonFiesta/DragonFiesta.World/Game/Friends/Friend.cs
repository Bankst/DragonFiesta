using DragonFiesta.World.Game.Character;
using System;
using System.Threading;
using DragonFiesta.Database.Models;

namespace DragonFiesta.World.Game.Friends
{
    public class Friend : DBFriend, IDisposable
    {

        public WorldCharacter Owner { get;  set; }
        public WorldCharacter MyFriend { get; private set; }

        public bool IsDisposed { get { return (IsDisposedInt > 0); } }
        private int IsDisposedInt;

      
        public Friend(WorldCharacter Owner, WorldCharacter MyFriend, DateTime RegisterDate)
        {
            this.Owner = Owner;
            this.MyFriend = MyFriend;
            this.RegisterDate = RegisterDate;
        }

        ~Friend()
        {
            Dispose();
        }
        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref IsDisposedInt, 1, 0) == 0)
            {
                Owner = null;
                MyFriend = null;
            }
        }

   

    }
}
