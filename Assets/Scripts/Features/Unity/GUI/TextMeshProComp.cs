// /** 
//  * TextMeshProComp.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zenobit.Components
{
    #region Dependencies

    using TMPro;
    using Zenobit.Common.ZenECS;

    #endregion

    public class TextMeshProComp : ComponentEcs
    {
        public TextMeshPro TextMesh;

        public override ComponentTypes ComponentType => ComponentTypes.TextMeshProComp;
    }
}