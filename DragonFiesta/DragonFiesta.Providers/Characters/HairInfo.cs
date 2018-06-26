using DragonFiesta.Utils.IO.SHN;

public sealed class HairInfo
{
    public byte ID { get; private set; }
    public string Index { get; private set; }
    public string Name { get; private set; }
    public BodyShopGrade Grade { get; private set; }

    public HairInfo(SHNResult pResult, int i)
    {
        ID = pResult.Read<byte>(i, "ID");
        Index = pResult.Read<string>(i, "IndexName");
        Name = pResult.Read<string>(i, "HairName");
        Grade = (BodyShopGrade)pResult.Read<byte>(i, "Grade");
    }
}