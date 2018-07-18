using DragonFiesta.Providers.Items;

namespace DragonFiesta.Zone.Data.Items
{
	[GameServerModule(ServerType.Zone, GameInitalStage.Item)]
	public class ZoneItemDataProvider : ItemDataProviderBase
	{


		[InitializerMethod]
		public static bool Initialize()
		{
			LoadItemInfo();
			LoadItemInfoServer();
			LoadBelongTypeInfo();
			LoadUpgradeInfo();
			LoadGradeItemOption();
			FillItemBaseInfos();
			return true;
		}
	}
}
