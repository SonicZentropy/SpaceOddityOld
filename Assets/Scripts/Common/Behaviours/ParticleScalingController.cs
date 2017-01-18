using UnityEngine;
using System.Collections;
using Zenobit.Common.Extensions;

public class ParticleScalingController : MonoBehaviour
{
	private ParticleSystem mainPS;
	private float CurrentScale;

	void Awake()
	{
		mainPS = (ParticleSystem) GetComponentInChildren(typeof(ParticleSystem));
		CurrentScale = 1;
	}

	public void SetScale(float newScale, bool includeChildren)
	{
		//Scaling is persistent across pooling reuses, so we need to adjust for previous iterations of the scaling
		if (!(newScale > 0.01f)) newScale = 1f;
		mainPS.ScaleByTransform(newScale / CurrentScale, includeChildren);
		CurrentScale = newScale;
	}
}
