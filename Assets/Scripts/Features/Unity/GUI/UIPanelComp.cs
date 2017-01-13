// /** 
//  * UIPanelComp.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zenobit.Components
{
    #region Dependencies

    using Zenobit.Common.ZenECS;

    #endregion

    public class UIPanelComp : ComponentEcs
    {
        public UIPanel UIPanel;

        public override ComponentTypes ComponentType => ComponentTypes.UIPanelComp;
    }
}