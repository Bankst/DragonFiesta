using System;
using System.Collections.Concurrent;
using DFEngine.Content.Game;
using DFEngine.Content.GameObjects;
using DFEngine.Network;

namespace ZoneServer.Behaviors
{
	public class Movement : Behavior
	{
		protected volatile ConcurrentQueue<MoveTo> PendingMoves;
		protected MoveTo CurrentMove;
		protected double MoveAngle;
		protected double MoveTotalTime;
		protected double MoveTakenTime;

		public override void Update(long now)
		{
			if (Object.IsDead)
			{
				Stop(Object.Position);
			}
		}

		public override void MoveTo(Vector2 to, double space = 0, bool force = false)
		{
			SendReinforceMovePacket(to);
		}

		public override void MoveTo(Vector2 from, Vector2 to, double space = 0, bool force = false)
		{
			Object.Position.D = Vector2.Fangle(from, to);
			if (from != null && Math.Abs(Vector2.Distance(Object.Position, from)) >= 5.0)
			{
				Vector2.Copy(from, Object.Position);
			}
			PendingMoves.Enqueue(new MoveTo(Object.Position, to));
			Object.IsActive = true;
			if (force)
			{
				SendReinforceMovePacket(to);
			}
			SendMovePacket(from, to);
		}

		public override void Stop(Vector2 at, bool force = false)
		{
			do
			{
			} while (PendingMoves.TryDequeue(out var result));

			Object.IsMoving = false;
			Object.IsActive = false;
			CurrentMove = null;
			if (force)
			{
				SendReinforceStopPacket(at);
			}
			SendStopPacket(at);
		}

		private void SendMovePacket(Vector2 from, Vector2 to)
		{
			if (Object.IsWalking)
			{
				new PROTO_NC_ACT_SOMEONEMOVEWALK_CMD(Object.Mount ?? Object, from, to).Broadcast(Object, (Object as Character)?.Client);
			}
			else
			{
				new PROTO_NC_ACT_SOMEONEMOVERUN_CMD(Object.Mount ?? Object, from, to).Broadcast(Object, (Object as Character)?.Client);
			}
		}

		private void SendReinforceMovePacket(Vector2 to)
		{
			new PROTO_NC_ACT_REINFORCE_RUN_CMD(to).Send((Object as Character)?.Client);
		}

		private void SendStopPacket(Vector2 at)
		{
			new PROTO_NC_ACT_SOMEONESTOP_CMD(Object.Mount ?? Object, at).Broadcast(Object, (Object as Character)?.Client);
		}

		private void SendReinforceStopPacket(Vector2 at)
		{
			new PROTO_NC_ACT_REINFORCE_STOP_CMD(at).Send((Object as Character)?.Client);
		}
	}
}
