// /** 
//  * WeaponComp.cs
//  * Will Hart and Dylan Bailey 
//  * 20161103
// */

using Zenobit.Common.ZenECS;

namespace Zenobit.Components
{
	#region Dependencies

	using System;
	using AdvancedInspector;
	using Common.Audio;
	using UnityEngine;
	using Zenobit.Common.ZenECS;

	#endregion

	public class WeaponComp : ComponentEcs
	{
		// Damage Information
		[Inspect]
		public int ShieldDamage { get; set; }

		[Inspect]
		public int HullDamage { get; set; }

		// Weapon Information
		[Inspect]
		public float AttackRange { get; set; }

		[Inspect]
		public float AttackRate { get; set; }

		[Inspect]
		public WeaponTypes WeaponType { get; set; }

		[Inspect]
		public WeaponResolutionTypes ResolutionType { get; set; }

		[Inspect]
		public SfxTypes FiringSoundEffect { get; set; } = SfxTypes.GunFire;

		[TextField(TextFieldType.Prefab)] public string WeaponPrefab = "None";

		public GameObject WeaponGameObject;

		// Projectile Information
		[Inspect]
		public float ProjectileSpeed { get; set; }

		[Inspect]
		public bool ProjectileIsEntity { get; set; }

		private bool ProjectileUsesPrefab() { return !ProjectileIsEntity; }

		[Inspect("ProjectileIsEntity")]
		[TextField(TextFieldType.JSON)]
		public string ProjectileEntity { get; set; }

		[Inspect("ProjectileUsesPrefab")] [TextField(TextFieldType.Prefab)] public string ProjectilePrefab = "None";

		[HideInInspector]
		public bool IsFitted { get; set; }

		[HideInInspector]
		public float NextAttackTime { get; set; }

		[Inspect]
		public ShipFitting fittingAttached { get; set; }

		public override ComponentTypes ComponentType => ComponentTypes.WeaponComp;
	}

	/*public interface ProjectileInfoPacket
	{
		float TimeToLive { get; set; }
		float ProjectileSpeed { get; set; }
		Vector3 StartPosition { get; set; }
		Vector3 fireDirection { get; set; }

		PositionComp OwningActorPos { get; set; }
		WeaponComp FiringWeaponComp { get; set; }
	}*/

	public enum DamageMode
	{
		Single,
		Area
	}

	public enum AttackTypes
	{
		RangedSplash,
		RangedPoint
	}

	public enum WeaponTypes
	{
		Laser,
		Missile,
		Flak,
		Beam
	}

	public enum WeaponResolutionTypes
	{
		Raycast,
		Projectile
	}

	public enum LaserFireType
	{
		None,
		FastLineRenderer,
		ProjectileGO,
		NormalLineRenderer,
		Particle
	}
}