namespace Zen.Components
{
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

	//public enum MissileRetargetBehavior
	//{
	//	SelfDestruct,
	//	FlyStraight,
	//	FindNewTarget
	//}

	public enum MissileHomingMethod
	{
		None,
		Chase,
		AdvNav,
		Cluster,
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

		[HideInInspector]public float ShieldDamage;
		[HideInInspector]public float HullDamage;
		[HideInInspector]
		public float FlightSpeed;
		public float RotationSpeed;
		[HideInInspector]
		public Vector3 StartPosition;
		[HideInInspector]
		public Vector3 fireDirection;
		[HideInInspector] public PositionComp OwningActorPos;
		[HideInInspector]
		public WeaponComp FiringWeaponComp;
		public bool ShouldDisperse;
		public float DispersalTime;
		public float DispersalRandomTime;
		public float DetonationDistance;
		public float PIDNavigationalConstant;
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
		
		public MissileHomingMethod missileHomingMethod;
		//public MissileRetargetBehavior missileRetargetBehavior;
	}
}