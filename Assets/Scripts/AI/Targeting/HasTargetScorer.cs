/****************
* HasTargetScorer.cs
* Dylan Bailey
* 2/1/2017
****************/

namespace Zen.AI.Apex.Scorers
{
	using global::Apex.AI;
	using Zen.AI.Apex.Contexts;

	public class HasTargetScorer  : ContextualScorerBase<ShipContext>
    {
	    public override float Score(ShipContext ctx)
	    {
		    float result = 0f;
		    //if (ctx?.scannerComp == null || ctx.targetComp == null) result = -10f;
		    if (ctx?.targetComp?.target != null) result =  score;

		    //Debug.Log($"Result is: {result}");
		    return result;
	    }
    }
} 
