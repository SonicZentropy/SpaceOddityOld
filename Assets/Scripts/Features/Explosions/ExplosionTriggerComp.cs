// /** 
//  * ExplosionTriggerComp.cs
//  * Dylan Bailey
//  * 20170214
// */

namespace Zen.Components
{
    #region Dependencies

    using UnityEngine;
    using Zen.Common.ZenECS;

    #endregion

    public class ExplosionTriggerComp : ComponentEcs
    {
        public override ComponentTypes ComponentType => ComponentTypes.ExplosionTriggerComp;
    }
}