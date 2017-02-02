// /** 
// * EntityTagInitSystem.cs
// * Dylan Bailey
// * 20170119
// */

namespace Zen.Systems
{
	#region Dependencies

	using System;
	using Common.Extensions;
	using Common.ZenECS;
	using Components;
	using UnityEngine;

	#endregion

	public class EntityTagInitSystem : AbstractEcsSystem, IDisposable
	{
		UnityPrefabComp etc;

		public override bool Init()
		{
			engine.OnEntityAdded += InitializeEntity;
			return false;
		}

		public void Dispose()
		{
			engine.OnEntityAdded -= InitializeEntity;
		}

		public void InitializeEntity(Entity e)
		{
			if (e.TryGetComponent(ComponentTypes.UnityPrefabComp, out etc))
			{
				GameObject go = e.Wrapper.gameObject;
				go.AddEntityTags(etc.entityTags);
				go.AddTags(etc.tags);
			}
		}

		public void TestTags(GameObject go)
		{
			if (go.HasEntityTag(EntityTags.Aermedian))
				Debug.Log($"Working");
			else Debug.LogError($"FAIL");
			if (go.HasEntityTag(EntityTags.Player))
				Debug.Log($"Working");
			else Debug.LogError($"FAIL");
			if (!go.HasEntityTag(EntityTags.Ship))
				Debug.Log($"Working");
			else Debug.LogError($"FAIL");
		}
	}
}