// /** 
//  * AbstractCollisionComp.cs
//  * Dylan Bailey
//  * 20161210
// */

namespace Zenobit.Components
{
    #region Dependencies

    using Zenobit.Common.ZenECS;

    #endregion

    public abstract class AbstractCollisionComp : ComponentEcs
    {
        public bool CollisionHandlingEnabled = true;

        public override ComponentTypes ComponentType => ComponentTypes.AbstractCollisionComp;
    }
}