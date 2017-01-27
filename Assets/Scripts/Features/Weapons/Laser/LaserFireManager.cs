using System;
//using DigitalRuby.FastLineRenderer;
using UnityEngine;
using Zenobit.Common;
using Zenobit.Common.Extensions;
using Zenobit.Common.ObjectPool;
using Zenobit.Components;

public class LaserFireManager : Singleton<LaserFireManager>
{
	//public FastLineRenderer LineRenderer;

	public void Fire(LaserComp wc)
	{
		switch (wc.laserInfoPacket.laserFireType)
		{
			case LaserFireType.ProjectileGO:
				FireProjectileLaser(wc);
				break;
			case LaserFireType.Particle:
				FireParticleLaser(wc);
				break;
		}
	}

	private void FireProjectileLaser(LaserComp wc)
	{
		//var lpc = Resources.Load<GameObject>(wc.ProjectilePrefab);
		var lpc = Res.Load(wc.ProjectilePrefab);
		var lpcinst = lpc.InstantiateFromPool();
		var laser = lpcinst.GetComponent<LaserProjectileController>();
		laser.InitFromLaserInfo(wc.laserInfoPacket);
		
	}

	private void FireParticleLaser(LaserComp wc)
	{
		//var lpc = Res.Load(Res.Prefabs.LaserBeamPrefabLR);
		//var laser = lpc.InstantiateFromPool().GetComponent<LaserProjectileController>();
		//laser.transform.position = projectileInfo.StartPosition;
		//laser.transform.rotation = projectileInfo.Owner.transform.rotation;
		//laser.projectileInfo = projectileInfo;
	}

	//public void FireLaserFastLR(ProjectileInfoPacket projectileInfo)
	//{
	//	FastLineRendererProperties props = new FastLineRendererProperties();
	//	FastLineRenderer r = FastLineRenderer.CreateWithParent(null, LineRenderer);
	//	r.Material.EnableKeyword("DISABLE_CAPS");
	//	r.SetCapacity(1000 * FastLineRenderer.VerticesPerLine);
	//	r.Turbulence = 1.0f;
	//	r.BoundsScale = new Vector3(1.0f, 1.0f, 1.0f);
	//	props.GlowIntensityMultiplier = 0.1f;
	//	props.GlowWidthMultiplier = 4.0f;
	//
	//	props.Start = projectileInfo.StartPosition;
	//	props.End = projectileInfo.StartPosition + (projectileInfo.FireDirection.normalized * 3f);
	//	props.Radius = 01.1f;
	//	props.SetLifeTime(projectileInfo.TimeToLive);
	//	props.Color = new Color32(ZenUtils.RandomByte(), ZenUtils.RandomByte(), ZenUtils.RandomByte(), ZenUtils.RandomByte());
	//	props.Velocity = projectileInfo.FireDirection.normalized * projectileInfo.ProjectileSpeed;
	//	r.AddLine(props);
	//
	//	r.Apply(false);
	//	r.SendToCacheAfter(TimeSpan.FromSeconds(projectileInfo.TimeToLive));
	//}
}

