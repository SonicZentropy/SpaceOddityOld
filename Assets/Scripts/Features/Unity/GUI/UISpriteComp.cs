// /** 
//  * UISpriteComp.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zenobit.Components
{
    #region Dependencies

    using Zenobit.Common.ZenECS;

    #endregion

    public class UISpriteComp : ComponentEcs
    {
        public UISprite UISprite;

        public override ComponentTypes ComponentType => ComponentTypes.UISpriteComp;
    }
}