namespace Zenobit.Components
{
	using AdvancedInspector;
	using Common.ZenECS;

	public class EntityTagComp : ComponentEcs
	{
		[Enum(true)]
		public EntityTags entityTags;
		[Enum(true)]
		public Tags tags;

		public override ComponentTypes ComponentType => ComponentTypes.EntityTagComp;
	}
}