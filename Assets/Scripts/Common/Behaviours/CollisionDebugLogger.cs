using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenobit.Common.Debug;

public class CollisionDebugLogger : MonoBehaviour
{
	private int numCollisions = 0;

	private void OnParticleCollision(GameObject other)
	{
		ZenLogger.Log($"{gameObject.name} Particle Collided with {other.name}");
	}
	
	void OnTriggerEnter(Collider other)
	{
		ZenLogger.Log($"{gameObject.name} triggered by {other.name}");
	}
	
	void OnCollisionEnter(Collision other)
	{
		ZenLogger.Log($"{gameObject.name} collided from {other.gameObject.name}");
	}

}
