namespace Zenobit.Components
{
	using Common.ZenECS;

	public class DamageComp : ComponentEcs
	{
		public float HealthDamage = 0;
		public float ShieldDamage = 0;

		public override ComponentTypes ComponentType => ComponentTypes.DamageComp;
	}
}