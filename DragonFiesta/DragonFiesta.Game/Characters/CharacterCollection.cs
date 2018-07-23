#region

using System.Collections.Concurrent;
using System.Threading;

#endregion

namespace DragonFiesta.Game.Characters
{
    public abstract class CharacterCollection<CharacterType> where CharacterType : CharacterBase
    {
        public int Count { get { return CharacterList.Count; } }
        public bool IsDisposed { get { return (IsDisposedInt > 0); } }
        protected int IsDisposedInt;
        protected SecureCollection<CharacterType> CharacterList;
        protected ConcurrentDictionary<int, CharacterType> CharactersByID;
        protected ConcurrentDictionary<string, CharacterType> CharactersByName;
        protected ConcurrentDictionary<byte, CharacterType> CharactersBySlot;
        protected object ThreadLocker;

        public CharacterCollection()
        {
            CharacterList = new SecureCollection<CharacterType>();
            CharactersByID = new ConcurrentDictionary<int, CharacterType>();
            CharactersByName = new ConcurrentDictionary<string, CharacterType>();
            CharactersBySlot = new ConcurrentDictionary<byte, CharacterType>();
            ThreadLocker = new object();
        }

        ~CharacterCollection()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref IsDisposedInt, 1, 0) == 0)
            {
                DisposeInternal();
                CharacterList.Dispose();
                CharacterList = null;
                CharactersByID.Clear();
                CharactersByID = null;
                CharactersByName.Clear();
                CharactersByName = null;
            }
        }

        protected virtual void DisposeInternal()
        {

        }
        public bool Add(CharacterType Character)
        {
            lock (ThreadLocker)
            {
                if (CharacterList.Add(Character))
                {
                    FinalizeCharacterAdd(Character);
                    return true;
                }
            }
            return false;
        }

        public bool Remove(CharacterType Character)
        {
            lock (ThreadLocker)
            {
                if (CharacterList.Remove(Character))
                {
                    FinalizeCharacterRemove(Character);
                    return true;
                }
            }
            return true;
        }

        protected abstract void FinalizeCharacterAdd(CharacterType Character);

        protected abstract void FinalizeCharacterRemove(CharacterType Character);

        public CharacterType this[int Index]
        {
            get { return CharacterList[Index]; }
        }

        public bool GetCharacterByID(int ID, out CharacterType Character)
        {
            lock (ThreadLocker)
            {
                return CharactersByID.TryGetValue(ID, out Character);
            }
        }

        public bool GetCharacterByName(string Name, out CharacterType Character)
        {
            lock (ThreadLocker)
            {
                return CharactersByName.TryGetValue(Name, out Character);
            }
        }

        public bool GetCharacterBySlot(byte Slot, out CharacterType Character)
        {
            lock (ThreadLocker)
            {
                return CharactersBySlot.TryGetValue(Slot, out Character);
            }
        }
    }
}