using UnityEngine;
using Zen.Common.ObjectPool;

public class ExplosionCleanupBehaviour : MonoBehaviour
{
	void OnEnable()
	{
		ParticleSystem ps = (ParticleSystem)gameObject.GetComponentInChildren(typeof(ParticleSystem), true);
		ps.Play(true);
		gameObject.ReleaseDelayed(ps.main.duration + ps.startLifetime);
	}
}
