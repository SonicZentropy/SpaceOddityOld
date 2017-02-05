// /** 
//  * WeaponComp.cs
//  * Dylan Bailey
//  * 20161103
// */

namespace Zen.Components
{
	#region Dependencies

	using AdvancedInspector;
	using Common.Audio;
	using UnityEngine;
	using Zen.Common.ZenECS;

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

		[TextField(TextFieldType.Prefab, "Weapons/Launchers")] public string LauncherPrefab = "None";

		[HideInInspector]public GameObject WeaponGameObject;

		// Projectile Information
		[Inspect]
		public float ProjectileSpeed { get; set; }

		private bool ProjIsNotMissile()
		{
			return WeaponType != WeaponTypes.Missile;
		}

		private bool projectileIsEntity;

		[Inspect("ProjIsNotMissile")]
		public bool ProjectileIsEntity
		{
			get { return (projectileIsEntity || WeaponType == WeaponTypes.Missile); }
			set { projectileIsEntity = value; }
		}

		private bool ProjectileUsesPrefab() { return !ProjectileIsEntity; }

		[Inspect("ProjectileIsEntity")]
		[TextField(TextFieldType.Entity)]
		public string ProjectileEntity { get; set; }

		[Inspect("ProjectileUsesPrefab")] [TextField(TextFieldType.Prefab, "Weapons/Projectiles")] public string ProjectilePrefab = "None";

		[HideInInspector]
		public bool IsFitted { get; set; }

		[HideInInspector]
		public float NextAttackTime { get; set; }

		[Inspect(500)]
		public ShipFitting fittingAttached { get; set; }

		public override ComponentTypes ComponentType => ComponentTypes.WeaponComp;
		public override string Grouping => "Weapons";
	}

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
		Projectile,
		Particle
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