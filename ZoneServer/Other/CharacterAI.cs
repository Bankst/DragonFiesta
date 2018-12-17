using System;
using DFEngine.Content.Game;
using DFEngine.Content.Game.Controllers;
using DFEngine.Content.GameObjects;
using DFEngine.Content.Items;
using DFEngine.Content.Other;
using DFEngine.Utils;
using ZoneServer.Services;

namespace ZoneServer.Other
{
	internal class CharacterAI : IAI, IDisposable
	{
		protected Character Character;
		protected long NextStatUpdate;
		protected MoveController MoveController;

		public void Init(GameObject obj)
		{
			Character = obj as Character;
			MoveController = new MoveController(obj);
		}

		public void Main(long now)
		{
			if (Character.State == GameObjectState.HOUSE)
			{
				if (Character.Stats.CurrentHP < Character.Stats.CurrentMaxHP && Character.NextMHHPTick <= now)
				{
					CharacterService.ChangeHP(Character, Math.Min(Character.Stats.CurrentHP + Character.MiniHouse.MiniHouse.HPRecoveryAmount, Character.Stats.CurrentMaxHP));
					Character.NextMHHPTick = now + Character.MiniHouse.MiniHouse.HPTick;
				}
				if (Character.Shape.BaseClass != CharacterClass.CC_CRUSADER && Character.Stats.CurrentSP < Character.Stats.CurrentMaxSP && Character.NextMHSPTick <= now)
				{
					CharacterService.ChangeSP(Character, Math.Min(Character.Stats.CurrentSP + Character.MiniHouse.MiniHouse.SPRecoveryAmount, Character.Stats.CurrentMaxSP));
					Character.NextMHSPTick = now + Character.MiniHouse.MiniHouse.SPTick;
				}
			}
			if (Character.Shape.BaseClass == CharacterClass.CC_CRUSADER && now - Character.LastLPUpdate >= ZoneData.SingleData["SenSPRecover_Time"].SingleDataValue * 1000 - ZoneData.SingleData["SenSPRecover_Time"].SingleDataValue * 1000 * Character.Stats.BonusLPRegenRate && Character.Stats.CurrentLP < Character.Stats.CurrentMaxLP)
				CharacterService.ChangeLP(Character, Math.Min(Character.Stats.CurrentLP + ZoneData.SingleData["SenSPRecover_Amount"].SingleDataValue, Character.Stats.CurrentMaxLP));
			if (NextStatUpdate > now)
				return;
			NextStatUpdate = now + 60000L;
			StatService.SendParameterUpdate(Character, null, 99, Array.Empty<StatType>());
		}

		public void SetIsWalking(bool value)
		{
			MoveController.SetWalk(value);
		}

		public void MoveTo(Vector2 from, Vector2 to, double breathe = 0.0, bool isReinforce = false)
		{
			MoveController.MoveTo(from, Vector2.Point(to, breathe, Vector2.Angle(to, Character.Position, true)), isReinforce);
		}

		public void MoveTo(Vector2 to, double breathe = 0.0)
		{
			MoveTo(Character.Position, to, breathe, false);
		}

		public void ReinforceMove(Vector2 to)
		{
			MoveTo(Character.Position, to, 0.0, true);
		}

		public void OnAttacked(GameObject Object, int aggroAmount, int damageAmount, bool fromFamily = false)
		{
		}

		public void OnDead(GameObject killer)
		{
//			BattleLogic.CeaseFire((GameObject)Character);
		}

		public void SetIsAutoAttacking(bool value)
		{
//			this.BattleController.SetAutoAttacking(value);
		}

//		public void Cast(ActiveSkillInstance skill, GameObject targetObject, Vector2 targetLocation)
//		{
//			this.BattleController.Cast(skill, targetObject, targetLocation, Time.Milliseconds);
//		}

//		public void Cast(ItemInstance item)
//		{
//			this.BattleController.Cast(item, Time.Milliseconds);
//		}

		public void StopMovement(Vector2 pos)
		{
			MoveController.Stop(pos, true, false);
		}

		public void ReinforceStop(Vector2 at)
		{
			MoveController.Stop(at, true, true);
		}

		public void EnableAggro()
		{
		}

		public void DisableAggro()
		{
		}

		public void Dispose()
		{
			Character = null;
			MoveController = null;
//			BattleController = null;
		}
	}
}
