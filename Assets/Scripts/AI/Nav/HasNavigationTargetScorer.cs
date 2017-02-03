/****************
* HasNavigationTargetScorer.cs
* Dylan Bailey
* 1/31/2017
****************/

namespace Zen.AI.Apex.Scorers
{
	using global::Apex.AI;
	using Zen.AI.Apex.Contexts;

	public sealed class HasNavigationTargetScorer  : ContextualScorerBase<ShipContext>
	{
		public override float Score(ShipContext context)
		{
			if (context.targetComp.target != null)
			{
				return score;
			}
			return 0f;
		}
	}
}
