namespace Zen.Systems
{
	using Common;
	using UnityEngine;
	using Zen.Components;

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
            ets = engine.CreateEntity(Res.Entities.Enemy);
            ets.Wrapper.transform.position = new Vector3(10, 0, 10);
            ets = engine.CreateEntity(Res.Entities.Enemy);
            ets.Wrapper.transform.position = new Vector3(0, 10, 10);

            InitRadar();
			return false;
		}

	    private void InitRadar()
	    {
	        var mgr = GameObject.Find("RadarMgr").GetComponent<FX_3DRadar_Mgr>();
	        mgr.Player = engine.FindEntity(Res.Entities.Player).GetComponent<PositionComp>().transform;
	        mgr.PlayerCameraC = engine.FindEntity(Res.Entities.Camera).GetComponent<CameraComp>().MainCamera;
	    }


	}
}