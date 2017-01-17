// /** 
// * MissileExplosionSystem.cs
// * Will Hart and Dylan Bailey
// * 20170115
// */

namespace Zenobit.Systems
{
	#region Dependencies

	using System.Collections.Generic;
	using Common.ObjectPool;
	using Common.ZenECS;
	using Components;

	#endregion

	public class MissileExplosionSystem : AbstractEcsSystem
	{
		private readonly Matcher missileMatcher = new Matcher(new List<ComponentTypes>
																 {
																	 ComponentTypes.LaunchedMissileComp,
																	 ComponentTypes.DamageComp
																 });
		public override bool Init()
		{
			return true;
		}

		public override void Update()
		{
			var matches = missileMatcher.GetMatches();

			for (int i = matches.Count -1; i >= 0; i--)
			{
				var lmc = matches[i].GetComponent<LaunchedMissileComp>();
				lmc.ExplosionPrefab.InstantiateFromPool(lmc.GetComponent<PositionComp>().transform.position);
				engine.DestroyEntity(matches[i]);
			}

		}
	}
}