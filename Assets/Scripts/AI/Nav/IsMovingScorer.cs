/****************
* IsNotMovingScorer.cs
* Dylan Bailey
* 1/31/2017
****************/

namespace Zen.AI.Apex.Scorers
{
	using global::AI.Core;
	using global::Apex.AI;
	using Zen.AI.Apex.Contexts;

	public sealed class IsMovingScorer  : ZenContextualScorer<ShipContext>
    {
	    public override float Score(ShipContext context)
	    {
		    return context.rbComp.velocity.sqrMagnitude > 0f ? Success : Failure;
	    }
    }
} 
