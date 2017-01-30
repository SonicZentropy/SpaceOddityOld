namespace Zen.Components
{
	using Common.ZenECS;
	using UnityEngine;

	public class LaserComp : WeaponComp
	{
		public LaserInfoPacket laserInfoPacket = new LaserInfoPacket();

		public override ComponentTypes ComponentType => ComponentTypes.LaserComp;
	}

	public struct LaserInfoPacket
	{
		public LaserFireType laserFireType;
		Vector3 FireDirection
		{
			get { return fireDirection; }
			set { fireDirection = value.normalized; }
		}

		public float TimeToLive { get; set; }
		public float ProjectileSpeed { get; set; }
		public Vector3 StartPosition { get; set; }
		public Vector3 fireDirection { get; set; }
		public PositionComp OwningActorPos { get; set; }
		public WeaponComp FiringWeaponComp { get; set; }
	}

}