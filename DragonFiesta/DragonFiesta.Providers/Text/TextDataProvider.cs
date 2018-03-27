using System.Collections.Concurrent;

namespace DragonFiesta.Providers.Text
{
    [GameServerModule(ServerType.Zone, GameInitalStage.Text)]
    [GameServerModule(ServerType.World, GameInitalStage.Text)]
    public class TextDataProvider
    {
        private static ConcurrentDictionary<uint, TextData> TextDataById;

        [InitializerMethod]
        public static bool OnStart()
        {
            try
            {
                LoadTextData();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool GetTextDataById(uint ID, out TextData mData)
        {
            return TextDataById.TryGetValue(ID, out mData);
        }

        public static void RealoadTextData()
        {
            SQLResult pResult = DB.Select(DatabaseType.Data, "SELECT * FROM TextData");

            for (int i = 0; i < pResult.Count; i++)
            {
                TextData mData = new TextData(pResult, i);
                TextDataById.AddOrUpdate(mData.TextId, mData, (key, oldValue) =>
                 {
                     if (!mData.Equals(oldValue))
                         return mData;

                     return oldValue;
                 });
            }
        }

        public static void LoadTextData()
        {
            TextDataById = new ConcurrentDictionary<uint, TextData>();

            SQLResult pResult = DB.Select(DatabaseType.Data, "SELECT * FROM TextData");

            DatabaseLog.WriteProgressBar(">> Load TextData");

            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (int i = 0; i < pResult.Count; i++)
                {
                    TextData mData = new TextData(pResult, i);
                    if (!TextDataById.TryAdd(mData.TextId, mData))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Error, "Dublicate Text With {0} found!", mData.TextId);
                    }
                    mBar.Step();
                }
                DatabaseLog.WriteProgressBar(">> Loaded {0} TextDatas", TextDataById.Count);
            }
        }
    }
}