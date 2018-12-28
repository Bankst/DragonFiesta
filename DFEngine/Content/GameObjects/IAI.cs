using DFEngine.Content.Game;

namespace DFEngine.Content.GameObjects
{
	public interface IAI
	{
		void Init(GameObject Object);

		void Main(long now);

		void MoveTo(Vector2 to, double breathe = 0.0);

		void MoveTo(Vector2 from, Vector2 to, double breathe = 0.0, bool isReinforce = false);

		void ReinforceMove(Vector2 to);

		void ReinforceStop(Vector2 at);

		void SetIsWalking(bool value);

		void StopMovement(Vector2 pos);

		void OnAttacked(GameObject attacker, int aggroAmount, int damageAmount, bool fromFamily = false);

		void OnDead(GameObject killer);

		// TODO: Skills and Items
//		void Cast(ActiveSkillInstance skill, GameObject targetObject, Vector2 targetLocation);
//
//		void Cast(ItemInstance item);

		void SetIsAutoAttacking(bool value);

		void EnableAggro();

		void DisableAggro();

		void Dispose();
	}
}
