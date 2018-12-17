using System;
using System.Collections.Concurrent;
using DFEngine.Content.Game.Engines;
using DFEngine.Content.GameObjects;
using DFEngine.Network;
using DFEngine.Utils;

namespace DFEngine.Content.Game.Controllers
{
	public class MoveController
	{
		protected volatile GameObject Object;
		protected volatile bool IsWalk;
		protected volatile bool IsActive;
		protected volatile ConcurrentQueue<MoveTo> PendingMoves;
		protected MoveTo CurrentMove;
		protected double MoveAngle;
		protected double MoveTotalTime;
		protected double MoveTakenTime;

		protected double Speed
		{
			get
			{
				int num;
				if (!IsWalk)
				{
					var currentRunSpeed = Object?.Mount?.Stats.CurrentRunSpeed;
					if (!currentRunSpeed.HasValue)
					{
						var gameObject = Object;
						num = gameObject?.Stats.CurrentRunSpeed ?? 0;
					}
					else
					{
						num = currentRunSpeed.GetValueOrDefault();
					}
				}

				var currentWalkSpeed = Object?.Mount?.Stats.CurrentWalkSpeed;
				if (!currentWalkSpeed.HasValue)
				{
					var gameObject = Object;
					num = gameObject?.Stats.CurrentWalkSpeed ?? 0;
				}
				else
				{
					num = currentWalkSpeed.GetValueOrDefault();
				}

				return num;
			}
		}

		public MoveController(GameObject gameObj)
		{
			Object = gameObj;
			PendingMoves = new ConcurrentQueue<MoveTo>();
			MoveEngine.MoveControllers.Add(this);
		}

		public void Main(double elapsed)
		{
			if (!IsActive)
			{
				return;
			}

			if (Object.IsDead)
			{
				Stop(Object.Position, true, false);
			}
			else if (PendingMoves.Count > 0 && PendingMoves.TryDequeue(out CurrentMove))
			{
				MoveTakenTime = 0.0;
				MoveTotalTime = Vector2.Distance(CurrentMove.From, CurrentMove.To) / Speed;
			}
			else if (CurrentMove == null)
			{
				Stop(Object.Position, false, false);
			}
			else
			{
				Object.IsMoving = true;
				if (Object.Mount != null && !Object.HasUpdatedMountSpeed)
				{
					Object.HasUpdatedMountSpeed = true;
					new PROTO_NC_MOVER_MOVESPEED_CMD(Object.Mount).Send(Object as Character);
				}

				MoveAngle = Vector2.Angle(Object.Position, CurrentMove.To, false) * Mathd.Deg2Rad;
				MoveTakenTime += elapsed;
				var num1 = Object.Position.X + Speed * elapsed * Math.Cos(MoveAngle);
				var num2 = Object.Position.Y + Speed * elapsed * Math.Sin(MoveAngle);

				/* TODO: BlockInfo!
				if (!Object.Position.Map.Map.BlockInfo.BlockCheck(num1, num2))
		        {
		          if (Object.Type == GameObjectType.CHARACTER)
		          {
		            Stop((Vector2) Object.Position, false, false);
		            return;
		          }
		          if (Object.Type == GameObjectType.MOB && !Object.IsRunningToTarget && (!Object.IsChasing && !Object.IsReturningToRegen))
		          {
		            MoveTo((Vector2) Object.Position,CurrentMove.From, false);
		            return;
		          }		          
		        }
				*/
				Object.Position.X = num1;
				Object.Position.Y = num2;
				if (Object.Type == GameObjectType.CHARACTER)
				{
					var character = Object as Character;
					// TODO: Gates!
					// Gate activation logic
					/*
					List<GameObject> objList = Object.TouchingObjects.Filter((Func<GameObject, bool>) (obj =>
					{
					  NPCInstance npcInstance;
					  if ((npcInstance = obj as NPCInstance) != null)
					    return npcInstance.Gate != null;
					  return false;
					}));
					if (fastList.Count > 0)
					  NPCLogic.Interact(Character, fastList[0].Handle);
					 */
				}

				if (Object.Type == GameObjectType.CHARACTER)
				{
					var character = Object as Character;
					// TODO: Parties!
					/*
					if (character.Party != null)
					{
						Vector2.Copy(Object.Position, (Vector2)character.PartyMember.Position);
						if (Vector2.Distance(Object.Position, Object.LastPartyPosition) >= 200.0)
						{
							Vector2.Copy(Object.Position, Object.LastPartyPosition);
							new PROTO_NC_PARTY_MEMBERLOCATION_CMD((byte)1, character.PartyMember).Broadcast(character.Party, true, character.Position.Map, new PartyMember[1]
							{
								character.PartyMember
							});
						}
					}
					*/
					// Update mount passenger position
					if (character?.Mount?.Passenger != null)
					{
						Vector2.Copy(Object.Position, character.Mount.Passenger.Position);
					}
				}

				if (CurrentMove?.From == null || CurrentMove?.To == null)
				{
					return;
				}

				if (Math.Abs(Vector2.Angle(CurrentMove.From, CurrentMove.To) + MoveAngle) <= 0.0 &&
				    Math.Abs(Vector2.Distance(Object.Position, CurrentMove.To)) > 0.0)
				{
					Stop(CurrentMove.To, false, false);
				}
				else if (MoveTakenTime >= MoveTotalTime ||
				         Math.Abs(Vector2.Distance(Object.Position, CurrentMove.To)) <= 0.0 ||
				         Math.Abs(Vector2.Angle(CurrentMove.From, CurrentMove.To) + MoveAngle) <= 0.0)
				{
					Stop(CurrentMove.To, false, false);
				}
			}
		}

		public void SetWalk(bool value)
		{
			IsWalk = value;
		}

		public void MoveTo(Vector2 from, Vector2 to, bool isReinforce = false)
		{
			Object.Position.D = Vector2.Fangle(from, to);
			if (from != null && Math.Abs(Vector2.Distance(Object.Position, from)) >= 5.0)
			{
				Vector2.Copy(from, Object.Position);
			}
			PendingMoves.Enqueue(new MoveTo(Object.Position, to));
			IsActive = true;
			if (isReinforce)
			{
				SendReinforceMovePacket(to);
			}
			SendMovePacket(from, to);
		}

		public void Stop(Vector2 at, bool withPacket = true, bool isReinforce = true)
		{
			do
			{
			} while (PendingMoves.TryDequeue(out var result));

			Object.IsMoving = false;
			Object.IsActive = false;
			CurrentMove = null;
			if (isReinforce)
			{
				SendReinforceStopPacket(at);
			}

			if (!withPacket)
			{
				return;
			}

			SendStopPacket(at);
		}

		private void SendMovePacket(Vector2 from, Vector2 to)
		{
			if (IsWalk)
			{
				new PROTO_NC_ACT_SOMEONEMOVEWALK_CMD(Object.Mount ?? Object, from, to).Broadcast(Object, (Object as Character)?.Client);
			}
			else
			{
				new PROTO_NC_ACT_SOMEONEMOVERUN_CMD(Object.Mount ?? Object, from, to).Broadcast(Object, (Object as Character)?.Client);
			}
				
		}

		private void SendStopPacket(Vector2 at)
		{
			new PROTO_NC_ACT_SOMEONESTOP_CMD(Object.Mount ?? Object, at).Broadcast(Object.Mount ?? Object, (Object as Character)?.Client);
		}

		private void SendReinforceMovePacket(Vector2 to)
		{
			new PROTO_NC_ACT_REINFORCE_RUN_CMD(to).Send((Object as Character)?.Client);
		}

		private void SendReinforceStopPacket(Vector2 at)
		{
			new PROTO_NC_ACT_REINFORCE_STOP_CMD(at).Send((Object as Character)?.Client);
		}

		public void Dispose()
		{
			Object = null;
			IsWalk = false;
			IsActive = false;
		}
	}
}
