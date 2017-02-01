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

	public class HasNoTargetScorer  : ContextualScorerBase<TargetContext>
	{
		//[ApexSerialization(defaultValue = 10f), FriendlyName("Score", "The score output for the option that evaluates true")]
		//public float score = 10f;

		public override float Score(TargetContext ctx)
		{
			float result;
			//if (ctx?.scannerComp == null || ctx.targetComp == null) result = -10f;
			if (ctx?.targetComp?.target == null) result =  score;
			else result = 0f;
			ZenLogger.Log($"Result is: {result}");
			return result;
		}
	}

	/*public class HasNoTargetScorer  : OptionScorerBase<GameObject>
    {
	    [ApexSerialization(defaultValue = 10f), FriendlyName("Score", "The score output for the option that evaluates true")]
	    public float score = 10f;

	    public override float Score(IAIContext context, GameObject option)
	    {
		    var ctx = (TargetContext) context;
		    //var targets = ctx.targets;
		    var targets = GetTargets(ctx);
		    var selfpos = ctx.self.position;

		    float closestdist = 100000f;
		    GameObject closestGO = null;

		    for (int i = 0; i < targets.Length; i++)
		    {
			    float dist = (selfpos - targets[i].transform.position).sqrMagnitude;
			    if (dist < closestdist) // new closest target found
			    {
				    closestdist = dist;
				    closestGO = targets[i];
			    }
		    }

		    if (closestGO != null && option == closestGO)
		    {
			    return score;
		    }

		    return 0f;
	    }

	    private GameObject[] GetTargets(TargetContext ctx)
	    {

		    var targs = Physics.OverlapSphere(ctx.self.position, 200f);
		    List<GameObject> targGOs = new List<GameObject>();
		    foreach (var tg in targs)
		    {
			    if (tg.gameObject != ctx.self.gameObject)
			    {
				    targGOs.Add(tg.gameObject);
			    }
		    }

		    return targGOs.ToArray();
	    }
    }*/
} 
