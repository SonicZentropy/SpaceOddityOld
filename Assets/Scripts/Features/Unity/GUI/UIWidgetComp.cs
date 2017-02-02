// /** 
//  * UIWidgetComp.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zen.Components
{
    #region Dependencies

    using Zen.Common.ZenECS;

    #endregion

    public class UIWidgetComp : ComponentEcs
    {
        public UIWidget UIWidget;

        public override ComponentTypes ComponentType => ComponentTypes.UIWidgetComp;
	    public override string Grouping => "UnityGUI";
    }
}