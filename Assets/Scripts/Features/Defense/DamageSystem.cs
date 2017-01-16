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

	public class DamageSystem : AbstractEcsSystem
	{
		public override bool Init()
		{
			return true;
		}

		public override void Update()
		{
			var matches = engine.Get(ComponentTypes.DamageComp);
			for (int i = 0; i < matches.Count; ++i)
			{
				var dam = matches[i].GetComponent<DamageComp>();
				ZenLogger.Log($"Dmg comp found on {dam.Owner.EntityName}");
				dam.Owner.RemoveComponent(dam);
			}
		}
	}
}