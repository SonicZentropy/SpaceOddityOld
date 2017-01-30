namespace Zen.Components
{
	using Common.ZenECS;
	using UnityEngine;

	public class RendererComp : ComponentEcs
	{
		public Renderer renderer;

		public override ComponentTypes ComponentType => ComponentTypes.RendererComp;
	}
}