/****************
* MoveToTargetAction.cs
* Dylan Bailey
* 2/2/2017
****************/

namespace Zen.AI.Apex.Actions
{
	using global::Apex.AI;
	using UnityEngine;
	using Zen.AI.Apex.Contexts;
	using Zen.Common.Extensions;

	public class MoveToTargetAction : ActionBase<ShipContext>
	{
		public override void Execute(ShipContext context)
		{
			if (context.targetComp.target != null)
			{
				Debug.Log($"moving to target action set");
				var navComp = context.navComp;
				navComp.ShouldMove = true;
				navComp.HasReachedTarget = false;
				//navComp.TargetTransform = context.targetComp.target;
				navComp.TargetPositionOffset = Vector3.zero;
			}
		}
	}
}
