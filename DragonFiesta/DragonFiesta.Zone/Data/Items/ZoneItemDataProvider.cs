﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
			//FillItemUpgradeInfos();
			FillItemBaseInfos();
			return true;
		}
	}
}
