using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
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

        public static bool GetTextDataById(uint id, out TextData mData)
        {
            return _textDataById.TryGetValue(id, out mData);
        }

	    public static void LoadTextData()
	    {
			LoadScript();
		    FillTextData();
	    }

	    private static void FillTextData()
	    {
            var watch = Stopwatch.StartNew();
            var textCount = _scriptData.Count;
			_textDataById = new ConcurrentDictionary<uint, TextData>();
            DataLog.WriteProgressBar(">> Fill TextData");
		    using (var mBar = new ProgressBar(textCount))
		    {
			    for (var i = 0; i < _scriptData.Count; i++)
			    {
				    var sData = _scriptData.ElementAt(i);
				    var tData = new TextData((uint)i, sData.ScrString);
				    if (!_textDataById.TryAdd((uint)i, tData))
				    {
                        DataLog.Write(DataLogLevel.Warning, $"Failed add TextData from Script.txt at ID: {i}");
					    continue;
				    }
					mBar.Step();
			    }
                watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {_textDataById.Count} rows from .txt Files in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
		}

	    private static void LoadScript()
	    {
		    var watch = Stopwatch.StartNew();

		    _scriptData = new SecureCollection<ScriptTXT>();
			var tResult = TXTManager.LoadSingle(TXTType.Script, "Script");
            DataLog.WriteProgressBar(">> Load Script TXT");

		    using (var tTable = tResult[0])
		    {
				using (var mBar = new ProgressBar(tTable.Count))
				{
			   
				    for (var i = 0; i < tTable.Count; i++)
				    {
					    _scriptData.Add(new ScriptTXT(i, tTable));
					    mBar.Step();
					}
                    watch.Stop();
                    DataLog.WriteProgressBar($">> Loaded {_scriptData.Count} rows from Script.txt in {(double)watch.ElapsedMilliseconds / 1000}s");
                }
		    }
		}
    }
}