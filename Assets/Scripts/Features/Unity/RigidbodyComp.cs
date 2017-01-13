// /** 
//  * RigidbodyComp.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zenobit.Components
{
    #region Dependencies

    using UnityEngine;
    using Zenobit.Common.ZenECS;

    #endregion

    public class RigidbodyComp : ComponentEcs
    {
        public Rigidbody Rigidbody;

        public override ComponentTypes ComponentType => ComponentTypes.RigidbodyComp;
    }
}