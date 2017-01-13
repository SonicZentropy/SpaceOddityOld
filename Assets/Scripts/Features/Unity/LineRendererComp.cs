// /** 
//  * LineRendererComp.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zenobit.Components
{
    #region Dependencies

    using UnityEngine;
    using Zenobit.Common.ZenECS;

    #endregion

    public class LineRendererComp : ComponentEcs
    {
        public LineRenderer LineRenderer;

        public override ComponentTypes ComponentType => ComponentTypes.LineRendererComp;
    }
}