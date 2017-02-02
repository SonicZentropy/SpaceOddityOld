using System;
using System.Collections.Generic;
using MEC;
using UnityEngine;
using Zen.Common.ObjectPool;

[RequireComponent(typeof(PKFxFX))]
public class ExplosionPFX : MonoBehaviour//, IOnAwake, IOnUpdate
{
	public PKFxFX ExplosionPfx;
	public Vector4 ExplosionRGBA;

	[Range(0, 10)]
	public float ExplosionScale = 1.0f;
	private bool EffectTriggered = false;
	private bool updatereported = false;
	private bool deathreported = false;

	public void Awake()
	{
		if (!ExplosionPfx)
			ExplosionPfx = GetComponent<PKFxFX>();

		var col = ExplosionPfx.GetAttribute("HitColor");
		col.ValueFloat4 = ExplosionRGBA;
		ExplosionPfx.SetAttribute(col);

		ChangeExplosionScale(ExplosionScale);
	}

	public void ChangeExplosionScale(float newScale)
	{
		Debug.Assert(newScale > 0, "New scale for explosion less than 0!");
		ExplosionScale = newScale;
		var col = ExplosionPfx.GetAttribute("HitScale");
		col.ValueFloat = ExplosionScale;
		ExplosionPfx.SetAttribute(col);
	}

	//void Update()
	//{
	//	if (!updatereported)
	//	{
	//		Debug.Log($"Started update, is alive? {ExplosionPfx.Alive()}");
	//		updatereported = true;
	//	}
	//	else if (!ExplosionPfx.Alive() && deathreported == false)
	//	{
	//		Debug.Log($"No longer alive in pudate");
	//		deathreported = true;
	//	}
	//}

	//private void OnFxStoppedDelegate(PKFxFX component)
	//{
	//	Debug.Log("FX Stopped DELEGATE, releasing");
	//	gameObject.Release();
	//}

	public void OnDisable()
	{
		ExplosionPfx.StopEffect();
	}

	public void OnEnable()
	{
		Timing.RunCoroutine(_CheckPfxAliveAndRelease());
		//ExplosionPfx.StartEffect();
		////Debug.Log($"Started on enable, is alive? {ExplosionPfx.Alive()}");
		////updatereported = false;
		////deathreported = false;
		//gameObject.ReleaseDelayed(1.2f);
	}

	IEnumerator<float> _CheckPfxAliveAndRelease()
	{
		ExplosionPfx.StartEffect();
		yield return Timing.WaitForSeconds(1.0f);

		while (ExplosionPfx.Alive())
		{
			yield return Timing.WaitForSeconds(1.0f);
		}

		ExplosionPfx.StopEffect();
		gameObject.Release();
	}

}