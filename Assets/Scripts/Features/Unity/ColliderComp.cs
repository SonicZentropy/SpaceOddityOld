// /** 
//  * ColliderComp.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zenobit.Components
{
    #region Dependencies

    using UnityEngine;
    using Zenobit.Common.ZenECS;

    #endregion

    public class ColliderComp : ComponentEcs
    {
        public Collider collider;

        public override ComponentTypes ComponentType => ComponentTypes.ColliderComp;
    }
}