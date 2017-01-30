// /**
//  * ShieldComp.cs
//  * Dylan Bailey
//  * 1/28/2017
// */

namespace Zen.Components
{
    #region Dependencies

	using Features.Defense;
    using Zen.Common.ZenECS;

    #endregion

    public class ShieldComp : ComponentEcs
    {
	    public IShieldTrigger shieldTrigger;
	    public float CurrentShieldEnergy;
	    public float MaxShieldEnergy;
	    public float ShieldRechargeRate;

        public override ComponentTypes ComponentType => ComponentTypes.ShieldComp;
    }
}