using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DFEngine.Content.GameObjects
{
	public static class GameObjectAllocator
	{
		private static readonly Queue<ushort> AxialFlagHandles = new Queue<ushort>(Enumerable.Range(13500, 17083).Select<int, ushort>((Func<int, ushort>)(i => (ushort)i)));
		private static readonly Queue<ushort> DropItemHandles = new Queue<ushort>(Enumerable.Range(10500, 13499).Select<int, ushort>((Func<int, ushort>)(i => (ushort)i)));
		private static readonly Queue<ushort> PlayerHandles = new Queue<ushort>(Enumerable.Range(8000, 9499).Select<int, ushort>((Func<int, ushort>)(i => (ushort)i)));
		private static readonly Queue<ushort> MiniHouseHandles = new Queue<ushort>(Enumerable.Range(9500, 10499).Select<int, ushort>((Func<int, ushort>)(i => (ushort)i)));
		private static readonly Queue<ushort> NPCHandles = new Queue<ushort>(Enumerable.Range(17084, 17339).Select<int, ushort>((Func<int, ushort>)(i => (ushort)i)));
		private static readonly Queue<ushort> MobHandles = new Queue<ushort>(Enumerable.Range(0, 7999).Select<int, ushort>((Func<int, ushort>)(i => (ushort)i)));
		private static readonly Queue<ushort> MagicFieldHandles = new Queue<ushort>(Enumerable.Range(20388, 20637).Select<int, ushort>((Func<int, ushort>)(i => (ushort)i)));
		private static readonly Queue<ushort> DoorHandles = new Queue<ushort>(Enumerable.Range(20638, 21637).Select<int, ushort>((Func<int, ushort>)(i => (ushort)i)));
		private static readonly Queue<ushort> BanditHandles = new Queue<ushort>(Enumerable.Range(17340, 19387).Select<int, ushort>((Func<int, ushort>)(i => (ushort)i)));
		private static readonly Queue<ushort> EffectHandles = new Queue<ushort>(Enumerable.Range(19388, 20387).Select<int, ushort>((Func<int, ushort>)(i => (ushort)i)));
		private static readonly Queue<ushort> ServantHandles = new Queue<ushort>(Enumerable.Range(21638, 22137).Select<int, ushort>((Func<int, ushort>)(i => (ushort)i)));
		private static readonly Queue<ushort> MoverHandles = new Queue<ushort>(Enumerable.Range(22138, 23637).Select<int, ushort>((Func<int, ushort>)(i => (ushort)i)));
		private static readonly Queue<ushort> PetHandles = new Queue<ushort>(Enumerable.Range(23638, 25137).Select<int, ushort>((Func<int, ushort>)(i => (ushort)i)));

		public static ushort Allocate(GameObjectType type)
		{
			switch ((ushort)type)
			{
				case 0:
					return GameObjectAllocator.AxialFlagHandles.Dequeue();
				case 1:
					return GameObjectAllocator.DropItemHandles.Dequeue();
				case 2:
					return GameObjectAllocator.PlayerHandles.Dequeue();
				case 3:
					return GameObjectAllocator.MiniHouseHandles.Dequeue();
				case 4:
					return GameObjectAllocator.NPCHandles.Dequeue();
				case 5:
					return GameObjectAllocator.MobHandles.Dequeue();
				case 6:
					return GameObjectAllocator.MagicFieldHandles.Dequeue();
				case 7:
					return GameObjectAllocator.DoorHandles.Dequeue();
				case 8:
					return GameObjectAllocator.BanditHandles.Dequeue();
				case 9:
					return GameObjectAllocator.EffectHandles.Dequeue();
				case 10:
					return GameObjectAllocator.ServantHandles.Dequeue();
				case 11:
					return GameObjectAllocator.MoverHandles.Dequeue();
				case 12:
					return GameObjectAllocator.PetHandles.Dequeue();
				default:
					return ushort.MaxValue;
			}
		}

		public static void Free(GameObjectType type, ushort handle)
		{
			switch ((ushort)type)
			{
				case 0:
					GameObjectAllocator.AxialFlagHandles.Enqueue(handle);
					break;
				case 1:
					GameObjectAllocator.DropItemHandles.Enqueue(handle);
					break;
				case 2:
					GameObjectAllocator.PlayerHandles.Enqueue(handle);
					break;
				case 3:
					GameObjectAllocator.MiniHouseHandles.Enqueue(handle);
					break;
				case 4:
					GameObjectAllocator.NPCHandles.Enqueue(handle);
					break;
				case 5:
					GameObjectAllocator.MobHandles.Enqueue(handle);
					break;
				case 6:
					GameObjectAllocator.MagicFieldHandles.Enqueue(handle);
					break;
				case 7:
					GameObjectAllocator.DoorHandles.Enqueue(handle);
					break;
				case 8:
					GameObjectAllocator.BanditHandles.Enqueue(handle);
					break;
				case 9:
					GameObjectAllocator.EffectHandles.Enqueue(handle);
					break;
				case 10:
					GameObjectAllocator.ServantHandles.Enqueue(handle);
					break;
				case 11:
					GameObjectAllocator.MoverHandles.Enqueue(handle);
					break;
				case 12:
					GameObjectAllocator.PetHandles.Enqueue(handle);
					break;
			}
		}
	}
}
