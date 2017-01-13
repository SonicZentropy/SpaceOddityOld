// /** 
//  * CollisionExitComp.cs
//  * Dylan Bailey`````
//  * 20161210
// */

namespace Zenobit.Components
{
    #region Dependencies

    using System.Collections.Generic;
    using UnityEngine;
    using Zenobit.Common.ZenECS;

    #endregion

    public class CollisionExitComp : AbstractCollisionComp
    {
        public List<Collision> Other = new List<Collision>(5);

        public override ComponentTypes ComponentType => ComponentTypes.CollisionExitComp;
    }
}