// /** 
//  * LineRendererComp.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zen.Components
{
    #region Dependencies

    using UnityEngine;
    using Zen.Common.ZenECS;

    #endregion

    public class LineRendererComp : ComponentEcs
    {
        public LineRenderer LineRenderer;

        public override ComponentTypes ComponentType => ComponentTypes.LineRendererComp;
    }
}