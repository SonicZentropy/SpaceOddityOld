namespace Zenobit.Components
{
	using Common.ZenECS;

	public class ShipConnectionComp : ComponentEcs
	{
		private GuidLink<ShipComp> ship;

		public ShipComp Ship
		{
			get { return ship.Value; }
			set { ship = value; }
		}

		public override ComponentTypes ComponentType => ComponentTypes.ShipConnectionComp;
	}
}