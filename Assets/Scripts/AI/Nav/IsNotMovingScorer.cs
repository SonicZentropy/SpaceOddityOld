/****************
* IsNotMovingScorer.cs
* Dylan Bailey
* 1/31/2017
****************/

namespace Zen.AI.Apex.Scorers
{
	using global::Apex.AI;
	using Zen.AI.Apex.Contexts;

	public sealed class IsNotMovingScorer  : ContextualScorerBase<ShipContext>
    {
	    public override float Score(ShipContext context)
	    {
		    return context.rbComp.velocity.sqrMagnitude > 0f ? 0f : score;
	    }
    }
} 
