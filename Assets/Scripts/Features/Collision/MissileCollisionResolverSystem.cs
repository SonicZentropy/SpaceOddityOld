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

	public class MissileCollisionResolverSystem : AbstractEcsSystem
	{
		private readonly Matcher missileMatcher = new Matcher(new List<ComponentTypes>
																 {
																	 ComponentTypes.LaunchedMissileComp,
																	 ComponentTypes.CollisionEnterComp
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
				foreach (var coll in cc.Other)
				{
					var oth = coll.gameObject.GetComponent<EntityWrapper>();
					if (oth != null)
					{
						if (!oth.Entity.HasComponent(ComponentTypes.DamageComp))
						{
							ZenLogger.Log($"Collision system adding collision to {oth.Entity.EntityName}");
							oth.Entity.AddComponent(ComponentTypes.DamageComp);
						}
					}
				}
			}
		}
	}
}