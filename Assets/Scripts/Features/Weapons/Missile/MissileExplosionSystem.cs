// /** 
// * MissileExplosionSystem.cs
// * Dylan Bailey
// * 20170115
// */

namespace Zen.Systems
{
	#region Dependencies

	using Common.ObjectPool;
	using Common.ZenECS;
	using Components;

	#endregion

	public class MissileExplosionSystem : AbstractEcsSystem
	{
		private readonly Matcher missileMatcher = new Matcher()
			.AllOf(ComponentTypes.LaunchedMissileComp, ComponentTypes.DamageComp);
	
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
				var ps = lmc.ExplosionPrefab.InstantiateFromPool(lmc.GetComponent<PositionComp>().transform.position);
				//ps.GetComponent<ParticleSystem>()?.ScaleByTransform(lmc.projectileInfo.ExplosionImpactRadius, true);
				ps.GetComponent<ParticleScalingController>()?.SetScale(lmc.projectileInfo.ExplosionImpactRadius, true);
				engine.DestroyEntity(matches[i]);
			}

		}
	}
}