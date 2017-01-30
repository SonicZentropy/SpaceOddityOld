// /** 
//  * AbstractCollisionComp.cs
//  * Dylan Bailey
//  * 20161210
// */

namespace Zen.Components
{
    #region Dependencies

    using Zen.Common.ZenECS;

    #endregion

    public abstract class AbstractCollisionComp : ComponentEcs
    {
        public bool CollisionHandlingEnabled = true;

        public override ComponentTypes ComponentType => ComponentTypes.AbstractCollisionComp;
    }
}