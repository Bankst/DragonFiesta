using System;
using System.Collections.Generic;
using System.Text;

namespace DFEngine.Content.GameObjects.MiniHouse
{
	public class MiniHouseInstance : GameObject
	{
		public MiniHouse MiniHouse { get; set; }
		public string Title { get; set; }

		public MiniHouseInstance(MiniHouse miniHouse)
		{
			MiniHouse = miniHouse;
			Type = GameObjectType.MINIHOUSE;
		}
	}
}
