﻿namespace Zenobit.Components
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

	public enum MissileRetargetBehavior
	{
		SelfDestruct,
		FlyStraight,
		FindNewTarget
	}

	public enum MissileHomingMethod
	{
		None,
		ProNav,
		Swarm,
		Swirl,
		PID
	}

	public struct MissileInfoPacket 
	{
		//ProjectileInfo
		public float TimeToLive;
		public float ProjectileSpeed;
		public Vector3 StartPosition;
		public Vector3 fireDirection;
		public PositionComp OwningActorPos;
		public WeaponComp FiringWeaponComp;

		//MissileInfo
		public MissileFireType missileFireType;
		Vector3 FireDirection
		{
			get { return fireDirection; }
			set { fireDirection = value.normalized; }
		}

		public Transform target;

		public bool ShouldDisperse;
		public float DispersalTime;
		public float DetonationDistance;
		public float PIDNavigationalConstant;
		public MissileHomingMethod missileHomingMethod;
		public MissileRetargetBehavior missileRetargetBehavior;
	}
}