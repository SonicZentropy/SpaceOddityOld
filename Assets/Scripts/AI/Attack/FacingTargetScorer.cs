/****************
* FacingTargetScorer.cs
* Dylan Bailey
* 2/4/2017
****************/

namespace Zen.AI.Apex.Scorers
{
	using global::AI.Core;
	using global::Apex.AI;
	using global::Apex.Serialization;
	using UnityEngine;
	using Zen.AI.Apex.Contexts;

	public sealed class FacingTargetScorer  : ZenContextualScorer<ShipContext>
    {
	    [ApexSerialization(defaultValue = 0.9f), FriendlyName("AngleDelta", "Difference in angles before firing. 1 = directly at, 0 = 90degree angle, -1 = 180 deg angle")]
	    public float AngleDelta = 0.9f;

	    public override float Score(ShipContext context)
	    {
		    //To see if they are in view, check the dot-product of the normalized
		    //collider-to-center direction and the normalized character-forward
		    //direction. The dot is 1.0 if the object is directly in front of the
		    //character (0 degrees), 0.0 if the object is directly left/right of
		    //the object (+-90 degrees), and -1.0 if the object is directly
		    //behind (+-180 degrees); it is the cosine of the angle between the
		    //object and forward.

		    var normforward = context.transform.forward.normalized;
		    var dir = (context.targetComp.target.position - context.transform.position).normalized;
		    var dot = Vector3.Dot(dir, normforward);

		    if (dot > AngleDelta)
		    {
			    Debug.Log("Starting attack");
			    return Success;
			    //objectSpotted = true;
		    }

		    Debug.Log("Not attackign");


		    return Failure;
	    }
    }
}
