// /**
//  * ShipAIContextInitSystem.cs
//  * Dylan Bailey
//  * 2/1/2017
// */

namespace Zen.Systems
{
	#region Dependencies

	using System;
	using global::AI;
	using UnityEngine;
	using Zen.AI.Apex;
	using Zen.AI.Apex.Contexts;
	using Zen.Common.ZenECS;
	using Zen.Components;

	#endregion

	public class ShipAIContextInitSystem : AbstractEcsSystem, IDisposable
	{
		public override bool Init()
		{
			var AIComps = engine.Get(ComponentTypes.ShipContextProviderComp);
			foreach (var fcp in AIComps)
			{
				InitContext((ShipContextProviderComp) fcp);
			}

			engine.OnEntityAdded += InitFromEntity;
			return false;
		}

		private void InitFromEntity(Entity entity)
		{
			if (entity.HasComponent(ComponentTypes.ShipContextProviderComp))
			{
				InitContext(entity.GetComponent<ShipContextProviderComp>());
			}
		}

		private void InitContext(ShipContextProviderComp fcp)
		{
			Debug.Log($"Init context");
			fcp.context = new ShipContext(fcp.Owner);
			var client = new ZenAIClient(Apex.AI.AINameMap.FighterAI, fcp);
			AIClientMgr.Instance.AddClient(client, fcp.Owner);
			client.Start();
		}

		public void Dispose()
		{
			engine.OnEntityAdded -= InitFromEntity;
		}
	}
}