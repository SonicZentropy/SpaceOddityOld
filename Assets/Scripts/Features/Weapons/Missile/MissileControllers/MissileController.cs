namespace Zenobit.Weapons
{
	using System;
	using Common.ZenECS;
	using UnityEngine;
	using Zenobit.Common.ObjectPool;
	using Zenobit.Components;
	using Random = UnityEngine.Random;

	public class MissileController : ZenBehaviour, IPoolRelease, IOnUpdate, IOnAwake
	{
		//Missile Controller
		public GameObject ExplosionPrefab;
		public MissileInfoPacket projectileInfo;

		protected float TimeToLive = 60f;
		protected float TimeAlive;
		protected Collider ownerCollider;
		protected Collider myCollider;

		public Transform target;
		public LayerMask layerMask;

		public float detonationDistance;

		public float lifeTime = 5f; // Missile life time
		public float despawnDelay; // Delay despawn in ms
		public float velocity = 300f; // Missile velocity
		public float alignSpeed = 1f;
		public float RaycastAdvance = 2f; // Raycast advance multiplier

		private bool ShouldDisperse = true;
		private float dispersalTime = 2f;
		[SerializeField, Tooltip("The maximum rotation speed of the missile. In degrees per second. Its actual rotation may vary, because it's generated randomly.")] float rotationSpeed = 10;

		#region AdvNav Method

		Vector3 previousLos;
		Vector3 los;
		Vector3 losDelta;
		Vector3 desiredRotation;

		/// Gets or sets the navigational constant. The rotational change in the line of sight between the missile and the target is multiplied by this value. It should be greater than 1.
		/// Even though it is called a constant, feel free to change this at any point to get more control over the missile.
		public float navigationalConstant;

		public bool UseAdvancedMode = true; //use advanced PID calculation, probably not necessary

		#endregion

		#region ClusterMethod

		private float timeRandom;
		private float xRandom = 0.01f;
		private float yRandom = 0.01f;
		private float zRandom = 0.01f;
		private float clusterRandomRange = 0.1f;

		#endregion

		#region SwirlMethod

		private float rocketTurnSpeed = 10f;
		private float randomSwirlOffset = 50;

		#endregion

		public bool DelayDespawn = false; // Missile despawn flag

		public ParticleSystem[] delayedParticles; // Array of delayed particles
		ParticleSystem[] particles; // Array of Missile particles

		private bool IsHit { get; set; } = false;

		bool isFXSpawned = false; // Hit FX prefab spawned flag

		Vector3 targetLastPos;
		Vector3 step;

		MeshRenderer meshRenderer;
		private float explosionTime;

		public override int ExecutionPriority { get; } = 0;
		public override Type ObjectType => typeof(MissileController);

		public void OnAwake()
		{
			// Cache transform and get all particle systems attached
			particles = GetComponentsInChildren<ParticleSystem>();
			meshRenderer = GetComponent<MeshRenderer>();
			myCollider = GetComponentInChildren<Collider>();
		}

//		public void InitFromProjectileInfo(MissileInfoPacket _projectileInfo)
//		{
//			projectileInfo = _projectileInfo;
//			transform.position = projectileInfo.StartPosition;
//			transform.rotation = Quaternion.LookRotation(projectileInfo.fireDirection);
//
//			IsHit = false;
//			isFXSpawned = false;
//			targetLastPos = Vector3.zero;
//			step = Vector3.zero;
//			meshRenderer.enabled = true;
//			TimeToLive = projectileInfo.TimeToLive;
//			TimeAlive = 0f;
//
//			detonationDistance = 1f; // how far away from something a missile explodes
//			velocity = projectileInfo.ProjectileSpeed;
//
//			//Swirl method
//			rocketTurnSpeed = 50.0f;
//			xRandom = Random.Range(-clusterRandomRange, clusterRandomRange);
//			yRandom = Random.Range(-clusterRandomRange, clusterRandomRange);
//			zRandom = Random.Range(0, clusterRandomRange);
//			timeRandom = Time.time + Random.Range(0.01f, 2);
//
//			//Explosion caching
//			if (ExplosionPrefab != null)
//			{
//				explosionTime = ExplosionPrefab.GetComponentInChildren<ParticleSystem>().main.duration;
//			}
//
//			ownerCollider = projectileInfo.FiringWeaponComp.GetComponent<ColliderComp>().collider;
//			Physics.IgnoreCollision(myCollider, ownerCollider, true);
//		}

		public void DeactivateBeforeRelease() { Physics.IgnoreCollision(myCollider, ownerCollider, false); }

		protected void TriggerExplosion()
		{
			if (ExplosionPrefab == null) return;

			var exp = ExplosionPrefab.InstantiateFromPool(transform.position);
			exp.ReleaseDelayed(explosionTime);
		}
		// Stop attached particle systems emission and allow them to fade out before despawning
		void Delay()
		{
			if (particles.Length > 0 && delayedParticles.Length > 0)
			{
				bool delayed;

				for (int i = 0; i < particles.Length; i++)
				{
					delayed = false;

					for (int y = 0; y < delayedParticles.Length; y++)
						if (particles[i] == delayedParticles[y])
						{
							delayed = true;
							break;
						}

					particles[i].Stop(false);

					if (!delayed)
						particles[i].Clear(false);
				}
			}
		}

		// Despaw routine
		void OnMissileDestroy()
		{
			//F3DPoolManager.Pools["GeneratedPool"].Despawn(transform);
			gameObject.Release();
		}

		void OnHit()
		{
			//TODO: SEND DAMAGE
			meshRenderer.enabled = false;
			IsHit = true;

			// Invoke delay routine if required
			if (DelayDespawn)
			{
				// Reset missile TimeAlive and let particles systems stop emitting and fade out correctly
				TimeAlive = 0f;
				Delay();
			}
		}

		void OnSelfDestruct(bool ShouldExplodeInAir)
		{
			meshRenderer.enabled = false;
			IsHit = true;

			if (!ShouldExplodeInAir)
				isFXSpawned = true;

			// Invoke delay routine if required
			if (DelayDespawn)
			{
				// Reset missile TimeAlive and let particles systems stop emitting and fade out correctly
				TimeAlive = 0f;
				Delay();
			}
		}

		public void OnUpdate()
		{
			//Lifetime handling
			TimeAlive += Time.deltaTime;
			if (TimeAlive > TimeToLive)
			{
				OnSelfDestruct(true);
			}

			// If something was hit
			if (IsHit)
			{
				// Execute once
				if (!isFXSpawned)
				{
					TriggerExplosion();
					isFXSpawned = true;
				}

				// Despawn current missile 
				if (!DelayDespawn || (DelayDespawn && (TimeAlive >= despawnDelay)))
				{
					OnMissileDestroy();
				}
			}
			// No collision occurred yet
			else
			{
				// Navigate
				if (target != null)
				{
					if (ShouldDisperse)
						UpdateDispersalMethod();
					else
					{
						//////PID
						if (projectileInfo.missileFireType == MissileFireType.Homing && projectileInfo.missileHomingMethod == MissileHomingMethod.PID)
						{
							Vector3 hitPos = Predict(transform.position, target.position, targetLastPos, velocity);
							targetLastPos = target.position;

							transform.rotation = Quaternion.Lerp(transform.rotation,
							                                Quaternion.LookRotation(hitPos - transform.position), Time.deltaTime * alignSpeed);
						}

						else if (projectileInfo.missileFireType == MissileFireType.Homing)
						{
							transform.rotation = Quaternion.Lerp(
							                                transform.rotation,
							                                Quaternion.LookRotation(target.position - transform.position),
							                                Time.deltaTime * alignSpeed);
						}
						else ZenLogger.Log($"WRONG MISSILE TYPE");
					}
				}

				if (target != null && Vector3.SqrMagnitude(transform.position - target.position) <= detonationDistance)
				{
					OnHit();
				}

				// Advances missile forward per frame based on velocity and time
				transform.position += transform.forward * Time.deltaTime * velocity;
			}
		}

		private void UpdateDumbfireMethod() { transform.rotation = Quaternion.LookRotation(projectileInfo.fireDirection); }

		private void UpdateDispersalMethod()
		{
			if (TimeAlive < dispersalTime)
			{
				transform.Rotate(transform.up, Random.Range(-rotationSpeed, rotationSpeed) * Time.deltaTime, Space.World);
				transform.Translate(transform.forward * velocity * Time.deltaTime);
			}
			else
			{
				ShouldDisperse = false;
			}
		}

		private void UpdateAdvancedNavMethod()
		{
			previousLos = los;
			los = target.position - transform.position;
			losDelta = los - previousLos;

			losDelta = losDelta - Vector3.Project(losDelta, los);

			if (UseAdvancedMode)
				desiredRotation = (Time.deltaTime * los) + (losDelta * navigationalConstant) + (Time.deltaTime * desiredRotation * navigationalConstant * 0.5f); // Augmented version of proportional navigation.
			else
				desiredRotation = (Time.deltaTime * los) + (losDelta * navigationalConstant); // Plain proportional navigation.

			// Use the Rotate function to consider the rotation rate of the missile.
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(desiredRotation, transform.up), Time.deltaTime * rocketTurnSpeed);
			transform.Translate(transform.forward * velocity * Time.deltaTime);
		}

		//http://answers.unity3d.com/questions/698009/swarm-missile-adding-random-movement-to-a-homing-m.html
		private void UpdateClusterMethod()
		{
			//defines a new random float for x,y,z every frame only if the time specfied has elapsed.  Creates random changes in x, y, z, variables at random times.
			if (Time.time > timeRandom)
			{
				xRandom = Random.Range(-clusterRandomRange, clusterRandomRange);
				yRandom = Random.Range(-clusterRandomRange, clusterRandomRange);
				zRandom = Random.Range(0, clusterRandomRange);
				timeRandom = Time.time + Random.Range(0.01f, 2);
			}

			//Moves the missile to its target
			transform.position = Vector3.MoveTowards(
			                                         transform.position, target.position,
			                                         Time.deltaTime * projectileInfo.FlightSpeed);

			if ((target.position - transform.position).magnitude > 10)
			{
				transform.Translate(xRandom, yRandom, zRandom, target);
			}
			//Adds randomized trajectory

			if ((target.position - transform.position).magnitude < 3 || TimeAlive > TimeToLive)
			{
				gameObject.Release();
			}

			//keeps the missile looking at its target
			transform.LookAt(target.position);
		}

		//https://www.reddit.com/r/Unity3D/comments/3xw8uc/cluster_homing_missiles_c_code/
		private void UpdateRandomSwirlMethod()
		{
			if (target != null)
			{
				if (TimeAlive > 1)
				{
					if ((target.position - transform.position).magnitude > 50)
					{
						randomSwirlOffset = 100.0f;
						rocketTurnSpeed = 90.0f;
					}
					else
					{
						randomSwirlOffset = 25f;
						//if close to target
						if ((target.position - transform.position).magnitude < 2)
						{
							randomSwirlOffset = 0f;
							rocketTurnSpeed = 180.0f;
						}
					}
				}

				Vector3 direction = target.position - transform.position + Random.insideUnitSphere * randomSwirlOffset;
				direction.Normalize();
				transform.rotation = Quaternion.RotateTowards(
				                                              transform.rotation, Quaternion.LookRotation(direction),
				                                              rocketTurnSpeed *
				                                              Time.deltaTime);
				transform.Translate(Vector3.forward * velocity * Time.deltaTime);
			}

			if (TimeAlive > TimeToLive)
			{
				gameObject.Release();
			}
		}

		public static Vector3 Predict(Vector3 sPos, Vector3 tPos, Vector3 tLastPos, float pSpeed)
		{
			// Target velocity
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
	}
}