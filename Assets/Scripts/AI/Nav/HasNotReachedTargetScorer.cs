/****************
* HasNotReachedTargetScorer.cs
* Dylan Bailey
* 2/3/2017
****************/

namespace Zen.AI.Apex.Scorers
{
	using global::Apex.AI;
	using Zen.AI.Apex.Contexts;

	public sealed class HasNotReachedTargetScorer  : ContextualScorerBase<ShipContext>
	{
		public override float Score(ShipContext context)
		{
			if(!context.navComp.HasReachedTarget)
				return score;
			return 0f;
		}
	}
}
