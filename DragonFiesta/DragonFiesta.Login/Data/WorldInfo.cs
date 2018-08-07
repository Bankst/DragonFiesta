using DragonFiesta.Database.SQL;

namespace DragonFiesta.Login.Data
{
    public class WorldInfo
    {
        public string WorldName { get; private set; }

        public bool IsTestServer { get; private set; }

        public string AllowIP { get; private set; }

        public ClientRegion Region { get; private set; }

        public byte WorldID { get; private set; }

        public WorldInfo(SQLResult Result, int i)
        {
            WorldID = Result.Read<byte>(i, "ID");
            WorldName = Result.Read<string>(i, "WorldName");
            IsTestServer = Result.Read<bool>(i, "TestServer");
            AllowIP = Result.Read<string>(i, "AllowIP");
            Region = (ClientRegion)Result.Read<byte>(i, "Region");
        }
    }
}