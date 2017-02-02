﻿// /** 
//  * ExtensionMethodsZen.cs
//  * Dylan Bailey
//  * 20161210
// */



#pragma warning disable 0414, 0219, 649, 169, 1570

namespace Zen.Common.Extensions
{
	#region Dependencies

	using System;
	using System.Collections.Generic;
	using UnityEngine;
	using ZenECS;

	#endregion

	public static class GameObjectExtensions
	{
		public static void AddTags(this GameObject go, params Tags[] tagsToAdd)
		{
			for (int i = 0; i < tagsToAdd.Length; i++)
				go.Tags |= (long) tagsToAdd[i];
		}

		public static void AddTagsFromFlags(this GameObject go, long tags)
		{
			go.Tags |= tags;
		}

		public static void RemoveTags(this GameObject go, params Tags[] tagsToRemove)
		{
			for (int i = 0; i < tagsToRemove.Length; i++)
				go.Tags &= ~((long) tagsToRemove[i]);
		}

		public static bool HasTag(this GameObject go, Tags tagToCheck)
		{
			return ((go.Tags & (long) tagToCheck) != 0);
		}

		public static bool HasNoTag(this GameObject go, Tags tagToCheck)
		{
			return ((go.Tags & (long) tagToCheck) == 0);
		}

		public static bool HasTagUpward(this GameObject go, Tags tagToCheck)
		{
			for (Transform g = go.transform; g != null; g = g.transform.parent)
			{
				if (g.gameObject.HasTag(tagToCheck))
					return true;
			}
			return false;
		}

		public static void ClearTags(this GameObject go)
		{
			go.Tags = 0;
		}

		////////////////////////////////////////
		public static void AddEntityTags(this GameObject go, params EntityTags[] tagsToAdd)
		{
			for (int i = 0; i < tagsToAdd.Length; i++)
				go.EntityTags |= (long) tagsToAdd[i];
		}

		public static void AddEntityTagsFromFlags(this GameObject go, long entityTags)
		{
			go.EntityTags |= entityTags;
		}

		public static void RemoveEntityTags(this GameObject go, params EntityTags[] tagsToRemove)
		{
			for (int i = 0; i < tagsToRemove.Length; i++)
				go.EntityTags &= ~((long) tagsToRemove[i]);
		}

		public static bool HasEntityTag(this GameObject go, EntityTags tagToCheck)
		{
			return ((go.EntityTags & (long) tagToCheck) != 0);
		}

		public static bool HasNoEntityTag(this GameObject go, EntityTags tagToCheck)
		{
			return ((go.EntityTags & (long) tagToCheck) == 0);
		}

		public static bool HasEntityTagUpward(this GameObject go, EntityTags tagToCheck)
		{
			for (Transform g = go.transform; g != null; g = g.transform.parent)
			{
				if (g.gameObject.HasEntityTag(tagToCheck))
					return true;
			}
			return false;
		}

		public static void ClearEntityTags(this GameObject go)
		{
			go.EntityTags = 0;
		}

		public static EntityWrapper GetEntityWrapper(this GameObject go)
		{
			var map = go.GetCustomMap();
			object ew;
			//Get existing entity wrapper from cache
			if (map.TryGetValue((int) ECustomGOMapTypes.EntityWrapper, out ew))
			{
				if (ew != null)
				{
					return (EntityWrapper) ew;
				}
			}

			//Get existing wrapper from component list and set cache equal to it, then return
			var ewrap = go.GetComponent<EntityWrapper>();
			map[(int) ECustomGOMapTypes.EntityWrapper] = ewrap;
			return ewrap;
		}

		public static Entity GetEntity(this GameObject go)
		{
			return go.GetEntityWrapper()?.Entity;
		}

		public static Dictionary<int, object> GetCustomMap(this GameObject go)
		{
			return go.CustomMap ?? (go.CustomMap = new Dictionary<int, object>());
		}
	}


	[Flags]
	public enum EntityTags
	{
		Player = 1 << 0,
		Ship = 1 << 1,
		Xenith = 1 << 2, //bio mech AI
		Aermedian = 1 << 3,
		IsEntity = 1 << 4,
		IsProjectile = 1 << 5,
		IsDamageable = 1 << 6
	}

	[Flags]
	public enum Tags
	{
		//None = 0,
		Disabled = 1 << 0
	}

	public enum ECustomGOMapTypes
	{
		Entity,
		EntityWrapper
	}
}