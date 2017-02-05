namespace Common.Behaviours
{
	using UnityEngine;

	public class CollisionParticleLogger : MonoBehaviour
	{
		private void OnParticleCollision(GameObject other)
		{
			Debug.Log($"Particle collision detected on {other.name}");
		}

		private void OnParticleTrigger()
		{
			Debug.Log($"Particle trigger detected");
		}
	}
}