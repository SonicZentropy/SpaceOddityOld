// /** 
//  * HullComp.cs
//  * Dylan Bailey
//  * 20161103
// */

namespace Zen.Components
{
    #region Dependencies

    using Zen.Common.ZenECS;

    #endregion

    public class HullComp : ComponentEcs
    {
	    public float MaxHull;
	    public float CurrentHull;

        public override ComponentTypes ComponentType => ComponentTypes.HullComp;
    }
}