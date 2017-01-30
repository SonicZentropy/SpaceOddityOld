namespace Zen.Components
{
	using Common.ZenECS;
	using UnityEngine;

	public class MissileAreaDamageComp : ComponentEcs
	{
		public float AreaRadius;
		public Vector3 ExplosionCenter;

		public override ComponentTypes ComponentType => ComponentTypes.MissileAreaDamageComp;
	}
}