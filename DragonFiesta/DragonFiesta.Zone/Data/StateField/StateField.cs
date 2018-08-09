using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Zone.Data.StateField
{
    public class StateField
    {
        public string AbStateInx { get; }

        public string MapName { get; }

        public uint StateSet { get; }

        public StateField(SHNResult pResult, int i)
        {
            AbStateInx = pResult.Read<string>(i, "AbStateInx");
            MapName = pResult.Read<string>(i, "MapName");
            StateSet = pResult.Read<uint>(i, "StateSet");
        }
    }
}
