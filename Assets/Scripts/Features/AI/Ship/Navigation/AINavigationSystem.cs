// /**
//  * AINavigationSystem.cs
//  * Dylan Bailey
//  * 2/2/2017
// */

namespace Zen.Systems
{
	#region Dependencies

	using Common.ZenECS;
	using Plugins.Zenobit;
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
						Debug.Log($"No target so moving to {nc.TargetPositionOffset}");
						moveto = nc.TargetPositionOffset;
					}
					var pc = nav.GetComponent<PositionComp>().transform;
					var move = moveto - pc.position;
					//var rb = nav.GetComponent<RigidbodyComp>().rigidbody;
					//pc.Translate(move.normalized * 5f * Time.deltaTime, Space.World);
					//rb.AddForce(move.normalized * 5f * Time.deltaTime);

					pc.rotation =
						ZenMath.QuaternionUtil.SlerpLookAtTarget(pc, move, 5f /*Rotation speed*/ * Time.deltaTime);

					//getting close to target
					if ((pc.position - moveto).sqrMagnitude < 1.1f)
					{
						//ZenLogger.Log("Close to target");
						nc.HasReachedTarget = true;
					}
					else
					{
						ZenLogger.LogGame($"Travel distance: {(pc.position - moveto).sqrMagnitude}");
					}
				}
			}
		}

		public override void FixedUpdate()
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
						Debug.Log($"No target so moving to {nc.TargetPositionOffset}");
						moveto = nc.TargetPositionOffset;
					}
					var pc = nav.GetComponent<PositionComp>().transform;
					var move = moveto - pc.position;
					var rb = nav.GetComponent<RigidbodyComp>().rigidbody;
					//pc.Translate(move.normalized * 5f * Time.deltaTime, Space.World);
					rb.AddForce(move.normalized * 150f * Time.deltaTime);

					//ZenGizmosDebug.Instance.targetPosition = moveto;
				}
			}
		}
	}
}