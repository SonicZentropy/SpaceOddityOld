﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenobit.Common.Debug;

public class CollisionDebugLogger : MonoBehaviour
{
	private int numCollisions = 0;
	public bool EnableLogging = false;
	
	private void OnParticleCollision(GameObject other)
	{
		if (EnableLogging)
			ZenLogger.Log($"{gameObject.name} Particle Collided with {other.name}");
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (EnableLogging)
			ZenLogger.Log($"{gameObject.name} triggered by {other.name}");
	}
	
	void OnCollisionEnter(Collision other)
	{
		if (EnableLogging)
			ZenLogger.Log($"{gameObject.name} collided from {other.gameObject.name}");
	}

}
