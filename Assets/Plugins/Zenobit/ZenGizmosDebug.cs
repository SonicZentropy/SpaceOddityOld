namespace Plugins.Zenobit
{
	using AdvancedInspector;
	using MissileBehaviours.Misc;
	using UnityEngine;
	using Zen.Common.Extensions;

	public class ZenGizmosDebug : MonoSingleton<ZenGizmosDebug>
	{
		public Vector3 targetPosition = new Vector3(-50000, 0, 0);

		private void OnDrawGizmos()
		{
			if (targetPosition.x.IsNotAlmost(-50000))
				Gizmos.DrawCube(targetPosition, Vector3.one * 3f);
		}

		[Inspect, Method()]
		public void Reset()
		{
			targetPosition = new Vector3(-50000, 0, 0);
		}
	}
}