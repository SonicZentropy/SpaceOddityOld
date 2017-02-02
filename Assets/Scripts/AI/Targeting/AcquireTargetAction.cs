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

	public class AcquireTargetAction : ActionBase<ShipContext>
    {
	    public override void Execute(ShipContext context)
	    {
		    Debug.Log($"In acquire target execute");
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
				    //Debug.Log($"found self");
				    continue;
			    }
			    float dist = (selfpos - rb.transform.position).sqrMagnitude;
			    if (dist < closestdist) // new closest target found
			    {
				    closestdist = dist;
				    closestTform = rb.transform;
				    //Debug.Log($"Closest tform set to {rb.gameObject.name}");
			    }
		    }

		    if (closestTform != null)
		    {
			    Debug.Log($"Setting new target: {closestTform.name}");
			    context.targetComp.target = closestTform;
		    }
	    }
    }
} 
