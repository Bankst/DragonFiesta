using DragonFiesta.Providers.Text;
using System;

namespace DragonFiesta.Zone.Data.Menu
{
    public class MenuDataInfo
    {
        public uint ID { get; private set; }

        public TextData DataText { get; private set; }
        public SecureCollection<TextData> AnswerData { get; private set; }

        public MenuDataInfo(SQLResult Res, int i)
        {
            ID = Res.Read<uint>(i, "ID");

            uint MenuTextId = Res.Read<uint>(i, "TextId");

            if (!TextDataProvider.GetTextDataById(MenuTextId, out TextData DataText))
            {
                throw new InvalidOperationException($"Can't find TextData {ID} for Menu Question");
            }

            this.DataText = DataText;

            AnswerData = new SecureCollection<TextData>();

            for (int ButtonIndex = 0; i < 1; ButtonIndex++)
            {
                int TextId = Res.Read<int>(i, "Button" + ButtonIndex);

                if (TextId == -1)
                    break;

                if (!TextDataProvider.GetTextDataById((uint)TextId, out TextData ButtonData))
                    throw new InvalidOperationException($"Can't find TextData {TextId} For Button{ButtonIndex} MenuId {ID}");

                AnswerData.Add(ButtonData);
            }
        }
    }
}