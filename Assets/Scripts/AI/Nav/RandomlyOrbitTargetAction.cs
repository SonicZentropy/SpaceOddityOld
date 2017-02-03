/****************
* RandomlyOrbitTargetAction.cs
* Dylan Bailey
* 2/3/2017
****************/

namespace Zen.AI.Apex.Actions
{
    #region Dependencies
	using global::Apex.AI;
	using UnityEngine;
	using Zen.AI.Apex.Contexts;
	using Zen.Common.Extensions;

	#endregion

	public class RandomlyOrbitTargetAction : ActionBase<ShipContext>
	{
		public override void Execute(ShipContext context)
		{
			if (context.targetComp.target != null)
			{
				Debug.Log($"Orbiting target action set");
				var navComp = context.navComp;
				navComp.ShouldMove = true;
				navComp.HasReachedTarget = false;
				//navComp.TargetTransform = context.targetComp.target;
				navComp.TargetPositionOffset = ZenUtils.Vec3Util.GetRandomVector(3, 3, 3);

			}
		}


	}
}
