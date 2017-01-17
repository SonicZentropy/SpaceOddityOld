// /** 
// * MissileCollisionResolverSystem.cs
// * Will Hart and Dylan Bailey
// * 20170115
// */

namespace Zenobit.Systems
{
	#region Dependencies

	using System.Collections.Generic;
	using Common.Extensions;
	using Common.Helpers;
	using Common.ZenECS;
	using Components;
	using UnityEngine;

	#endregion

	public class MissileCollisionResolverSystem : AbstractEcsSystem
	{
		private readonly Matcher missileMatcher = new Matcher(
			new List<ComponentTypes>
			{
				ComponentTypes.LaunchedMissileComp,
				ComponentTypes.CollisionEnterComp
			} ,
			new List<ComponentTypes>
			{
				//ComponentTypes.DamageComp
				ComponentTypes.MissileAreaDamageComp
			});

		public override bool Init()
		{
			return true;
		}

		public override void Update()
		{
			var matches = missileMatcher.GetMatches();
			foreach (var match in matches)
			{
				var cc = match.GetComponent<CollisionEnterComp>();
				var lmc = match.GetComponent<LaunchedMissileComp>();

				if (lmc.projectileInfo.ExplosionImpactRadius > 0 && cc.Other.Count > 0)
				{
					ZenLogger.Log($"Performing area explosion from collision");
					RangedCombatHelper.PerformAreaExplosion(lmc);
				}
				else
				{
					foreach (var coll in cc.Other)
					{
						var oth = coll.gameObject.GetComponent<EntityWrapper>();
						if (oth != null)
						{
							if (!oth.Entity.HasComponent(ComponentTypes.DamageComp))
							{
								ZenLogger.Log($"Missile Collision system adding collision to {oth.Entity.EntityName}");
								var dc = oth.Entity.AddComponent<DamageComp>(ComponentTypes.DamageComp);
								dc.HealthDamage = lmc.projectileInfo.HullDamage;
								dc.ShieldDamage = lmc.projectileInfo.ShieldDamage;
								ZenUtils.PhysicsUtil.ApplyExplosionForce(oth, coll.contacts[0].point, lmc.projectileInfo.ExplosionImpactRadius);
							}
						}
					}

					if (cc.Other.Count > 0) // missile hit *something* so blow up
					{
						ZenLogger.Log($"Missile Collision system adding damage from collision to missile");
						match.AddComponent(ComponentTypes.DamageComp);
					}
				}
			}
		}
	}
}