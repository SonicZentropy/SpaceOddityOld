using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PKFxFX))]
public class ShieldPFX : MonoBehaviour//, IOnAwake, IOnUpdate
{
	public PKFxFX ShieldPfx;
	private bool EffectTriggered = false;
	private Vector3 AxisRotation;

	public void Awake()
	{
		if (!ShieldPfx)
			ShieldPfx = GetComponent<PKFxFX>();
		AxisRotation = new Vector3(0, 1, 0);
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.V))
		{
			EffectTriggered = false;
			if (!EffectTriggered)
			{
				AxisRotation = Random.onUnitSphere.normalized;
				var aor = ShieldPfx.GetAttribute("AxisOfRotation");
				if (aor != null)
				{
					aor.ValueFloat3 = AxisRotation;
					ShieldPfx.SetAttribute(aor);
				}
				EffectTriggered = true;
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
		//else
		//{
		//	ShieldPfx.StopEffect();
		//	EffectTriggered = false;
	 //}
	}

	//public override int ExecutionPriority => 0;
	//public override Type ObjectType => typeof(ShieldPFX);
}