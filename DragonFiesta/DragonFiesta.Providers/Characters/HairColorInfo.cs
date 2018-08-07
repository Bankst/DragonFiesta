using DragonFiesta.Utils.IO.SHN;

public sealed class HairColorInfo
{
    public byte ID { get; private set; }
    public string Index { get; private set; }
    public string Name { get; private set; }
    public BodyShopGrade Grade { get; private set; }

    public HairColorInfo(SHNResult pResult, int i)
    {
        ID = pResult.Read<byte>(i, "ID");
        Index = pResult.Read<string>(i, "IndexName");
        Name = pResult.Read<string>(i, "Name");
        Grade = (BodyShopGrade)pResult.Read<byte>(i, "Grade");
    }
}