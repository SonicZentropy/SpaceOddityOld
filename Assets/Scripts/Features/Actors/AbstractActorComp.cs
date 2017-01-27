namespace Zenobit.Components
{
	using AdvancedInspector;
	using Common.ZenECS;

	public class AbstractActorComp : ComponentEcs
	{
		[TextField(TextFieldType.Entity)]
		[Inspect(90)]
		public string CurrentShip;

		public ShipComp ship => GetComponent<ShipConnectionComp>().Ship;

		public override ComponentTypes ComponentType => ComponentTypes.AbstractActorComp;
	}
}