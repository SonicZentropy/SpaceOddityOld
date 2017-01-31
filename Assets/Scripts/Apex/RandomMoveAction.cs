/****************
* RandomMoveAction.cs
* Dylan Bailey
* 1/31/2017
****************/

namespace Zen.AI.Apex.Actions
{
	using global::Apex.AI;
	using global::Apex.Serialization;
	using UnityEngine;
	using Zen.AI.Apex.Contexts;

	public class RandomMoveAction  : ActionBase<BasicNavigationContext>
    {
	    [ApexSerialization(defaultValue = 5f), FriendlyName("Move Radius", "The distance at which random positions are generated")]
	    public float moveRadius = 5f;

	    [ApexSerialization(defaultValue = 1f), FriendlyName("Sampling Threshold", "Random point sampling threshold")]
	    public float samplingThreshold = 1f;

	    public override void Execute(BasicNavigationContext context)
	    {
		    var position = Random.onUnitSphere * moveRadius;
		    context.rbComp.AddForce(position);
		    ZenLogger.Log($"Executing random move action");
	    }
    }
} 
