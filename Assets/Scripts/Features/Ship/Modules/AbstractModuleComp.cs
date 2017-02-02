namespace Zen.Components
{
	using Common.ZenECS;

	public class AbstractModuleComp : ComponentEcs
	{
		public bool ModuleEquipped;
		public bool ModuleEnabled;	

		public override ComponentTypes ComponentType => ComponentTypes.AbstractModuleComp;
		public override string Grouping => "Modules";
	}
}