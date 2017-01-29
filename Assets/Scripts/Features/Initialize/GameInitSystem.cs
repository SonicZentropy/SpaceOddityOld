namespace Zenobit.Systems
{
	using Common;
	using Components;
	using UnityEngine;
	using Zenobit.Common.ZenECS;

	public class GameInitSystem : AbstractEcsSystem
	{
		public override bool Init()
		{
			engine.AddSystem(new EntityTagInitSystem())
			      .AddSystem(new PooledEntityInitSystem());

			var cam = GameObject.FindGameObjectWithTag("MainCamera");
			Object.Destroy(cam);
			
			engine.CreateEntity(Res.Entities.Player);
			engine.CreateEntity(Res.Entities.Camera);
			engine.CreateEntity(Res.Entities.SectorGenerationMain);
			engine.CreateEntity(Res.Entities.GameSettings);

			var ets = engine.CreateEntity(Res.Entities.Enemy);
			ets.Wrapper.transform.position = new Vector3(0, 0, 10);
			
			//ets = engine.CreateEntity(Res.Entities.EnemyTestShip);
			//ets.Wrapper.transform.position = new Vector3(5, 0, 25);
			//
			//ets = engine.CreateEntity(Res.Entities.EnemyTestShip);
			//ets.Wrapper.transform.position = new Vector3(5, 5, 25);
			//
			//ets = engine.CreateEntity(Res.Entities.EnemyTestShip);
			//ets.Wrapper.transform.position = new Vector3(10, 10, 25);

			return false;
		}
	}
}