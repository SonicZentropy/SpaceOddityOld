// /** 
//  * CollisionExitComp.cs
//  * Dylan Bailey`````
//  * 20161210
// */

namespace Zen.Components
{
    #region Dependencies

    using System.Collections.Generic;
    using UnityEngine;
    using Zen.Common.ZenECS;

    #endregion

    public class CollisionExitComp : AbstractCollisionComp
    {
        public List<Collision> Other = new List<Collision>(5);

        public override void OnDestroy()
        {
            Other.Clear();
        }


        public override ComponentTypes ComponentType => ComponentTypes.CollisionExitComp;
	    public override string Grouping => "Unity";
    }
}