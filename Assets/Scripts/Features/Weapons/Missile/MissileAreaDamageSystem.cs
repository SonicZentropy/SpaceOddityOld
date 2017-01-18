// /** 
// * MissileAreaDamageSystem.cs
// * Will Hart and Dylan Bailey
// * 20170116
// */

namespace Zenobit.Systems
{
	#region Dependencies

	using Common.Extensions;
	using Common.ZenECS;
	using Components;
	using UnityEngine;

	#endregion

	public class MissileAreaDamageSystem : AbstractEcsSystem
	{
		int mask;
		public override bool Init()
		{
			mask = ZenUtils.LayerMaskFromIDs(SRLayerMask.npc, SRLayerMask.player, SRLayerMask.foreground);
			return true;
		}

		public override void Update()
		{
			var areas = engine.Get(ComponentTypes.MissileAreaDamageComp);
			for (int i = areas.Count - 1; i >= 0; i--)
			{
				//#HERE: testing rotations and area explosions causing other missiles to throw exceptions
				var hits = PerformAreaCast((MissileAreaDamageComp) areas[i]);
				foreach (var hit in hits)
				{
					var ew = (EntityWrapper) hit.gameObject.GetComponentDownThenUp<EntityWrapper>();
					if (ew != null)
					{
						//ZenLogger.Log($"adding dmg comp to {ew.Entity.EntityName}");
						var lmc = ((MissileAreaDamageComp) areas[i]).GetComponent<LaunchedMissileComp>();
						var dc = ew.Entity.GetOrAddComponent<DamageComp>(ComponentTypes.DamageComp);
						dc.damagePackets.Push(new DamagePacket(lmc.projectileInfo.HullDamage, lmc.projectileInfo.ShieldDamage));
						ZenUtils.PhysicsUtil.ApplyExplosionForce(ew, lmc.Owner.Wrapper.transform.position, lmc.projectileInfo.ExplosionImpactRadius);
					}
				}

				//Adding damage comp here so the missile itself explodes
				areas[i].Owner.AddComponent(ComponentTypes.DamageComp);
				areas[i].Owner.RemoveComponent(areas[i]);
			}
		}
		
		private Collider[] PerformAreaCast(MissileAreaDamageComp adc)
		{
			return Physics.OverlapSphere(adc.ExplosionCenter, adc.AreaRadius, mask);
		}
	}
}