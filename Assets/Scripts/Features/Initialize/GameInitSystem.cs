﻿namespace Zenobit.Systems
{
	using Common;
	using Components;
	using UnityEngine;
	using Zenobit.Common.ZenECS;

	public class GameInitSystem : AbstractEcsSystem
	{
		public override bool Init()
		{
			var cam = GameObject.FindGameObjectWithTag("MainCamera");
			Object.Destroy(cam);
			
			engine.CreateEntity(Res.Entities.Player);
			engine.CreateEntity(Res.Entities.Camera);
			engine.CreateEntity(Res.Entities.SectorGenerationMain);
			engine.CreateEntity(Res.Entities.GameSettings);

			var ets = engine.CreateEntity(Res.Entities.EnemyTestShip);
			ets.Wrapper.transform.position = new Vector3(5, 0, 75);

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