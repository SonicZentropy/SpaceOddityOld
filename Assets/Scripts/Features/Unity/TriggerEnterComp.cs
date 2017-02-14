// /** 
//  * TriggerEnterComp.cs
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

    public class TriggerEnterComp : AbstractCollisionComp
    {
        public List<Collider> Other = new List<Collider>(5);

        public override void OnDestroy()
        {
            Other.Clear();
        }


        public override ComponentTypes ComponentType => ComponentTypes.TriggerEnterComp;
	    public override string Grouping => "Unity";
    }
}