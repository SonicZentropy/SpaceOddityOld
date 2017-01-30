// /** 
// * PlayerMechAiBundle.cs
// * Dylan Bailey
// * 20161215
// */

namespace Zen.AI.Bundles
{
	#region Dependencies

	using System.Collections.Generic;
	using Actions;

	#endregion

	public class PlayerMechAiBundle : AiActionBundle
	{
		public PlayerMechAiBundle() : base(new List<AbstractAiAction>
		                                   {
			                                   //new PlayerMechNavigateToPositionAction()
											   
		                                   })
		{
		}
	}
}