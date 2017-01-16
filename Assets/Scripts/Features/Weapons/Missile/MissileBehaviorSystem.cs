// /**
// * MissileBehaviorSystem.cs
// * Will Hart and Dylan Bailey
// * 20170111
// */

namespace Zenobit.Systems
{
	#region Dependencies

	using System.Linq;
	using Common.ObjectPool;
	using Common.ZenECS;
	using Components;
	using TypeSafe;
	using UnityEngine;

	#endregion

	public class MissileBehaviorSystem : AbstractEcsSystem
	{
		public override bool Init() { return true; }

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
			if (lmc.TimeAlive > lmc.projectileInfo.TimeToLive)
			{
				OnSelfDestruct(lmc, true);
			}

			// If something was hit
			if (lmc.IsHit)
			{
				// Execute once
				if (!lmc.isFXSpawned)
				{
					TriggerExplosion(lmc);
					lmc.isFXSpawned = true;
				}

				// Despawn current missile
				if (lmc.despawnDelay <= 0 || ((lmc.despawnDelay > 0) && (lmc.TimeAlive >= lmc.despawnDelay)))
				{
					OnMissileDestroy(lmc);
				}
			}
			// No collision occurred yet
			else
			{
				// Navigate
				if (lmc.projectileInfo.target != null)
				{
					if (lmc.projectileInfo.ShouldDisperse)
						UpdateDispersalMethod(lmc);
					else
					{
						//////PID
						if (lmc.projectileInfo.missileFireType == MissileFireType.Homing || lmc.projectileInfo.missileFireType == MissileFireType.Swarm)
						{
							if (lmc.projectileInfo.missileHomingMethod == MissileHomingMethod.PID)
							{
								Vector3 hitPos = Predict(lmc.transform.position, lmc.projectileInfo.target.position, lmc.targetLastPos, lmc.projectileInfo.ProjectileSpeed);
								lmc.targetLastPos = lmc.projectileInfo.target.position;

								lmc.transform.rotation = Quaternion.Lerp(
								                                         lmc.transform.rotation,
								                                         Quaternion.LookRotation(hitPos - lmc.transform.position), Time.deltaTime * lmc.rotationSpeed);
							}
							else
							{
								lmc.transform.rotation = Quaternion.Lerp(
								                                         lmc.transform.rotation,
								                                         Quaternion.LookRotation(lmc.projectileInfo.target.position - lmc.transform.position),
								                                         Time.deltaTime * lmc.rotationSpeed);
							}
						}

						else if (lmc.projectileInfo.missileFireType == MissileFireType.Dumbfire)
						{
							UpdateDumbfireMethod(lmc);
						}
						else
						{
							ZenLogger.LogError("Missile type not found!");
						}
					}
				}

				if (lmc.projectileInfo.target != null && Vector3.SqrMagnitude(lmc.transform.position - lmc.projectileInfo.target.position) <= lmc.detonationDistance)
				{
					ZenLogger.Log("Close to object");
					OnHit(lmc);
				}

				// Advances missile forward per frame based on projectileInfo.ProjectileSpeed and time
				lmc.transform.position += lmc.transform.forward * Time.deltaTime * lmc.projectileInfo.ProjectileSpeed;

			}
		}



		private void UpdateDumbfireMethod(LaunchedMissileComp lmc) { lmc.transform.rotation = Quaternion.LookRotation(lmc.projectileInfo.fireDirection); }

		private void UpdateDispersalMethod(LaunchedMissileComp lmc)
		{
			if (lmc.TimeAlive < lmc.dispersalTime)
			{
				lmc.transform.Rotate(lmc.transform.up, Random.Range(-lmc.rotationSpeed, lmc.rotationSpeed) * Time.deltaTime, Space.World);
				lmc.transform.Translate(lmc.transform.forward * lmc.projectileInfo.ProjectileSpeed * Time.deltaTime);
			}
			else
			{
				lmc.projectileInfo.ShouldDisperse = false;
			}
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
			lmc.transform.rotation = Quaternion.RotateTowards(lmc.transform.rotation, Quaternion.LookRotation(lmc.desiredRotation, lmc.transform.up), Time.deltaTime * lmc.rotationSpeed);
			lmc.transform.Translate(lmc.transform.forward * lmc.projectileInfo.ProjectileSpeed * Time.deltaTime);
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

			//Moves the missile to its target
			lmc.transform.position = Vector3.MoveTowards(
			                                             lmc.transform.position, lmc.projectileInfo.target.position,
			                                             Time.deltaTime * lmc.projectileInfo.ProjectileSpeed);

			if ((lmc.projectileInfo.target.position - lmc.transform.position).magnitude > 10)
			{
				lmc.transform.Translate(lmc.xRandom, lmc.yRandom, lmc.zRandom, lmc.projectileInfo.target);
			}
			//Adds randomized trajectory

			//keeps the missile looking at its target
			lmc.transform.LookAt(lmc.projectileInfo.target.position);

			if ((lmc.projectileInfo.target.position - lmc.transform.position).magnitude < 3 || lmc.TimeAlive > lmc.projectileInfo.TimeToLive)
			{
				engine.DestroyEntity(lmc.Owner);
			}
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
						lmc.rotationSpeed = 90.0f;
					}
					else
					{
						lmc.randomSwirlOffset = 25f;
						//if close to target
						if ((lmc.projectileInfo.target.position - lmc.transform.position).magnitude < 2)
						{
							lmc.randomSwirlOffset = 0f;
							lmc.rotationSpeed = 180.0f;
						}
					}
				}

				Vector3 direction = lmc.projectileInfo.target.position - lmc.transform.position + Random.insideUnitSphere * lmc.randomSwirlOffset;
				direction.Normalize();
				lmc.transform.rotation = Quaternion.RotateTowards(
				                                                  lmc.transform.rotation, Quaternion.LookRotation(direction),
				                                                  lmc.rotationSpeed *
				                                                  Time.deltaTime);
				lmc.transform.Translate(Vector3.forward * lmc.projectileInfo.ProjectileSpeed * Time.deltaTime);
			}

			if (lmc.TimeAlive > lmc.projectileInfo.TimeToLive)
			{
				engine.DestroyEntity(lmc.Owner);
			}
		}

		public static Vector3 Predict(Vector3 sPos, Vector3 tPos, Vector3 tLastPos, float pSpeed)
		{
			// Target projectileInfo.ProjectileSpeed
			Vector3 tVel = (tPos - tLastPos) / Time.deltaTime;

			// Time to reach the target
			float flyTime = GetProjFlightTime(tPos - sPos, tVel, pSpeed);

			if (flyTime > 0)
				return tPos + flyTime * tVel;
			return tPos;
		}

		static float GetProjFlightTime(Vector3 dist, Vector3 tVel, float pSpeed)
		{
			float a = Vector3.Dot(tVel, tVel) - pSpeed * pSpeed;
			float b = 2.0f * Vector3.Dot(tVel, dist);
			float c = Vector3.Dot(dist, dist);

			float det = b * b - 4 * a * c;

			if (det > 0)
				return 2 * c / (Mathf.Sqrt(det) - b);
			return -1;
		}

		public void DeactivateBeforeRelease(LaunchedMissileComp lmc)
		{
			Physics.IgnoreCollision(lmc.myCollider, lmc.ownerCollider, false);
		}

		protected void TriggerExplosion(LaunchedMissileComp lmc)
		{
			if (lmc.ExplosionPrefab == null) return;

			var exp = lmc.ExplosionPrefab.InstantiateFromPool(lmc.transform.position);
			exp.ReleaseDelayed(lmc.explosionTime);
		}

		// Stop attached particle systems emission and allow them to fade out before despawning
		void Delay(LaunchedMissileComp lmc)
		{
			if (lmc.particles != null)
			{
				//bool delayed;

				lmc.particles.Stop(false);

				//for (int i = 0; i < particles.Length; i++)
				//{
				//	delayed = false;

				//	for (int y = 0; y < delayedParticles.Length; y++)
				//		if (particles[i] == delayedParticles[y])
				//		{
				//			delayed = true;
				//			break;
				//		}

				//	particles[i].Stop(false);

				//	if (!delayed)
				//		particles[i].Clear(false);
				//}
			}
		}

		// Despawn routine
		void OnMissileDestroy(LaunchedMissileComp lmc)
		{
			//F3DPoolManager.Pools["GeneratedPool"].Despawn(transform);
			engine.DestroyEntity(lmc.Owner);
		}

		void OnHit(LaunchedMissileComp lmc)
		{
			//TODO: SEND DAMAGE
			lmc.meshRenderer.enabled = false;
			lmc.IsHit = true;

			// Invoke delay routine if required
			if (lmc.despawnDelay > 0)
			{
				// Reset missile TimeAlive and let particles systems stop emitting and fade out correctly
				lmc.TimeAlive = 0f;
				Delay(lmc);
			}
		}

		void OnSelfDestruct(LaunchedMissileComp lmc, bool ShouldExplodeInAir)
		{
			lmc.meshRenderer.enabled = false;
			lmc.IsHit = true;

			if (!ShouldExplodeInAir)
				lmc.isFXSpawned = true;

			// Invoke delay routine if required
			if (lmc.despawnDelay > 0)
			{
				// Reset missile TimeAlive and let particles systems stop emitting and fade out correctly
				lmc.TimeAlive = 0f;
				Delay(lmc);
			}
		}
	}
}