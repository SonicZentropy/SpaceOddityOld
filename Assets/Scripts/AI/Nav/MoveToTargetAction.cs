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
	using Zen.AI.Common;
	using Zen.Common.Extensions;
	using Zen.Components;

    public class MoveToTargetAction : ActionBase<ShipContext>
	{
		public override void Execute(ShipContext context)
		{
			if (context.targetComp.target != null)
			{
				Debug.Log($"moving to target action set");
				var AiShipComp = context.AiShipComp;
				AiShipComp.Navigation.SetNavState(EAINavState.APPROACH);
				AiShipComp.Navigation.HasReachedTarget = false;
				AiShipComp.Navigation.TargetPositionOffset = Vector3.zero;
				//AiShipComp.TargetTransform = context.targetComp.target;
			}
		}
	}
}
