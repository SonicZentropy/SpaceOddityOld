// /** 
//  * UICameraComp.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zenobit.Components
{
    #region Dependencies

    using Zenobit.Common.ZenECS;

    #endregion

    public class UICameraComp : ComponentEcs
    {
        public UICamera UICamera;

        public override ComponentTypes ComponentType => ComponentTypes.UICameraComp;
    }
}