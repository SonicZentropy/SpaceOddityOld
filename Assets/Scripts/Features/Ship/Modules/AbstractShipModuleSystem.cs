// /** 
// * ShipModuleSystem.cs
// * Will Hart and Dylan Bailey
// * 20161224
// */

namespace Zenobit.Systems
{
	#region Dependencies

	using Common;
	using Common.ZenECS;
	using Components;

	#endregion

	public abstract class AbstractShipModuleSystem : AbstractEcsSystem
	{
		public override bool Init()
		{
			engine.OnComponentAdded += AddModule;
			engine.OnComponentRemoved += RemoveModule;

			ApplyAllModules();

			return false;
		}

		public virtual void Dispose()
		{
			engine.OnComponentAdded -= AddModule;
			engine.OnComponentRemoved -= RemoveModule;
		}

		protected abstract void ApplyAllModules();
		protected abstract void ApplyModule(ComponentEcs comp);
		protected abstract void AddModule(ComponentEcs comp);
		protected abstract void RemoveModule(ComponentEcs comp);

	}
}