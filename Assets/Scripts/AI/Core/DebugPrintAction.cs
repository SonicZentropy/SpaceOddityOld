/****************
* AcquireTargetAction.cs
* Dylan Bailey
* 2/1/2017
****************/

namespace Zen.AI.Apex.Actions
{
	using global::Apex.AI;
	using Zen.AI.Apex.Contexts;

	public class DebugPrintAction : ActionBase<ZenContext>
	{
		public override void Execute(ZenContext context)
		{
			UnityEngine.Debug.Log("In Debug print action execute");
		}
	}

	public class DebugPrintActionTwo : ActionBase<ZenContext>
	{
		public override void Execute(ZenContext context)
		{
			UnityEngine.Debug.Log("In Debug print action 2 execute");
		}
	}
}
