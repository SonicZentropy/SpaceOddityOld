// /** 
//  * LightComp.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zenobit.Components
{
    #region Dependencies

    using UnityEngine;
    using Zenobit.Common.ZenECS;

    #endregion

    public class LightComp : ComponentEcs
    {
        public Light Light;

        public override ComponentTypes ComponentType => ComponentTypes.LightComp;
    }
}