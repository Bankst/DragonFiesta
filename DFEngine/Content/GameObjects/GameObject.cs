using System;
using System.Collections.Generic;
using System.Linq;
using DFEngine.Content.Game;
using DFEngine.Content.Game.Controllers;
using DFEngine.Content.GameObjects.Movers;

namespace DFEngine.Content.GameObjects
{
	public abstract class GameObject
	{
		public ushort Handle { get; set; }

		public bool IsVisible { get; set; }
		public bool IsMoving { get; set; }
		public bool IsActive { get; set; }
		public bool IsDead { get; set; }
		public byte Level { get; set; }
		public GameObjectType Type { get; set; }
		public GameObjectState State { get; set; } = GameObjectState.NONBATTLE;
		public GameObjectFlags Flags { get; set; }
		public Stats Stats { get; set; }
		public Position Position { get; set; }
		public MoverInstance Mount { get; set; }
		public string MapIndx => Position?.Map?.Info.MapName;

		public IController Controller { get; set; }
		public IAI AI { get; set; }

		public List<GameObject> VisibleObjects { get; set; }

		public List<Character> VisibleCharacters => VisibleObjects.OfType<Character>().ToList();
		public List<GameObject> TouchingObjects => VisibleObjects.Filter(obj => Vector2.Distance(Position, obj.Position) <= 10.0);

		public string AnimationIndx { get; set; }
		public byte AnimationLevel { get; set; }

		private GameObject _target;
		public GameObject Target
		{
			get => _target;
			set
			{
				if (value != null && !value.SelectedBy.Contains(this))
					value.SelectedBy.Add(this);
				_target = value;
			}
		}

		public void ToSelectedBy(Action<GameObject> action, bool onlyCharacters = false)
		{
			for (var upperBound = SelectedBy.GetUpperBound(); upperBound >= 0; --upperBound)
			{
				try
				{
					var gameObject = SelectedBy[upperBound];
					if (gameObject.Target != this) continue;
					if (onlyCharacters)
					{
						if (!(gameObject is Character))
							continue;
					}
					action(gameObject);
				}
				catch
				{
					// ignored
				}
			}
		}

		public bool HasUpdatedMountSpeed { get; set; }

		public List<GameObject> SelectedBy { get; set; }

		public ushort HPChangeOrder => _hpChangeOrder++;

		private ushort _hpChangeOrder;

		protected GameObject()
		{
			Stats = new Stats(this);
			Position = new Position();

			VisibleObjects = new List<GameObject>();
		}
	}
}
