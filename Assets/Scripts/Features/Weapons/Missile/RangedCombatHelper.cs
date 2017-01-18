// /** 
// * RangedCombatHelper.cs
// * Will Hart and Dylan Bailey
// * 20170117
// */

namespace Zenobit.Common.Helpers
{

	#region Dependencies

	using System;
	using UnityEngine;
	using Zenobit.Common.ZenECS;
	using Zenobit.Components;

	#endregion

	public static class RangedCombatHelper
	{
		public static void PerformAreaExplosion(LaunchedMissileComp lmc)
		{
			if (lmc.projectileInfo.ExplosionImpactRadius > 0)
			{
				try
				{
					var mdc = lmc.Owner.AddComponent<MissileAreaDamageComp>(ComponentTypes.MissileAreaDamageComp);
					mdc.AreaRadius = lmc.projectileInfo.ExplosionImpactRadius;
					mdc.ExplosionCenter = lmc.GetComponent<PositionComp>().transform.position;
				}
				catch (Exception)
				{
					ZenLogger.Log($"exception");
				}
				
			}
			else //just self destruct
			{
				lmc.Owner.AddComponent(ComponentTypes.DamageComp);
			}
		}

		public static Vector3 Predict(Vector3 sPos, Vector3 tPos, Vector3 tLastPos, float pSpeed)
		{
			// Target projectileInfo.ProjectileSpeed
			Vector3 tVel = (tPos - tLastPos) / Time.deltaTime;

			// Time to reach the target
			float flyTime = GetProjFlightTime(tPos - sPos, tVel, pSpeed);

			if (flyTime > 0)
				return tPos + flyTime * tVel;
			return tPos;
		}

		public static float GetProjFlightTime(Vector3 dist, Vector3 tVel, float pSpeed)
		{
			float a = Vector3.Dot(tVel, tVel) - pSpeed * pSpeed;
			float b = 2.0f * Vector3.Dot(tVel, dist);
			float c = Vector3.Dot(dist, dist);

			float det = b * b - 4 * a * c;

			if (det > 0)
				return 2 * c / (Mathf.Sqrt(det) - b);
			return -1;
		}
	}
}