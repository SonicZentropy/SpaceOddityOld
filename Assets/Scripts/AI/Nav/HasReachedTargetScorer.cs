/****************
* HasReachedTargetScorer.cs
* Dylan Bailey
* 2/3/2017
****************/

namespace Zen.AI.Apex.Scorers
{
	using global::AI.Core;
	using global::Apex.AI;
	using Zen.AI.Apex.Contexts;

	public sealed class HasReachedTargetScorer  : ZenContextualScorer<ShipContext>
    {
	    public override float Score(ShipContext context)
	    {
		    if(context.AiShipComp.Navigation.HasReachedTarget)
		    	return Success;
		    return Failure;
	    }
    }
}
