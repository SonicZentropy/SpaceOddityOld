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

		col = ExplosionPfx.GetAttribute("HitScale");
		col.ValueFloat = ExplosionScale;
		ExplosionPfx.SetAttribute(col);

		//Debug.Log("Setting delegate");
		//ExplosionPfx.m_OnFxStopped += OnFxStoppedDelegate;

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
	//		ZenLogger.Log($"No longer alive in pudate");
	//		deathreported = true;
	//	}
	//}

	//private void OnFxStoppedDelegate(PKFxFX component)
	//{
	//	ZenLogger.Log("FX Stopped DELEGATE, releasing");
	//	gameObject.Release();
	//}

	public void OnDisable()
	{
		ExplosionPfx.StopEffect();
	}

	public void OnEnable()
	{
		ExplosionPfx.StartEffect();
		//Debug.Log($"Started on enable, is alive? {ExplosionPfx.Alive()}");
		//updatereported = false;
		//deathreported = false;
		gameObject.ReleaseDelayed(1.2f);
	}

}