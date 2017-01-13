// /** 
//  * UILabelComp.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zenobit.Components
{
    #region Dependencies

    using Zenobit.Common.ZenECS;

    #endregion

    public class UILabelComp : ComponentEcs
    {
        public UILabel UILabel;

        public override ComponentTypes ComponentType => ComponentTypes.UILabelComp;
    }
}