using Features.Defense;
using UnityEngine;
using Zen.Common.ZenECS;
using Zen.Components;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PKFxFX))]
public class ShieldPFX : MonoBehaviour, IShieldTrigger, ICustomInit
{
    // #TODO: Changing camera on player should enable/disable based on cockpit view
    public bool DisableParticleFX;
    public PKFxFX ShieldPfx;

	//private bool EffectTriggered = false;
	private Vector3 AxisRotation;

	public void Awake()
	{
		if (!ShieldPfx)
			ShieldPfx = GetComponent<PKFxFX>();
		//AxisRotation = new Vector3(0, 1, 0);
	    AxisRotation = Random.onUnitSphere.normalized;
	}
    
	public void TriggerShield()
	{
	    if (DisableParticleFX) return;

		AxisRotation = Random.onUnitSphere.normalized;
		var aor = ShieldPfx.GetAttribute("AxisOfRotation");
		if (aor != null)
		{
			aor.ValueFloat3 = AxisRotation;
			ShieldPfx.SetAttribute(aor);
		}

		var localPos = ShieldPfx.GetAttribute("LocalPosOffset");
		if (localPos != null)
		{
			localPos.ValueFloat3 = transform.position;
			ShieldPfx.SetAttribute(localPos);
		}
		ShieldPfx.StopEffect();
		ShieldPfx.StartEffect();
	}

	public void ExecuteInitialization(Entity e, GameObject go)
	{
		e.GetComponent<ShieldComp>().shieldTrigger = this;
	}
}