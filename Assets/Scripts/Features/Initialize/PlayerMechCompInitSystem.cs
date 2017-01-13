// /** 
// * PlayerMechCompInitSystem.cs
// * Will Hart and Dylan Bailey
// * 20161216
// */

namespace Zenobit.Systems
{
	#region Dependencies

	using System;
	using System.Linq;
	using Common.ZenECS;
	using Components;

	#endregion

	public class PlayerMechCompInitSystem : AbstractEcsSystem, IDisposable
	{

		public override bool Init()
		{
			engine.OnComponentAdded += InitializeComponent;
			return false;
		}

		public void Dispose()
		{
			engine.OnComponentAdded -= InitializeComponent;
		}

		public void InitializeComponent(ComponentEcs comp)
		{
			//if (comp.ComponentType != ComponentTypes.PlayerMechComp) return;
			//var fo = engine.Get<FocusObjectComp>(ComponentTypes.FocusObjectComp).First();
			//((PlayerMechComp)comp).FocusObject = fo.Owner.GetComponent<PositionComp>();
		}
	}
}