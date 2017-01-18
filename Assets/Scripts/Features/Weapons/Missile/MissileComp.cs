namespace Zenobit.Components
{
	using AdvancedInspector;
	using Common.ZenECS;
	using UnityEngine;

	public class MissileComp : WeaponComp
	{
		public MissileInfoPacket missileInfoPacket = new MissileInfoPacket();
		public uint numberSwarmMissiles;

		public override ComponentTypes ComponentType => ComponentTypes.MissileComp;
	}

	public enum MissileFireType
	{
		Dumbfire,
		Homing,
		Swarm
	}

	public enum MissileRetargetBehavior
	{
		SelfDestruct,
		FlyStraight,
		FindNewTarget
	}

	public enum MissileHomingMethod
	{
		None,
		Chase,
		ProNav,
		Swarm,
		Swirl,
		PID
	}

	public struct MissileInfoPacket
	{
		[Tooltip("Set to -1 in order to calculate TLL automatically based on speed and launcher range")]
		public float TimeToLive;
		[Tooltip("0 for no area effect")]
		public float ExplosionImpactRadius;

		public float ExplosionForce;

		[ReadOnly]public float ShieldDamage;
		[ReadOnly]public float HullDamage;
		public float ProjectileSpeed;
		public float RotationSpeed;
		public Vector3 StartPosition;
		[HideInInspector]
		public Vector3 fireDirection;
		[HideInInspector] public PositionComp OwningActorPos;
		[HideInInspector]
		public WeaponComp FiringWeaponComp;

		//MissileInfo
		public MissileFireType missileFireType;
		[HideInInspector]
		public Vector3 FireDirection
		{
			get { return fireDirection; }
			set { fireDirection = value.normalized; }
		}

		[HideInInspector]
		public Transform target;

		public bool ShouldDisperse;
		public float DispersalTime;
		public float DispersalRandomTime;
		public float DetonationDistance;
		public float PIDNavigationalConstant;
		public MissileHomingMethod missileHomingMethod;
		public MissileRetargetBehavior missileRetargetBehavior;
	}
}