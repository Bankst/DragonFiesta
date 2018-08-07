using DragonFiesta.World.Data.Settings;
using DragonFiesta.World.Game.Character;
using System;
using System.IO;
using System.Linq;
using System.Text;
using DragonFiesta.Database.SQL;
using DragonFiesta.Utils.Logging;

namespace DragonFiesta.World.Game.Settings
{
    public class CharacterKeyMap : IDisposable
    {
        public SecureCollection<CharacterKeyMapOptions> KeyMap { get; private set; }

        public int KeyMapCount => KeyMap.Count;
        public const byte MaxKeyMapSize = 95;//need sniffdata...

        private WorldCharacter Owner { get; set; }

        public CharacterKeyMap()
        {
            KeyMap = new SecureCollection<CharacterKeyMapOptions>();
        }

        public CharacterKeyMap(WorldCharacter Owner) : base()
        {
            KeyMap = new SecureCollection<CharacterKeyMapOptions>();

            this.Owner = Owner;
        }
        ~CharacterKeyMap()
        {
            Dispose();
        }


        public bool AddKeyOptions(CharacterKeyMapOptions KeyOption)
        {

            if (Contains(KeyOption.KeyType))
            {
                GameLog.Write(GameLogLevel.Warning, "Invalid Key {0} of KeyMapping CharacterId {1}", KeyOption.KeyType, Owner.Info.CharacterID);
                return false;
            }
            if (!KeyMap.Add(KeyOption))
            {
         
                return false;
            }

            return true;
        }

        public bool UpdateKey(
            ushort Key,
            byte ExtendKey,
            char ASCIIKey)
        {
            if (this.Contains(Key))
            {
                KeyMap[Key].ExtendKey = ExtendKey;
                KeyMap[Key].ASCIIKey = ASCIIKey;
                return true;
            }
            return false;
        }

        public bool LoadKeyMapFromData(byte[] DataArray)
        {
            try
            {
                using (var Reader = new BinaryReader(new MemoryStream(DataArray), Encoding.ASCII))
                {
                    ushort UpdateCount = Reader.ReadUInt16();
                    for (int i = 0; i < UpdateCount; i++)
                    {
                        if (!AddKeyOptions(new CharacterKeyMapOptions(Reader)))
                        {
                            GameLog.Write(GameLogLevel.Warning, "Invalid Key Data found for Character {0}", Owner.Info.CharacterID);
                            continue;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                GameLog.Write(ex, "Failed Load KeyMap For Character {0}", Owner.Info.Name);
                return false;
            }
        }

        public byte[] GetKeyMapData()
        {
            using (MemoryStream Stream = new MemoryStream())
            {
                using (var Writer = new BinaryWriter(Stream))
                {
                    Writer.Write((ushort)KeyMap.Count);

                    foreach (var Key in KeyMap)
                    {
                        Key.Write(Writer);
                    }

                    return Stream.ToArray();
                }
            }
        }

        public bool RefreshFromSQL(SQLResult Result, int i)
        {
            try
            {
                KeyMap.Clear();

                byte[] KeyData = Result.GetBytes(i, "KeyMapping");

                if (KeyData.Length != 1)
                {
                    return  LoadKeyMapFromData(KeyData);
                }
                else
                {
                    KeyMap = GameSettingDataProvider.DefaultKeyMap.KeyMap;
                    return true;
                }
            }
            catch (Exception ex)
            {
                GameLog.Write(ex, "Failed Load CharacterKeyMap");
                return false;
            }
        }

        public KeyMapOptions this[ushort Index]
        {
            get { return KeyMap[Index]; }
        }
        public bool Contains(ushort Key)
        {
            return KeyMap.FirstOrDefault(m => m.KeyType == Key) != null;
        }

        public void Dispose()
        {
            Owner = null;
        }
    }
}
