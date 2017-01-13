namespace Zenobit.Components
{
	using System.Collections.Generic;
	using AdvancedInspector;
	using Common.ZenECS;

	public class AvailableWeaponsComp : ComponentEcs
	{
		[CreateDerived]
		public List<WeaponComp> AvailableWeapons = new List<WeaponComp>();

		public override ComponentTypes ComponentType => ComponentTypes.AvailableWeaponsComp;
	}
}