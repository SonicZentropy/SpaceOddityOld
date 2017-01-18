// /** 
// * MissileCollisionResolverSystem.cs
// * Will Hart and Dylan Bailey
// * 20170115
// */

namespace Zenobit.Systems
{
	#region Dependencies

	using System.Collections.Generic;
	using Common.ZenECS;
	using Components;

	#endregion

	public class ShipDamageSystem : AbstractEcsSystem
	{
		private readonly Matcher shipMatcher = new Matcher()
			.AllOf(ComponentTypes.ShipComp, ComponentTypes.DamageComp);

		public override bool Init()
		{
			return true;
		}

		public override void Update()
		{
			var matches = shipMatcher.GetMatches();
			for (int i = 0; i < matches.Count; ++i)
			{
				var dam = matches[i].GetComponent<DamageComp>();
				for (int j = 0; j < dam.damagePackets.Count; j++)
				{
					var damageDone = dam.damagePackets.Pop();
					ZenLogger.Log($"Dealing damage to ship: {damageDone.HealthDamage}");
				}
			}
		}
	}
}