// /** 
//  * RangedCombatSystems.cs
//  * Will Hart and Dylan Bailey
//  * 20161104
// */

namespace Zenobit.Systems
{
	#region Dependencies

	using Common.ZenECS;
	using Components;
	using System.Collections.Generic;
	using Common;
	using UnityEngine;

	#endregion

	public class RangedCombatSystem : AbstractEcsSystem
	{
		private readonly Matcher playerShipMatcher = new Matcher(new List<ComponentTypes>
		                                                         {
			                                                         ComponentTypes.CombatComp,
			                                                         ComponentTypes.PlayerShipComp
		                                                         });

		private readonly Matcher enemyShipMatcher = new Matcher(new List<ComponentTypes>
		                                                        {
			                                                        ComponentTypes.CombatComp
		                                                        }, new List<ComponentTypes>
		                                                           {
			                                                           ComponentTypes.PlayerShipComp
		                                                           });

		private CommandComp playerCommand;

		public override bool Init()
		{
			playerCommand = engine.FindEntity(Res.Entities.Player).GetComponent<CommandComp>();
			return true;
		}

		/// <summary>
		/// Loop through all living combat components and apply attacks
		/// </summary>
		public override void Update()
		{
			HandlePlayerShooting();
			HandleEnemyShooting();
		}

		private void HandlePlayerShooting()
		{
			//if (!_command.AttackPressed) return;

			var ents = playerShipMatcher.GetMatches();

			foreach (var ent in ents)
			{
				if (playerCommand.AttackPressed)
				{
					//ZenLogger.Log("Attack triggered");
					ExecuteAttack(ent);
				}
			}
		}

		private void HandleEnemyShooting()
		{
			var combatComps = enemyShipMatcher.GetMatches();

			foreach (var ent in combatComps)
			{
				ExecuteAttack(ent);
			}
		}

		private static void ExecuteAttack(Entity entity)
		{
			var combat = entity.GetComponent<CombatComp>();

			var weaponsToShoot = combat.GetComponent<ShipFittingsComp>().fittingList;

			foreach (var fitting in weaponsToShoot)
			{
				WeaponComp selectedWeapon = fitting.FittedWeapon;
				if (selectedWeapon == null || selectedWeapon.IsFitted == false ||
				    !UnitCanAttack(combat, selectedWeapon)) return;

				bool attacked = FireProjectile(selectedWeapon, entity.Wrapper.transform.forward);

				if (!attacked) return;

				PlayAttackNoise(combat, selectedWeapon);
				SetNextAttackTime(combat, selectedWeapon);
			}
		}

		private static bool FireProjectile(WeaponComp selectedWeapon, Vector3 direction)
		{
			var pc = selectedWeapon.GetComponent<ShipComp>().OwningActor.GetComponent<PositionComp>();
			if (selectedWeapon.WeaponType == WeaponTypes.Laser)
			{
				//need to set weapon comp owner properly i think?
				CreateLaserInfoPacket(selectedWeapon, pc, direction);
				LaserFireManager.Instance.Fire((LaserComp) selectedWeapon);
				return true;
			}
			else if (selectedWeapon.WeaponType == WeaponTypes.Missile)
			{
				CreateMissileInfoPacket(selectedWeapon, pc, direction);
				MissileFireManager.Instance.Fire((MissileComp) selectedWeapon);
				return true;
			}

			ZenLogger.LogError($"Selected weapon type not found");
			return false;
		}

		private static void CreateMissileInfoPacket(WeaponComp selectedWeapon, PositionComp pc, Vector3 direction)
		{
			MissileComp mc = (MissileComp) selectedWeapon;
			mc.missileInfoPacket.TimeToLive = selectedWeapon.AttackRange / selectedWeapon.ProjectileSpeed;
			mc.missileInfoPacket.fireDirection = direction;
			mc.missileInfoPacket.StartPosition = pc.transform.TransformPoint(selectedWeapon.fittingAttached.PositionOffset);
			mc.missileInfoPacket.OwningActorPos = pc;
			mc.missileInfoPacket.FiringWeaponComp = selectedWeapon;
			mc.missileInfoPacket.target = mc.GetComponent<TargetComp>().target;
		}

		private static void CreateLaserInfoPacket(WeaponComp selectedWeapon, PositionComp pc, Vector3 direction)
		{
			LaserComp lc = (LaserComp) selectedWeapon;
			lc.laserInfoPacket.TimeToLive = selectedWeapon.AttackRange / selectedWeapon.ProjectileSpeed;
			lc.laserInfoPacket.fireDirection = direction;
			lc.laserInfoPacket.StartPosition = pc.transform.TransformPoint(selectedWeapon.fittingAttached.PositionOffset);
			lc.laserInfoPacket.OwningActorPos = pc;
			lc.laserInfoPacket.FiringWeaponComp = selectedWeapon;
		}

		private static void SetNextAttackTime(CombatComp combat, WeaponComp selectedWeapon) { selectedWeapon.NextAttackTime = Time.time + selectedWeapon.AttackRate; }

		private static bool UnitCanAttack(CombatComp combat, WeaponComp selectedWeapon) { return selectedWeapon.NextAttackTime < Time.time; }

		private static void PlayAttackNoise(CombatComp attacker, WeaponComp weapon)
		{
			if (attacker.Owner.HasComponent(ComponentTypes.AudioSourceComp))
			{
				attacker.Owner.GetComponent<AudioSourceComp>().TriggerSfx(weapon.FiringSoundEffect);
			}
		}

		/// <summary>
		/// Make sure bullets don't immediately collide with their firing entity
		/// </summary>
		/// <param name="firer"></param>
		/// <param name="projectile"></param>
		private static void IgnoreProjectileCollisions(CombatComp firer, GameObject projectile)
		{
			var projectileCollider = projectile.GetComponentInChildren<Collider>();
			foreach (var col in firer.Owner.Wrapper.GetComponentsInChildren<Collider>())
			{
				Physics.IgnoreCollision(projectileCollider, col);
			}
		}
	}
}