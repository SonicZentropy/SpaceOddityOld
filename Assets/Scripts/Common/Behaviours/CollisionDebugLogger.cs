using UnityEngine;

public class CollisionDebugLogger : MonoBehaviour
{
	//private int numCollisions = 0;
	public bool EnableLogging = true;
	
	private void OnParticleCollision(GameObject other)
	{
		if (EnableLogging)
			Debug.Log($"{gameObject.name} Particle Collided with {other.name}");
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (EnableLogging)
			Debug.Log($"{gameObject.name} triggered by {other.name}");
	}
	
	void OnCollisionEnter(Collision other)
	{
		if (EnableLogging)
			Debug.Log($"{gameObject.name} collided from {other.gameObject.name}");
	}

}
