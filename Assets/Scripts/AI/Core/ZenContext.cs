/****************
* ZenContext.cs
* Dylan Bailey
* 2/1/2017
****************/

namespace Zen.AI.Apex.Contexts
{
	using global::Apex.AI;
	using Zen.Common.ZenECS;

	public class ZenContext  : IAIContext
	{
		public Entity entity { get; private set; }

		public ZenContext(Entity inEntity)
		{
			entity = inEntity;
		}
	}
}
