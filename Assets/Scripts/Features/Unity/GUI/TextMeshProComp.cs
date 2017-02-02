// /** 
//  * TextMeshProComp.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zen.Components
{
    #region Dependencies

    using TMPro;
    using Zen.Common.ZenECS;

    #endregion

    public class TextMeshProComp : ComponentEcs
    {
        public TextMeshPro TextMesh;

        public override ComponentTypes ComponentType => ComponentTypes.TextMeshProComp;
	    public override string Grouping => "UnityGUI";
    }
}