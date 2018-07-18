using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using DragonFiesta.Providers.Text.TXT;
using DragonFiesta.Utils.IO.TXT;

namespace DragonFiesta.Providers.Text
{
    [GameServerModule(ServerType.Zone, GameInitalStage.Text)]
    [GameServerModule(ServerType.World, GameInitalStage.Text)]
    public class TextDataProvider
    {
        private static ConcurrentDictionary<uint, TextData> _textDataById;
	    private static SecureCollection<ScriptTXT> _scriptData;
	    private static SecureCollection<MenuStringTXT> _menuStringData;

        [InitializerMethod]
        public static bool OnStart()
        {
            try
            {
				LoadTextData();
                //LoadTextDataSql();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool GetTextDataById(uint id, out TextData mData)
        {
            return _textDataById.TryGetValue(id, out mData);
        }

	    public static void LoadTextData()
	    {
			LoadScript();
//		    LoadMenuString();
		    FillTextData();
	    }

	    private static void FillTextData()
	    {
		    var textCount = _scriptData.Count;
			_textDataById = new ConcurrentDictionary<uint, TextData>();
			DatabaseLog.WriteProgressBar(">> Fill TextData from loaded ShineTables");
		    using (var mBar = new ProgressBar(textCount))
		    {
			    for (var i = 0; i < _scriptData.Count; i++)
			    {
				    var sData = _scriptData.ElementAt(i);
				    var tData = new TextData((uint)i, sData.ScrString);
				    if (!_textDataById.TryAdd((uint)i, tData))
				    {
					    DatabaseLog.Write(DatabaseLogLevel.Warning, $"Failed add TextData from Script.txt at ID: {i}");
					    continue;
				    }
					mBar.Step();
			    }
			}
		    DatabaseLog.WriteProgressBar($"Loaded {_textDataById.Count} rows");
		}

	    private static void LoadScript()
	    {
		    var watch = Stopwatch.StartNew();

		    _scriptData = new SecureCollection<ScriptTXT>();
			var tResult = TXTManager.LoadSingle(TXTType.Script, "Script");
			DatabaseLog.WriteProgressBar(">> Load Script TXT");

		    using (var tTable = tResult[0])
		    {
				using (var mBar = new ProgressBar(tTable.Count))
				{
			   
				    for (var i = 0; i < tTable.Count; i++)
				    {
					    _scriptData.Add(new ScriptTXT(i, tTable));

					    mBar.Step();
					}
				}
		    }
		    watch.Stop();
			DatabaseLog.WriteProgressBar($"Loaded {_scriptData.Count} rows from Script.txt in {(double) watch.ElapsedMilliseconds / 1000}s");
		}

	    private static void LoadMenuString()
	    {

	    }

        public static void ReloadTextData()
        {
            SQLResult pResult = DB.Select(DatabaseType.Data, "SELECT * FROM TextData");

            for (int i = 0; i < pResult.Count; i++)
            {
                TextData mData = new TextData(pResult, i);
                _textDataById.AddOrUpdate(mData.TextId, mData, (key, oldValue) =>
                 {
                     if (!mData.Equals(oldValue))
                         return mData;

                     return oldValue;
                 });
            }
        }

        public static void LoadTextDataSql()
        {
            _textDataById = new ConcurrentDictionary<uint, TextData>();

            SQLResult pResult = DB.Select(DatabaseType.Data, "SELECT * FROM TextData");

            DatabaseLog.WriteProgressBar(">> Load TextData");

            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (int i = 0; i < pResult.Count; i++)
                {
                    TextData mData = new TextData(pResult, i);
                    if (!_textDataById.TryAdd(mData.TextId, mData))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Error, "Duplicate Text With {0} found!", mData.TextId);
                    }
                    mBar.Step();
                }
                DatabaseLog.WriteProgressBar(">> Loaded {0} TextDatas", _textDataById.Count);
            }
        }
    }
}