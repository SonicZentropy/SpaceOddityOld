// /** 
//  * UIWidgetComp.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zenobit.Components
{
    #region Dependencies

    using Zenobit.Common.ZenECS;

    #endregion

    public class UIWidgetComp : ComponentEcs
    {
        public UIWidget UIWidget;

        public override ComponentTypes ComponentType => ComponentTypes.UIWidgetComp;
    }
}