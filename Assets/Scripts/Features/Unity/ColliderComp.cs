// /** 
//  * ColliderComp.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zen.Components
{
    #region Dependencies

    using UnityEngine;
    using Zen.Common.ZenECS;

    #endregion

    public class ColliderComp : ComponentEcs
    {
        public Collider collider;

        public override ComponentTypes ComponentType => ComponentTypes.ColliderComp;
    }
}