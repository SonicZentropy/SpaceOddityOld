// /**
// * MissileCollisionSystem
// * Will Hart and Dylan Bailey
// * 1/14/2017
// */

namespace Zenobit.Systems
{
	#region Dependencies

	using System.Collections.Generic;
	using AdvancedInspector;
	using Common.ZenECS;
	using Components;

	#endregion

	public class MissileCollisionSystem : AbstractEcsSystem
	{
		private readonly Matcher missileMatcher = new Matcher()
			.AllOf(ComponentTypes.LaunchedMissileComp, ComponentTypes.CollisionEnterComp);
		                                                     
		public override bool Init()
		{
			return true;
		}

		public override void Update()
		{
			var matches = missileMatcher.GetMatches();
			foreach (var match in matches)
			{
				var colls = match.GetComponent<CollisionEnterComp>();
				if (colls.Other.Count <= 0)
					continue;

				var lmc = match.GetComponent<LaunchedMissileComp>();
				foreach (var o in colls.Other)
				{
					var ew = o.gameObject.GetComponent<EntityWrapper>();
					if (ew != null)
					{
						var dc = ew.Entity.GetOrAddComponent<DamageComp>(ComponentTypes.DamageComp);
						dc.damagePackets.Push(new DamagePacket(lmc.projectileInfo.HullDamage, lmc.projectileInfo.ShieldDamage));
					}
				}

			}
		}
	}
}