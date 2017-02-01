/****************
* AcquireTargetAction.cs
* Dylan Bailey
* 2/1/2017
****************/

namespace Zen.AI.Apex.Actions
{
	using global::Apex.AI;
	using UnityEngine;
	using Zen.AI.Apex.Contexts;

	public class AcquireTargetAction : ActionBase<TargetContext>
    {
	    public override void Execute(TargetContext context)
	    {
		    if (context.targetComp.target != null) return;

		    var selfpos = context.transform.position;
		    var targets = Physics.OverlapSphere(context.transform.position, 200f);


		    float closestdist = 100000f;
		    Transform closestTform = null;

		    for (int i = 0; i < targets.Length; i++)
		    {
			    var rb = targets[i].attachedRigidbody;
			    if (rb.gameObject == context.transform.gameObject)
			    {
				    ZenLogger.Log($"found self");
				    continue;
			    }
			    float dist = (selfpos - rb.transform.position).sqrMagnitude;
			    if (dist < closestdist) // new closest target found
			    {
				    closestdist = dist;
				    closestTform = targets[i].transform;
				    ZenLogger.Log($"Closest tform set to {rb.gameObject.name}");
			    }
		    }

		    if (closestTform != null)
		    {
			    ZenLogger.Log($"Setting new target: {closestTform.name}");
			    context.targetComp.target = closestTform;
		    }
	    }
    }
} 
