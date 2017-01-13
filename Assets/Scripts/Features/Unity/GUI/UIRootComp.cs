// /** 
//  * UIRootComp.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zenobit.Components
{
    #region Dependencies

    using Zenobit.Common.ZenECS;

    #endregion

    public class UIRootComp : ComponentEcs
    {
        public UIRoot UIRoot;

        public override ComponentTypes ComponentType => ComponentTypes.UIRootComp;
    }
}