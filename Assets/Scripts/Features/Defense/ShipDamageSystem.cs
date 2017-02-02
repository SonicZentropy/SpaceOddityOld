// /** 
// * MissileCollisionResolverSystem.cs
// * Dylan Bailey
// * 20170115
// */

namespace Zen.Systems
{
	#region Dependencies

	using Common.ZenECS;
	using Components;
	using UnityEngine;

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
					//Debug.Log($"Dealing damage to ship: {damageDone.HullDamage}");
					ApplyDamage(ref damageDone, matches[i]);
				}
			}
		}

		public void ApplyDamage(ref DamagePacket dp, Entity e)
		{
			var shield = e.GetComponent<ShieldComp>();
			var hull   = e.GetComponent<HullComp>();

			if (shield.CurrentShieldEnergy > 0)
			{
				shield.CurrentShieldEnergy -= dp.ShieldDamage;
				dp.HullDamage -= dp.ShieldDamage;
				e.GetComponent<ShieldComp>().shieldTrigger.TriggerShield();
			}

			if (shield.CurrentShieldEnergy <= 0)
			{
				dp.HullDamage += Mathf.Abs(shield.CurrentShieldEnergy) / dp.ShieldDamage; // Add hull dmg ratio
				shield.CurrentShieldEnergy = 0;
			}
			else
			{
				return; //Don't deal hull damage since shields absorbed it all
			}

			hull.CurrentHull -= dp.HullDamage;
		}
	}
}