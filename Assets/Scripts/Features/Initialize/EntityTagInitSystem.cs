﻿// /** 
// * EntityTagInitSystem.cs
// * Will Hart and Dylan Bailey
// * 20170119
// */

namespace Zenobit.Systems
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
		EntityTagComp etc;

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
			if (e.TryGetComponent(ComponentTypes.EntityTagComp, out etc))
			{
				GameObject go = e.Wrapper.gameObject;
				go.AddEntityTags(etc.entityTags);
				go.AddTags(etc.tags);
			}
		}

		public void TestTags(GameObject go)
		{
			if (go.HasEntityTag(EntityTags.Aermedian))
				ZenLogger.Log($"Working");
			else ZenLogger.LogError($"FAIL");
			if (go.HasEntityTag(EntityTags.Player))
				ZenLogger.Log($"Working");
			else ZenLogger.LogError($"FAIL");
			if (!go.HasEntityTag(EntityTags.Ship))
				ZenLogger.Log($"Working");
			else ZenLogger.LogError($"FAIL");

			if (go.HasTag(Tags.NPC))
				ZenLogger.Log($"Working");
			else ZenLogger.LogError($"FAIL");
			if (go.HasTag(Tags.Station))
				ZenLogger.Log($"Working");
			else ZenLogger.LogError($"FAIL");
			if (go.HasTag(Tags.DisableDistanceTrigger))
				ZenLogger.Log($"Working");
			else ZenLogger.LogError($"FAIL");

			if (!go.HasTag(Tags.Player))
				ZenLogger.Log($"Working");
			else ZenLogger.LogError($"FAIL");
			if (!go.HasTag(Tags.Ally))
				ZenLogger.Log($"Working");
			else ZenLogger.LogError($"FAIL");
			if (!go.HasTag(Tags.Eclipse))
				ZenLogger.Log($"Working");
			else ZenLogger.LogError($"FAIL");
		}
	}
}