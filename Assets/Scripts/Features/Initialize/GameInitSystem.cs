namespace Zen.Systems
{
	using Common;
	using UnityEngine;

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
			//engine.CreateEntity(Res.Entities.SectorGenerationMain);
			engine.CreateEntity(Res.Entities.GameSettings);

			//var ets = engine.CreateEntity(Res.Entities.Enemy);
			//ets.Wrapper.transform.position = new Vector3(0, 0, 10);

			//ZenLogger.LogGame("Completed init");

			return false;
		}


	}
}