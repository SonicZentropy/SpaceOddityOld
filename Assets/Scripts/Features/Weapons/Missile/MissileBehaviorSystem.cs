﻿// /**
// * MissileBehaviorSystem.cs
// * Will Hart and Dylan Bailey
// * 20170111
// */

namespace Zenobit.Systems
{
	#region Dependencies

	using System.Linq;
	using Common.Extensions;
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
							case MissileHomingMethod.AdvNav:
								UpdateAdvancedNavMethod(lmc);
								break;
							case MissileHomingMethod.Cluster:
								UpdateClusterMethod(lmc);
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

			lmc.transform.position += lmc.transform.forward * Time.deltaTime * lmc.projectileInfo.FlightSpeed;
		}

		private void UpdateChaseMethod(LaunchedMissileComp lmc)
		{
			lmc.transform.rotation = Quaternion.Lerp(lmc.transform.rotation,
			                                         Quaternion.LookRotation(lmc.projectileInfo.target.position - lmc.transform.position),
			                                         Time.deltaTime * lmc.projectileInfo.RotationSpeed);
			lmc.transform.position += lmc.transform.forward * Time.deltaTime * lmc.projectileInfo.FlightSpeed;
		}

		private void UpdatePIDMethod(LaunchedMissileComp lmc)
		{
			Vector3 hitPos = RangedCombatHelper.Predict(lmc.transform.position, lmc.projectileInfo.target.position, lmc.targetLastPos, lmc.projectileInfo.FlightSpeed);
			lmc.targetLastPos = lmc.projectileInfo.target.position;

			lmc.transform.rotation = Quaternion.Lerp(lmc.transform.rotation,
			                                         Quaternion.LookRotation(hitPos - lmc.transform.position),
			                                         Time.deltaTime * lmc.projectileInfo.RotationSpeed);

			lmc.transform.position += lmc.transform.forward * Time.deltaTime * lmc.projectileInfo.FlightSpeed;
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
			//lmc.transform.rotation = Quaternion.RotateTowards(lmc.transform.rotation, Quaternion.LookRotation(lmc.desiredRotation, lmc.transform.up), Time.deltaTime * lmc.projectileInfo.RotationSpeed);
			lmc.transform.rotation = Quaternion.Lerp(lmc.transform.rotation,
			                                         Quaternion.LookRotation(lmc.desiredRotation), // lmc.transform.up),
			                                         Time.deltaTime * lmc.projectileInfo.RotationSpeed);

			lmc.transform.position += lmc.transform.forward * Time.deltaTime * lmc.projectileInfo.FlightSpeed;
		}

		private void UpdateDispersalMethod(LaunchedMissileComp lmc)
		{
			if (lmc.TimeAlive < lmc.projectileInfo.DispersalTime)
			{
				if (lmc.dispersalTarget == Vector3.zero)
					CalculateDispersalTarget(lmc);

				lmc.transform.rotation = Quaternion.Lerp(lmc.transform.rotation,
													 Quaternion.LookRotation(lmc.dispersalTarget - lmc.transform.position), // lmc.transform.up),
													 Time.deltaTime * lmc.projectileInfo.RotationSpeed);

				//lmc.transform.Rotate(lmc.transform.up, Random.Range(-lmc.projectileInfo.RotationSpeed, lmc.projectileInfo.RotationSpeed) * Time.deltaTime, Space.Self);
				//lmc.transform.Translate(lmc.transform.forward * lmc.projectileInfo.ProjectileSpeed * Time.deltaTime);
				//Quaternion desired = lmc.transform.rotation * Quaternion.LookRotation(lmc.transform.up * Random.Range(-lmc.projectileInfo.RotationSpeed, lmc.projectileInfo.RotationSpeed) * Time.deltaTime, Vector3.up);
				//lmc.transform.rotation = Quaternion.RotateTowards(lmc.transform.rotation, desired, Time.deltaTime * 20);
				//var range = Random.Range(-lmc.projectileInfo.RotationSpeed, lmc.projectileInfo.RotationSpeed);
				////lmc.transform.Rotate(lmc.transform.up, range * Time.deltaTime, Space.World);
				//lmc.transform.rotation = Quaternion.AngleAxis(range, lmc.transform.up);
				//if (considerController)
				//	controller.Rotate(transform.rotation * Quaternion.LookRotation(transform.up * Random.Range(-rotationSpeed, rotationSpeed) * Time.deltaTime, Vector3.up));
				//else
				//	transform.Rotate(transform.up, Random.Range(-rotationSpeed, rotationSpeed) * Time.deltaTime, Space.World);
				lmc.transform.position += lmc.transform.forward * Time.deltaTime * lmc.projectileInfo.FlightSpeed;
			}
			else
			{
				ZenLogger.Log($"Completed dispersal time of {lmc.projectileInfo.DispersalTime} at time alive: {lmc.TimeAlive}");
				lmc.projectileInfo.ShouldDisperse = false;
			}
		}

		private static void CalculateDispersalTarget(LaunchedMissileComp lmc)
		{
			var randomX = Random.Range(-15, 15);
			var randomY = Random.Range(-15, 15);
			var zDistance = lmc.projectileInfo.DispersalTime * lmc.projectileInfo.FlightSpeed;

			lmc.dispersalTarget = lmc.transform.position + new Vector3(randomX, randomY, zDistance);

		}

		//http://answers.unity3d.com/questions/698009/swarm-missile-adding-random-movement-to-a-homing-m.html
		private void UpdateClusterMethod(LaunchedMissileComp lmc)
		{
			//defines a new random float for x,y,z every frame only if the time specfied has elapsed.  Creates random changes in x, y, z, variables at random times.
			if (Time.time > lmc.timeRandom)
			{
				lmc.xRandom = Random.Range(-lmc.clusterRandomRange, lmc.clusterRandomRange);
				lmc.yRandom = Random.Range(-lmc.clusterRandomRange, lmc.clusterRandomRange);
				lmc.zRandom = Random.Range(0, lmc.clusterRandomRange);
				lmc.timeRandom = Time.time + Random.Range(0.01f, 2);
			}

			//Perform swarming slide behavior
			if ((lmc.projectileInfo.target.position - lmc.transform.position).magnitude > 10)
			{
				lmc.transform.Translate(lmc.xRandom, lmc.yRandom, lmc.zRandom, lmc.projectileInfo.target);
			}

			//keeps the missile looking at its target
			//lmc.transform.LookAt(lmc.projectileInfo.target.position);
			lmc.transform.rotation = Quaternion.Slerp(lmc.transform.rotation,
			                                         Quaternion.LookRotation(lmc.projectileInfo.target.position - lmc.transform.position), // lmc.transform.up),
			                                         Time.deltaTime * lmc.projectileInfo.RotationSpeed);
			lmc.transform.position += lmc.transform.forward * Time.deltaTime * lmc.projectileInfo.FlightSpeed;
		}

		//https://www.reddit.com/r/Unity3D/comments/3xw8uc/cluster_homing_missiles_c_code/
		private void UpdateRandomSwirlMethod(LaunchedMissileComp lmc)
		{
			if (lmc.projectileInfo.target != null)
			{
				if (lmc.TimeAlive > lmc.projectileInfo.DispersalTime + 1.5f)
				{
					if ((lmc.projectileInfo.target.position - lmc.transform.position).magnitude > 50)
					{
						lmc.randomSwirlOffset = 100.0f;
						//lmc.projectileInfo.RotationSpeed = 90.0f;
						lmc.randomSwirlRotation = 90f;
					}
					else
					{
						lmc.randomSwirlOffset = 15f;
						//if close to target
						if ((lmc.projectileInfo.target.position - lmc.transform.position).magnitude < 10)
						{
							lmc.randomSwirlOffset = 0f;
							//lmc.projectileInfo.RotationSpeed = 180.0f;
							lmc.randomSwirlRotation = 180f;
						}
					}
				}

				Vector3 direction = lmc.projectileInfo.target.position - lmc.transform.position + Random.insideUnitSphere * lmc.randomSwirlOffset;
				direction.Normalize();
				lmc.transform.rotation = Quaternion.RotateTowards(lmc.transform.rotation,
				                                                  Quaternion.LookRotation(direction),
				                                                  lmc.randomSwirlRotation * Time.deltaTime);

				lmc.transform.position += lmc.transform.forward * Time.deltaTime * lmc.projectileInfo.FlightSpeed;
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
			if (lmc.detonationDistance.IsAlmost(0)) return;
			if (lmc.projectileInfo.target != null && Vector3.SqrMagnitude(lmc.transform.position - lmc.projectileInfo.target.position) <= lmc.detonationDistance)
			{
				//ZenLogger.Log("Close to object");
				//OnHit(lmc);
				RangedCombatHelper.PerformAreaExplosion(lmc);
			}
		}
	}
}