/****************
* HasTargetScorer.cs
* Dylan Bailey
* 2/1/2017
****************/

namespace Zen.AI.Apex.Qualifiers
{
	using global::Apex.AI;
	using Zen.AI.Apex.Contexts;

	public class HasTargetScorer  : ContextualScorerBase<TargetContext>
    {
	    public override float Score(TargetContext ctx)
	    {
		    float result;
		    //if (ctx?.scannerComp == null || ctx.targetComp == null) result = -10f;
		    if (ctx?.targetComp?.target != null) result =  score;
		    else result = 0f;
		    ZenLogger.Log($"Result is: {result}");
		    return result;
	    }
    }
} 
