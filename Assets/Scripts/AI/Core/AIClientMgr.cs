namespace Zen.AI.Apex
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using global::Apex.AI.Components;
	using UnityEngine;
	using Zen.Common.Extensions;
	using Zen.Common.ZenECS;

	public class AIClientMgr : Singleton<AIClientMgr>
	{
		private Dictionary<GameObject, List<IUtilityAIClient>> UtilityAiMappings = new Dictionary<GameObject, List<IUtilityAIClient>>();


		/// <summary>
		/// Returns proper AI client associated with a given GameObject
		/// </summary>
		/// <param name="host">Host game object for which to obtain the AI Client.</param>
		/// <param name="aiID">ID of the specific AI Client wanted</param>
		/// <returns>AI Client matching the ID or null if no match found</returns>
		public IUtilityAIClient GetLinkedAIClient(GameObject host, Guid aiID)
		{
			//Debug.Log($"in get linked ai client");

			if (!UtilityAiMappings.ContainsKey(host))
			{
				return null;
			}

			return UtilityAiMappings[host].FirstOrDefault(x => x.ai.id == aiID);
		}

		public void AddClient(IUtilityAIClient newClient, Entity owner)
		{
			var go = owner.Wrapper.gameObject;
			if (UtilityAiMappings.ContainsKey(go))
			{
				UtilityAiMappings[go].Add(newClient);
			}
			else
			{
				var list = new List<IUtilityAIClient>(1) {newClient};
				UtilityAiMappings.Add(go, list);
			}
		}

		public void RemoveClient(IUtilityAIClient clientToRemove, Entity owner)
		{
			UtilityAiMappings[owner.Wrapper.gameObject].Remove(clientToRemove);
		}
	}

}