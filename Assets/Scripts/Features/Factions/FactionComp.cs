namespace Zenobit.Components
{
	using Common.ZenECS;

	public class FactionComp : ComponentEcs
	{
		public FactionType EntityFaction;

		public override ComponentTypes ComponentType => ComponentTypes.FactionComp;
	}

	public enum FactionType
	{
		Player,
		Xenith, //bio mech AI
		Aermedian
	}
}