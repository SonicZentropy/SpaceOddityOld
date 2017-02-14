// /** 
//  * TriggerExitComp.cs
//  * Dylan Bailey
//  * 20161210
// */

namespace Zen.Components
{
    #region Dependencies

    using System.Collections.Generic;
    using UnityEngine;
    using Zen.Common.ZenECS;

    #endregion

    public class TriggerExitComp : AbstractCollisionComp
    {
        public List<Collider> Other = new List<Collider>(5);

        public override void OnDestroy()
        {
            Other.Clear();
        }


        public override ComponentTypes ComponentType => ComponentTypes.TriggerExitComp;
	    public override string Grouping => "Unity";
    }
}