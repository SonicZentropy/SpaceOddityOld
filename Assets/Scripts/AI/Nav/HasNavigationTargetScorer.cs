/****************
* HasNavigationTargetScorer.cs
* Dylan Bailey
* 1/31/2017
****************/

namespace Zen.AI.Apex.Scorers
{
	using global::AI.Core;
	using global::Apex.AI;
	using UnityEngine;
	using Zen.AI.Apex.Contexts;

	public sealed class HasNavigationTargetScorer  : ZenContextualScorer<ShipContext>
	{
		public override float Score(ShipContext context)
		{
			if (context.targetComp.target != null)
			{
				return Success;
			}
			Debug.Log("Has no nav target");
			return Failure;
		}
	}
}
