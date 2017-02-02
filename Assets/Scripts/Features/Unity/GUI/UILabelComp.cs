// /** 
//  * UILabelComp.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zen.Components
{
    #region Dependencies

    using Zen.Common.ZenECS;

    #endregion

    public class UILabelComp : ComponentEcs
    {
        public UILabel UILabel;

        public override ComponentTypes ComponentType => ComponentTypes.UILabelComp;
	    public override string Grouping => "UnityGUI";
    }
}