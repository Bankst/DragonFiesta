using DragonFiesta.Providers.Maps;
using DragonFiesta.World.Game.Character;
using DragonFiesta.World.Game.Zone;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;

namespace DragonFiesta.World.Game.Maps
{
    [Serializable]
    public class WorldServerMap : IMap
    {
        private int IsDisposedInt;

        public ZoneServer Zone { get; private set; }

        public FieldInfo Info { get; private set; }

        public ushort MapId => Info.MapInfo.ID;

        public MapInfo MapInfo => Info.MapInfo;

        public byte ZoneId => Info.ZoneID;

        private SecureCollection<WorldCharacter> Characters;
        private ConcurrentDictionary<int, WorldCharacter> CharactersById;

        internal object ThreadLocker { get; set; }



        public WorldServerMap(ZoneServer mServer, FieldInfo mInfo)
        {
            Info = mInfo;
            Zone = mServer;
            ThreadLocker = new object();
            Characters = new SecureCollection<WorldCharacter>();
            CharactersById = new ConcurrentDictionary<int, WorldCharacter>();
        }

        ~WorldServerMap()
        {
            Dispose();
        }
        protected WorldServerMap(SerializationInfo Sinfo, StreamingContext context)
        {
            ushort MapId = Sinfo.GetUInt16("MapId");

            if (MapDataProvider.GetFieldInfosByMapID(MapId, out FieldInfo MInfo))
            {
                Info = MInfo;
            }
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("MapId", MapId);
        }

        public virtual bool Start()
        {
            return true;
        }

        public virtual bool Stop()
        {
            return true;
        }

        public bool AddCharacter(WorldCharacter pChar) => Characters.Add(pChar)
            && CharactersById.TryAdd(pChar.Info.CharacterID, pChar);

        public bool RemoveCharacter(WorldCharacter pChar) => Characters.Remove(pChar)
            && CharactersById.TryRemove(pChar.Info.CharacterID, out WorldCharacter Char);

        public void Broadcast(FiestaPacket Packet, params WorldCharacter[] Exclude)
        {
            CharacterAction((character) =>
            {
                character.Session.SendPacket(Packet);
            }, true, Exclude);
        }

        public void CharacterAction(Action<WorldCharacter> Action, bool OnlyConnected = true, params WorldCharacter[] Exclude)
        {
            lock (ThreadLocker)
            {
                for (int i = 0; i < Characters.Count; i++)
                {
                    var character = Characters[i];

                    if ((OnlyConnected
                        && !character.IsConnected)
                        || Exclude.Contains(character))
                        continue;

                    try
                    {
                        Action.Invoke(character);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
        }


        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref IsDisposedInt, 1, 0) == 0)
            {
                DisposeInternal();
            }
        }

        protected virtual void DisposeInternal()
        {

            Zone = null;
            Info = null;

            Characters.Clear();
            Characters = null;

            CharactersById.Clear();
            CharactersById = null;

            ThreadLocker = null;
        }
    }
}