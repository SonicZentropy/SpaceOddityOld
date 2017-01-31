namespace Zen.AI.Bundles
{
	using System.Collections.Generic;
	using Zen.AI.Actions;

	public class EnemyFighterBundle : AiActionBundle
	{
		public EnemyFighterBundle() : base(new List<AbstractAiAction>
		{
			//new PlayerMechNavigateToPositionAction()

		})
		{
		}
	}
}