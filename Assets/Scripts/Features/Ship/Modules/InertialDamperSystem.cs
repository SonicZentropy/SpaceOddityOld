// /** 
// * InertialDamperSystem.cs
// * Dylan Bailey
// * 20161224
// */

namespace Zen.Systems
{
	#region Dependencies

	using Common.ZenECS;
	using Components;
	using UnityEngine.Assertions;

	#endregion

	public class InertialDamperSystem : AbstractShipModuleSystem
	{
		protected override void ApplyAllModules()
		{
			foreach (InertialDamperModComp comp in engine.Get(ComponentTypes.InertialDamperModComp))
			{
				ApplyModule(comp);
			}
		}

		protected override void ApplyModule(ComponentEcs comp)
		{
			comp.Owner.GetComponent<ShipComp>().HasInertialDampers = true;

			var rb = comp.GetComponent<RigidbodyComp>().rigidbody;
			rb.angularDrag = 5f;
			rb.drag = 1;
		}

		protected void DeactivateModule(ComponentEcs comp)
		{
			((InertialDamperModComp) comp).ModuleEnabled = false;
			var rb = comp.GetComponent<RigidbodyComp>()?.rigidbody;
			Assert.IsNotNull(rb, "Inertial damper comp without a rigidbody");
			rb.angularDrag = 0f;
			rb.drag = 0f;
		}

		protected override void AddModule(ComponentEcs comp)
		{
			if (comp.ComponentType == ComponentTypes.InertialDamperModComp)
				ApplyModule(comp);
		}

		protected override void RemoveModule(ComponentEcs comp)
		{
			if (comp.ComponentType != ComponentTypes.InertialDamperModComp) return;
			if (comp.Owner == null) return;

			comp.Owner.GetComponent<ShipComp>().HasInertialDampers = false;
			var rb = comp.GetComponent<RigidbodyComp>()?.rigidbody;
			if (rb == null) return;
			rb.angularDrag = 0f;
			rb.drag = 0f;
		}
	}
}