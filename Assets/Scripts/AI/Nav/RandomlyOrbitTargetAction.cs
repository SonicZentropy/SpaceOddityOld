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
	using ZR = Common.Extensions.ZenUtils.RandUtil;

	#endregion

	public class RandomlyOrbitTargetAction : ActionBase<ShipContext>
	{
		public override void Execute(ShipContext context)
		{
			//Debug.Log($"In randomly orbit action");
			if (context.targetComp.target != null)
			{
				var navComp = context.navComp;
				navComp.ShouldMove = true;
				navComp.HasReachedTarget = false;
				//navComp.TargetTransform = context.targetComp.target;
				//navComp.TargetPositionOffset = ZenUtils.Vec3Util.GetRandomVector(3, 3, 3);
				navComp.TargetPositionOffset = GetRandomOrbitPosition(15, 6);
				//Debug.Log($"Orbiting target action set to {context.targetComp.target.position + navComp.TargetPositionOffset}");
			}
		}

		private Vector3 GetRandomOrbitPosition(float maxOrbitRange, float minOrbitRange)
		{
			return new Vector3(Random.Range(minOrbitRange, maxOrbitRange) * ZR.GetRandomSign(),
			                    Random.Range(minOrbitRange, maxOrbitRange) * ZR.GetRandomSign(),
			                    Random.Range(minOrbitRange, maxOrbitRange) * ZR.GetRandomSign());
		}
	}
}