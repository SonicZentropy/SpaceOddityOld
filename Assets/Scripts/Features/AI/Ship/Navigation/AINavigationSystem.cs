// /**
//  * AINavigationSystem.cs
//  * Dylan Bailey
//  * 2/2/2017
// */

namespace Zen.Systems
{
	#region Dependencies

	using Common.ZenECS;
	using UnityEngine;
	using Zen.Common.Extensions;
	using Zen.Components;

	#endregion

	public class AINavigationSystem : AbstractEcsSystem
	{
		public override bool Init()
		{
			return true;
		}

		public override void Update()
		{
			foreach (var nav in engine.Get(ComponentTypes.AINavigationComp))
			{
				var nc = (AINavigationComp) nav;
				var targ = nav.GetComponent<TargetComp>().target;
				if (nc.ShouldMove)
				{
					Vector3 moveto;
					if (targ != null)
					{
						moveto = targ.position + nc.TargetPositionOffset;
					}
					else
					{
						moveto = nc.TargetPositionOffset;
					}
					var pc = nav.GetComponent<PositionComp>().transform;
					var move = moveto - pc.position;

					pc.Translate(move.normalized * 5f * Time.deltaTime);

					pc.rotation =
						ZenUtils.QuaternionUtil.SlerpLookAtTarget(pc, moveto, 5f /*Rotatoin speed*/ * Time.deltaTime);

					//getting close to target
					if ((pc.position - moveto).sqrMagnitude < 2f)
					{
						ZenLogger.LogGame("Close to target", true);
						nc.HasReachedTarget = true;
					}
					else
					{
						ZenLogger.LogGame($"Travel distance: {(pc.position - moveto).sqrMagnitude}");
					}
				}
			}
		}
	}
}