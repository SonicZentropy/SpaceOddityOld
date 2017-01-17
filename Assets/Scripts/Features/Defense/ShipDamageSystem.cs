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
		private readonly Matcher shipMatcher = new Matcher(new List<ComponentTypes>
																 {
																	 ComponentTypes.ShipComp,
																	 ComponentTypes.DamageComp
																 });
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
				ZenLogger.Log($"SHIP Dmg comp found on {dam.Owner.EntityName} for {dam.ShieldDamage} shield and {dam.HealthDamage} hull dmg");
				dam.Owner.RemoveComponent(dam);
			}
		}
	}
}