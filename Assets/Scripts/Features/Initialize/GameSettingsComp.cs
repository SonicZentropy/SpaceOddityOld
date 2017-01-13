namespace Zenobit.Components
{
	using AdvancedInspector;
	using Common.ZenECS;
	using UnityEngine;

	public class GameSettingsComp : ComponentEcs
	{
		private float invertYAxis;
		[Inspect]public float InvertYAxis
		{
			get
			{
				if (!Mathf.Approximately(invertYAxis, -1))
					return 1;
				else return -1;
			}
			set { invertYAxis = value; }
		}

		public override ComponentTypes ComponentType => ComponentTypes.GameSettingsComp;
	}
}