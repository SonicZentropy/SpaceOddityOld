using System;
using UnityEngine;
using Zenobit.Common.Debug;
using Zenobit.Common.ObjectPool;
using Zenobit.Common.ZenECS;
using Zenobit.Components;

public class LaserProjectileController : ZenBehaviour, IOnUpdate
{
	public LaserInfoPacket projectileInfo;
	private float TimeToLive = 60f;
	private float TimeAlive;

	private Vector3 oldPos;

	void OnEnable()
	{
		TimeAlive = 0f;
		TimeToLive = projectileInfo.TimeToLive;

		oldPos = transform.position;
	}

	public void InitFromLaserInfo(LaserInfoPacket ProjectileInfo)
	{
		projectileInfo = ProjectileInfo;
		transform.position = projectileInfo.StartPosition;
		transform.rotation = Quaternion.LookRotation(projectileInfo.fireDirection);
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

	public override int ExecutionPriority { get; } = 0;
	public override Type ObjectType { get; } = typeof(LaserProjectileController);
}