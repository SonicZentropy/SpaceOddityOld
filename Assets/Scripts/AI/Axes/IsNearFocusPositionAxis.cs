// /** 
//  * IsNotNearHeroAxis.cs
//  * Will Hart
//  * 20161103
// */

namespace Zenobit.AI.Axes
{
	using Common.Debug;
	using Components;
	#region Dependencies

	using Zenobit.AI.Core;

	#endregion

	public class IsNearFocusPositionAxis : IAxis
	{
		private readonly float _nearDistance;

		public IsNearFocusPositionAxis(float nearDistance = 12)
		{
			_nearDistance = nearDistance;
		}

		public float Score(AiContext context)
		{
			//#HERE
			//PositionComp focus = context.GetComponent<PlayerMechComp>().FocusObject;
			//if (focus == null)
			//{
			//	ZenLogger.LogError("Focus position missing");
			//	return 0;
			//}
			//
			//var myPos = context.GetComponent<PositionComp>().Position;
			//var sqrMag = (focus.Position - myPos).sqrMagnitude;
			//return Functions.Quartic(_nearDistance * _nearDistance / sqrMag);
			return -1;
		}

		public string Name => "Is Near Focus";
		public string Description => "Returns 1 when the position is near the focus, and returns 0 as the focus moves outside the passed nearDistance";
	}
}