/****************
* IsNotMovingQualifier.cs
* Dylan Bailey
* 1/31/2017
****************/

namespace Zen.AI.Apex.Qualifiers
{
	using global::Apex.AI;
	using Zen.AI.Apex.Contexts;

	public sealed class IsNotMovingQualifier  : ContextualScorerBase<BasicNavigationContext>
    {
	    public override float Score(BasicNavigationContext context)
	    {
		    return context.rbComp.velocity.sqrMagnitude > 0f ? 0f : score;
	    }
    }
} 
