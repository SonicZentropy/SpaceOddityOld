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
			var rb = comp.GetComponent<RigidbodyComp>().Rigidbody;
			rb.angularDrag = 5f;
			rb.drag = 1;
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
			var rb = comp.GetComponent<RigidbodyComp>()?.Rigidbody;
			if (rb == null) return;
			rb.angularDrag = 0f;
			rb.drag = 0f;
		}
	}
}