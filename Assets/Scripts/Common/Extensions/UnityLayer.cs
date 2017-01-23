using System;
using UnityEngine;

[System.Serializable]
public class UnityLayer
{
	[SerializeField]
	private int m_LayerIndex = 0;
	public int LayerIndex
	{
		get { return m_LayerIndex; }
		set
		{
			if (value >= 0 && value < 32)
			{
				m_LayerIndex = value;
			}
			else
			{
				ZenLogger.LogError($"LAYER OUT OF RANGE");
				m_LayerIndex = Mathf.Clamp(value, 0, 32);
			}
		}
	}

	public UnityLayer(int layerValue)
	{
		LayerIndex = layerValue;
	}

	public static implicit operator int(UnityLayer unityLayer)
	{
		return unityLayer.LayerIndex;
	}

	public static implicit operator UnityLayer(int layerValue)
	{
		return new UnityLayer(layerValue);
	}

	public int Mask => 1 << m_LayerIndex;
}