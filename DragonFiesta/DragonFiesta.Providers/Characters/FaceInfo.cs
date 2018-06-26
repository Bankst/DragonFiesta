using DragonFiesta.Utils.IO.SHN;

public sealed class FaceInfo
{
    public byte ID { get; private set; }

    public string Name { get; private set; }

    public BodyShopGrade Grade { get; private set; }

    public FaceInfo(SHNResult pResult, int i)
    {
        ID = pResult.Read<byte>(i, "ID");
        Name = pResult.Read<string>(i, "FaceName");
        Grade = (BodyShopGrade)pResult.Read<byte>(i, "Grade");
    }
}