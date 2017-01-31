namespace Zen.Components
{
	using Common.ZenECS;

	public class FactionComp : ComponentEcs
	{
		public EFactionType EntityFaction;

		public override ComponentTypes ComponentType => ComponentTypes.FactionComp;
	}

	public enum EFactionType
	{
		Player,
		Xenith, //bio mech AI
		Aermedian
	}
}