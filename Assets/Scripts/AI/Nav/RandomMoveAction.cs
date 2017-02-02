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

	public class RandomMoveAction  : ActionBase<ShipContext>
    {
	    [ApexSerialization(defaultValue = 5f), FriendlyName("Move Radius", "The distance at which random positions are generated")]
	    public float moveRadius = 5f;

	    public override void Execute(ShipContext context)
	    {
		    var position = Random.onUnitSphere * moveRadius;
		    context.rbComp.AddForce(position);
		    Debug.Log($"Executing random move action");
	    }
    }
} 
