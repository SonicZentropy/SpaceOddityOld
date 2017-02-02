// /** 
//  * RigidbodyComp.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zen.Components
{
    #region Dependencies

    using UnityEngine;
    using Zen.Common.ZenECS;

    #endregion

    public class RigidbodyComp : ComponentEcs
    {
        public Rigidbody Rigidbody;

        public override ComponentTypes ComponentType => ComponentTypes.RigidbodyComp;
	    public override string Grouping => "Unity";
    }
}