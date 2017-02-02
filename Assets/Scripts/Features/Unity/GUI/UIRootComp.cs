// /** 
//  * UIRootComp.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zen.Components
{
    #region Dependencies

    using Zen.Common.ZenECS;

    #endregion

    public class UIRootComp : ComponentEcs
    {
        public UIRoot UIRoot;

        public override ComponentTypes ComponentType => ComponentTypes.UIRootComp;
	    public override string Grouping => "UnityGUI";
    }
}