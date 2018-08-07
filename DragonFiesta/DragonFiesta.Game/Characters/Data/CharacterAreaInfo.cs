#region

using DragonFiesta.Database.SQL;
using DragonFiesta.Providers.Maps;

#endregion

namespace DragonFiesta.Game.Characters.Data
{
    public sealed class CharacterAreaInfo : AreaInfo
    {

        private IMap _Map { get; set; }
        public IMap Map
        {
            get => _Map;
            set
            {
                _Map = value;
                MapInfo = _Map.MapInfo;
            }
        }

        public bool IsInInstance => (_Map is IInstanceMap);

        public ushort InstanceId
        {
            get
            {
                if (Map is IInstanceMap e)
                {
                    return e.InstanceId;
                }

                return 0;
            }
        }

        public CharacterAreaInfo()
        {

        }
        public CharacterAreaInfo(MapInfo mapInfo)
        {
            this.MapInfo = mapInfo;
        }
        public override bool RefreshFromSQL(SQLResult pRes, int i)
        {
            return base.RefreshFromSQL(pRes, i);
        }
    }
}