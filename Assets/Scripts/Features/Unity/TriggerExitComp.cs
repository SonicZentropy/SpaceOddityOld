// /** 
//  * TriggerExitComp.cs
//  * Dylan Bailey
//  * 20161210
// */

namespace Zenobit.Components
{
    #region Dependencies

    using System.Collections.Generic;
    using UnityEngine;
    using Zenobit.Common.ZenECS;

    #endregion

    public class TriggerExitComp : AbstractCollisionComp
    {
        public List<Collider> Other = new List<Collider>(5);

        public override ComponentTypes ComponentType => ComponentTypes.TriggerExitComp;
    }
}