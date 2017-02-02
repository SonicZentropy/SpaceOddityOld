// /** 
// * MissileCollisionResolverSystem.cs
// * Dylan Bailey
// * 20170115
// */

namespace Zen.Systems
{
	#region Dependencies

	using Common.Extensions;
	using Common.Helpers;
	using Common.ZenECS;
	using Components;
	using UnityEngine;

	#endregion

	public class MissileCollisionResolverSystem : AbstractEcsSystem
	{
		private Matcher missileMatcher = new Matcher()
			.AllOf(ComponentTypes.LaunchedMissileComp, ComponentTypes.CollisionEnterComp)
			.NoneOf(ComponentTypes.MissileAreaDamageComp);

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
					Debug.Log($"Performing area explosion from collision");
					RangedCombatHelper.PerformAreaExplosion(lmc);
				}
				else
				{
					foreach (var coll in cc.Other)
					{
						//var oth = coll.gameObject.GetComponent<EntityWrapper>();
						var oth = coll.gameObject.GetEntityWrapper();
						if (oth != null)
						{
							if (!oth.Entity.HasComponent(ComponentTypes.DamageComp))
							{
								Debug.Log($"Missile Collision system adding collision to {oth.Entity.EntityName}");
								var dc = oth.Entity.GetOrAddComponent<DamageComp>(ComponentTypes.DamageComp);
								dc.damagePackets.Push(new DamagePacket(lmc.projectileInfo.HullDamage, lmc.projectileInfo.ShieldDamage));
								
								ZenUtils.PhysicsUtil.ApplyExplosionForce(oth, coll.contacts[0].point, lmc.projectileInfo.ExplosionImpactRadius);
							}
						}
					}

					if (cc.Other.Count > 0) // missile hit *something* so blow up
					{
						Debug.Log($"Missile Collision system adding damage from collision to missile");
						match.GetOrAddComponent<DamageComp>(ComponentTypes.DamageComp).damagePackets.Push(new DamagePacket());
					}
				}
			}
		}
	}
}