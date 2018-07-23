using DragonFiesta.Providers.Text;
using System;
using DragonFiesta.Database.SQL;
using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Zone.Data.Menu
{
    public class MenuDataInfo
    {
        public uint ID { get; }

        public TextData DataText { get; }
        public SecureCollection<TextData> AnswerData { get; }

        public MenuDataInfo(SQLResult res, int i)
        {
            ID = res.Read<uint>(i, "ID");

            var menuTextId = res.Read<uint>(i, "TextId");

            if (!TextDataProvider.GetTextDataById(menuTextId, out var dataText))
            {
                throw new InvalidOperationException($"Can't find TextData {ID} for Menu Question");
            }

            DataText = dataText;

            AnswerData = new SecureCollection<TextData>();

            for (var buttonIndex = 0; i < 1; buttonIndex++)
            {
                var textId = res.Read<int>(i, "Button" + buttonIndex);

                if (textId == -1)
                    break;

                if (!TextDataProvider.GetTextDataById((uint)textId, out var buttonData))
                    throw new InvalidOperationException($"Can't find TextData {textId} For Button{buttonIndex} MenuId {ID}");

                AnswerData.Add(buttonData);
            }
        }

	    public MenuDataInfo(SHNResult pResult, int i)
	    {
		    ID = pResult.Read<uint>(i, "ID");
		    var menuTextId = pResult.Read<uint>(i, "TextId");

			if (!TextDataProvider.GetTextDataById(menuTextId, out var dataText))
		    {
			    throw new InvalidOperationException($"Can't find TextData {ID} for Menu Question");
			}

		    DataText = dataText;

			AnswerData = new SecureCollection<TextData>();

		    for (var buttonIndex = 0; i < 1; buttonIndex++)
		    {
			    var textId = pResult.Read<int>(i, "Button" + buttonIndex);

			    if (textId == -1)
				    break;

			    if (!TextDataProvider.GetTextDataById((uint)textId, out var buttonData))
				    throw new InvalidOperationException($"Can't find TextData {textId} For Button{buttonIndex} MenuId {ID}");

			    AnswerData.Add(buttonData);
		    }
		}
    }
}