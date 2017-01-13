// /** 
//  * MeshComp.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zenobit.Components
{
    #region Dependencies

    using UnityEngine;
    using Zenobit.Common.ZenECS;

    #endregion

    public class MeshComp : ComponentEcs
    {
        public MeshFilter MeshFilter;
        public MeshRenderer MeshRenderer;

        public override ComponentTypes ComponentType => ComponentTypes.MeshComp;
    }
}