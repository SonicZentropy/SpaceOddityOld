/****************
* HasNoTargetScorer.cs
* Dylan Bailey
* 1/31/2017
****************/

namespace Zen.AI.Apex.Qualifiers
{
	using System.Collections.Generic;
	using global::Apex.AI;
	using global::Apex.Serialization;
	using UnityEngine;
	using Zen.AI.Apex.Contexts;

	public class HasNoTargetScorer  : ContextualScorerBase<ShipContext>
	{
		//[ApexSerialization(defaultValue = 10f), FriendlyName("Score", "The score output for the option that evaluates true")]
		//public float score = 10f;

		public override float Score(ShipContext ctx)
		{
			float result = 0f;
			//if (ctx?.scannerComp == null || ctx.targetComp == null) result = -10f;
			if (ctx?.targetComp?.target == null) result =  score;

			//Debug.Log($"Result is: {result}");
			return result;
		}
	}
} 
