namespace Zen.AI.Apex
{
	using System.Collections.Generic;
	using global::Apex.AI.Components;
	using Zen.Common.Extensions;

	public class AIClientMgr : Singleton<AIClientMgr>
	{
		public List<IUtilityAIClient> UtilityAIClients = new List<IUtilityAIClient>();
		//	IUtilityAIClient[] utilityAIClientArray = this.clients;
		//	for (int i = 0; i < (int)utilityAIClientArray.Length; i++)
		//{
		//	if (utilityAIClientArray[i] != null && this.aiConfigs[i].isActive)
		//	{
		//		utilityAIClientArray[i].Start();
		//	}
		//}
	}
}