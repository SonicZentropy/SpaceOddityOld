// /**
// * MissileBehaviorSystem.cs
// * Will Hart and Dylan Bailey
// * 20170111
// */

namespace Zenobit.Systems
{
	#region Dependencies

	using System.Linq;
	using Common.Helpers;
	using Common.ObjectPool;
	using Common.ZenECS;
	using Components;
	using TypeSafe;
	using UnityEngine;

	#endregion

	public class MissileBehaviorSystem : AbstractEcsSystem
	{
		public override bool Init()
		{
			return true;
		}

		public override void Update()
		{
			var launchedMissiles = engine.Get<LaunchedMissileComp>(ComponentTypes.LaunchedMissileComp).ToList();
			for (int i = 0; i < launchedMissiles.Count(); ++i)
			{
				MissileUpdate(launchedMissiles[i]);
			}
		}

		public void MissileUpdate(LaunchedMissileComp lmc)
		{
			//projectileInfo.TimeToLive handling
			lmc.TimeAlive += Time.deltaTime;

			// If something was hit
			//if (lmc.IsHit)
			//{
			//	// Execute once
			//	if (!lmc.isFXSpawned)
			//	{
			//		TriggerExplosion(lmc);
			//		lmc.isFXSpawned = true;
			//	}
			//
			//	// Despawn current missile
			//	if (lmc.despawnDelay <= 0 || ((lmc.despawnDelay > 0) && (lmc.TimeAlive >= lmc.despawnDelay)))
			//	{
			//		OnMissileDestroy(lmc);
			//	}
			//}
			// No collision occurred yet
			if (!CheckPastTimeToLive(lmc))
			{
				// Navigate
				if (lmc.projectileInfo.missileFireType == MissileFireType.Dumbfire)
				{
					UpdateDumbfireMethod(lmc);
				}
				else if (lmc.projectileInfo.target != null)
				{
					if (lmc.projectileInfo.ShouldDisperse)
						UpdateDispersalMethod(lmc);
					else
					{
						switch (lmc.projectileInfo.missileHomingMethod)
						{
							case MissileHomingMethod.Chase:
								UpdateChaseMethod(lmc);
								break;
							case MissileHomingMethod.PID:
								UpdatePIDMethod(lmc);
								break;
							case MissileHomingMethod.ProNav:
								UpdateAdvancedNavMethod(lmc);
								break;
							case MissileHomingMethod.Swarm:
								UpdateSwarmMethod(lmc);
								break;
							case MissileHomingMethod.Swirl:
								UpdateRandomSwirlMethod(lmc);
								break;
							default:
								ZenLogger.LogError("Missile homing method not found");
								break;
						}
					}
				}

				HandleDetonationDistance(lmc);
			}
		}

		private void UpdateDumbfireMethod(LaunchedMissileComp lmc)
		{
			lmc.transform.rotation = Quaternion.LookRotation(lmc.projectileInfo.fireDirection);

			lmc.transform.position += lmc.transform.forward * Time.deltaTime * lmc.projectileInfo.ProjectileSpeed;
		}

		private void UpdateChaseMethod(LaunchedMissileComp lmc)
		{
			lmc.transform.rotation = Quaternion.Lerp(
			                                         lmc.transform.rotation,
			                                         Quaternion.LookRotation(lmc.projectileInfo.target.position - lmc.transform.position),
			                                         Time.deltaTime * lmc.projectileInfo.RotationSpeed);
			lmc.transform.position += lmc.transform.forward * Time.deltaTime * lmc.projectileInfo.ProjectileSpeed;
		}

		private void UpdatePIDMethod(LaunchedMissileComp lmc)
		{
			Vector3 hitPos = RangedCombatHelper.Predict(lmc.transform.position, lmc.projectileInfo.target.position, lmc.targetLastPos, lmc.projectileInfo.ProjectileSpeed);
			lmc.targetLastPos = lmc.projectileInfo.target.position;

			lmc.transform.rotation = Quaternion.Lerp(
			                                         lmc.transform.rotation,
			                                         Quaternion.LookRotation(hitPos - lmc.transform.position), Time.deltaTime * lmc.projectileInfo.RotationSpeed);

			lmc.transform.position += lmc.transform.forward * Time.deltaTime * lmc.projectileInfo.ProjectileSpeed;
		}

		private void UpdateAdvancedNavMethod(LaunchedMissileComp lmc)
		{
			lmc.previousLos = lmc.los;
			lmc.los = lmc.projectileInfo.target.position - lmc.transform.position;
			lmc.losDelta = lmc.los - lmc.previousLos;

			lmc.losDelta = lmc.losDelta - Vector3.Project(lmc.losDelta, lmc.los);

			if (lmc.UseAdvancedMode)
				lmc.desiredRotation = (Time.deltaTime * lmc.los) + (lmc.losDelta * lmc.navigationalConstant) + (Time.deltaTime * lmc.desiredRotation * lmc.navigationalConstant * 0.5f); // Augmented version of proportional navigation.
			else
				lmc.desiredRotation = (Time.deltaTime * lmc.los) + (lmc.losDelta * lmc.navigationalConstant); // Plain proportional navigation.

			// Use the Rotate function to consider the rotation rate of the missile.
			lmc.transform.rotation = Quaternion.RotateTowards(lmc.transform.rotation, Quaternion.LookRotation(lmc.desiredRotation, lmc.transform.up), Time.deltaTime * lmc.projectileInfo.RotationSpeed);

			lmc.transform.position += lmc.transform.forward * Time.deltaTime * lmc.projectileInfo.ProjectileSpeed;
		}

		private void UpdateDispersalMethod(LaunchedMissileComp lmc)
		{
			if (lmc.TimeAlive < lmc.dispersalTime)
			{
				lmc.transform.Rotate(lmc.transform.up, Random.Range(-lmc.projectileInfo.RotationSpeed, lmc.projectileInfo.RotationSpeed) * Time.deltaTime, Space.World);
				lmc.transform.Translate(lmc.transform.forward * lmc.projectileInfo.ProjectileSpeed * Time.deltaTime);
			}
			else
			{
				lmc.projectileInfo.ShouldDisperse = false;
			}
		}

		//http://answers.unity3d.com/questions/698009/swarm-missile-adding-random-movement-to-a-homing-m.html
		private void UpdateSwarmMethod(LaunchedMissileComp lmc)
		{
			//defines a new random float for x,y,z every frame only if the time specfied has elapsed.  Creates random changes in x, y, z, variables at random times.
			if (Time.time > lmc.timeRandom)
			{
				lmc.xRandom = Random.Range(-lmc.swarmRandomRange, lmc.swarmRandomRange);
				lmc.yRandom = Random.Range(-lmc.swarmRandomRange, lmc.swarmRandomRange);
				lmc.zRandom = Random.Range(0, lmc.swarmRandomRange);
				lmc.timeRandom = Time.time + Random.Range(0.01f, 2);
			}

			//Perform swarming slide behavior
			if ((lmc.projectileInfo.target.position - lmc.transform.position).magnitude > 10)
			{
				lmc.transform.Translate(lmc.xRandom, lmc.yRandom, lmc.zRandom, lmc.projectileInfo.target);
			}

			//keeps the missile looking at its target
			lmc.transform.LookAt(lmc.projectileInfo.target.position);
			lmc.transform.position += lmc.transform.forward * Time.deltaTime * lmc.projectileInfo.ProjectileSpeed;
		}

		//https://www.reddit.com/r/Unity3D/comments/3xw8uc/cluster_homing_missiles_c_code/
		private void UpdateRandomSwirlMethod(LaunchedMissileComp lmc)
		{
			if (lmc.projectileInfo.target != null)
			{
				if (lmc.TimeAlive > 1)
				{
					if ((lmc.projectileInfo.target.position - lmc.transform.position).magnitude > 50)
					{
						lmc.randomSwirlOffset = 100.0f;
						lmc.projectileInfo.RotationSpeed = 90.0f;
					}
					else
					{
						lmc.randomSwirlOffset = 25f;
						//if close to target
						if ((lmc.projectileInfo.target.position - lmc.transform.position).magnitude < 2)
						{
							lmc.randomSwirlOffset = 0f;
							lmc.projectileInfo.RotationSpeed = 180.0f;
						}
					}
				}

				Vector3 direction = lmc.projectileInfo.target.position - lmc.transform.position + Random.insideUnitSphere * lmc.randomSwirlOffset;
				direction.Normalize();
				lmc.transform.rotation = Quaternion.RotateTowards(
				                                                  lmc.transform.rotation, Quaternion.LookRotation(direction),
				                                                  lmc.projectileInfo.RotationSpeed *
				                                                  Time.deltaTime);

				lmc.transform.position += lmc.transform.forward * Time.deltaTime * lmc.projectileInfo.ProjectileSpeed;
			}
		}

		public void DeactivateBeforeRelease(LaunchedMissileComp lmc)
		{
			Physics.IgnoreCollision(lmc.myCollider, lmc.ownerCollider, false);
		}

		private bool CheckPastTimeToLive(LaunchedMissileComp lmc)
		{
			if (lmc.TimeAlive > lmc.projectileInfo.TimeToLive)
			{
				//ZenLogger.Log($"Past time to live with {lmc.TimeAlive} > {lmc.projectileInfo.TimeToLive}");
				//OnSelfDestruct(lmc, true);
				RangedCombatHelper.PerformAreaExplosion(lmc);
				return true;
			}
			return false;
		}

		private void HandleDetonationDistance(LaunchedMissileComp lmc)
		{
			if (lmc.projectileInfo.target != null && Vector3.SqrMagnitude(lmc.transform.position - lmc.projectileInfo.target.position) <= lmc.detonationDistance)
			{
				ZenLogger.Log("Close to object");
				//OnHit(lmc);
				RangedCombatHelper.PerformAreaExplosion(lmc);
			}
		}

		
	}
}