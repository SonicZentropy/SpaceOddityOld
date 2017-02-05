/****************
* IsWithinAttackRangeScorer.cs
* Dylan Bailey
* 2/4/2017
****************/

namespace Zen.AI.Apex.Scorers
{
	#region Dependencies

	using System.Linq;
	using global::AI.Core;
	using Zen.AI.Apex.Contexts;
	using Zen.Components;

	#endregion

	public sealed class IsWithinAttackRangeScorer  : ZenContextualScorer<ShipContext>
    {
	    public override float Score(ShipContext context)
	    {
		    var distanceToTarget = (context.targetComp.target.position - context.transform.position).sqrMagnitude;
		    var attackRange = context.entity.GetComponent<ShipFittingsComp>().fittingList.First().FittedWeapon.AttackRange;


	        if (distanceToTarget <= attackRange* attackRange)
		        return Success;
		    return Failure;
	    }
    }
}
