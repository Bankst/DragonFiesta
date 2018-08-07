using DragonFiesta.Database.SQL;

namespace DragonFiesta.Providers.Text
{
    public class TextData
    {
        public uint TextId { get; set; }
        public string Text { get; set; }

        public TextData(SQLResult pResult, int i)
        {
            TextId = pResult.Read<uint>(i, "TextId");
            Text = pResult.Read<string>(i, "Text");
        }

	    public TextData(uint textId, string text)
	    {
		    TextId = textId;
		    Text = text;
	    }
    }
}