// /** 
//  * HullComp.cs
//  * Dylan Bailey
//  * 20161103
// */

namespace Zenobit.Components
{
    #region Dependencies

    using Zenobit.Common.ZenECS;

    #endregion

    public class HullComp : ComponentEcs
    {
	    public float MaxHull;
	    public float CurrentHull;

        public override ComponentTypes ComponentType => ComponentTypes.HullComp;
    }
}