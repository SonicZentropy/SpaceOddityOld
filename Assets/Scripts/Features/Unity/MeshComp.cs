// /** 
//  * MeshComp.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zen.Components
{
    #region Dependencies

    using UnityEngine;
    using Zen.Common.ZenECS;

    #endregion

    public class MeshComp : ComponentEcs
    {
        public MeshFilter MeshFilter;
        public MeshRenderer MeshRenderer;

        public override ComponentTypes ComponentType => ComponentTypes.MeshComp;
    }
}