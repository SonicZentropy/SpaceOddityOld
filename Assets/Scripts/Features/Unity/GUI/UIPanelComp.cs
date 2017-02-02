// /** 
//  * UIPanelComp.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zen.Components
{
    #region Dependencies

    using Zen.Common.ZenECS;

    #endregion

    public class UIPanelComp : ComponentEcs
    {
        public UIPanel UIPanel;

        public override ComponentTypes ComponentType => ComponentTypes.UIPanelComp;
	    public override string Grouping => "UnityGUI";
    }
}