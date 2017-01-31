using System;
using Features.Explosions;
using UnityEngine;
using Zen.Common;
using Zen.Common.Extensions;
using Zen.Common.ObjectPool;
using Zen.Common.ZenECS;
using Zen.Components;

public class LaserProjectileController : ZenBehaviour, IOnUpdate
{
	public LaserInfoPacket projectileInfo;
	private float TimeToLive = 60f;
	private float TimeAlive;

	private Collider myCollider;

	private Collider ownerCollider;
	//private Vector3 oldPos;

	void OnEnable()
	{
		if (myCollider == null) myCollider = GetComponentInChildren<Collider>();

		TimeAlive = 0f;
		TimeToLive = projectileInfo.TimeToLive;

		//oldPos = transform.position;
	}

	void OnDisable()
	{
		DeactivateBeforeRelease();
	}

	public void DeactivateBeforeRelease()
	{
		if (myCollider != null && ownerCollider != null)
		{
			Physics.IgnoreCollision(myCollider, ownerCollider, false);
		}
	}

	public void InitFromLaserInfo(LaserInfoPacket ProjectileInfo)
	{
		projectileInfo = ProjectileInfo;
		transform.position = projectileInfo.StartPosition;
		transform.rotation = Quaternion.LookRotation(projectileInfo.fireDirection);
		ownerCollider = projectileInfo.FiringWeaponComp.GetComponent<ColliderComp>().collider;
		Physics.IgnoreCollision(myCollider, ownerCollider, true);
		TimeAlive = 0f;
		TimeToLive = projectileInfo.TimeToLive;
	}

	public void OnUpdate()
	{
		TimeAlive += Time.deltaTime;
		if (TimeAlive > TimeToLive)
		{
			gameObject.Release();
			return;
		}

		transform.position += projectileInfo.fireDirection * projectileInfo.ProjectileSpeed * Time.deltaTime;
	}

	public void OnTriggerEnter(Collider other)
	{
		var go = other.attachedRigidbody.gameObject;

		//ZenLogger.Log($"Laser hit {go.name}");
		if (go.HasEntityTag(EntityTags.IsDamageable))
		{
			//ZenLogger.Log($"Adding dmg component to ship");
			go.GetEntity().GetComponent<DamageComp>().damagePackets.Push(new DamagePacket(10, 10));
		}
		Explosions.Create(Res.Prefabs.ExplosionPFX_Blue, transform.position);
		gameObject.Release();
	}

	public override int ExecutionPriority { get; } = 0;
	public override Type ObjectType { get; } = typeof(LaserProjectileController);
}