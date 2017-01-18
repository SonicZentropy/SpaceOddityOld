namespace Zenobit.Components
{
	using System.Collections.Generic;
	using Common.ZenECS;

	public class DamageComp : ComponentEcs
	{
		public Stack<DamagePacket> damagePackets = new Stack<DamagePacket>(10);

		public override ComponentTypes ComponentType => ComponentTypes.DamageComp;
	}

	public struct DamagePacket
	{
		public float HealthDamage;
		public float ShieldDamage;

		public DamagePacket(float _healthDamage, float _shieldDamage)
		{
			HealthDamage = _healthDamage;
			ShieldDamage = _shieldDamage;
		}
	}
}