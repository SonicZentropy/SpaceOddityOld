/****************
* RotateToTargetAction.cs
* Dylan Bailey
* 1/31/2017
****************/

namespace Zen.AI.Apex.Actions
{
	using System.Collections.Generic;
	using global::Apex.AI;
	using UniRx.Examples;
	using UnityEngine;
	using Zen.AI.Apex.Contexts;

	public class RotateToTargetAction : ActionBase<TargetContext>
	{
		public override void Execute(TargetContext context)
		{
			ZenLogger.Log($"rotating target action");
			// #TODO: switch to assert
			if (context.targetComp.target == null)
			{
				ZenLogger.Log($"No target in RotateToTargetAction");
				return;
			}
			ZenLogger.Log($"Looking at {context.targetComp.target}");
			context.transform.LookAt(context.targetComp.target);
		}
	}
}