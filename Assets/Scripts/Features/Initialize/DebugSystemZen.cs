// /**
// * DebugSystemZen.cs
// * Will Hart and Dylan Bailey
// * 20161208
// */

namespace Zenobit.Systems
{
	#region Dependencies

	using System.Collections.Generic;
	using Common.ZenECS;
	using Components;

	#endregion

	public class DebugSystemZen : AbstractEcsSystem
	{
		private List<Entity> EntityList = new List<Entity>();
		private int _totalCollisions;

		public override bool Init()
		{
			InitSystems();

			return true;
		}

		private void InitSystems()
		{
			engine.AddSystem(new PlayerInitSystem())
				  //.AddSystem(new SectorGenerationSystem())
				  .AddSystem(new ShipInitSystem())
				  .AddSystem(new PlayerInputSystem())

				  .AddSystem(new PlayerMovementSystem())
				  .AddSystem(new PlayerTargetingSystem())
				  .AddSystem(new TacticalAiPlanningSystem())
				  .AddSystem(new TacticalAiMovementSystem())
				  .AddSystem(new PositionUpdateSystem())
				  .AddSystem(new CameraControlSystem())

				  .AddSystem(new RangedCombatSystem())
				  
				  .AddSystem(new MissileFlightSystem())
				  .AddSystem(new MissileCollisionResolverSystem())
				  .AddSystem(new MissileAreaDamageSystem())
				  .AddSystem(new MissileExplosionSystem())
				  .AddSystem(new ShipDamageSystem())
				  .AddSystem(new DeathRemovalSystem())
				  
				  .AddSystem(new InertialDamperSystem())
				  .AddSystem(new CollisionCleanupSystem())

				  //Reactives
				  ;

			engine.InitAfterECS();
		}

		public override void Update()
		{
			var collenter = engine.Get(ComponentTypes.CollisionEnterComp);
			foreach (CollisionEnterComp coll in collenter)
			{
				if (coll.Other.Count > 0)
					//ZenLogger.Log($"System found {coll.Owner} colliding with {coll.other}");
					_totalCollisions++;
			}

			var collexit = engine.Get(ComponentTypes.CollisionExitComp);
			foreach (CollisionExitComp coll in collexit)
			{
				if (coll.Other.Count > 0)
					//ZenLogger.Log($"System found {coll.Owner} UNcolliding with {coll.other}");
					_totalCollisions++;
			}
		}
	}
}