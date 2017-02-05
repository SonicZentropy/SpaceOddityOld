/****************
* RotateToTargetAction.cs
* Dylan Bailey
* 1/31/2017
****************/

namespace Zen.AI.Apex.Actions
{
	using global::Apex.AI;
	using global::Apex.Serialization;
	using UnityEngine;
	using Zen.AI.Apex.Contexts;
	using Zen.Common.Extensions;

	public class RotateToTargetAction : ActionBase<ShipContext>
	{
		[ApexSerialization(defaultValue = 5f), FriendlyName("Rotation Speed", "How quickly the ship turns toward target")]
		public float rotationSpeed = 5f;

		public override void Execute(ShipContext context)
		{
			//Debug.Log($"rotating target action");
			// #TODO: switch to assert
			if (context.targetComp.target == null)
			{
				Debug.Log($"No target in RotateToTargetAction");
				return;
			}
			//Debug.Log($"Looking at {context.targetComp.target}");
			//context.transform.LookAt(context.targetComp.target);
			var targDir = context.targetComp.target.position - context.transform.position;
			context.transform.rotation =
				ZenMath.QuaternionUtil.SlerpLookAtTarget(context.transform, context.targetComp.target.position,
				                                         rotationSpeed * Time.deltaTime);

			//context.transform.rotation = Quaternion.Slerp(context.transform.rotation, context.targetComp.target)
		}
	}
}