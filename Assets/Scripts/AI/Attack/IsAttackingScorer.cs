/****************
* IsAttackingScorer.cs
* Dylan Bailey
* 2/4/2017
****************/

namespace Zen.AI.Apex.Scorers
{
	using global::AI.Core;
	using global::Apex.AI;
	using Zen.AI.Apex.Contexts;

	public sealed class IsAttackingScorer  : ZenContextualScorer<ShipContext>
    {
	    public override float Score(ShipContext context)
	    {
		    if (context.commandComp.AttackPressed)
			    return Success;
		    return Failure;
	    }
    }
}
