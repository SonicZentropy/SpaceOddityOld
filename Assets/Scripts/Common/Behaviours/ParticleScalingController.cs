using UnityEngine;
using Zen.Common.Extensions;

public class ParticleScalingController : MonoBehaviour
{
	private ParticleSystem mainPS;
	private float CurrentScale;
	public float DesiredScale = 1f;

	void Awake()
	{
		mainPS = (ParticleSystem) GetComponentInChildren(typeof(ParticleSystem));
		CurrentScale = 1;
		if (DesiredScale.IsNotAlmost(1f))
		{
			SetScale(DesiredScale, true);
		}
	}

	public void SetScale(float newScale, bool includeChildren)
	{
		//Scaling is persistent across pooling reuses, so we need to adjust for previous iterations of the scaling
		if (!(newScale > 0.01f)) newScale = 1f;
		mainPS.ScaleByTransform(newScale / CurrentScale, includeChildren);
		CurrentScale = newScale;
	}
}
