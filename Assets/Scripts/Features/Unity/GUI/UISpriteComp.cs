// /** 
//  * UISpriteComp.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zen.Components
{
    #region Dependencies

    using Zen.Common.ZenECS;

    #endregion

    public class UISpriteComp : ComponentEcs
    {
        public UISprite UISprite;

        public override ComponentTypes ComponentType => ComponentTypes.UISpriteComp;
    }
}