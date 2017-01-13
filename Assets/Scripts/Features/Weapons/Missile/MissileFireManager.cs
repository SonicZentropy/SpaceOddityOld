using UnityEngine;
using Zenobit.Common;
using Zenobit.Common.Extensions;
using Zenobit.Common.ObjectPool;
using Zenobit.Common.ZenECS;
using Zenobit.Components;
using Zenobit.Weapons;

public class MissileFireManager : Singleton<MissileFireManager>
{

	public void Fire(MissileComp wc)
	{
		switch (wc.missileInfoPacket.missileFireType)
		{
			case MissileFireType.Swarm:
				FireSwarmMissiles(wc);
				break;
			case MissileFireType.Dumbfire:
					FireMissile(wc);
				break;
			case MissileFireType.Homing:
				FireHomingMissile(wc);
				break;
		}
	}

	private void FireMissile(MissileComp wc)
	{
		//var lpc = Res.Load(wc.ProjectilePrefab);
		//var lpcinst = lpc.InstantiateFromPool();
		//lpcinst.GetComponent<MissileController>().InitFromProjectileInfo(wc.missileInfoPacket);

		EcsEngine.Instance.CreateEntity(Res.Entities.DumbfireMissile);
	}

	private void FireHomingMissile(MissileComp wc)
	{
		//var lpc = Res.Load(wc.ProjectilePrefab);
		//var lpcinst = lpc.InstantiateFromPool();
		//lpcinst.GetComponent<MissileController>().InitFromProjectileInfo(wc.missileInfoPacket);
		Entity miss = EcsEngine.Instance.CreateEntity(Res.Entities.HomingMissile);
		InitFromProjectileInfo(miss, wc);
	}

	private void FireSwarmMissiles(MissileComp wc)
	{
		//var lpc = Res.Load(wc.ProjectilePrefab);
		for(int i = 0; i < wc.numberSwarmMissiles; i++)
		{
			Entity miss = EcsEngine.Instance.CreateEntity(Res.Entities.HomingMissile);
			InitFromProjectileInfo(miss, wc);
		}
	}

	private void InitFromProjectileInfo(Entity missile, MissileComp mc)
	{
		var projectileInfo = mc.missileInfoPacket;
		var pc = missile.GetComponent<PositionComp>();
		var transform = pc.transform;
		var lmc = missile.GetComponent<LaunchedMissileComp>();
		transform.position = projectileInfo.StartPosition;
		transform.rotation = Quaternion.LookRotation(projectileInfo.fireDirection);

		lmc.IsHit = false;
		lmc.isFXSpawned = false;
		lmc.targetLastPos = Vector3.zero;
		lmc.step = Vector3.zero;
		lmc.lifeTime = projectileInfo.TimeToLive;
		lmc.TimeAlive = 0f;

		lmc.velocity = projectileInfo.ProjectileSpeed;

		//Swirl method
		lmc.rotationSpeed = 50.0f;
		lmc.xRandom = Random.Range(-lmc.swarmRandomRange, lmc.swarmRandomRange);
		lmc.yRandom = Random.Range(-lmc.swarmRandomRange, lmc.swarmRandomRange);
		lmc.zRandom = Random.Range(0, lmc.swarmRandomRange);
		lmc.timeRandom = Time.time + Random.Range(0.01f, 2);

		//Explosion caching
		if (!lmc.ExplosionPrefabLink.Equals("Prefabs/None"))
		{
			lmc.ExplosionPrefab = Res.Load(lmc.ExplosionPrefabLink);

			lmc.explosionTime = lmc.ExplosionPrefab.GetComponentInChildren<ParticleSystem>().main.duration;
		}

		lmc.ownerCollider = projectileInfo.FiringWeaponComp.GetComponent<ColliderComp>().collider;
		lmc.myCollider = missile.Wrapper.gameObject.GetComponentInChildren<Collider>();
		Physics.IgnoreCollision(lmc.myCollider, lmc.ownerCollider, true);
	}
}

