using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.World.Data.MsgWorldManager
{
    public class MsgWorldManager
    {
        public string Desc { get; }

        public MsgWorldManager(SHNResult pResult, int i)
        {
            Desc = pResult.Read<string>(i, "Desc");
        }
    }
}
