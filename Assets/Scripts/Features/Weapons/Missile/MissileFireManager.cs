using System.Collections.Generic;
using MEC;
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

			Entity miss = EcsEngine.Instance.CreateEntity(Res.Entities.DumbfireMissile);
			InitFromProjectileInfo(miss, wc);

	}

	private void FireHomingMissile(MissileComp wc)
	{
		if (!HasTarget(wc)) return; //don't shoot if no target to home toward
		//var lpc = Res.Load(wc.ProjectilePrefab);
		//var lpcinst = lpc.InstantiateFromPool();
		//lpcinst.GetComponent<MissileController>().InitFromProjectileInfo(wc.missileInfoPacket);
		Entity miss = EcsEngine.Instance.CreateEntity(Res.Entities.HomingMissile);
		InitFromProjectileInfo(miss, wc);
	}

	private void FireSwarmMissiles(MissileComp wc)
	{
		if (!HasTarget(wc)) return; //don't shoot if no target to home toward

		Timing.RunCoroutine(FireSwarmMissileCrt(wc, (int) wc.numberSwarmMissiles));

		//for (int i = 0; i < wc.numberSwarmMissiles; i++)
		//{
		//	Entity miss = EcsEngine.Instance.CreateEntity(Res.Entities.SwarmMissile);
		//	InitFromProjectileInfo(miss, wc);
		//}
	}

	private IEnumerator<float> FireSwarmMissileCrt(MissileComp mc, int numMissiles)
	{
		for (int i = 0; i < numMissiles; i++)
		{
			Entity miss = EcsEngine.Instance.CreateEntity(Res.Entities.SwarmMissile);
			InitFromProjectileInfo(miss, mc);
			yield return 0;
		}
	}

	private bool HasTarget(MissileComp mc)
	{
		return mc.missileInfoPacket.target != null;
	}

	private void InitFromProjectileInfo(Entity missile, MissileComp mc)
	{
		//var projectileInfo = mc.missileInfoPacket;

		var pc = missile.GetComponent<PositionComp>();
		var transform = pc.transform;
		var lmc = missile.GetComponent<LaunchedMissileComp>();

		lmc.projectileInfo = mc.missileInfoPacket;
		lmc.projectileInfo.ShieldDamage = mc.ShieldDamage;
		lmc.projectileInfo.HullDamage = mc.HullDamage;
		transform.position = lmc.projectileInfo.StartPosition;
		transform.rotation = Quaternion.LookRotation(lmc.projectileInfo.fireDirection);

		lmc.IsHit = false;
		lmc.isFXSpawned = false;
		lmc.targetLastPos = Vector3.zero;
		lmc.step = Vector3.zero;
		lmc.TimeAlive = 0f;

		//Dispersal randomization
		if (lmc.projectileInfo.ShouldDisperse && lmc.projectileInfo.DispersalRandomTime > 0)
		{
			lmc.projectileInfo.DispersalTime += Random.Range(-lmc.projectileInfo.DispersalRandomTime, lmc.projectileInfo.DispersalRandomTime);
		}

		//Swirl method
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

		lmc.ownerCollider = lmc.projectileInfo.FiringWeaponComp.GetComponent<ColliderComp>().collider;
		lmc.myCollider = missile.GetComponent<ColliderComp>().collider;
		lmc.meshRenderer = missile.GetComponent<RendererComp>().renderer;
		lmc.particles = missile.GetComponent<ParticleSystemComp>().ParticleSystem;
		lmc.transform = missile.GetComponent<PositionComp>().transform;


		Physics.IgnoreCollision(lmc.myCollider, lmc.ownerCollider, true);
	}
}

