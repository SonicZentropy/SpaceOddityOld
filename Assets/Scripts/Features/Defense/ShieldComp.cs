// /**
//  * ShieldComp.cs
//  * Dylan Bailey
//  * 1/28/2017
// */

namespace Zenobit.Components
{
    #region Dependencies

	using Features.Defense;
    using Zenobit.Common.ZenECS;

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