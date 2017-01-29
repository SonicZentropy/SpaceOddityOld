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
		public float HullDamage;
		public float ShieldDamage;

		public DamagePacket(float hullDamage, float _shieldDamage)
		{
			HullDamage = hullDamage;
			ShieldDamage = _shieldDamage;
		}
	}
}