// /**
//  * AIInitSystem.cs
//  * Dylan Bailey
//  * 2/2/2017
// */

namespace Zen.Systems
{
	#region Dependencies

	using Apex.AI;
	using Common.ZenECS;
	using Zen.AI.Apex;

	#endregion

	public class AIInitSystem : AbstractEcsSystem
	{
		public override bool Init()
		{
			//To override the default behaviour, you need to assign a method (delegate) to AIManager.GetAIClient,
			//the signature of which is (GameObject host, Guid aiID) with a return value of IUtilityAIClient.
			//This method must match return the AI client for the specified AI for the host game object.

			AIManager.GetAIClient += AIClientMgr.Instance.GetLinkedAIClient;

			return false;
		}
	}
}